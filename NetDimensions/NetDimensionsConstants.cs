using System;
using System.Linq;

namespace SitefinityWebApp.NetDimensions
{
    public static class NetDimensionsConstants
    {
        public static readonly string API_UsersCsv = "{0}/contentHandler/usersCsv";
        public static readonly string API_Organizations = "{0}/contentHandler/imsEnterprise";
        public static readonly string API_GetOrganization = "{0}/api/organization";
        public static readonly string API_GetUsers = "{0}/api/users";
        public static readonly string API_GetUserInOrganization = "{0}/api/users200510Xml";

        public readonly static string ActionResult_NoErrorToken = "No Error Occurred";
        public readonly static string ActionResult_WarningToken = "WARNING. ";
        
        public static readonly string CsvPattern = @"""\s*,\s*""";

        public static class UserActions
        {
            public static readonly string Add = "A";
            public static readonly string Update = "U";
            public static readonly string AddOrUpdate = "AU";
            public static readonly string Delete = "D";
        }

        public enum OrganizationActions
        { 
            Add = 1,
            Update = 2,
            Delete = 3
        }

        public static class Genders
        {
            public static readonly string Male = "M";
            public static readonly string Female = "F";
        }

        public static class UserStatuses
        {
            public static readonly string Active = "Active";
            public static readonly string Suspended = "Suspend";
            public static readonly string Closed = "Close";
        }
    }
}