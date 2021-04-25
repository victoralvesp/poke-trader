using NUnit.Framework;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Tests.Mocks;

namespace PokeTrader.Tests
{
    [TestFixture]
    public class ConcurrentCachedHistoryFixture : ITradeHistoryFixture
    {
        protected override IHistory<Trade> History => MockCreate.NullHistory<Trade>();
    }
}