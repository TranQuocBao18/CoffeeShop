using System.ComponentModel.DataAnnotations.Schema;
using CoffeeShop.Domain.Shared.Common;
using CoffeeShop.Domain.Shared.Enums;

namespace CoffeeShop.Domain.Identity.Entities
{
    public class User : AuditableBaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsLocked { get; set; }
        public List<RolesEnum> Roles { get; set; }

        [NotMapped]
        public bool IsAdmin { get; set; }
    }
}
