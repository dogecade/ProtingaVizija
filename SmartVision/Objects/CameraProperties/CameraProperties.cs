namespace Objects.CameraProperties
{
    public class CameraProperties
    {
        public string StreetName { get; }
        public string HouseNumber { get; }
        public string CityName { get; }
        public string CountryName { get; }
        public string PostalCode { get; }
        public bool IsBus { get; }
        public int BusId { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        public CameraProperties(int busId)
        {
            BusId = busId;
            IsBus = true;
        }

        public CameraProperties(string streetName, string houseNumber, string cityName, string countryName, string postalCode, int busId, bool isBus)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
            CityName = cityName;
            CountryName = countryName;
            PostalCode = postalCode;
            IsBus = isBus;
            BusId = busId;
            Latitude = 0;
            Longitude = 0;
        }

        public CameraProperties(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}