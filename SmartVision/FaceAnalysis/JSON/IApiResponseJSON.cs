using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    interface IApiResponseJSON
    {
        int time_used { get; set; }
        string request_id { get; set; }
    }
}
