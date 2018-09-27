using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms
{
    interface ICallApi
    {
       Task<string> CallApi(byte[] image);
    }
}
