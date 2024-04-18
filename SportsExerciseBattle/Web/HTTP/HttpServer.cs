using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Utilities;

namespace SportsExerciseBattle.Web.HTTP
{
    public class HttpServer
    {
        public static void StartServer(int port, Router router)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            Console.WriteLine("HTTP Server started on port " + port);

            Task.Run(async () =>
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    await HandleClient(client, router);
                }
            });
        }

        private static async Task HandleClient(TcpClient client, Router router)
        {
            using (var networkStream = client.GetStream())
            using (var reader = new StreamReader(networkStream))
            using (var writer = new StreamWriter(networkStream) { AutoFlush = true })
            {
                string requestLine = await reader.ReadLineAsync();
                if (requestLine == null) return;

                var requestParts = requestLine.Split(' ');
                var method = requestParts[0];
                var url = requestParts[1];
                var headers = new StringBuilder();
                string line;
                while ((line = await reader.ReadLineAsync()) != null && line != string.Empty)
                    headers.Append(line + "\n");

                var bodyBuilder = new StringBuilder();
                while (reader.Peek() != -1)
                    bodyBuilder.Append((char)reader.Read());
                var body = bodyBuilder.ToString();

                await router.RouteRequest(writer, method, url, body);
                client.Close();
            }
        }

    }
}