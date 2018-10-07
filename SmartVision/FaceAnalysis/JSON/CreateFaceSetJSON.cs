using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms.FaceAnalysis.JSON
{
    public class CreateFaceSetJSON
    {
        public string faceset_token { get; set; }
        public int time_used { get; set; }
        public int face_count { get; set; }
        public int face_added { get; set; }
        public string request_id { get; set; }
        public string outer_id { get; set; }
        public IList<object> failure_detail { get; set; }
    }
}
