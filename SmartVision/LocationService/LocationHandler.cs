﻿using Constants;

namespace LocationService
{
    public class LocationHandler
    {
        private const string rootUrl = "https://maps.googleapis.com/maps/api/staticmap?";
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

            return rootUrl +
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

            return rootUrl +
                   "center=" + coordinates +
                   "&zoom=" + zoom +
                   "&size=" + size +
                   "&maptype=" + mapType +
                   "&markers=color:" + markerColor + "%7C" + coordinates + // %7C is necessary between parameters of marker
                   "&key=" + Keys.googleApiKey;
        }
    }
}