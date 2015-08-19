using System;
using System.Linq;
using Telerik.Newtonsoft.Json;

namespace SitefinityWebApp.NetDimensions
{
    public class NetDimensionsOrganization
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("children")]
        public NetDimensionsOrganization[] Children { get; set; }

        //[JsonProperty("attributes")]
        //public string[] Attributes { get; set; }
    }
}