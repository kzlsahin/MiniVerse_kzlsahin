using Newtonsoft.Json;

namespace ApplicationCore.Repo
{
    public class LocationProperties
    {
        [JsonProperty("ClosestCity")]
        public City ClosestCity { get; set; }
        [JsonProperty("epiCenter")]
        public City EpiCenter { get; set; }
    }
}
