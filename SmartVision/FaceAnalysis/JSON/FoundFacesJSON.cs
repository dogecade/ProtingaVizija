using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FaceAnalysis
{
    public struct FoundFacesJSON : IApiResponseJSON
    {
        public string Request_id { get; set; }
        public int Time_used { get; set; }
        public Thresholds thresholds { get; set; }
        public IList<Result> Results { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is FoundFacesJSON))
            {
                return false;
            }

            var jSON = (FoundFacesJSON)obj;
            return Request_id == jSON.Request_id &&
                   Time_used == jSON.Time_used &&
                   EqualityComparer<Thresholds>.Default.Equals(thresholds, jSON.thresholds) &&
                   EqualityComparer<IList<Result>>.Default.Equals(Results, jSON.Results);
        }

        public override int GetHashCode()
        {
            var hashCode = 571236380;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Request_id);
            hashCode = hashCode * -1521134295 + Time_used.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Thresholds>.Default.GetHashCode(thresholds);
            hashCode = hashCode * -1521134295 + EqualityComparer<IList<Result>>.Default.GetHashCode(Results);
            return hashCode;
        }

        public static bool operator ==(FoundFacesJSON lhs, FoundFacesJSON rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(FoundFacesJSON lhs, FoundFacesJSON rhs)
        {
            return !(lhs == rhs);
        }
    }

    public struct Thresholds
    {
        [JsonProperty(PropertyName = "1e-3")]
        public double E3 { get; set; }

        [JsonProperty(PropertyName = "1e-4")]
        public double E4 { get; set; }

        [JsonProperty(PropertyName = "1e-5")]
        public double E5 { get; set; }
    }



    public struct Result
    {
        public double Confidence { get; set; }
        public string User_id { get; set; }
        public string Face_token { get; set; }
    }
}
