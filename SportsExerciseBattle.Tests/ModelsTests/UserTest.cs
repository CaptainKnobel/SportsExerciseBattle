using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Tests.ModelsTests
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void User_Initialization_ShouldSetDefaultValues()
        {
            // Arrange
            var user = new User();

            // Assert
            Assert.That(user.Username, Is.EqualTo(""));
            Assert.That(user.Password, Is.EqualTo(""));
            Assert.That(user.Bio, Is.EqualTo(""));
            Assert.That(user.Image, Is.EqualTo(""));
            Assert.That(user.Name, Is.EqualTo(""));
        }

        [Test]
        public void User_SetterAndGetter_ShouldWorkCorrectly()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Password = "testpass",
                Bio = "Test Bio",
                Image = "Test Image",
                Name = "Test User"
            };

            // Assert
            Assert.That(user.Username, Is.EqualTo("testuser"));
            Assert.That(user.Password, Is.EqualTo("testpass"));
            Assert.That(user.Bio, Is.EqualTo("Test Bio"));
            Assert.That(user.Image, Is.EqualTo("Test Image"));
            Assert.That(user.Name, Is.EqualTo("Test User"));
        }
    }
}
