using System.Collections.Generic;

namespace WindowsForms.FaceAnalysis
{
    public struct FacesetDetails
    {
        public string faceset_token { get; set; }
        public string tags { get; set; }
        public int time_used { get; set; }
        public string user_data { get; set; }
        public string display_name { get; set; }
        public IList<string> face_tokens { get; set; }
        public int face_count { get; set; }
        public string request_id { get; set; }
        public string outer_id { get; set; }
    }
}
