namespace Imarat_Shariah.Utilities
{

    public static class ApplicationPermissions
    {
        // Role Names Constants
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Staff = "Staff";
        }

        // Custom Claim Type
        public const string PermissionClaimType = "Permission";

        // Centralized Policy Names
        public static class Policies
        {
            public const string CanViewSiyajat = "CanViewSiyajat";
            public const string CanCreateSiyajat = "CanCreateSiyajat";
            public const string CanUpdateSiyajat = "CanUpdateSiyajat";
            public const string CanDeleteSiyajat = "CanDeleteSiyajat";

            public const string CanViewKhula = "CanViewKhula";
            public const string CanCreateKhula = "CanCreateKhula";
            public const string CanUpdateKhula = "CanUpdateKhula";
            public const string CanDeleteKhula = "CanDeleteKhula";
        }

        // Fine-Grained Permissions
        public static class Siyajat
        {
            public const string View = "Permissions.Siyajat.View";
            public const string Create = "Permissions.Siyajat.Create";
            public const string Update = "Permissions.Siyajat.Update";
            public const string Delete = "Permissions.Siyajat.Delete";
        }

        public static class Khula
        {
            public const string View = "Permissions.Khula.View";
            public const string Create = "Permissions.Khula.Create";
            public const string Update = "Permissions.Khula.Update";
            public const string Delete = "Permissions.Khula.Delete";
        }

        // Helper Method: Pure system ki saari permissions ki list automatic nikalne ke liye
        public static List<string> GetAllPermissions()
        {
            return new List<string>
            {
                Siyajat.View, Siyajat.Create, Siyajat.Update, Siyajat.Delete,
                Khula.View, Khula.Create, Khula.Update, Khula.Delete
            };
        }
    }
}