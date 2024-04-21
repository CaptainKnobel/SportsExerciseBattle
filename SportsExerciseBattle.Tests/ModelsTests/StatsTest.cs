using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.Tests.ModelsTests
{
    [TestFixture]
    public class StatsTests
    {
        [Test]
        public void Stats_DefaultValues_AreCorrect()
        {
            // Arrange
            var stats = new Stats();

            // Assert
            Assert.That(stats.Username, Is.EqualTo(""));
            Assert.That(stats.Name, Is.EqualTo(""));
            Assert.That(stats.Elo, Is.EqualTo(100));
            Assert.That(stats.Count, Is.EqualTo(0));
        }

        [Test]
        public void Stats_SetAndGetProperties_WorksCorrectly()
        {
            // Arrange
            var stats = new Stats
            {
                Username = "testUser",
                Name = "Test User",
                Elo = 1200,
                Count = 5
            };

            // Assert
            Assert.That(stats.Username, Is.EqualTo("testUser"));
            Assert.That(stats.Name, Is.EqualTo("Test User"));
            Assert.That(stats.Elo, Is.EqualTo(1200));
            Assert.That(stats.Count, Is.EqualTo(5));
        }
    }
}
