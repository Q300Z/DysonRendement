namespace DysonRendement.Models;

public class ViewModel
{
    public ViewModel(CompasModel compasModel, GpsModel gpsModel, OrientationModel orientationModel)
    {
        CompasModel = compasModel;
        GpsModel = gpsModel;
        OrientationModel = orientationModel;
    }
    public CompasModel CompasModel { get; set; }
    public GpsModel GpsModel { get; set; }
    public OrientationModel OrientationModel { get; set; }
}