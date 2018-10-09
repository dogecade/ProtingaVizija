using System.Collections.Generic;

namespace FaceAnalysis.JSON
{
    public class RemoveFaceJSON
    {
        public string faceset_token { get; set; }
        public int face_removed { get; set; }
        public int time_used { get; set; }
        public int face_count { get; set; }
        public string request_id { get; set; }
        public string outer_id { get; set; }
        public IList<object> failure_detail { get; set; }
    }
}