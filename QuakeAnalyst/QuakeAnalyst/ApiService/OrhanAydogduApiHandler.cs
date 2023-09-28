﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using QuakeAnalyst.MvcModels;
using QuakeAnalyst.Repo;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;

namespace QuakeAnalyst.ApiService
{
    public class OrhanAydogduApiHandler : IEarthquakeApiService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        string _geoLocationsQueryString = "https://api.orhanaydogdu.com.tr/deprem/statics/cities";
        List<City> _geoLocations = new List<City>();
        DateTime _lastGeoLocationUpdate = DateTime.MinValue;
        TimeSpan _validUpdateSpan = new TimeSpan(1,0,0,0);
        public OrhanAydogduApiHandler()
        {
            QueryGeoLocations();
        }
        public async Task<List<City>> GetCities()
        {
            bool res;
            bool isDataAvailable = _geoLocations?.Count > 0;
            bool isDataUpToDate = (DateTime.Now - _lastGeoLocationUpdate) > _validUpdateSpan;
            if(isDataAvailable && isDataUpToDate)
            {
                return _geoLocations ?? new List<City>(); 
            }
            else
            {
                res = await QueryGeoLocations();
            }
            return _geoLocations ?? new List<City>();
        }

        public async Task<List<Earthquake>> GetEarthquakes(RequestEarthquakeFilter filter)
        {
            List<Earthquake> earthquakes = await QueryEarthquakeData(filter.FromDay, filter.ToDay);
            if (earthquakes.Count == 0)
                return earthquakes;
            if (filter.MaxMagnitute == double.MaxValue && filter.MinMagnitute == 0)
                return earthquakes;
            return earthquakes.Where(x => x.Magnitude < filter.MaxMagnitute && x.Magnitude > filter.MinMagnitute).ToList();
        }
        private async Task<bool> QueryGeoLocations()
        {
            HttpClient client = new HttpClient();
            try
            {
                using HttpResponseMessage response = await client.GetAsync(_geoLocationsQueryString);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                QueryResult<List<City>> qResult = JsonConvert.DeserializeObject<QueryResult<List<City>>>(responseBody);
                if (qResult is null)
                {                    
                    _logger.Info($"City list returned empty from the api endpoint. HTTP Request was : {_geoLocationsQueryString}");
                    return false;
                }
                _geoLocations = qResult.result;
                _lastGeoLocationUpdate = DateTime.Now;
                return true;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }
           
        }

        private string EarthquakeQueryString(DateTime fromDate, DateTime toDate, int minMag = 4, int limit = int.MaxValue)
        {

            return $"https://api.orhanaydogdu.com.tr/deprem/kandilli/archive?skip={minMag}&limit={limit}&date={fromDate.Year}-{fromDate.Month:D2}-{fromDate.Day:D2}&date_end={toDate.Year}-{toDate.Month:D2}-{toDate.Day:D2}";
        }
        private async Task<List<Earthquake>> QueryEarthquakeData(DateTime? fromDate, DateTime? toDate)
        {
            if(fromDate == null || toDate == null)
            {
                return new List<Earthquake>();
            }
            HttpClient client = new HttpClient();
            string query = EarthquakeQueryString((DateTime)fromDate, (DateTime)toDate);
            using HttpResponseMessage response = await client.GetAsync(query);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var qResult = JsonConvert.DeserializeObject<QueryResult<List<Earthquake>>>(responseBody);
            return qResult.result;
        }
        class QueryResult<T>
        {
            public bool status;
            public int httpStatus;
            public int serverloadms;
            public string desc;
            public T result;
        }
    }
}
