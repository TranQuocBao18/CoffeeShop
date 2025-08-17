namespace CoffeeShop.Model.Dto.Shared.Dtos
{
    public class GroupUserDto
{

    public Guid GroupId { get; set; }
    public IList<Guid> UserIds { get; set; } = [];
}
}