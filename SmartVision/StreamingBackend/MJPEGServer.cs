using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using FaceAnalysis;

namespace StreamingBackend
{
    public class MJPEGServer
    {
        public int Port { get; private set; }
        private const string boundary = "--boundary";
        private const string header = "HTTP/1.1 200 OK\r\n" +
                                        "Content-Type: multipart/x-mixed-replace; boundary=" +
                                        boundary +
                                        "\r\n";
        private bool listen;
        private Task listeningTask;
        private ConcurrentDictionary<TcpClient, NetworkStream> clients = new ConcurrentDictionary<TcpClient, NetworkStream>();
        private readonly TcpListener listener;
        private readonly ProcessableVideoSource source;
        public MJPEGServer(ProcessableVideoSource source, int port = 0, bool start = false)
        {
            Port = port >= 0 ? port : 0;
            this.source = source;

            listener = new TcpListener(IPAddress.Any, Port);

            if (start)
                Start();

        }

        public void Start()
        {
            listener.Start();
            Port = int.Parse(listener.LocalEndpoint.ToString().Split(':')[1]);
            Debug.WriteLine(string.Format("Server has started on port {0}, waiting for a connection...", Port));

            source.NewFrame += SendNewFrameToClients;
            if (!source.Stream.IsRunning)
                source.Start();

            listen = true;
            listeningTask = Task.Run(() => Listen());
        }

        public async Task Stop()
        {
            foreach (var client in clients.Keys)
                client.Dispose();
            clients = new ConcurrentDictionary<TcpClient, NetworkStream>();

            source.Stop();

            listener.Stop();
            listen = false;
            await listeningTask;
        }

        private async Task Listen()
        {
            while (listen)
            {
                var client = await listener.AcceptTcpClientAsync();
                Debug.WriteLine("A client connected.");
                clients[client] = client.GetStream();
                await SendStringToClientAsync(client, header);
                client.GetStream().Flush();
            }
        }

        private bool CheckClientStatus(TcpClient client)
        {
            if (!client.Connected)
            {
                Debug.WriteLine("Client has disconnected");
                clients.TryRemove(client, out _);
                client.Dispose();
                return false;
            }
            else
                return true;
        }

        private async Task SendStringToClientAsync(TcpClient client, string text)
        {
            var bytes = Encoding.ASCII.GetBytes(text);
            try
            {
                await client.GetStream().WriteAsync(bytes, 0, bytes.Length);
            }
            catch (System.IO.IOException e)
            {
                Debug.WriteLine("An error occured when sending string to client stream: ");
                Debug.WriteLine(e);
                CheckClientStatus(client);
            }
        }

        private async Task SendImageToClientAsync(TcpClient client, Bitmap img)
        {
            var image = HelperMethods.ImageToByte(img);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(boundary);
            stringBuilder.AppendLine("Content-Type: image/jpeg");
            stringBuilder.AppendLine("Content-Length: " + image.Length.ToString());
            stringBuilder.AppendLine();

            try
            {
                await SendStringToClientAsync(client, stringBuilder.ToString());
                await client.GetStream().WriteAsync(image, 0, image.Length);
                await SendStringToClientAsync(client, "\r\n");
                client.GetStream().Flush();
            }
            catch (System.IO.IOException e)
            {
                Debug.WriteLine("An error occured when sending image to client stream: ");
                Debug.WriteLine(e);
                CheckClientStatus(client);
            }
        }

        private async void SendNewFrameToClients(object sender, NewFrameEventArgs e)
        {
            var bitmap = new Bitmap(e.Frame);
            foreach (var client in clients.Keys)
                await SendImageToClientAsync(client, new Bitmap(bitmap));
            bitmap.Dispose();
        }
    }
}