namespace Objects.CameraProperties
{
    public class CameraProperties
    {
        public string StreamUrl { get; }
        public string StreetName { get; }
        public string HouseNumber { get; }
        public string CityName { get; }
        public string CountryName { get; }
        public string PostalCode { get; }
        public bool IsBus { get; }
        public int BusId { get; }

        public CameraProperties(string streamUrl, int busId)
        {
            StreamUrl = streamUrl;
            BusId = busId;
            IsBus = true;
        }

        public CameraProperties(string streamUrl, string streetName, string houseNumber, string cityName, string countryName, string postalCode, int busId, bool isBus)
        {
            StreamUrl = streamUrl;
            StreetName = streetName;
            HouseNumber = houseNumber;
            CityName = cityName;
            CountryName = countryName;
            PostalCode = postalCode;
            IsBus = isBus;
            BusId = busId;
        }
    }
}