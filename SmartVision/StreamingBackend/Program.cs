using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceAnalysis;

namespace StreamingBackend
{
    public static class MJPEGStreamManager
    {
        private static readonly ConcurrentDictionary<ProcessableVideoSource, MJPEGServer> streamServers = new ConcurrentDictionary<ProcessableVideoSource, MJPEGServer>();
        private static readonly FaceProcessor processor = new FaceProcessor();
        static void Main(string[] args)
        {
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public static string AddStream(ProcessableVideoSource source)
        {
            var server = new MJPEGServer(source);
            streamServers[source] = server;
            processor.AddSource(source);
            return "http://localhost:" + server.Port.ToString();
        }

        public static async Task RemoveStream(ProcessableVideoSource source)
        {
            streamServers.TryRemove(source, out var server);
            await server.Stop();
            processor.RemoveSource(source);
        }

        public static IEnumerable<string> GetStreamUrls()
        {
            return streamServers.Values.Select(server => "http://localhost:" + server.Port.ToString());
        }
    }
}
