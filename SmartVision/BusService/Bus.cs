namespace BusService
{
    public class Bus
    {
        public BusType busType { get; }
        public string busRoute { get; }
        public string busNumber { get; }
        public double latitude { get; }
        public double longitude { get; }

        public Bus(int busType, string busNumber, double latitude, double longitude)
        {
            this.busType = (BusType)busType;
            this.busNumber = busNumber;
            this.latitude = latitude;
            this.longitude = longitude;
            busRoute = BusHelpers.GetRouteName(busNumber);
        }
    }
}