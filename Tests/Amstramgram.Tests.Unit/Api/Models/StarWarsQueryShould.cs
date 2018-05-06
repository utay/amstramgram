using AutoMapper;
using Moq;
using Api.Models;
using Core.Data;
using Core.Logic;
using Xunit;

namespace Amstramgram.Tests.Unit.Api.Models
{
    public class AmstramgramQueryShould
    {
        [Fact]
        [Trait("test", "unit")]
        public void HaveHeroField()
        {
            // Given
            var trilogyHeroes = new Mock<ITrilogyHeroes>();
            var droidRepository = new Mock<IDroidRepository>();
            var humanRepository = new Mock<IHumanRepository>();
            var mapper = new Mock<IMapper>();

            // When
            var starWarsQuery = new AmstramgramQuery(trilogyHeroes.Object, droidRepository.Object, humanRepository.Object, mapper.Object);            

            // Then
            Assert.NotNull(starWarsQuery);
            Assert.True(starWarsQuery.HasField("hero"));
        }
    }
}
