using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Objects.CameraProperties;

namespace AdminWeb.Models
{
    public class CameraPropertiesModel
    {
        public string StreamUrl { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public bool IsBus { get; set; }
        public int BusId { get; set; }

        public static implicit operator CameraProperties(CameraPropertiesModel model)
        {
            return new CameraProperties
            (
                streamUrl: model.StreamUrl,
                busId: model.BusId,
                cityName: model.CityName,
                countryName: model.CountryName,
                postalCode: model.PostalCode,
                isBus: model.IsBus,
                houseNumber: model.HouseNumber,
                streetName: model.StreetName
            );
        }
    }
}