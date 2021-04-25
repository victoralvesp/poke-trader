using NUnit.Framework;
using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using static PokeTrader.Tests.Mocks.MockCreate;

namespace PokeTrader.Tests
{
    public class DefaultTraderFixture : ITraderFixture
    {

        private ITrader _trader = NullTrader();

        protected override ITrader Trader => _trader;

    }
}