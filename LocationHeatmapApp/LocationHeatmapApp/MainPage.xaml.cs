using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using SQLite;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using LocationHeatmapApp.Models;
using LocationHeatmapApp.Services;

namespace LocationHeatmapApp;

public partial class MainPage : ContentPage
{
    private readonly IGeolocation _geolocation;
    private readonly LocationDatabase _database;
    private readonly ObservableCollection<LocationPoint> _locationPoints = new();

    public MainPage()
    {
        InitializeComponent();
        _geolocation = Geolocation.Default;
        _database = new LocationDatabase();
        LoadMap();
    }

    private async void LoadMap()
    {
        var map = new Microsoft.Maui.Controls.Maps.Map(MapSpan.FromCenterAndRadius(new Location(37.7749, -122.4194), Distance.FromMiles(5)))
        {
            IsShowingUser = true
        };

        MapContainer.Children.Add(map);
        await StartTracking(map);
    }

    private async Task StartTracking(Microsoft.Maui.Controls.Maps.Map map)
    {
        try
        {
            while (true)
            {
                var location = await _geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    var point = new LocationPoint
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.UtcNow
                    };

                    await _database.SaveLocationAsync(point);
                    _locationPoints.Add(point);
                    AddHeatPointToMap(map, point);
                }

                await Task.Delay(5000);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error in tracking: " + ex);
        }
    }

    private void AddHeatPointToMap(Microsoft.Maui.Controls.Maps.Map map, LocationPoint point)
    {
        var pin = new Pin
        {
            Location = new Location(point.Latitude, point.Longitude),
            Label = "Heat Point",
            Type = PinType.Place
        };
        map.Pins.Add(pin);
    }
}