using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Model.Dto.Identity.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}