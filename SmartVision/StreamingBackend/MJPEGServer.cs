using System;
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
        private ConcurrentDictionary<TcpClient, NewFrameEventHandler> clients = new ConcurrentDictionary<TcpClient, NewFrameEventHandler>();
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

            if (!source.Stream.IsRunning)
                source.Start();

            listen = true;
            listeningTask = Task.Run(() => Listen());
        }

        public void Stop()
        {
            foreach (var client in clients.Keys)
                client.Dispose();
            clients = new ConcurrentDictionary<TcpClient, NewFrameEventHandler>();

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
                catch (ObjectDisposedException)
                {
                    Debug.WriteLine("Stopping listening");
                    return;
                }
                Debug.WriteLine("A client connected.");
                void eventHandler(object sender, NewFrameEventArgs e) => SendNewFrameToClient(sender, e, client);
                clients[client] = eventHandler;
                source.NewFrame += eventHandler;
                await SendStringToClientAsync(client, header);
                client.GetStream().Flush();
            }
        }

        private void RemoveClient(TcpClient client)
        {
            if (clients.TryRemove(client, out var eventHandler))
            {
                source.NewFrame -= eventHandler;
                client.Dispose();
            }
        }

        private async Task SendStringToClientAsync(TcpClient client, string text)
        {
            var bytes = Encoding.ASCII.GetBytes(text);
            await client.GetStream().WriteAsync(bytes, 0, bytes.Length);
        }

        private async Task SendByteImageToClientAsync(TcpClient client, byte[] image)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(boundary);
            stringBuilder.AppendLine("Content-Type: image/jpeg");
            stringBuilder.AppendLine("Content-Length: " + image.Length.ToString());
            stringBuilder.AppendLine();

            await SendStringToClientAsync(client, stringBuilder.ToString());
            await client.GetStream().WriteAsync(image, 0, image.Length);
            await SendStringToClientAsync(client, "\r\n");
            client.GetStream().Flush();
        }

        private async void SendNewFrameToClient(object sender, NewFrameEventArgs e, TcpClient client)
        {
            byte[] byteImage;
            lock (sender)
                byteImage = HelperMethods.ImageToByte(new Bitmap(e.Frame));
            try
            {
                await SendByteImageToClientAsync(client, byteImage);
            }
            catch (Exception exception) when (exception is System.IO.IOException || exception is InvalidOperationException || exception is ArgumentException)
            {
                if (!client.Connected)
                {
                    Debug.WriteLine("Client has disconnected");
                    RemoveClient(client);
                }
            }
        }
    }
}