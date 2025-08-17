namespace CoffeeShop.Shared.Auth.Constants
{
    public enum PermissionLevel
    {
        FULL,
        VIEW,
        CREATE,
        DELETE,
        UPDATE,
    }

    public static class PermissionLevelExtensions
    {
        public static string GetCode(this PermissionLevel permissionLevel)
            => permissionLevel switch
            {
                PermissionLevel.FULL => "f",
                PermissionLevel.VIEW => "v",
                PermissionLevel.CREATE => "c",
                PermissionLevel.DELETE => "d",
                PermissionLevel.UPDATE => "u",
                _ => throw new ArgumentException("Permission Code was not supported.", nameof(permissionLevel)),
            };

        public static IEnumerable<string> GetCodes()
            => Enum
            .GetValues(typeof(PermissionLevel))
            .Cast<PermissionLevel>()
            .Select(x => x.GetCode());

        /// <summary>
        /// Get the codes that are allowed to be used in the system.
        /// Eg: Using in PermissionService at ValidatePermissionCode method 
        /// to check if the code is allowed for granting permissions.
        /// </summary> <returns>IIEnumerable<string></returns> <summary>
        public static IEnumerable<string> GetCodesAllowed()
            => Enum
            .GetValues(typeof(PermissionLevel))
            .Cast<PermissionLevel>()
            .Where(x => x switch
            {
                PermissionLevel.FULL => true,
                PermissionLevel.VIEW => true,
                PermissionLevel.CREATE => false,
                PermissionLevel.DELETE => false,
                PermissionLevel.UPDATE => false,
                _ => false,
            })
            .Select(x => x.GetCode());

    }
}