using System;
using NUnit.Framework;
using PokeTrader.Core;
using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Tests.Mocks;
using static PokeTrader.Tests.Mocks.MockCreate;

namespace PokeTrader.Tests
{

    // H.1 - IHistory always returns non null values
    // H.2 - IHistory persists history values
    // H.3 - ICollectionMeasurer always returns non null values 
    [TestFixture]
    public class DefaultTraderFixture : ITraderFixture
    {

        private ITrader? _trader;
        IHistory<Trade> _history = NullHistory<Trade>();
        ICollectionMeasurer<Pokemon> _measurer = NullMeasure<Pokemon>();

        private ITrader CreateTrader()
        {
            _history = CreateValidHistory<Trade>();
            _measurer = CreateValidMeasure<Pokemon>();
            return new DefaultTrader(_measurer, _history)
            {
                FairnessTierSize = Constants.DEFAULT_MEASURE_TIER
            };
        }

        protected override ITrader Trader
        {
            get
            {
                _trader ??= CreateTrader();
                return _trader;
            }
        }
    }
}