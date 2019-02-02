using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video;
using FaceAnalysis;
using Microsoft.Extensions.Hosting;

namespace AdminWeb
{
    public class FaceProcesssingService : BackgroundService
    {
        public FaceProcessor Processor { get; }
        private readonly ConcurrentDictionary<ProcessableVideoSource, MJPEGServer> streamServers = new ConcurrentDictionary<ProcessableVideoSource, MJPEGServer>();

        public FaceProcesssingService()
        {
            //TODO: add camera properties?
            Processor = new FaceProcessor();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(async () =>
            {
                await Processor.Complete();
                foreach (var source in streamServers.Keys)
                    RemoveStream(source);
            });
        }

        public async Task<(string url, string id)> AddStreamAsync(IVideoSource source)
        {
            return await AddStreamAsync(new ProcessableVideoSource(source));
        }

        public async Task<(string url, string id)> AddStreamAsync(ProcessableVideoSource source)
        {
            var server = new MJPEGServer(source, start: true);
            streamServers[source] = server;
            await Processor.AddSourceAsync(source);
            return (server.Url, source.Id.ToString());
        }

        public void RemoveStream(string sourceId)
        {
            foreach (var source in streamServers.Keys)
                if (source.Id.ToString() == sourceId)
                {
                    RemoveStream(source);
                    return;
                }

        }

        public void RemoveStream(ProcessableVideoSource source, bool removeFromProcessor = false)
        {
            if(streamServers.TryRemove(source, out var server))
                server.Stop();
            if (removeFromProcessor)
                Processor.RemoveSource(source);
        }

        public IEnumerable<(string url, string id)> GetStreams()
        {
            return streamServers.Select(pair => (pair.Value.Url, pair.Key.Id.ToString()));
        }
    }
}
