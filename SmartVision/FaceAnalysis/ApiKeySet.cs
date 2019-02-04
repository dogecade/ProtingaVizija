using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class ApiKeySet
    {
        public ApiKeySet(string key, string secret, string facesetToken)
        {
            Key = key;
            Secret = secret;
            FacesetToken = facesetToken;
        }

        public string Key { get; }
        public string Secret { get; }
        public string FacesetToken { get; }

        public (KeyValuePair<string, string> keyPair, KeyValuePair<string, string> secretPair) GetKeyValuePairsWithoutFaceset()
        {
            return (new KeyValuePair<string, string>("api_key", Key),
                    new KeyValuePair<string, string>("api_secret", Secret));
        }
        public (KeyValuePair<string, string> keyPair, KeyValuePair<string, string> secretPair, KeyValuePair<string, string> facesetTokenPair) GetKeyValuePairs()
        {
            return (new KeyValuePair<string, string>("api_key", Key),
                    new KeyValuePair<string, string>("api_secret", Secret),
                    new KeyValuePair<string, string>("faceset_token", FacesetToken));
        }
    }
}
