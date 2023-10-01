﻿using Microsoft.AspNetCore.Mvc;
using QuakeAnalyst.Analyzer;
using ApplicationCore.ApiInterfaces;
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
            if(filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            var magnitutes = await _analyzer.MagnituteAvaragesOverDay(filter);
            return Json(magnitutes);
        }

        [HttpPost]
        public async Task<JsonResult> CountAvaragesOverDay([FromBody] RequestEarthquakeFilter filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            var magnitutes = await _analyzer.CountAvaragesOverDay(filter);
            return Json(magnitutes);
        }
    }
}
