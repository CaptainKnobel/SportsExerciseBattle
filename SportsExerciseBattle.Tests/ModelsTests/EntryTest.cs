using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.Tests.ModelsTests
{
    [TestFixture]
    public class EntryTests
    {
        [Test]
        public void EntryProperties_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var expectedUsername = "testUser";
            var expectedEntryName = "Test Entry";
            var expectedCount = 5;
            var expectedDurationInSeconds = 300;
            var expectedTimestamp = DateTime.Now;

            // Act
            var entry = new Entry
            {
                Username = expectedUsername,
                EntryName = expectedEntryName,
                Count = expectedCount,
                DurationInSeconds = expectedDurationInSeconds,
                Timestamp = expectedTimestamp
            };

            // Assert
            Assert.That(entry.Username, Is.EqualTo(expectedUsername));
            Assert.That(entry.EntryName, Is.EqualTo(expectedEntryName));
            Assert.That(entry.Count, Is.EqualTo(expectedCount));
            Assert.That(entry.DurationInSeconds, Is.EqualTo(expectedDurationInSeconds));
            Assert.That(entry.Timestamp, Is.EqualTo(expectedTimestamp).Within(TimeSpan.FromSeconds(1)), "Timestamps should be equal within a second.");
        }
    }
}
