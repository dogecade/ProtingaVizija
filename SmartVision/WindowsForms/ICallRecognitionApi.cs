using System.Threading.Tasks;

namespace WindowsForms
{
    interface ICallRecognitionApi
    {
       Task<string> CallApi(byte[] image);
    }
}
