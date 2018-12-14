using BusService;

namespace LocationService
{
    public class Location
    {
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public Location(string StreetName, string houseNumber, string CityName, string CountryName,
            string postalCode)
        {
            this.StreetName = StreetName;
            this.HouseNumber = houseNumber;
            this.CityName = CityName;
            this.CountryName = CountryName;
            this.PostalCode = postalCode;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }

        public Location(double Latitude, double Longitude)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }

        public Location(Bus bus)
        {
            this.Latitude = bus.Latitude;
            this.Longitude = bus.Longitude;
        }
    }
}