using ApplicationCore.Repo;

namespace ApplicationCore.ApiInterfaces
{
    public interface IEarthquakeApiService
    {
        Task<List<City>> GetCities();
        Task<List<Earthquake>> GetEarthquakes(RequestEarthquakeFilter filter);
    }
}
