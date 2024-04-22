/*
namespace SportsExerciseBattle.Tests
{
    [TestFixture]
    public class HttpRequestTests
    {
        [Test]
        public void TestHttpRequestParsing()
        {
            var requestText = "GET /home?user=abc HTTP/1.1\r\n" +
                              "Host: localhost\r\n" +
                              "Content-Length: 0\r\n" +
                              "\r\n";
            using var stream = new MemoryStream(Encoding.ASCII.GetBytes(requestText));
            using var reader = new StreamReader(stream);

            var request = new HttpRequest(reader);
            request.Parse();

            Assert.AreEqual(HttpMethod.GET, request.Method);
            Assert.AreEqual("/home", request.Path[1]);
            Assert.AreEqual("abc", request.QueryParams["user"]);
            Assert.AreEqual("HTTP/1.1", request.HttpVersion);
            Assert.AreEqual("localhost", request.Headers["Host"]);
            Assert.AreEqual(0, request.Content?.Length);
        }
    }

    [TestFixture]
    public class HttpResponseTests
    {
        [Test]
        public void TestHttpResponseSending()
        {
            var responseText = new StringBuilder();
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            var response = new HttpResponse(writer);
            response.SetSuccess("OK", 200);
            response.Content = "Hello, world!";
            response.Headers["Content-Type"] = "text/plain";
            response.Send();

            stream.Position = 0; // Reset stream position to read from beginning
            using var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();

            var expectedResponse = "HTTP/1.0 200 OK\r\nContent-Length: 13\r\nContent-Type: text/plain\r\n\r\nHello, world!";
            Assert.AreEqual(expectedResponse, result.Replace("\r\n", "\n").Trim());
        }
    }
}
*/