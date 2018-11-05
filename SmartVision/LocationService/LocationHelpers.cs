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


    }
}