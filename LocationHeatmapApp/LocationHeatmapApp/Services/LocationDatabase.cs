using SQLite;
using System.IO; // Add this namespace for Path.Combine
using System.Threading.Tasks; // Add this namespace for Task
using System.Collections.Generic; // Add this namespace for List
using LocationHeatmapApp.Models;

namespace LocationHeatmapApp.Services
{
    public class LocationDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public LocationDatabase()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Locations.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LocationPoint>().Wait();
        }

        public Task<List<LocationPoint>> GetLocationsAsync()
        {
            return _database.Table<LocationPoint>().ToListAsync();
        }

        public Task<int> SaveLocationAsync(LocationPoint point)
        {
            return _database.InsertAsync(point);
        }
    }
}