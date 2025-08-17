namespace CoffeeShop.Shared.Auth
{
    public class AuthOptions
    {
        public bool TokenManagerDisabled { get; set; } = false; // auto-log-in, log-out when user opens multiple browser tabs
    }
}