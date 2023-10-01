using ApplicationCore.ApiInterfaces;
using ApplicationCore.ApiInterfaces;
using ApplicationCore.Repo;
using Newtonsoft.Json;
using ApplicationCore.Repo;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrhanaydogduApiHandler
{
    public class OrhanAydogduApiHandler : IEarthquakeApiService
    {
        string _geoLocationsQueryString = "https://api.orhanaydogdu.com.tr/deprem/statics/cities";
        List<City> _geoLocations = new List<City>();
        DateTime _lastGeoLocationUpdate = DateTime.MinValue;
        TimeSpan _validUpdateSpan = new TimeSpan(1, 0, 0, 0);
        public OrhanAydogduApiHandler()
        {
            QueryGeoLocations();
        }
        public async Task<List<City>> GetCities()
        {
            bool res;
            bool isDataAvailable = _geoLocations?.Count > 0;
            bool isDataUpToDate = (DateTime.Now - _lastGeoLocationUpdate) > _validUpdateSpan;
            if (isDataAvailable && isDataUpToDate)
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
            List<Earthquake> earthquakes = new();
            DateTime from = filter.FromDay;
            DateTime to = filter.ToDay;
            while (from < to)
            {
                DateTime localTo;
                if (to - from > TimeSpan.FromDays(3))
                {
                    localTo = from + TimeSpan.FromDays(3);
                }
                else
                {
                    localTo = to;
                }
                string query = EarthquakeQueryString(from, localTo, filter.MinMagnitute, filter.MaxMagnitute);
                List<Earthquake> responseData = await QueryEarthquakeData(query);
                earthquakes.AddRange(
                    responseData.Where(x => x.Magnitude < filter.MaxMagnitute && x.Magnitude > filter.MinMagnitute).ToList()
                );
                from = localTo;
            }

            return earthquakes;
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

        private string EarthquakeQueryString(DateTime fromDate, DateTime toDate, double minMag = 4, double limit = 900)
        {

            return $"https://api.orhanaydogdu.com.tr/deprem/kandilli/archive?skip={minMag}&limit={limit}&date={fromDate.Year}-{fromDate.Month:D2}-{fromDate.Day:D2}&date_end={toDate.Year}-{toDate.Month:D2}-{toDate.Day:D2}";
        }
        private async Task<List<Earthquake>> QueryEarthquakeData(string query)        {
            HttpClient client = new HttpClient();            
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