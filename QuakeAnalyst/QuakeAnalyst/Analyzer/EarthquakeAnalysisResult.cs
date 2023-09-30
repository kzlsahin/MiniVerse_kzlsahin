using Newtonsoft.Json;

namespace QuakeAnalyst.Analyzer
{
    public class EarthquakeAnalysisResult
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<double> Content { get; set; }
        public int EarthquakeCounted { get; set; }
    }
}
