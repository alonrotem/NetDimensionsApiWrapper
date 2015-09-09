using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NetDimensionsWrapper.NetDimensions
{
    /// <summary>
    /// Converts a JSON structure of organizations (each with its parent, up to the ROOT), to a hierarchical comma-delimited string or organization codes.
    /// </summary>
    public class JsonOrganizationConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            NetDimensionsOrganization organization = new NetDimensionsOrganization();
            StringBuilder sb = new StringBuilder();
            dynamic organizationObject = serializer.Deserialize(reader);
            organization.Id = organizationObject["id"].ToString();
            organization.Description = organizationObject["description"].ToString();

            sb.Insert(0, organizationObject["code"].ToString());
            dynamic parentOrg = organizationObject["parent"];

            while (parentOrg != null)
            {
                string parentCode = parentOrg["code"].ToString();
                if (!parentCode.ToUpper().Equals("ROOT", StringComparison.InvariantCultureIgnoreCase))
                    sb.Insert(0, parentCode + ",");
                parentOrg = parentOrg["parent"];
            }

            organization.Code = sb.ToString();
            return organization;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}