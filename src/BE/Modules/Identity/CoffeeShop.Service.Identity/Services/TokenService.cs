using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CoffeeShop.Domain.Identity.Entities;
using CoffeeShop.Domain.Shared.Settings;
using CoffeeShop.Infrastructure.Identity.Helpers;
using CoffeeShop.Infrastructure.Identity.Interfaces;
using CoffeeShop.Infrastructure.Shared.Constants;
using CoffeeShop.Service.Identity.Interfaces;
using CoffeeShop.Utilities.Extensions;

namespace CoffeeShop.Service.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<SystemRole> _roleManager;
        private readonly IRoleRepository _roleRepository;   
        private readonly JWTSettings _jwtSettings;
        private readonly IPermissionService _permissionService;

        public TokenService(IOptions<JWTSettings> jwtSettings, RoleManager<SystemRole> roleManager, UserManager<ApplicationUser> userManager, IPermissionService permissionService, IRoleRepository roleRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _roleManager = roleManager;
            _userManager = userManager;
            _permissionService = permissionService;
            _roleRepository = roleRepository;
        }

        public async Task<string> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id.AsGuid());

            var roleClaims = new List<Claim>();
            var rolePermisions = new List<string>();

            var permisisonsFromDb = await _permissionService.GetPermissionsByRolesAsync(roles.Select(r => r.RoleName).ToList());
            if (permisisonsFromDb == null || permisisonsFromDb.Data == null)
            {
                return string.Empty;
            }
            
            var permissions = permisisonsFromDb.Data.Select(p => $"{p.Key}.{p.Permission}");
            rolePermisions.AddRange(permissions);

            if (rolePermisions.Any())
            {
                foreach (var rolepermision in rolePermisions.Distinct())
                {
                    roleClaims.Add(new Claim(IdentityConstant.Permission, rolepermision));
                }
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress),
                new Claim("role", roles.FirstOrDefault()?.RoleName ?? string.Empty),
                new Claim("group", roles.FirstOrDefault()?.Code ?? string.Empty),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
            claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}