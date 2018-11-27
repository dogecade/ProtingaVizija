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

        public CameraProperties(int busId)
        {
            BusId = busId;
            IsBus = true;
        }

        public CameraProperties(string streetName, string houseNumber, string cityName, string countryName, string postalCode, bool isBus, int busId = 0)
        {
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