using System.Linq;
using NUnit.Framework;
using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Tests.Mocks;

namespace PokeTrader.Tests
{
    /// D.1 - Returns sum of base_experience of pokemons
    [TestFixture]
    public class BaseExperienceMeasureFixture
    {
        public ICollectionMeasurer<Pokemon> Measurer { get; set; } = new BaseExperienceMeasure();
        [Test(Description = "Tests domain requirement D.1")]
        public void ReturnsSumOfPokemonBaseExperinces()
        {
            //Arrange
            var pokemons = new[]
            {
                MockCreate.RandomPokemon(),
                MockCreate.RandomPokemon(),
                MockCreate.RandomPokemon(),
                MockCreate.RandomPokemon(),
            };

            //Act
            var measure = Measurer.Measure(pokemons);

            //Assert
            var expected = pokemons.Sum(poke => poke.BaseExperience);
            Assert.AreEqual(expected, measure);
        }
        
    }
}