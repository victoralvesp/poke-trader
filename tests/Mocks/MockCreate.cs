using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using PokeTrader.Core.Trader.Models;
using System;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Repositories.Abstractions;
using System.Threading.Tasks;

namespace PokeTrader.Tests.Mocks
{
    internal static class MockCreate
    {
        private static readonly Random _random = new();
        public static ITrader NullTrader()
        {
            var obj = Mock.Of<ITrader>(MockBehavior.Loose);

            var mock = Mock.Get(obj);

            mock.SetReturnsDefault(InvalidTrade());
            mock.SetReturnsDefault(InvalidTradeInfo());
            mock.SetReturnsDefault(Enumerable.Empty<Trade>());

            return obj;
        }

        internal static IHistoryRepository<U> ValidHistoryRepo<U>()
            where U : notnull
        {
            var repo = Mock.Of<IHistoryRepository<U>>();
            var list = new List<U>();
            var mock = Mock.Get(repo);


            mock.Setup(rp => rp.Get())
                .Returns(list.AsEnumerable());
            mock.Setup(rp => rp.Add(It.IsAny<U>()))
                .Callback<U>(u => list.Add(u))
                ;
    
            return repo;
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

        internal static IFilter<T> Filter<T>(Func<T, bool>? filterFunc = null)
        where T : notnull
        {
            filterFunc ??= (_) => _random.Next(1, 10) <= 5;

            var filter = Mock.Of<IFilter<T>>();

            Mock.Get(filter).Setup(flt => flt.Pass(It.IsAny<T>()))
                .Returns(filterFunc);

            return filter;
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

        private static TradeInfo RandomTradeInfo()
        => new()
        {
            First = RandomTrader(),
            Second = RandomTrader(),
            TradeFairness = (TradeInfo.Fairness)_random.Next(0, 5)
        };

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
            Trader = new() { Name = RandomString() },
            TradeOffers = new[] { RandomPokemon() }
        };

        public static Pokemon RandomPokemon()
        => new()
        {
            Id = _random.Next(),
            BaseExperience = _random.Next(0, 100)
        };

        public static string RandomString() => "Random";


        private static Trade InvalidTrade()
        => new()
        {
            Info = InvalidTradeInfo(),
            TradeDate = DateTime.MinValue
        };
        public static Trade RandomTrade()
        => new()
        {
            Info = RandomTradeInfo(),
            TradeDate = DateTime.FromBinary(_random.Next())
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

            mock.SetReturnsDefault(Enumerable.Empty<T>());

            return obj;
        }

        public static T Random<T>()
        where T : class
        {
            var mock = new Mock<T>();
            //var byteBuffer = new byte[10];
            var value = _random.Next();
            mock.SetReturnsDefault(value);
            mock.SetReturnsDefault(_random.NextDouble());
            // mock.SetReturnsDefault(_random.NextBytes(byteBuffer));
            mock.SetReturnsDefault(RandomString());
            mock.SetReturnsDefault(DateTime.FromBinary(_random.Next()));

            return mock.Object;
        }

        



    }
}