namespace CoffeeShop.Presentation.Shared.Permissions
{
    public static class ApplicationPermissions
    {
        public static class Master
        {
            public const string FULL = "0.f";
        }

        public static class Group
        {
            public const string FULL = "1.f";
            public const string VIEW = "1.v";
            public const string CREATE = "1.c";
            public const string DELETE = "1.d";
            public const string UPDATE = "1.u";
        }
        
        public static class Permission
        {
            public const string FULL = "1.f";
            public const string VIEW = "1.v";
            public const string CREATE = "1.c";
            public const string DELETE = "1.d";
            public const string UPDATE = "1.u";
        }

        public static class Role
        {
            public const string FULL = "2.f";
            public const string VIEW = "2.v";
            public const string CREATE = "2.c";
            public const string DELETE = "2.d";
            public const string UPDATE = "2.u";
        }

        public static class User
        {
            public const string FULL = "3.f";
            public const string VIEW = "3.v";
            public const string CREATE = "3.c";
            public const string DELETE = "3.d";
            public const string UPDATE = "3.u";
        }
    }
}