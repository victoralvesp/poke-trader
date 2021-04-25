using System;
using System.Threading.Tasks;
using NUnit.Framework;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Tests.Mocks;

namespace PokeTrader.Tests
{
    /// T.3 - History is the same as repo after cacheTime 
    [TestFixture]
    public class ConcurrentCachedHistoryFixture : ITradeHistoryFixture
    {
        IHistory<Trade> _history = new ConcurrentCachedHistory<Trade>(MockCreate.ValidHistoryRepo<Trade>());
        protected override IHistory<Trade> History => _history;

        
        [Test(Description = "Tests technical requirement T.3")]
        public async Task HistoryIsSynchronizedAfterCacheTime()
        {
            //Arrange
            var repo = MockCreate.ValidHistoryRepo<Trade>();
            _history = new ConcurrentCachedHistory<Trade>(repo);
            var trade = MockCreate.RandomTrade();
            var outoflocationTrade = MockCreate.RandomTrade();

            _ = repo.Add(outoflocationTrade);
            _history.Add(trade);

            await Task.Delay(TimeSpan.FromSeconds(2));
            //await Task.Delay(TimeSpan.FromMilliseconds(50));

            var expected = await repo.Get();
            var actual = _history.Get();
            CollectionAssert.AreEquivalent(expected, actual);
        } 
    }
}