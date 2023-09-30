using QuakeAnalyst.Repo;

namespace QuakeAnalyst.ApiService
{
    public interface IEarthquakeApiService
    {
        Task<List<City>> GetCities();
        Task<List<Earthquake>> GetEarthquakes(RequestEarthquakeFilter filter);
    }
}
