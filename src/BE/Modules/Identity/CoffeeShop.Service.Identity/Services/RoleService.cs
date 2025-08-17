using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Domain.Shared.Enums;
using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.Infrastructure.Shared.ErrorCodes;
using CoffeeShop.Infrastructure.Shared.Exceptions;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Requests;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Service.Identity.Interfaces;
using CoffeeShop.Shared.Auth;
using CoffeeShop.Utilities.Extensions;

namespace CoffeeShop.Service.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        private readonly ISecurityContextAccessor _securityContextAccessor;
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<SystemRole> _roleManager;
        private List<RolesEnum> RoleAdmins = [RolesEnum.SuperAdmin];

        public RoleService(
            IMapper mapper,
            ILogger<RoleService> logger,
            IIdentityUnitOfWork identityUnitOfWork,
            ISecurityContextAccessor securityContextAccessor,
            RoleManager<SystemRole> roleManager,
            IPermissionService permissionService)
        {
            _mapper = mapper;
            _logger = logger;
            _identityUnitOfWork = identityUnitOfWork;
            _securityContextAccessor = securityContextAccessor;
            _roleManager = roleManager;
            _permissionService = permissionService;
        }

        public async Task<Response<Guid>> DeleteGroup(Guid groupId, CancellationToken cancellationToken) 
        {
            var groupFromDb = await _roleManager.FindByIdAsync(groupId.ToString());
            if (groupFromDb is null)
            {
                return new Response<Guid>(ErrorCodeEnum.ROG_ERR_001);
            }

            var hasUser = await _identityUnitOfWork.RoleRepository.AnyUserInGroupAsync(groupFromDb.Name);
            if (hasUser)
            {
                return new Response<Guid>(ErrorCodeEnum.ROG_ERR_007);
            }

            await _identityUnitOfWork.RoleRepository.DeleteGroup(groupFromDb.Name, cancellationToken: cancellationToken);

            return new Response<Guid>(groupId);
        }

        public async Task<Response<bool>> GrantPermissions(GrantPermissionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _identityUnitOfWork.BeginTransactionAsync();

                var groupId = request.GroupId;
                var permissionsToUpdate = request.Permissions;

                var groupFromDb = await _roleManager.FindByIdAsync(groupId.ToString());

                if (groupFromDb is null)
                {
                    return new Response<bool>(ErrorCodeEnum.ROG_ERR_001);
                }

                await _permissionService.RemovePermissionsFromGroup(groupFromDb);
                await _permissionService.UpdatePermissionsFromGroup(permissionsToUpdate, groupFromDb);

                await _identityUnitOfWork.CommitAsync();

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                await _identityUnitOfWork.RollbackAsync();
                throw new ApiException($"{ex.Message}");
            }
        }

        public async Task<Response<Guid>> CloneGroup(CloneGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _identityUnitOfWork.BeginTransactionAsync();
                var currentUserLoggedInId = _securityContextAccessor.UserId.ToString();

                var roleEntity = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == request.Id.ToString());
                if (roleEntity is null)
                {
                    throw new ApiException("Role is not existing.");
                }
                var cloneName = roleEntity.Name + " Clone " + DateTime.Now.ToUnixTime().ToString();

                var sysRole = new SystemRole
                {
                    Name = cloneName?.Trim(),                    
                    Description = roleEntity.Description,
                    CreatedBy = currentUserLoggedInId.ToString(),
                    Created = DateTime.UtcNow,
                    NormalizedName = cloneName?.ToUpperInvariant(),
                };

                var result = await _identityUnitOfWork.RoleRepository.CreateRoleWithCodeAsync(sysRole, cancellationToken);

                if (result == Guid.Empty)
                {
                    return new Response<Guid>(ErrorCodeEnum.ROG_ERR_002);
                }

                // Get Permissions to Clone
                var groupToCloneFromDb = await _roleManager.FindByIdAsync(result.ToString());
                if (groupToCloneFromDb is null)
                {
                    return new Response<Guid>(ErrorCodeEnum.ROG_ERR_001);
                }

                var permissionFromGroupToClone = await _permissionService.GetFormattedPermissionsByRolesAsync([groupToCloneFromDb.Name!], cancellationToken);
                var permissionsToClone = permissionFromGroupToClone.Data.Select(p => new PermissionsRequest
                {
                    Name = p.Name,
                    Key = p.Key,
                    Permission = p.Permission,
                }).ToList();

                // Clone Permissions for Group
                var groupFromDb = await _roleManager.FindByIdAsync(sysRole.Id.ToString());
                if (groupFromDb is null)
                {
                    return new Response<Guid>(ErrorCodeEnum.ROG_ERR_001);
                }
                await _permissionService.UpdatePermissionsFromGroup(permissionsToClone, groupFromDb);

                await _identityUnitOfWork.CommitAsync();

                return new Response<Guid>(result);
            }
            catch (Exception ex)
            {
                await _identityUnitOfWork.RollbackAsync();
                throw new ApiException($"{ex.Message}");
            }
        }

        public async Task<Response<Guid>> CreateGroup(GroupRequest request, CancellationToken cancellationToken)
        {
            var currentUserLoggedInId = _securityContextAccessor.UserId.ToString();
            var role = request;

            var duplicateRoleCount = await _roleManager.Roles
                .Where(r => r.Name.Equals(role.Name) && !r.IsDeleted)
                .CountAsync(cancellationToken);

            if (duplicateRoleCount > 0)
            {
                throw new ApiException("Role is existing.");
            }

            var sysRole = new SystemRole
            {
                Name = request.Name.Trim(),
                CreatedBy = currentUserLoggedInId.ToString(),
                Created = DateTime.UtcNow,
                NormalizedName = request.Name.ToUpperInvariant(),
            };

            var result = await _identityUnitOfWork.RoleRepository.CreateRoleWithCodeAsync(sysRole, cancellationToken);

            if (result == Guid.Empty)
            {
                return new Response<Guid>(ErrorCodeEnum.ROG_ERR_002);
            }

            return new Response<Guid>(result);
        }

        public async Task<Response<Guid>> UpdateGroup(GroupRequest request, CancellationToken cancellationToken)
        {
            await _identityUnitOfWork.BeginTransactionAsync();
            try
            {
                var currentUserLoggedInId = _securityContextAccessor.UserId.ToString();
                var role = request;

                var roleEntity = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == request.Id.ToString());
                if (roleEntity is null)
                {
                    throw new ApiException("Role is not existing.");
                }
                var duplicateRoleCount = await _roleManager.Roles
                   .Where(r => r.Name.Equals(request.Name) && !r.IsDeleted)
                   .CountAsync(cancellationToken);

                if (duplicateRoleCount > 1)
                {
                    throw new ApiException("Role is existing.");
                }

                roleEntity.Name = request.Name;
                roleEntity.LastModifiedBy = currentUserLoggedInId.ToString();
                roleEntity.LastModified = DateTime.UtcNow;
                roleEntity.NormalizedName = request.Name.Trim().ToUpperInvariant();

                var result = await _roleManager.UpdateAsync(roleEntity);
                if (result.Succeeded == false)
                {
                    throw new ApiException($"{result.Errors}");
                }

                await _identityUnitOfWork.CommitAsync();
                return new Response<Guid>(roleEntity.Id.AsGuid());
            }
            catch (Exception ex)
            {
                await _identityUnitOfWork.RollbackAsync();
                throw new ApiException(ex.Message);
            }
        }

        public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _identityUnitOfWork.RoleRepository.GetRolesAsync();
            var rolesResponse = new List<RolesResponse>();
            foreach (var role in roles)
            {
                rolesResponse.Add(ConvertToRoleReponse(role));
            }
            return new PagedResponse<IReadOnlyList<RolesResponse>>(rolesResponse, 0, 1000, rolesResponse.Count());
        }

        private static RolesResponse ConvertToRoleWithPermissionReponse(Role role, IList<PermissionsResponse> permissions)
        {
            return new RolesResponse
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Code = role.Code ?? string.Empty,
                permissions = permissions,
                Created = role.Created,
                IsDeleted = role.IsDeleted,
                CreatedBy = role.CreatedBy,
                LastModified = role.LastModified,
                LastModifiedBy = role.LastModifiedBy,
            };
        }

        private static RolesResponse ConvertToRoleReponse(Role role)
        {
            return new RolesResponse
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Code = role.Code ?? string.Empty,
                Created = role.Created,
                IsDeleted = role.IsDeleted,
                CreatedBy = role.CreatedBy,
                LastModified = role.LastModified,
                LastModifiedBy = role.LastModifiedBy
            };
        }

        public async Task<Response<RolesResponse>> GetRoleWithPermissionsByIdAsync(Guid roleId, CancellationToken cancellationToken)
        {
            var role = (await _identityUnitOfWork.RoleRepository.GetRolesAsync(r => r.Id == roleId.ToString() && !r.IsDeleted, cancellationToken)).FirstOrDefault();
            if (role is null)
            {
                return new Response<RolesResponse>(ErrorCodeEnum.ROG_ERR_001);
            }

            // Add Permissions
            var permissions = await _permissionService.GetFormattedPermissionsByRolesAsync([role.RoleName], cancellationToken);
            var result = ConvertToRoleWithPermissionReponse(role, permissions.Data);

            return new Response<RolesResponse>(result);
        }

        public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var rolesByUserId = await _identityUnitOfWork.RoleRepository.GetRolesByUserIdAsync(userId);
            var rolesResponse = new List<RolesResponse>();
            foreach (var role in rolesByUserId)
            {
                rolesResponse.Add(ConvertToRoleReponse(role));
            }
            return new PagedResponse<IReadOnlyList<RolesResponse>>(rolesResponse, 0, 1000, rolesResponse.Count());
        }

        private bool CheckCurrentRoleAdminInToken()
        {
            var roles = _securityContextAccessor.Roles;
            foreach (var role in roles)
            {
                var inValidRole = Enum.TryParse(role, out RolesEnum roleEnum);
                if (!inValidRole)
                {
                    return false;
                }

                bool isAdmin = RoleAdmins.Contains(roleEnum);
                return isAdmin;
            }
            return false;
        }

        private async Task<List<RolesEnum>> GetCurrentUserRolesInDatabaseAsync()
        {
            var result = new List<RolesEnum>();
            var userId = _securityContextAccessor.UserId;
            var rolesByUserId = await _identityUnitOfWork.RoleRepository.GetRolesByUserIdAsync(userId);
            if (rolesByUserId == null || !rolesByUserId.Any())
            {
                return result;
            }
            foreach (var roleByUser in rolesByUserId)
            {
                var roleName = roleByUser.RoleName;
                var inValidRole = Enum.TryParse(roleName, out RolesEnum roleEnum);
                if (inValidRole)
                {
                    result.Add(roleEnum);
                }
            }
            return result;
        }

        public async Task<bool> AssignUserToGroupByUserIdAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
        {
            var user = await _identityUnitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken) ?? throw new ApiException("User is not existing.");
            var groupFromDb = await _roleManager.FindByIdAsync(groupId.ToString()) ?? throw new ApiException("Group is not existing.");
            var res = await _identityUnitOfWork.RoleRepository.AddUserToGroupByUserIdAsync(user, groupFromDb);

            return res;
        }

        public async Task<bool> AssignRolesByUserIdAsync(Guid userId, bool hasAdmin, CancellationToken cancellationToken)
        {
            var isAssigned = false;
            if (hasAdmin)
            {
                var roleNames = new List<string>()
                {
                    RolesEnum.Customer.ToString()
                };
                isAssigned = await _identityUnitOfWork.RoleRepository.GrantRolesByUserIdAsync(userId, roleNames);
            }
            else
            {
                string roleName = RolesEnum.Customer.ToString();
                isAssigned = await _identityUnitOfWork.RoleRepository.GrantRolesByUserIdAsync(userId, new List<string> { roleName });
            }
            return isAssigned;
        }
    }
}