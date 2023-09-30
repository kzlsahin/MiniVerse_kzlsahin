

using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace QuakeAnalyst.ApiService
{
    public class RequestEarthquakeFilter
    {
        [JsonProperty("fromDay")]
        public DateTime FromDay { get; set; }

        [JsonProperty("toDay")]
        public DateTime ToDay { get; set; }

        [JsonProperty("minMagnitute")]
        public double MinMagnitute { get; set; } = 6;

        [JsonProperty("maxMagnitute")]
        public double MaxMagnitute { get; set; } = 20;

    }
}
