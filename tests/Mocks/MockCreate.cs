using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using PokeTrader.Core.Trader.Models;
using System;

namespace PokeTrader.Tests.Mocks
{
    internal static class MockCreate
    {
        private static Random _random = new();
        public static ITrader NullTrader()
        {
            var obj = Mock.Of<ITrader>(MockBehavior.Loose);

            var mock = Mock.Get(obj);

            mock.SetReturnsDefault<Trade>(InvalidTrade());
            mock.SetReturnsDefault<TradeInfo>(InvalidTradeInfo());
            mock.SetReturnsDefault<IEnumerable<Trade>>(Enumerable.Empty<Trade>());

            return obj;
        }

        private static TradeInfo InvalidTradeInfo()
        => new()
        {
            First = RandomTrader(),
            Second = RandomTrader(),
            TradeFairness = TradeInfo.Fairness.InvalidTrade
        };

        public static TradeParticipant RandomTrader() 
        => new()
        {
            Trader = new() { Name = RandomName() },
            TradeOffer = new[] { RandomPokemon() }
        };

        private static Pokemon RandomPokemon()
        => new()
        {
            Id = _random.Next(),
        };

        public static string RandomName() => "Random";


        private static Trade InvalidTrade()
        => new()
        {
            Info = InvalidTradeInfo(),
            TradeDate = DateTime.MinValue
        };

        public static ICollectionMeasurer<T> NullMeasure<T>()
        where T : notnull
        {
            var obj = Mock.Of<ICollectionMeasurer<T>>(MockBehavior.Loose);

            var mock = Mock.Get(obj);

            mock.SetReturnsDefault<int>(0);

            return obj;
        }
        public static IHistory<T> NullHistory<T>()
        where T : notnull
        {
            var obj = Mock.Of<IHistory<T>>(MockBehavior.Loose);

            var mock = Mock.Get(obj);

            mock.SetReturnsDefault<IEnumerable<T>>(Enumerable.Empty<T>());

            return obj;
        }

        
    }   
}