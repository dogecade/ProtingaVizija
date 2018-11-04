namespace LocationService
{
    public class Bus
    {
        private string busRoute;
        private string busNumber;
        private double latitude;
        private double longitude;

        public Bus(string busNumber, double latitude, double longitude)
        {
            this.busNumber = busNumber;
            this.latitude = latitude;
            this.longitude = longitude;
            busRoute = LocationHelpers.GetRouteName(busNumber);
        }
    }
}