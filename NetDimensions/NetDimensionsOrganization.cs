using Newtonsoft.Json;
using System;
using System.Linq;

namespace NetDimensionsWrapper.NetDimensions
{
    /// <summary>
    /// Represents an organization in the NetDimensions system
    /// </summary>
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