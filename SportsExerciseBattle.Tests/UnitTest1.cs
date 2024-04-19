using NUnit.Framework;
using SportsExerciseBattle.Web.HTTP;  // Assuming your classes to test are in this namespace

namespace SportsExerciseBattle.Tests
{
    [TestFixture]
    public class HttpRequestTests
    {
        private HttpRequest request;

        [SetUp]
        public void Setup()
        {
            // Setup code to initialize HttpRequest object before each test
            var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("GET / HTTP/1.1\r\n\r\n")));
            request = new HttpRequest(reader);
        }

        [Test]
        public void TestHttpRequestMethodIsGet()
        {
            request.Parse();
            Assert.AreEqual(HttpMethod.GET, request.Method, "The HTTP method should be GET.");
        }
    }
}