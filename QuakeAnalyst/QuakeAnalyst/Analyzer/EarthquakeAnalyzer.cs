

using Microsoft.AspNetCore.Mvc;
using QuakeAnalyst.ApiService;
using QuakeAnalyst.Controllers;
using QuakeAnalyst.Repo;
using System.Text.Json;

namespace QuakeAnalyst.Analyzer
{
    public class EarthquakeAnalyzer
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEarthquakeApiService _apiHandler;
        public EarthquakeAnalyzer(IEarthquakeApiService apiHandler, ILogger<HomeController> logger)
        {
            _apiHandler = apiHandler;
            _logger = logger;
        }
        public async Task<EarthquakeAnalysisResult> MagnituteAvaragesOverDay(RequestEarthquakeFilter filter)
        {
            EarthquakeAnalysisResult result = new();
            List<double> magnitutes = new();
            List<Earthquake> earthquakes = new();
            earthquakes = await _apiHandler.GetEarthquakes(filter);
            for (int i = 0; i < 24; i++)
            {
                double magnitute = earthquakes
                    .Where(x => x.Date.Hour == i)
                    .Select(x => x.Magnitude)
                    .DefaultIfEmpty(0)
                    .Average();
                magnitutes.Add(magnitute);
            }
            result.From = filter.FromDay;
            result.To = filter.ToDay;
            result.EarthquakeCounted = earthquakes.Count();
            result.Content = magnitutes;
            return result;
        }

        public async Task<EarthquakeAnalysisResult> CountAvaragesOverDay(RequestEarthquakeFilter filter)
        {
            EarthquakeAnalysisResult result = new();
            List<double> countAvgs = new();
            List<Earthquake> earthquakes = new();
            
            earthquakes = await _apiHandler.GetEarthquakes(filter);
            int eartquakeCount = earthquakes.Count();
            for (int i = 0; i < 24; i++)
            {
                int count = earthquakes
                    .Where(x => x.Date.Hour == i)
                    .Count();
                countAvgs.Add((double)count / (double)eartquakeCount);
            }
            result.From = filter.FromDay;
            result.To = filter.ToDay;
            result.EarthquakeCounted = eartquakeCount;
            result.Content = countAvgs;
            return result;
        }
    }
}
