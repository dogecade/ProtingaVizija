using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video;
using FaceAnalysis;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace AdminWeb
{
    public class FaceProcessingService : BackgroundService, IFaceProcessingService
    {
        private IHubContext<FaceDetectedAlertHub> hubContext;
        public FaceProcessor Processor { get; }
        private readonly ConcurrentDictionary<ProcessableVideoSource, MJPEGServer> streamServers = new ConcurrentDictionary<ProcessableVideoSource, MJPEGServer>();

        public FaceProcessingService(IHubContext<FaceDetectedAlertHub> hubContext)
        {
            this.hubContext = hubContext;
            Processor = new FaceProcessor();
            Processor.FacesDetected += HandleFacesDetectedEvent;
        }

        private async void HandleFacesDetectedEvent(object sender, FacesDetectedEventArgs e)
        {
            await hubContext.Clients.All.SendAsync("FacesDetected", e.Sources.Select(source => source.Id.ToString()));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(async () =>
            {
                await Processor.Complete();
                foreach (var source in streamServers.Keys)
                    RemoveStream(source);
                Processor.FacesDetected -= HandleFacesDetectedEvent;
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
                    RemoveStream(source, removeFromProcessor: true);
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
