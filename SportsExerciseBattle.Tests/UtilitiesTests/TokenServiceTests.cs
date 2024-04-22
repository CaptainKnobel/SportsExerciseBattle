using SportsExerciseBattle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using NUnit.Framework.Legacy;

namespace SportsExerciseBattle.Tests.UtilitiesTests
{
    [TestFixture]
    public class TokenServiceTests
    {
        [Test]
        public void GenerateToken_ValidUsername_ReturnsValidToken()
        {
            // Arrange
            string username = "testUser";

            // Act
            string token = TokenService.GenerateToken(username);

            // Assert
            ClassicAssert.IsNotNull(token);
            ClassicAssert.IsNotEmpty(token);
        }

        [Test]
        public void ValidateToken_ValidTokenAndUsername_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string token = TokenService.GenerateToken(username);

            // Act
            bool isValid = TokenService.ValidateToken(token, username);

            // Assert
            ClassicAssert.IsTrue(isValid);
        }

        [Test]
        public void ValidateToken_InvalidToken_ReturnsFalse()
        {
            // Arrange
            string invalidToken = "invalid_token";

            // Act
            bool isValid = TokenService.ValidateToken(invalidToken, "testUser");

            // Assert
            ClassicAssert.IsFalse(isValid);
        }

        [Test]
        public void ValidateTokenAndGetUsername_ValidToken_ReturnsUsername()
        {
            // Arrange
            string username = "testUser";
            string token = TokenService.GenerateToken(username);

            // Act
            string extractedUsername = TokenService.ValidateTokenAndGetUsername(token);

            // Assert
            ClassicAssert.AreEqual(username, extractedUsername);
        }

        [Test]
        public void ValidateTokenAndGetUsername_InvalidToken_ReturnsEmptyString()
        {
            // Arrange
            string invalidToken = "invalid_token";

            // Act
            string extractedUsername = TokenService.ValidateTokenAndGetUsername(invalidToken);

            // Assert
            ClassicAssert.IsEmpty(extractedUsername);
        }

        [Test]
        public void GenerateToken_ReturnsValidBase64EncodedString()
        {
            // Arrange
            string username = "testUser";
            string expectedSubstring = $"{username}:";

            // Act
            string token = TokenService.GenerateToken(username);
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            // Assert
            ClassicAssert.IsTrue(decodedToken.StartsWith(expectedSubstring), "The token should start with the username followed by a colon.");
            ClassicAssert.DoesNotThrow(() => Convert.FromBase64String(token), "Token should be a valid Base64 string.");
        }

        [Test]
        public void ValidateToken_CorrectToken_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string token = TokenService.GenerateToken(username);

            // Act
            bool isValid = TokenService.ValidateToken(token, username);

            // Assert
            ClassicAssert.IsTrue(isValid, "Token validation should pass for the correct username.");
        }

        [Test]
        public void ValidateToken_IncorrectToken_ReturnsFalse()
        {
            // Arrange
            string username = "testUser";
            string token = TokenService.GenerateToken(username);

            // Act
            bool isValid = TokenService.ValidateToken(token, "wrongUser");

            // Assert
            ClassicAssert.IsFalse(isValid, "Token validation should fail for the wrong username.");
        }

        [Test]
        public void ValidateTokenAndGetUsername_ValidToken_ReturnsCorrectUsername()
        {
            // Arrange
            string username = "testUser";
            string token = TokenService.GenerateToken(username);

            // Act
            string resultUsername = TokenService.ValidateTokenAndGetUsername(token);

            // Assert
            ClassicAssert.AreEqual(username, resultUsername, "The extracted username should match the one encoded in the token.");
        }


    }
}
