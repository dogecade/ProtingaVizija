using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Constants;
using Helpers;
using Newtonsoft.Json;

namespace LocationService
{
    public class LocationHelpers
    {
        private const string rootUrl = "https://maps.googleapis.com/maps/api/geocode/json?";
        public static Location CoordinatesToLocation(double latitude, double longitude)
        {
            string requestUrl = String.Format(rootUrl + "latlng=" + latitude + "," + longitude + "&location_type=ROOFTOP&result_type=street_address&key=" + Keys.googleApiKey);

            var result = JsonConvert.DeserializeObject<LocationJSON>(new HttpClientWrapper().Get(requestUrl).Result);

            return new Location(result.results[0].address_components[1].long_name,
                                result.results[0].address_components[0].long_name,
                                result.results[0].address_components[2].long_name,
                                result.results[0].address_components[5].long_name,
                                result.results[0].address_components[6].long_name);
        }

        public static Bus GetBusData(int rowNumber)
        {
            var allBuses = new List<string>();
            using (var client = new WebClient())
            {
                string result = client.DownloadString("https://www.stops.lt/vilnius/gps.txt");
                allBuses = result.Split('\n').ToList();
            }

            var busData = allBuses[rowNumber - 1].Split(',');

            return new Bus(busData[1], Convert.ToDouble(busData[3])/1000000, Convert.ToDouble(busData[2])/1000000);
        }

        public static string GetRouteName(string busNumber)
        {
            var lines = Resource.routes.Split('\n');
            string result = lines.FirstOrDefault(x => x.Contains(busNumber));
            return result.Split(',')[2];
        }
    }
}