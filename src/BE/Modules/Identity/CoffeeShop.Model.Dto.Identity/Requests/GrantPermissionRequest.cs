namespace CoffeeShop.Model.Dto.Identity.Requests
{
    public class GrantPermissionRequest
    {
        public Guid GroupId { get; set; }
        public IList<PermissionsRequest> Permissions { get; set; } = [];
    }
}