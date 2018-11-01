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

        public Location(string StreetName, string StreetNumber, string CityName, string CountryName, string PostalNumber)
        {
            this.StreetName = StreetName;
            this.StreetNumber = StreetNumber;
            this.CityName = CityName;
            this.CountryName = CountryName;
            this.PostalNumber = PostalNumber;
        }
    }
}