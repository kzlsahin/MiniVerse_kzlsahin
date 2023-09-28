using Microsoft.AspNetCore.Mvc;
using QuakeAnalyst.Analyzer;
using QuakeAnalyst.ApiService;
using QuakeAnalyst.MvcModels;
using QuakeAnalyst.Repo;
using System.Reflection;

namespace QuakeAnalyst.Controllers
{
    public class EarthquakeAnalysis : Controller
    {
        EarthquakeAnalyzer _analyzer;
        public EarthquakeAnalysis(EarthquakeAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> MagnituteAvaragesOverDay([FromBody] RequestEarthquakeFilter filter)
        {
            var magnitutes = await _analyzer.MagnituteAvaragesOverDay(filter);
            return Json(magnitutes);
        }
    }
}
