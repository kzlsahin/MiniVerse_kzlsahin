using ApplicationCore.ApiInterfaces;
using OrhanaydogduApiHandler;
using ApplicationCore.Repo;
using Microsoft.Data.Sqlite;

namespace DB_Filler
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var _apiHandler = new OrhanAydogduApiHandler();
            var filter = new RequestEarthquakeFilter();
            filter.MinMagnitute = 2;
            filter.MaxMagnitute = 16;
            filter.FromDay = new DateTime(2010, 01, 01);
            filter.ToDay = new DateTime(2010, 01, 10);
            List<Earthquake> earthquakes = new();
            earthquakes = await _apiHandler.GetEarthquakes(filter);
            fill(earthquakes);
        }

        static void fill(List<Earthquake> erthquakse)
        {
            using (var connection = new SqliteConnection("Data Source=..\\earthquake.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
        SELECT name
        FROM user
        WHERE id = $id
    ";
                //command.Parameters.AddWithValue("$id", id);

                //using (var reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        var name = reader.GetString(0);

                //        Console.WriteLine($"Hello, {name}!");
                //    }
                //}
            }
        }
    }
}