using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
//using ClassicAssert = NUnit.Framework.Assert;
using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.Tests.BusinessLayerTests
{
    [TestFixture]
    public class TournamentManagerTests
    {
        private TournamentManager manager;
        private Mock<ITournamentRepository> mockRepository;

        [SetUp]
        public void Setup()
        {
            // Create a mock of ITournamentRepository
            mockRepository = new Mock<ITournamentRepository>();
            // Set up the mock to return a default list of participants when GetParticipants is called
            mockRepository.Setup(repo => repo.GetParticipants(It.IsAny<Tournament>()))
                          .Returns(new List<string> { "user1", "user2" });

            // Initialize the TournamentManager with the mocked repository
            manager = new TournamentManager(mockRepository.Object);
        }

        [Test]
        public void TestStartTournament_SetsIsRunningTrue()
        {
            // Act: Start the tournament
            manager.StartTournament();

            // ClassicAssert: Check if the tournament is set to running
            ClassicAssert.IsTrue(Tournament.Instance.IsRunning);
        }

        [Test]
        public void TestEndTournament_SetsIsRunningFalse()
        {
            // Prepare: Start the tournament first
            manager.StartTournament();
            // Act: End the tournament
            manager.EndTournament();

            // ClassicAssert: Tournament should no longer be running
            ClassicAssert.IsFalse(Tournament.Instance.IsRunning);
        }

        [Test]
        public void TestStartTournament_InitializesParticipantsCorrectly()
        {
            // Act: Start the tournament
            manager.StartTournament();

            // ClassicAssert: Validate that participants were initialized correctly
            ClassicAssert.AreEqual(2, Tournament.Instance.Participants.Count);
            ClassicAssert.Contains("user1", Tournament.Instance.Participants);
            ClassicAssert.Contains("user2", Tournament.Instance.Participants);
        }

        [Test]
        public void TestEndTournament_CallsUpdateElo()
        {
            // Arrange: Setup the mock to expect a call to UpdateElo with a specific parameter
            mockRepository.Setup(repo => repo.UpdateElo(It.IsAny<Tournament>(), It.IsAny<bool>()));

            // Act: End the tournament
            manager.EndTournament();

            // ClassicAssert: Verify that UpdateElo was called once
            mockRepository.Verify(repo => repo.UpdateElo(It.IsAny<Tournament>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
