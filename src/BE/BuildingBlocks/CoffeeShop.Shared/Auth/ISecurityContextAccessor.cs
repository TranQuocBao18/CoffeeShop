namespace CoffeeShop.Shared.Auth
{
    public interface ISecurityContextAccessor
    {
        Guid UserId { get; }
        string Email { get; }
        string Role { get; }
        List<string> Roles { get; }
        IEnumerable<string> PermissionLists { get; }
        string JwtToken { get; }
        string IPAddressClient { get; }
        bool IsAuthenticated { get; }
    }
}