using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SportsExerciseBattle.Tests.BusinessLayerTests
{
    [TestFixture]
    public class TournamentManagerTests
    {
        private TournamentManager _tournamentManager;
        private Mock<TournamentDAO> _mockTournamentDAO;

        [SetUp]
        public void Setup()
        {
            // Create a mock TournamentDAO
            _mockTournamentDAO = new Mock<TournamentDAO>();

            _tournamentManager = new TournamentManager();
        }

        [Test]
        public void StartTournament_ShouldSetIsRunningToTrue()
        {
            // Act
            _tournamentManager.StartTournament();

            // Assert
            Assert.That(Tournament.Instance.IsRunning, Is.True);
        }

        [Test]
        public void StartTournament_ShouldStartTimer()
        {
            // Act
            _tournamentManager.StartTournament();

            // Assert
            Assert.That(_tournamentManager.TournamentTimer.Enabled, Is.True);
        }
    }
}
