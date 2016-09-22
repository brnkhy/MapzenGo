namespace Snook.GIS
{
    public interface IGPSService
    {
        bool ActiveAndConnected { get; }
        GeoLocationCoordinate Location { get; }

        event GPSEventDelegate Changed;

        event GPSEventDelegate Connected;

        GeoLocationCoordinate GetStartLocation();
    }
}