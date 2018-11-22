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

        public string Url { get { return "http://localhost:" + Port.ToString();  } }
        private const string boundary = "--boundary";
        private const string header = "HTTP/1.1 200 OK\r\n" +
                                        "Content-Type: multipart/x-mixed-replace; boundary=" +
                                        boundary +
                                        "\r\n";
        private bool listen;
        private Task listeningTask;
        private ConcurrentDictionary<TcpClient, bool> clients = new ConcurrentDictionary<TcpClient, bool>();
        private readonly TcpListener listener;
        private readonly ProcessableVideoSource source;
        public MJPEGServer(ProcessableVideoSource source, int port = 0, bool start = false)
        {
            this.source = source;

            listener = new TcpListener(IPAddress.Any, port >= 0 ? port : 0);

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

        public void Stop()
        {
            foreach (var client in clients.Keys)
                client.Dispose();
            clients = new ConcurrentDictionary<TcpClient, bool>();

            source.Stop();

            listener.Stop();
            listen = false;
        }

        private async Task Listen()
        {
            while (listen)
            {
                TcpClient client;
                try
                {
                    client = await listener.AcceptTcpClientAsync();
                }
                catch (System.ObjectDisposedException)
                {
                    Debug.WriteLine("Stopping listening");
                    return;
                }

                Debug.WriteLine("A client connected.");
                clients[client] = true;
                await SendStringToClientAsync(client, header);
                client.GetStream().Flush();
            }
        }

        private void CheckClientStatus(TcpClient client)
        {
            if (!client.Connected)
            {
                Debug.WriteLine("Client has disconnected");
                if (clients.TryRemove(client, out _))
                {
                    client.Dispose();
                }
            }
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
                try
                {
                    await SendImageToClientAsync(client, new Bitmap(bitmap));
                }
                catch (System.ObjectDisposedException)
                {
                    Debug.WriteLine("Client has already disconnected");
                }
                
            bitmap.Dispose();
        }
    }
}