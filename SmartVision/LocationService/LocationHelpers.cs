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
        private const string mapRootUrl = "https://maps.googleapis.com/maps/api/staticmap?";
        private const string geocodingRootUrl = "https://maps.googleapis.com/maps/api/geocode/json?";

        private const string zoom = "16";
        private const string size = "1920x1080";
        private const string mapType = "hybrid"; // roadmap / satellite / hybrid / terrain
        private const string markerColor = "red";

        /// <summary>
        /// Forms an URL of map picture
        /// </summary>
        /// <param name="location">Camera location</param>
        /// <returns>Link of image</returns>
        public static string CreateLocationPictureFromAddress(Location location)
        {
            string address = location.StreetName + "+" + location.StreetNumber + "+" + location.CityName +
                             "+" + location.CountryName + "+" + location.PostalNumber;

            return mapRootUrl +
                   "center=" + address +
                   "&zoom=" + zoom +
                   "&size=" + size +
                   "&maptype=" + mapType +
                   "&markers=color:" + markerColor + "%7C" + address + // %7C is necessary between parameters of marker
                   "&key=" + Keys.googleApiKey;
        }

        /// <summary>
        /// Forms an URL of map picture
        /// </summary>
        /// <param name="location">Camera location</param>
        /// <returns>Link of image</returns>
        public static string CreateLocationPictureFromCoordinates(Location location)
        {
            string coordinates = location.Latitude + "," + location.Longitude;
            var x = Keys.googleApiKey;

            return mapRootUrl +
                   "center=" + coordinates +
                   "&zoom=" + zoom +
                   "&size=" + size +
                   "&maptype=" + mapType +
                   "&markers=color:" + markerColor + "%7C" + coordinates + // %7C is necessary between parameters of marker
                   "&key=" + Keys.googleApiKey;
        }

        /// <summary>
        /// Returns approximate address from latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static Location CoordinatesToLocation(double latitude, double longitude)
        {
            string requestUrl = String.Format(geocodingRootUrl + "latlng=" + latitude + "," + longitude + "&location_type=ROOFTOP&result_type=street_address&key=" + Keys.googleApiKey);

            var result = JsonConvert.DeserializeObject<LocationJSON>(new HttpClientWrapper().Get(requestUrl).Result);

            return new Location(result.results[0].address_components[1].long_name,
                                result.results[0].address_components[0].long_name,
                                result.results[0].address_components[2].long_name,
                                result.results[0].address_components[5].long_name,
                                result.results[0].address_components[6].long_name);
        }


    }
}