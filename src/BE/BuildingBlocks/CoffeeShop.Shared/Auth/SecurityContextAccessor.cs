using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CoffeeShop.Shared.Auth.Constants;
using CoffeeShop.Shared.Types;

namespace CoffeeShop.Shared.Auth
{
    public class SecurityContextAccessor : ISecurityContextAccessor
    {
        private readonly ILogger<SecurityContextAccessor> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecurityContextAccessor(IHttpContextAccessor httpContextAccessor, ILogger<SecurityContextAccessor> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.GetClaim(ClaimTypeExtend.UserId);
                _logger.LogDebug("{SecurityContext} Current user_id: {UserId}", "SecurityContext", userId);

                if (string.IsNullOrEmpty(userId))
                {
                    return Guid.Empty;
                }
                Guid.TryParse(userId, out Guid result);
                return result;
            }
        }

        public string Email
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email);
                return claim?.Value;
            }
        }

        public string JwtToken
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];
            }
        }

        public string IPAddressClient
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                var isAuthenticated = _httpContextAccessor.HttpContext?.User?.Identities?.FirstOrDefault()?.IsAuthenticated;
                if (!isAuthenticated.HasValue)
                {
                    return false;
                }

                return isAuthenticated.Value;
            }
        }

        public string Role
        {
            get
            {
                _logger.LogDebug("{SecurityContext} Current role: {Role}", "SecurityContext", _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value);
                var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
                return role;
            }
        }

        public List<string> Roles
        {
            get
            {
                _logger.LogDebug("{SecurityContext} Current role: {Role}", "SecurityContext", _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value);
                var roleClaims = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value);
                return roleClaims.ToList();
            }
        }

        public IEnumerable<string> PermissionLists
        {
            get
            {
                _logger.LogDebug(
                    "{SecurityContext} Authenticated: {IsAuthenticated}", "SecurityContext", _httpContextAccessor.HttpContext?.User?.Identities?.FirstOrDefault().IsAuthenticated);
                _logger.LogDebug(
                    "{SecurityContext} Claims count: {ClaimsCount}", "SecurityContext", _httpContextAccessor.HttpContext?.User?.Identities?.FirstOrDefault().Claims.Count());
                var permissions = _httpContextAccessor.HttpContext?.User?
                    .FindAll(Permissions.PERMISSION_CLAIM_TYPE)
                    .Select(x => x.Value);
                return permissions;
            }
        }
    }
}