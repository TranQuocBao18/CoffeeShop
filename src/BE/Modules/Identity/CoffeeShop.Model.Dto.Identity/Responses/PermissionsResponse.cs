namespace CoffeeShop.Model.Dto.Identity.Responses
{
    public class PermissionsResponse
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
        public List<PermissionsResponse>? Children { get; set; }
    }
}