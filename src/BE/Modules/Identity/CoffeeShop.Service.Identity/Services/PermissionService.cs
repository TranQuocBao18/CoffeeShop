using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.Infrastructure.Shared.Constants;
using CoffeeShop.Infrastructure.Shared.ErrorCodes;
using CoffeeShop.Infrastructure.Shared.Exceptions;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Presentation.Shared.Permissions;
using CoffeeShop.Service.Identity.Interfaces;
using CoffeeShop.Shared.Auth.Constants;
using CoffeeShop.Utilities.Extensions;

namespace CoffeeShop.Service.Identity.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionService> _logger;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<SystemRole> _roleManager;

        public PermissionService(IMapper mapper,
            ILogger<PermissionService> logger,
            IIdentityUnitOfWork identityUnitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<SystemRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _identityUnitOfWork = identityUnitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response<IReadOnlyList<PermissionsResponse>>> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            var rolePermisions = new List<PermissionsResponse>();
            return new PagedResponse<IReadOnlyList<PermissionsResponse>>(rolePermisions, 0, 1000, rolePermisions.Count());
        }

        public async Task<PagedResponse<IList<PermissionsResponse>>> GetPermissionsByRolesAsync(IList<string> roles, CancellationToken cancellationToken = default)
        {
            List<System.Security.Claims.Claim> identityClaims = [];

            if (roles.Any())
            {
                foreach (var roleName in roles)
                {
                    var identityRole = await _roleManager.FindByNameAsync(roleName);
                    var permisions = await _roleManager.GetClaimsAsync(identityRole);
                    if (permisions.Any())
                    {
                        identityClaims.AddRange(permisions);
                    }
                }
            }

            var rolePermissions = identityClaims
                .Where(x => x.Type == IdentityConstant.Permission)
                .Select(x =>
                {
                    var permissionParsed = x.Value.Split('.');

                    return new PermissionsResponse
                    {
                        Key = permissionParsed[0],
                        Name = string.Empty,
                        Permission = permissionParsed[1],
                    };
                })
                .ToList();


            return new PagedResponse<IList<PermissionsResponse>>(rolePermissions, 0, 1000, rolePermissions.Count);
        }

        public async Task<PagedResponse<IList<PermissionsResponse>>> GetFormattedPermissionsByRolesAsync(IList<string> roles, CancellationToken cancellationToken = default)
        {
            List<System.Security.Claims.Claim> identityClaims = [];

            if (roles.Any())
            {
                foreach (var roleName in roles)
                {
                    var identityRole = await _roleManager.FindByNameAsync(roleName);
                    var permisions = await _roleManager.GetClaimsAsync(identityRole);
                    if (permisions.Any())
                    {
                        identityClaims.AddRange(permisions);
                    }
                }
            }

            var rolePermisions = GetPermissionsFormatted(identityClaims);

            return new PagedResponse<IList<PermissionsResponse>>(rolePermisions, 0, 1000, rolePermisions.Count);
        }


        public async Task<Response<IList<PermissionsResponse>>> GetPermissionsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var userEntity = await _userManager.FindByIdAsync(userId.ToString());
            if (userEntity == null || userEntity.IsDeleted)
            {
                return new Response<IList<PermissionsResponse>>(ErrorCodeEnum.USE_ERR_001.ToString(), "User is not existing.");
            }
            var roles = await _userManager.GetRolesAsync(userEntity);

            return await GetPermissionsByRolesAsync(roles, cancellationToken);
        }

        public async Task RemovePermissionsFromGroup(SystemRole groupFromDb)
        {
            var oldPermissions = await _roleManager.GetClaimsAsync(groupFromDb);
            await _identityUnitOfWork
                .RoleRepository
                .RemovePermissionsById(groupFromDb.Id.AsGuid(), oldPermissions.Select(p => p.Value));
        }

        public async Task UpdatePermissionsFromGroup(IList<PermissionsRequest> permissionsToUpdate, SystemRole groupFromDb)
        {
            IList<PermissionsResponse> systemPermissions =
                GetDefaultSystemPermissions()
                .DistinctBy(x => x.Key)
                .OrderBy(x => int.Parse(x.Key))
                .ToList();

            if (permissionsToUpdate == null || permissionsToUpdate.Count == 0 || permissionsToUpdate.Count != systemPermissions.Count)
            {
                throw new ApiException("Permission list to update invalid.");
            }

            foreach (var newPermission in permissionsToUpdate)
            {
                var hasPermission = string.IsNullOrWhiteSpace(newPermission.Permission) == false;

                if (!hasPermission)
                {
                    continue;
                }

                ValidateNewPermission(systemPermissions, newPermission);

                var permissionToUpdate = $"{newPermission.Key}.{newPermission.Permission}";
                var claim = new System.Security.Claims.Claim(IdentityConstant.Permission, permissionToUpdate);
                await _roleManager.AddClaimAsync(groupFromDb, claim);
            }
        }

        #region Private Methods

        private static IList<PermissionsResponse> GetPermissionsFormatted(IList<System.Security.Claims.Claim> identityClaims)
        {
            IList<PermissionsResponse> permissionsResponse = [];

            var identityClaimDict = identityClaims.ToDictionary(x => x.Value);

            PermissionsResponse ProcessAndMapToRepsonse(string permissionFunction, string permissionValue)
            {
                var permissionParsed = permissionValue.Split('.');
                bool hasPermission = identityClaimDict.ContainsKey(permissionValue);

                return new PermissionsResponse
                {
                    Key = permissionParsed[0],
                    Name = permissionFunction,
                    Permission = hasPermission ? permissionParsed[1] : string.Empty,
                };
            }
            ;

            CollectPermissionsResponse(permissionsResponse, typeof(ApplicationPermissions), ProcessAndMapToRepsonse);
            CollectPermissionsResponse(permissionsResponse, typeof(IdentityPermissions), ProcessAndMapToRepsonse);

            return permissionsResponse.OrderBy(x => int.Parse(x.Key)).ToList();
        }

        private static void CollectPermissionsResponse(
            IList<PermissionsResponse> permissionsResponse,
            Type permissionType,
            Func<string, string, PermissionsResponse> processToResponse)
        {
            Dictionary<string, PermissionsResponse> permissionsResponseDict = [];

            var permissionGroups = permissionType.GetTypeInfo().DeclaredNestedTypes.ToList();

            foreach (var permissionGroup in permissionGroups)
            {
                if (permissionGroup.Name.Equals(nameof(ApplicationPermissions.Master)))
                {
                    continue;
                }

                var permissions = permissionGroup.GetFields().ToList();
                foreach (var permission in permissions)
                {
                    var permissionValue = permission.GetValue(permission)?.ToString();
                    // Can be check allowd permission code but this is just mapping from db so we can skip it.
                    // See UpdatePermissionsFromGroup method for code validation.
                    var permissionGroupName = permissionGroup.Name;
                    var permissionResponse = processToResponse(permissionGroupName, permissionValue);
                    if (permissionsResponseDict.ContainsKey(permissionGroupName))
                    {
                        if (string.IsNullOrWhiteSpace(permissionResponse.Permission) == false)
                        {                        
                            permissionsResponseDict.Remove(permissionGroupName);    
                            permissionsResponseDict[permissionGroupName] = permissionResponse;
                        }
                    }
                    else
                    {
                        permissionsResponseDict.Add(permissionGroupName, permissionResponse);
                    }                    
                }
            }

            permissionsResponse.AddRange(permissionsResponseDict.Values.ToList());
        }


        private static void ValidateNewPermission(IList<PermissionsResponse> systemPermissions, PermissionsRequest newPermission)
        {
            // Validate Permission Level Code. Eg: f, v, vl, ...
            ValidatePermissionCode(newPermission);

            // Validate Permission Function Name. Eg: Group, User, ...
            var systemPermission = systemPermissions.FirstOrDefault(x => x.Name == newPermission.Name);
            if (systemPermission is null)
            {
                throw new ApiException($"{newPermission.Name} - Permission function was not supported.");
            }

            // Validate Permission Key. Eg: 1, 2, 3, 4, 5, ...
            ValidatePermissionKey(newPermission, systemPermission);
        }

        private static void ValidatePermissionKey(PermissionsRequest newPermission, PermissionsResponse systemPermission)
        {
            var newPermissionKey = newPermission.Key;
            var systemPermissionKey = systemPermission.Key;
            if (newPermissionKey.Equals(systemPermissionKey) == false)
            {
                throw new ApiException($"{newPermission.Name} - Permission function was not supported.");
            }
        }

        private static void ValidatePermissionCode(PermissionsRequest newPermission)
        {
            var permissionCode = newPermission.Permission;
            if (!PermissionLevelExtensions.GetCodesAllowed().Contains(permissionCode))
            {
                throw new ApiException($"{newPermission.Name} - Permission function was not supported.");
            }
        }

        private static IList<PermissionsResponse> GetDefaultSystemPermissions(bool hasPermission = true)
        {
            IList<PermissionsResponse> permissionsResponse = [];

            PermissionsResponse mapToResponse(string permissionFunction, string permissionValue)
            {
                var permissionParsed = permissionValue.Split('.');

                return new PermissionsResponse
                {
                    Key = permissionParsed[0],
                    Name = permissionFunction,
                    Permission = hasPermission ? permissionParsed[1] : string.Empty,
                };
            }

            CollectPermissionsResponse(permissionsResponse, typeof(ApplicationPermissions), mapToResponse);
            CollectPermissionsResponse(permissionsResponse, typeof(IdentityPermissions), mapToResponse);

            return permissionsResponse;
        }

        #endregion

    }
}