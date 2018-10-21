using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    interface IApiResponseJSON
    {
        int Time_used { get; set; }
        string Request_id { get; set; }
    }
}
