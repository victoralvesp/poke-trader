using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using PokeTrader.Core.Trader.Models;
using System;
using PokeTrader.Core.Filters.Abstractions;

namespace PokeTrader.Tests.Mocks
{
    internal static class MockCreate
    {
        private static Random _random = new();
        public static ITrader NullTrader()
        {
            var obj = Mock.Of<ITrader>(MockBehavior.Loose);

            var mock = Mock.Get(obj);

            mock.SetReturnsDefault(InvalidTrade());
            mock.SetReturnsDefault(InvalidTradeInfo());
            mock.SetReturnsDefault(Enumerable.Empty<Trade>());

            return obj;
        }

        internal static ICollectionMeasurer<T> CreateValidMeasure<T>()
        where T : notnull
        {
            var measure = NullMeasure<T>();

            var mock = Mock.Get(measure);
            mock.Name = $"{typeof(ICollectionMeasurer<>).Name} - {_random.Next(0, 100)}";

            mock.Setup(msr => msr.Measure(It.IsAny<T[]>()))
                .Returns<T[]>(tarr => tarr.Sum(t => t.ToString()?.Length ?? 0));

            return measure;
        }
        internal static IHistory<T> CreateValidHistory<T>()
        where T : notnull
        {
            var history = NullHistory<T>();

            var mock = Mock.Get(history);
            var historyList = new List<T>();
            mock.Name = $"{typeof(IHistory<>).Name} - {_random.Next(0, 100)}";
            mock.Setup(hist => hist.Add(It.IsAny<T>()))
                .Callback<T>(t => historyList.Add(t));

            mock.Setup(hist => hist.Get())
                .Returns(historyList.AsEnumerable());

            mock.Setup(hist => hist.Get(It.IsAny<IFilter<T>>()))
                .Returns<IFilter<T>>(filter => historyList.AsEnumerable().Where(t => filter.Pass(t)));

            return history;
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