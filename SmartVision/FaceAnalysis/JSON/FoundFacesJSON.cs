using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FaceAnalysis
{
    public class FoundFacesJSON : IApiResponseJSON
    {
        public string Request_id { get; set; }
        public int Time_used { get; set; }
        public Thresholds Thresholds { get; set; }
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
                   EqualityComparer<Thresholds>.Default.Equals(Thresholds, jSON.Thresholds) &&
                   EqualityComparer<IList<Result>>.Default.Equals(Results, jSON.Results);
        }

        public override int GetHashCode()
        {
            var hashCode = 571236380;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Request_id);
            hashCode = hashCode * -1521134295 + Time_used.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Thresholds>.Default.GetHashCode(Thresholds);
            hashCode = hashCode * -1521134295 + EqualityComparer<IList<Result>>.Default.GetHashCode(Results);
            return hashCode;
        }

        public static bool operator ==(FoundFacesJSON lhs, FoundFacesJSON rhs)
        {
            if (lhs is null)
                return rhs is null;           
            return lhs.Equals(rhs);
        }

        public static bool operator !=(FoundFacesJSON lhs, FoundFacesJSON rhs)
        {
            return !(lhs == rhs);
        }

        public IEnumerable<LikelinessResult> LikelinessConfidences()
        {
            if (Results == null)
                yield break;
            foreach (Result result in Results)
            {
                LikelinessResult likelinessResult = default(LikelinessResult);
                likelinessResult.FaceToken = result.Face_token;
                if (result.Confidence < Thresholds.E3)
                    likelinessResult.Confidence = LikelinessConfidence.LowProbability;
                else if (result.Confidence < Thresholds.E4)
                    likelinessResult.Confidence = LikelinessConfidence.NormalProbability;
                else if (result.Confidence < Thresholds.E5)
                    likelinessResult.Confidence = LikelinessConfidence.HighProbability;
                else
                    likelinessResult.Confidence = LikelinessConfidence.VeryHighProbability;
                yield return likelinessResult;
            }
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

    public struct LikelinessResult
    {
        public LikelinessConfidence Confidence { get; set; }
        public string FaceToken { get; set; }
    }

    public struct Result
    {
        public double Confidence { get; set; }
        public string User_id { get; set; }
        public string Face_token { get; set; }
    }
}
