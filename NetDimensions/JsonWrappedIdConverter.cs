using System;
using System.Linq;
using Telerik.Newtonsoft.Json;

namespace SitefinityWebApp.NetDimensions
{
    /// <summary>
    /// Extracts the ID string out of a JSON structure of an object with an "id" field
    /// </summary>
    public class JsonWrappedIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic mainIdContainer = serializer.Deserialize(reader);
            dynamic idProperty = mainIdContainer["id"];
            return idProperty.ToString();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}