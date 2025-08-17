using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Domain.Identity.Entities
{
    public class ApplicationSetting
    {
        [Key]
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Enable { get; set; }
    }
}
