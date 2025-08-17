using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Domain.Shared.Common
{
    public interface IEntity<T>
    {
        [Key]
        T Id { get; set; }
    }
}