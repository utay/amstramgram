using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Core.Models;
using Data.EntityFramework;
using Data.EntityFramework.Repositories;
using Data.EntityFramework.Seed;
using Xunit;

namespace Amstramgram.Tests.Unit.Data.EntityFramework.Repositories
{
    public class DroidRepositoryShould
    {
        private readonly DroidRepository _droidRepository;
        private DbContextOptions<AmstramgramContext> _options;
        private Mock<ILogger<AmstramgramContext>> _dbLogger;
        public DroidRepositoryShould()
        {
            // Given
            _dbLogger = new Mock<ILogger<AmstramgramContext>>();
            // https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
            _options = new DbContextOptionsBuilder<AmstramgramContext>()
                .UseInMemoryDatabase(databaseName: "Amstramgram_DroidRepositoryShould")
                .Options;
            using (var context = new AmstramgramContext(_options, _dbLogger.Object))
            {
                context.EnsureSeedData();
            }
            var starWarsContext = new AmstramgramContext(_options, _dbLogger.Object);
            var repoLogger = new Mock<ILogger<DroidRepository>>();
            _droidRepository = new DroidRepository(starWarsContext, repoLogger.Object);
        }

        [Fact]
        [Trait("test", "unit")]
        public async void ReturnR2D2DroidGivenIdOf2001()
        {
            // When
            var droid = await _droidRepository.Get(2001);

            // Then
            Assert.NotNull(droid);
            Assert.Equal("R2-D2", droid.Name);
        }

        [Fact]
        [Trait("test", "unit")]
        public async void ReturnR2D2FriendsAndEpisodes()
        {
            // When
            var character = await _droidRepository.Get(2001, includes: new[] { "CharacterEpisodes.Episode", "CharacterFriends.Friend" });

            // Then
            Assert.NotNull(character);
            Assert.NotNull(character.CharacterEpisodes);
            var episodes = character.CharacterEpisodes.Select(e => e.Episode.Title);
            Assert.Equal(new[] { "NEWHOPE", "EMPIRE", "JEDI" }, episodes);
            Assert.NotNull(character.CharacterFriends);
            var friends = character.CharacterFriends.Select(e => e.Friend.Name);
            Assert.Equal(new[] { "Luke Skywalker", "Han Solo", "Leia Organa" }, friends);
        }

        [Fact]
        [Trait("test", "unit")]
        public async void AddNewDroid()
        {
            // Given
            var droid2101 = new Droid { Id = 2101, Name = "Droid2101", PrimaryFunction = "Function2101" };

            // When
            _droidRepository.Add(droid2101);
            var saved = await _droidRepository.SaveChangesAsync();

            // Then
            Assert.True(saved);
            using (var db = new AmstramgramContext(_options, _dbLogger.Object))
            {
                var droid = await db.Droids.FindAsync(2101);
                Assert.NotNull(droid);
                Assert.Equal(2101, droid.Id);
                Assert.Equal("Droid2101", droid.Name);

                // Cleanup
                db.Droids.Remove(droid);
                await db.SaveChangesAsync();
            }
        }

        [Fact]
        [Trait("test", "unit")]
        public async void UpdateExistingDroid()
        {
            // Given
            var threepio = await _droidRepository.Get(2000);
            threepio.PrimaryFunction = "Function2000";

            // When
            _droidRepository.Update(threepio);
            var saved = await _droidRepository.SaveChangesAsync();

            // Then
            Assert.True(saved);
            using (var db = new AmstramgramContext(_options, _dbLogger.Object))
            {
                var droid = await db.Droids.FindAsync(2000);
                Assert.NotNull(droid);
                Assert.Equal(2000, droid.Id);
                Assert.Equal("Function2000", droid.PrimaryFunction);

                // Cleanup
                droid.PrimaryFunction = "Protocol";
                db.Droids.Update(droid);
                await db.SaveChangesAsync();
            }
        }

        [Fact]
        [Trait("test", "unit")]
        public async void DeleteExistingDroid()
        {
            // Given
            using (var db = new AmstramgramContext(_options, _dbLogger.Object))
            {
                var droid2100 = new Droid { Id = 2100, Name = "Droid2100", PrimaryFunction = "Function2100" };
                await db.Droids.AddAsync(droid2100);
                await db.SaveChangesAsync();
            }

            // When
            _droidRepository.Delete(2100);
            var saved = await _droidRepository.SaveChangesAsync();

            // Then
            Assert.True(saved);
            using (var db = new AmstramgramContext(_options, _dbLogger.Object))
            {
                var deletedDroid = await db.Droids.FindAsync(2100);
                Assert.Null(deletedDroid);
            }
        }
    }
}
