using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Repo;
using ApplicationCore.ApiInterfaces;
using QuakeAnalyst.MvcModels;
using System.Diagnostics;

namespace QuakeAnalyst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEarthquakeApiService _apiHandler;

        public HomeController(ILogger<HomeController> logger, IEarthquakeApiService ApiHandler)
        {
            _logger = logger;
            _apiHandler = ApiHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> GeoLocations()
        {
            var model = new GeoLocationsModel();
            model.GeoLocations = await _apiHandler.GetCities();
            return View(model);
        }

        //public ActionResult EarthQuakes()
        //{
        //    var model = new EarthquakesModel();
        //    return View(model);
        //}


        public async Task<IActionResult> EarthQuakes(string fromDay, string toDay)
        {            
            var model = new EarthquakesModel();
            if(fromDay != null && toDay != null)
            {
                var from = DateTime.Parse(fromDay);
                var to = DateTime.Parse(toDay);

                model.Earthquakes = await _apiHandler.GetEarthquakes(new RequestEarthquakeFilter { FromDay = from, ToDay = to});
            }            
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}