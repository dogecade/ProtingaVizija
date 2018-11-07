using System.Drawing;
using System.Net;
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
            string PostalNumber, double Latitude = 0, double Longitude = 0)
        {
            this.StreetName = StreetName;
            this.StreetNumber = StreetNumber;
            this.CityName = CityName;
            this.CountryName = CountryName;
            this.PostalNumber = PostalNumber;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }
    }
}