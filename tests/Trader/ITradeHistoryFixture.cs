using System;
using NUnit.Framework;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Tests.Mocks;

namespace PokeTrader.Tests
{

    /// T.1 - From IDefaultTrader H.1 - should always return non null values
    /// T.2 - From IDefaultTrader H.2 - should persist value after add 
    public abstract class ITradeHistoryFixture
    {
        protected abstract IHistory<Trade> History { get; }

        [Test(Description = "Tests technical requirement T.1")]
        public void ReturnsNonNullHistory()
        {
            //Act 
            var history = History.Get();
            //Assert
            Assert.NotNull(history);
        }
        [Test(Description = "Tests technical requirement T.1")]
        public void ReturnsNonNullHistoryForLambdaFilter()
        {
            //Arrange  
            var filter = MockCreate.Filter<Trade>();
            //Act 
            var history = History.Get(filter);

            //Assert
            Assert.NotNull(history);
        }

        [Test(Description = "Tests technical requirement T.2")]
        public void PersistsTradeOnHistory()
        {
            //Arrange
            var randomValueToAdd = MockCreate.Random<Trade>();
            //Act
            History.Add(randomValueToAdd);

            //Assert
            var history = History.Get();
            CollectionAssert.Contains(history, randomValueToAdd);
        }

        [Test(Description = "Tests technical requirement T.2")]
        public void PersistsTradeOnHistoryWithFilter()
        {
            //Arrange
            var randomValueToAdd = MockCreate.Random<Trade>();
            var filter = MockCreate.Filter<Trade>((_) => true);
            //Act
            History.Add(randomValueToAdd);

            //Assert
            var history = History.Get(filter);
            CollectionAssert.Contains(history, randomValueToAdd);
        }
    }
}