namespace BusService
{
    public class Bus
    {
        public BusType busType { get; }
        public string busRoute { get; }
        public string busNumber { get; }
        public long routeId { get; }
        public int busId { get; }
        public double latitude { get; }
        public double longitude { get; }
        public int speed { get; }

        public Bus(string busType, string busNumber, long routeId, int busId, double latitude, double longitude, int speed)
        {
            if (busType == "Autobusai")
                this.busType = BusType.Bus;

            else
                this.busType = BusType.Trolley;

            this.busNumber = busNumber;
            this.routeId = routeId;
            this.busId = busId;
            this.latitude = latitude;
            this.longitude = longitude;
            this.speed = speed;
            busRoute = BusHelpers.GetRouteName(busNumber);
        }
    }
}