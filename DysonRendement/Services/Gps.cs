using DysonRendement.Models;

namespace DysonRendement.Services;

public interface IGps
{
    Task<GpsModel> GetCachedLocation();
    Task<GpsModel> GetCurrentLocation();
    void CancelRequest();
}

public class Gps : IGps
{
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    // ReSharper disable once InconsistentNaming
    private GpsModel _gpsModel { get; set; }

    public async Task<GpsModel> GetCachedLocation()
    {
        try
        {
            var location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                if (_gpsModel != null)
                {
                    _gpsModel.Altitude = location.Altitude ?? 0;
                    _gpsModel.Latitude = location.Latitude;
                    _gpsModel.Longitude = location.Longitude;
                }
                else
                {
                    return new GpsModel(location.Latitude, location.Longitude, location.Altitude ?? 0);
                }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            if (_gpsModel == null)
            {
                _gpsModel = new GpsModel(true, fnsEx.Message);
            }
            else
            {
                _gpsModel.Error = true;
                _gpsModel.ErrorMessage = fnsEx.Message;
            }
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
            if (_gpsModel == null)
            {
                _gpsModel = new GpsModel(true, fneEx.Message);
            }
            else
            {
                _gpsModel.Error = true;
                _gpsModel.ErrorMessage = fneEx.Message;
            }
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            if (_gpsModel == null)
            {
                _gpsModel = new GpsModel(true, pEx.Message);
            }
            else
            {
                _gpsModel.Error = true;
                _gpsModel.ErrorMessage = pEx.Message;
            }
        }
        catch (Exception ex)
        {
            // Unable to get location
            if (_gpsModel == null)
            {
                _gpsModel = new GpsModel(true, ex.Message);
            }
            else
            {
                _gpsModel.Error = true;
                _gpsModel.ErrorMessage = ex.Message;
            }
        }

        if (_gpsModel == null)
        {
            _gpsModel = new GpsModel(false, "");
        }
        else
        {
            _gpsModel.Error = false;
            _gpsModel.ErrorMessage = "";
        }

        return _gpsModel;
    }

    public async Task<GpsModel> GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
                if (_gpsModel != null)
                {
                    _gpsModel.Altitude = location.Altitude ?? 0;
                    _gpsModel.Latitude = location.Latitude;
                    _gpsModel.Longitude = location.Longitude;
                }
                else
                {
                    return new GpsModel(location.Latitude, location.Longitude, location.Altitude ?? 0);
                }
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            if (_gpsModel == null)
            {
                _gpsModel = new GpsModel(true, ex.Message);
            }
            else
            {
                _gpsModel.Error = true;
                _gpsModel.ErrorMessage = ex.Message;
            }
        }
        finally
        {
            _isCheckingLocation = false;
        }

        return _gpsModel;
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
}