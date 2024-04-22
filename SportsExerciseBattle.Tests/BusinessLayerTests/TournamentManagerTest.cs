using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading.Tasks;
using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;
using Moq;
using SportsExerciseBattle.Datatypes;

namespace SportsExerciseBattle.Tests.BusinessLayerTests
{
    [TestFixture]
    public class TournamentManagerTests
    {
        private TournamentManager _tournamentManager;
        private Mock<ITournamentRepository> _mockTournamentRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IHistoryRepository> _mockHistoryRepository;

        [SetUp]
        public void Setup()
        {
            _mockTournamentRepository = new Mock<ITournamentRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockHistoryRepository = new Mock<IHistoryRepository>();
            _tournamentManager = new TournamentManager(); // Assuming a parameterless constructor exists

            // Injecting mock repositories via reflection if no public constructor is available
            typeof(TournamentManager).GetField("_tournamentRepository", BindingFlags.NonPublic | BindingFlags.Instance)
                                     ?.SetValue(_tournamentManager, _mockTournamentRepository.Object);
            typeof(TournamentManager).GetField("_userRepository", BindingFlags.NonPublic | BindingFlags.Instance)
                                     ?.SetValue(_tournamentManager, _mockUserRepository.Object);
            typeof(TournamentManager).GetField("_historyRepository", BindingFlags.NonPublic | BindingFlags.Instance)
                                     ?.SetValue(_tournamentManager, _mockHistoryRepository.Object);
        }

        [Test]
        public void StartTournament_CallsRepositoryMethod()
        {
            // Arrange
            int tournamentId = 1;
            ExerciseType exerciseType = ExerciseType.PushUp;

            // Act
            _tournamentManager.StartTournament(tournamentId, exerciseType);

            // Assert
            _mockTournamentRepository.Verify(repo => repo.StartUserTournament(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void EndTournament_CallsUpdateMethod()
        {
            // Arrange
            int tournamentId = 1;

            // Using reflection to invoke private methods
            MethodInfo startMethod = typeof(TournamentManager).GetMethod("StartTournament", BindingFlags.NonPublic | BindingFlags.Instance);
            startMethod?.Invoke(_tournamentManager, new object[] { tournamentId, ExerciseType.PushUp });

            // Act
            MethodInfo endMethod = typeof(TournamentManager).GetMethod("EndTournament", BindingFlags.NonPublic | BindingFlags.Instance);
            var endTask = (Task)endMethod?.Invoke(_tournamentManager, new object[] { tournamentId });
            endTask?.GetAwaiter().GetResult(); // Correctly await the task

            // Assert
            _mockTournamentRepository.Verify(repo => repo.UpdateTournamentAsync(It.IsAny<Tournament>()), Times.Once);
        }
    }
}
