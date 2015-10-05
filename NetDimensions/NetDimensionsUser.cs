using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace NetDimensionsWrapper.NetDimensions
{
    /// <summary>
    /// Represents a user in the NetDimensions system
    /// </summary>
    public class NetDimensionsUser
    {
        /// <summary>
        /// Gets or sets the user's ID.
        /// </summary>
        [JsonProperty("id")]
        [CsvField("UserID")]
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        [JsonProperty("given")]
        [CsvField("GivenName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's middle name.
        /// </summary>
        [JsonProperty("middle")]
        [CsvField("MiddleName")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        [JsonProperty("family")]
        [CsvField("FamilyName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's other name.
        /// </summary>
        [JsonProperty("other")]
        [CsvField("OtherName")]
        public string OtherName { get; set; }

        /// <summary>
        /// Gets or sets the user's gender. "M"/"F"
        /// </summary>
        [CsvField("Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the user's date of birth.
        /// Converted to dd-mmm-yy
        /// </summary>
        [CsvField("BirthDate")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [JsonIgnore]
        [CsvField("Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's status. "Active", "Suspend", or "Close" (case insensitive)
        /// </summary>
        [JsonProperty("status")]
        [CsvField("Status")]
        public string Status { get; set; }

        /// <summary>
        /// Indicates whether the user uses external authentication.
        /// Converted to "Y"/"N"
        /// </summary>
        [JsonIgnore]
        [CsvField("ExternalAuthentication")]
        public bool UseExternalAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the user's employee number.
        /// </summary>
        [JsonProperty("employeeNumber")]
        [CsvField("Employee Num")]
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's expiration date. 
        /// Converted to dd-mmm-yy
        /// </summary>
        [JsonIgnore]
        [CsvField("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the user's language.
        /// Convertsed to ISO 2-char codes: en, fr_CA, es_ES
        /// </summary>
        [JsonIgnore]
        [CsvField("LanguagePref")]
        public CultureInfo Language { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [JsonProperty("email")]
        [CsvField("Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's join date.
        /// Converted to dd-mmm-yy
        /// </summary>
        [JsonIgnore]
        [CsvField("Join Date")]
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// Gets or sets the user's organization.
        /// Should contain the organization's code in a comma-delimited hierarchy list of organizations (not counting ROOT). e.g.: "PSC,AutoGen"
        /// </summary>
        [JsonConverter(typeof(JsonOrganizationConverter))]
        [JsonProperty("organization")]
        public NetDimensionsOrganization Organization { get; set; }

        /// <summary>
        /// Gets or sets the user's primary role.
        /// </summary>
        [JsonConverter(typeof(JsonWrappedIdConverter))]
        [CsvField("UserRole")]
        public string PrimaryRole { get; set; }

        /// <summary>
        /// Gets or sets the user's direct appraiser's user ID.
        /// Must be a NetDimensions User ID.
        /// </summary>
        [JsonConverter(typeof(JsonWrappedIdConverter))]
        [JsonProperty("directAppraiser")]
        [CsvField("Direct Appraiser")]
        public string DirectAppraiserUserID { get; set; }

        /// <summary>
        /// Gets or sets the user's department ID.
        /// </summary>
        [JsonProperty("departmentId")]
        [CsvField("DeptId")]
        public string DepartmentID { get; set; }

        /// <summary>
        /// Gets or sets the user's department name.
        /// </summary>
        [JsonProperty("departmentName")]
        [CsvField("Department")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user's HR manager.
        /// </summary>
        [JsonIgnore]
        [CsvField("HR Mgr")]
        public string HrManagerName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user's HR manager.
        /// </summary>
        [JsonIgnore]
        [CsvField("HR Mgr Email")]
        public string HrManagerEmal { get; set; }

        /// <summary>
        /// Gets or sets the name of the user's manager.
        /// </summary>
        [JsonIgnore]
        [CsvField("ManagerName")]
        public string ManagerName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user's manager.
        /// </summary>
        [JsonIgnore]
        [CsvField("ManagerEmail")]
        public string ManagerEmail { get; set; }

        /// <summary>
        /// Gets or sets the user's cost center.
        /// </summary>
        [JsonProperty("costCenter")]
        [CsvField("Cost Center")]
        public string CostCenter { get; set; }

        /// <summary>
        /// Gets or sets the user's skin.
        /// </summary>
        [JsonIgnore]
        [CsvField("Skin")]
        public string Skin { get; set; }

        /// <summary>
        /// Gets or sets the name of the user's company.
        /// </summary>
        [JsonIgnore]
        [CsvField("CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the user's employment country.
        /// Converted to ISO 3166-1 alpha-3
        /// </summary>
        [JsonIgnore]
        [CsvField("EmploymentCountryCode")]
        public RegionInfo EmploymentCountry { get; set; }

        /// <summary>
        /// Gets or sets the user's first line of geographic address.
        /// </summary>
        [JsonProperty("address1")]
        [CsvField("Company Address 1")]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the user's second line of geographic address.
        /// </summary>
        [JsonProperty("address2")]
        [CsvField("Company Address 2")]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the user's city.
        /// </summary>
        [JsonProperty("city")]
        [CsvField("City")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the user's address state or province.
        /// </summary>
        [JsonProperty("provinceState")]
        [CsvField("Province State")]
        public string ProvinceState { get; set; }

        /// <summary>
        /// Gets or sets the user's address postal code.
        /// </summary>
        [JsonProperty("postalCodeZip")]
        [CsvField("PostalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the user's country.
        /// Converted to ISO 3166-1 alpha-3
        /// </summary>
        [JsonIgnore]
        [CsvField("Country")]
        public RegionInfo Country { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        [JsonProperty("phone")]
        [CsvField("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #1
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr1")]
        public string UserAttr1 { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #2
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr2")]
        public string UserAttr2 { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #3
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr3")]
        public string UserAttr3 { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #4
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr4")]
        public string UserAttr4 { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #5
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr5")]
        public string UserAttr5 { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #6
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr6")]
        public string UserAttr6 { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #7
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr7")]
        public string UserAttr7 { get; set; }

        /// <summary>
        /// Gets or sets the user's custom attribute #8
        /// </summary>
        [JsonIgnore]
        [CsvField("UserAttr8")]
        public string UserAttr8 { get; set; }

        /// <summary>
        /// Constructs the CSV string of multiple user details and action to perform (add/update/delete) for API calls.
        /// </summary>
        /// <param name="action">The action to perform (e.g. add/update/delete).</param>
        /// <param name="users">The user object to perform the action on.</param>
        /// <returns>CSV string of multiple user details for API calls.</returns>
        public static string GetCsv(string action, NetDimensionsUser[] users)
        {
            StringBuilder csvSb = new StringBuilder();
            if (users.Length == 0)
            {
                return string.Empty;
            }

            PropertyInfo[] properties = typeof(NetDimensionsUser).GetProperties();
            string[] csvHeaders = new string[properties.Length + 1];
            csvHeaders[0] = "Actions";
            string[][] csvData = new string[users.Length][];
            int actualHeaderIndex = 1;
            foreach (PropertyInfo property in properties)
            {
                string fieldName = property.Name;
                object[] attributes = property.GetCustomAttributes(typeof(CsvFieldAttribute), true);
                if (attributes.Length == 0)
                {
                    continue;
                }

                CsvFieldAttribute att = (CsvFieldAttribute)attributes[0];
                if ((att != null) && (!string.IsNullOrWhiteSpace(att.CsvFieldHeader)))
                {
                    fieldName = att.CsvFieldHeader;
                }

                csvHeaders[actualHeaderIndex] = fieldName;
                for (int userRow = 0; userRow < users.Length; userRow++)
                {
                    if (csvData[userRow] == null)
                    {
                        csvData[userRow] = new string[properties.Length + 1];
                        csvData[userRow][0] = action;
                    }
                   
                    object propValue = property.GetValue(users[userRow], null);
                    if (propValue != null) 
                    {
                        string valueString;
                        if (property.PropertyType == typeof(DateTime?))
                        {
                            if (((DateTime?)propValue).HasValue)
                            {
                                valueString = ((DateTime?)propValue).Value.ToString(
                                    "dd-MMM-yyyy",
                                    CultureInfo.CreateSpecificCulture("en-US"))
                                    .ToLower();
                            }
                            else
                            {
                                valueString = string.Empty;
                            }
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            valueString = ((bool)propValue) ? "Y" : "N";
                        }
                        else if (property.PropertyType == typeof(RegionInfo))
                        {
                            if ((RegionInfo)propValue != null)
                            {
                                valueString = ((RegionInfo)propValue).ThreeLetterISORegionName;
                            }
                            else
                            {
                                valueString = string.Empty;
                            }
                        }
                        else if (property.PropertyType == typeof(CultureInfo))
                        {
                            if (((CultureInfo)propValue) == null)
                            {
                                valueString = new CultureInfo("en-US").Name.Replace("-", "_");
                            }
                            else
                            {
                                valueString = ((CultureInfo)propValue).Name.Replace("-", "_");
                            }
                        }
                        else
                        {
                            if ((propValue != null) && (!string.IsNullOrWhiteSpace(propValue.ToString())))
                            {
                                valueString = "\"" + propValue.ToString() + "\"";
                            }
                            else
                            {
                                valueString = string.Empty;
                            }
                        }

                        csvData[userRow][actualHeaderIndex] = valueString;
                    }
                }

                actualHeaderIndex++;
            }

            csvSb.Append(string.Join(",", csvHeaders));
            csvSb.Append("\n");
            for (int u = 0; u < users.Length; u++)
            {
                if (u > 0)
                {
                    csvSb.Append("\n");
                }

                csvSb.Append(string.Join(",", csvData[u]));
            }

            return csvSb.ToString();
        }
    }
}