using DysonRendement.Models;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace DysonRendement.Services;

public interface IGps
{
    Task<GpsModel> GetCachedLocation();
    Task<GpsModel> GetCurrentLocation();
    void CancelRequest();
}

public class Gps : IGps
{
    public async Task<GpsModel> GetCachedLocation()
    {
        try
        {
            var location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                // return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                return new GpsModel(location.Latitude, location.Longitude, location.Altitude ?? 0);
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            return new GpsModel(true, fnsEx.Message);
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
            return new GpsModel(true, fneEx.Message);
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            return new GpsModel(true, pEx.Message);
        }
        catch (Exception ex)
        {
            // Unable to get location
            return new GpsModel(true, ex.Message);
        }

        return new GpsModel(false, "");
    }

    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    public async Task<GpsModel> GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
                return new GpsModel(location.Latitude, location.Longitude, location.Altitude ?? 0);
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
            return new GpsModel(true, ex.Message);
        }
        finally
        {
            _isCheckingLocation = false;
        }

        return new GpsModel(false, "");
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
}