using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApplicationCore.Repo
{
    public class City
    {
        [JsonProperty("city")]
        public string Name { get; set; }
        [JsonProperty("cityCode")]
        public int CityCode { get; set; }
        [JsonProperty("population")]
        public int? Populations { get; set; }
    }
}
