namespace CoffeeShop.Model.Dto.Identity.Requests
{
    public class PermissionsRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
    }
}