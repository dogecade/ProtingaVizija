using System.Threading.Tasks;

namespace WindowsForms
{
    interface ICallApi
    {
       Task<string> CallApi(byte[] image);
    }
}
