using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WindowsForms.FaceAnalysis
{
    public struct FoundFacesJSON
    {
        public string request_id { get; set; }
        public int time_used { get; set; }
        public Thresholds thresholds { get; set; }
        public IList<Result> results { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            FoundFacesJSON objectToCompare = (FoundFacesJSON) obj;

            return (request_id == objectToCompare.request_id) &&
                   (time_used == objectToCompare.time_used) &&
                   (thresholds.e3 == objectToCompare.thresholds.e3) &&
                   (thresholds.e4 == objectToCompare.thresholds.e4) &&
                   (thresholds.e5 == objectToCompare.thresholds.e5) &&
                   (results.Count == objectToCompare.results.Count);
        }
    }

    public struct Thresholds
    {
        [JsonProperty(PropertyName = "1e-3")]
        public double e3 { get; set; }

        [JsonProperty(PropertyName = "1e-4")]
        public double e4 { get; set; }

        [JsonProperty(PropertyName = "1e-5")]
        public double e5 { get; set; }
    }



    public struct Result
    {
        public double confidence { get; set; }
        public string user_id { get; set; }
        public string face_token { get; set; }
    }
}
