using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;

namespace ApplicationCore.Repo
{
    public class Geolocation
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; } = new double[2];
    }
}

