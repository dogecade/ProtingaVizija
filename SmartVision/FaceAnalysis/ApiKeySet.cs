using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class ApiKeySet
    {
        public ApiKeySet(string key, string secret, string facesetTaken)
        {
            Key = key;
            Secret = secret;
            FacesetTaken = facesetTaken;
        }

        public string Key { get; }
        public string Secret { get; }
        public string FacesetToken { get; }

        public (StringContent keyContent, StringContent secretContent) GetHttpContentWithoutFaceset()
        {
            return (new StringContent(Key),
                    new StringContent(Secret));
        }
        public (StringContent keyContent, StringContent secretContent, StringContent FacesetTokenContent) GetHttpContent()
        {
            return (new StringContent(Key),
                    new StringContent(Secret),
                    new StringContent(FacesetToken));
        }
    }
}
