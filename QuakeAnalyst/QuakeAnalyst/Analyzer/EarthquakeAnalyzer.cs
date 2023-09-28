

using Microsoft.AspNetCore.Mvc;
using QuakeAnalyst.ApiService;
using QuakeAnalyst.Controllers;
using QuakeAnalyst.MvcModels;
using QuakeAnalyst.Repo;

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
        public async Task<List<double>> MagnituteAvaragesOverDay(RequestEarthquakeFilter filter)
        {
            List<double> magnitutes = new();
            List<Earthquake>  earthquakes = new();
            if (filter.FromDay != null && filter.ToDay != null)
            {
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
            }
            else
            {
                for (int i = 0; i < 24; i++)
                {
                    magnitutes.Add(0);
                }
            }
            return magnitutes;
        }
    }
}
