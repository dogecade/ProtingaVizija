using System.Drawing;
using System.Net;
using BusService;
using Constants;

namespace LocationService
{
    public class Location
    {
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string PostalNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public Location(string streetName, string houseNumber, string cityName, string countryName,string postalNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
            CityName = cityName;
            CountryName = countryName;
            PostalNumber = postalNumber;
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location(Bus bus)
        {
            Latitude = bus.Latitude;
            Longitude = bus.Longitude;
        }
    }
}