using NUnit.Framework;
using NUnit.Framework.Legacy;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.Endpoints;
using SportsExerciseBattle.Web.HTTP;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace SportsExerciseBattle.Tests.Endpoints
{
    /*
    [TestFixture]
    public class UsersEndpointTests
    {
        private UsersEndpoint usersEndpoint;

        [SetUp]
        public void SetUp()
        {
            usersEndpoint = new UsersEndpoint();
        }

        [Test]
        public void GetUserData_ValidRequest_ReturnsUserData()
        {
            // Arrange
            var request = new HttpRequest();
            request.Path = new List<string> { "testUser" }; // Assuming "testUser" exists in the system
            var response = new HttpResponse();

            // Act
            bool result = InvokePrivateMethod("GetUserData", request, response);

            // Assert
            ClassicAssert.IsTrue(result);
            ClassicAssert.AreEqual(200, response.ResponseCode);

            // Deserialize the response content to verify the user data
            var userJson = response.Content;
            var user = JsonSerializer.Deserialize<User>(userJson);
            ClassicAssert.IsNotNull(user);
            ClassicAssert.AreEqual("testUser", user.Username); // Assuming the user has username "testUser"
        }

        // Helper method to invoke private methods using reflection
        private bool InvokePrivateMethod(string methodName, params object[] parameters)
        {
            MethodInfo methodInfo = typeof(UsersEndpoint).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)methodInfo.Invoke(usersEndpoint, parameters);
        }
    }
    */
}
