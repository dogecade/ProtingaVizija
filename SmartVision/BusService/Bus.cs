namespace BusService
{
    public class Bus
    {
        public BusType busType { get; }
        public string busRoute { get; }
        public string busNumber { get; }
        public double latitude { get; }
        public double longitude { get; }
        public int speed { get; }

        public Bus(int busType, string busNumber, double latitude, double longitude, int speed)
        {
            this.busType = (BusType)busType;
            this.busNumber = busNumber;
            this.latitude = latitude;
            this.longitude = longitude;
            this.speed = speed;
            busRoute = BusHelpers.GetRouteName(busNumber);
        }
    }
}