using System.Drawing;
using System.Net;
using BusService;
using Constants;

namespace LocationService
{
    public class Location
    {
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string PostalNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public Location(string StreetName, string StreetNumber, string CityName, string CountryName,
            string PostalNumber)
        {
            this.StreetName = StreetName;
            this.StreetNumber = StreetNumber;
            this.CityName = CityName;
            this.CountryName = CountryName;
            this.PostalNumber = PostalNumber;
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
            this.Latitude = bus.latitude;
            this.Longitude = bus.longitude;
        }
    }
}