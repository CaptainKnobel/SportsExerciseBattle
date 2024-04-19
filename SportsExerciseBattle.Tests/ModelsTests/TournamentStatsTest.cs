using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Tests.ModelsTests
{
    [TestFixture]
    public class TournamentStatsTests
    {
        [Test]
        public void TournamentStats_DefaultValues_AreCorrect()
        {
            // Arrange
            var stats = new TournamentStats();

            // Assert
            Assert.That(stats.Wins, Is.EqualTo(0));
            Assert.That(stats.Draws, Is.EqualTo(0));
            Assert.That(stats.Losses, Is.EqualTo(0));
        }

        [Test]
        public void TournamentStats_SetAndGetProperties_WorksCorrectly()
        {
            // Arrange
            var stats = new TournamentStats
            {
                Wins = 5,
                Draws = 2,
                Losses = 3
            };

            // Assert
            Assert.That(stats.Wins, Is.EqualTo(5));
            Assert.That(stats.Draws, Is.EqualTo(2));
            Assert.That(stats.Losses, Is.EqualTo(3));
        }
    }
}
