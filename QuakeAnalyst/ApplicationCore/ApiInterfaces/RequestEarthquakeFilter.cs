using Newtonsoft.Json;

namespace ApplicationCore.ApiInterfaces
{
    public class RequestEarthquakeFilter
    {
        [JsonProperty("fromDay")]
        public DateTime FromDay { get; set; }

        [JsonProperty("toDay")]
        public DateTime ToDay { get; set; }

        [JsonProperty("minMagnitute")]
        public double MinMagnitute { get; set; } = 4;

        [JsonProperty("maxMagnitute")]
        public double MaxMagnitute { get; set; } = 20;

    }
}
