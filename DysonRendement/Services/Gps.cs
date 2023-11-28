using DysonRendement.Models;

namespace DysonRendement.Services;

// Interface pour le GPS
public interface IGps
{
    Task<GpsModel> GetCachedLocation();
    Task<GpsModel> GetCurrentLocation();
    void CancelRequest();
}

// Classe pour le GPS qui implémente l'interface IGps et qui permet de récupérer la position GPS.
public class Gps : IGps
{
    // Propriétés
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    private GpsModel _gpsModel { get; set; }

    // Méthodes pour récupérer la dernière position connue du GPS
    public async Task<GpsModel> GetCachedLocation()
    {
        try
        {
            // Récupère la dernière position connue du GPS
            var location = await Geolocation.Default.GetLastKnownLocationAsync();
            //  Vérifie si la position est nulle
            if (location != null)
                // Vérifie si le modèle est nul
                if (_gpsModel != null)
                {
                    // Met à jour le modèle avec les coordonnées GPS
                    _gpsModel.Altitude = location.Altitude ?? 0;
                    _gpsModel.Latitude = location.Latitude;
                    _gpsModel.Longitude = location.Longitude;
                }
                else
                {
                    // Retourne un nouveau modèle avec les coordonnées GPS
                    return new GpsModel(location.Latitude, location.Longitude, location.Altitude ?? 0);
                }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // En cas d'erreur, retourne un modèle avec l'erreur
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

        // Retourne le modèle
        return _gpsModel;
    }

    // Méthode pour récupérer la position actuelle du GPS
    public async Task<GpsModel> GetCurrentLocation()
    {
        try
        {
            // Vérifie si la position est en cours de récupération
            _isCheckingLocation = true;
            // Récupère la position actuelle du GPS avec une précision moyenne et un délai de 10 secondes
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            // Initialise le jeton d'annulation
            _cancelTokenSource = new CancellationTokenSource();
            // Récupère la position actuelle du GPS avec le jeton d'annulation et la requête de la position actuelle du GPS
            var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
            // Vérifie si la position est nulle
            if (location != null)
                // Vérifie si le modèle est nul
                if (_gpsModel != null)
                {
                    // Met à jour le modèle avec les coordonnées GPS
                    _gpsModel.Altitude = location.Altitude ?? 0;
                    _gpsModel.Latitude = location.Latitude;
                    _gpsModel.Longitude = location.Longitude;
                }
                else
                {
                    // Crée un nouveau modèle avec les coordonnées GPS
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

        // Retourne le modèle
        return _gpsModel;
    }

    // Méthode pour annuler la requête de la position actuelle du GPS
    public void CancelRequest()
    {
        // Vérifie si la position est en cours de récupération et si le jeton d'annulation n'est pas nul et si la requête n'est pas annulée
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
}