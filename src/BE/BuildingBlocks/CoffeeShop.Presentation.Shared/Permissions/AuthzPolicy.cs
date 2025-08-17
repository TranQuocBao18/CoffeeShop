namespace CoffeeShop.Presentation.Shared.Permissions
{
    public struct AuthzPolicy
    {
        // Supper Admin Access
        public const string ALLOW_SUPPER_ADMIN_ACCESS = "AllowSupperAdmin";
        // Customer Access
        public const string ALLOW_CUSTOMER_ACCESS = "AllowCustomer";
        // Basic Access
        public const string ALLOW_BASIC_ACCESS = "AllowBasic";
    }
}