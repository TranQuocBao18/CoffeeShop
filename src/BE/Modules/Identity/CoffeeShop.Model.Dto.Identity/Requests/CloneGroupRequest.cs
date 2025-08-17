namespace CoffeeShop.Model.Dto.Identity.Requests
{
    public class CloneGroupRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }                
    }
}