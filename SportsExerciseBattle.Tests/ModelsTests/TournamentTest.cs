using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Tests.ModelsTests
{
    public class TournamentTest
    {
        [Test]
        public void Tournament_Singleton_ShouldBeInitializedCorrectly()
        {
            // Arrange
            var expectedStartTime = DateTime.MinValue; // Angenommen, der Standardwert für StartTime ist DateTime.MinValue
            var expectedIsRunning = false;
            var expectedParticipants = new List<string>();
            var expectedLeadingUsers = new List<string>();

            // Act
            var tournament = Tournament.Instance;

            // Assert
            Assert.That(tournament, Is.Not.Null, "Tournament instance should not be null.");
            Assert.That(tournament.StartTime, Is.EqualTo(expectedStartTime), "StartTime should be initialized to the default value.");
            Assert.That(tournament.IsRunning, Is.EqualTo(expectedIsRunning), "IsRunning should be initialized to false.");
            Assert.That(tournament.Participants, Is.EquivalentTo(expectedParticipants), "Participants should be initialized to an empty list.");
            Assert.That(tournament.LeadingUsers, Is.EquivalentTo(expectedLeadingUsers), "LeadingUsers should be initialized to an empty list.");
        }

        [Test]
        public void Tournament_Singleton_ShouldBeUnique()
        {
            // Arrange
            var tournament1 = Tournament.Instance;

            // Act
            var tournament2 = Tournament.Instance;

            // Assert
            Assert.That(tournament1, Is.SameAs(tournament2), "Tournament instance should be the same (singleton).");
        }
    }
}

