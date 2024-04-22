using Npgsql;
using NUnit.Framework.Legacy;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Tests.DataAccessLayerTests
{
    [TestFixture]
    public class HistoryRepositoryTests
    {
        private HistoryRepository _historyRepository;
        private NpgsqlConnection _connection;

        [SetUp]
        public void Setup()
        {
            _historyRepository = new HistoryRepository();
            // Set up your connection or mock it here
        }

        [Test]
        public async Task GetEntries_ReturnsCorrectHistoryEntries()
        {
            var username = "testuser";
            var expected = new List<HistoryEntry>()
        {
            new HistoryEntry { EntryName = "Pushup", Count = 30, DurationInSeconds = 60, Timestamp = DateTime.Now }
        };

            // Using reflection to access private methods if necessary
            MethodInfo method = typeof(HistoryRepository).GetMethod("GetEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var actual = await (Task<List<HistoryEntry>>)method.Invoke(_historyRepository, new object[] { username });

            ClassicAssert.AreEqual(expected.Count, actual.Count);
            ClassicAssert.AreEqual(expected[0].EntryName, actual[0].EntryName);
        }
    }

}
