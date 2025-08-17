namespace CoffeeShop.Model.Dto.Identity.Requests
{
    public class GroupRequest
    {
        public Guid? Id { get; set; }
        public required string Name { get; set; }
        public string? Code { get; set; }
    }
}