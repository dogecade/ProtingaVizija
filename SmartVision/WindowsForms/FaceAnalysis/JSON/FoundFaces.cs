using System.Collections.Generic;
using Newtonsoft.Json;

namespace WindowsForms.FaceAnalysis
{
    public struct FoundFaces
    {
        public string request_id { get; set; }
        public int time_used { get; set; }
        public Thresholds thresholds { get; set; }
        public IList<Result> results { get; set; }
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
