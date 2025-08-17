using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Domain.Shared.Common
{
    public abstract class BaseEntity : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public virtual bool IsDeleted { get; set; }
    }
}