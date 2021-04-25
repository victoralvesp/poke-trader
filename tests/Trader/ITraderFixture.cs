using System;
using NUnit.Framework;
using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;

using static PokeTrader.Tests.Mocks.MockCreate;

namespace PokeTrader.Tests
{

    /// H.1 - All trade participants are not null and all trade participants information are not null    
    /// D.1 - Check check trade fairness and return a TradeFairness with the results
    /// D.2 - MakeTrade makes a trade and persists in history
    /// D.3 - ShowHistory shows all trade history
    /// D.4 - ShowPlayerHistory shows all trades made by a specific player
    /// D.5 - Check result can be used to MakeTrade
    public abstract class ITraderFixture
    {
        protected abstract ITrader Trader { get; }

#region SanityChecks
        [Test(Description = "Tests domain requirement D.1")]
        public void ReturnsSomeValidFairnessResult()
        {
            // Arrange
            DefineRandomTraders(out var firstTrader, out var secondTrader);


            // Act
            var fairness = Trader.Check(firstTrader, secondTrader);

            // Assert
            Assert.That(fairness, Is.InstanceOf<TradeInfo>()
                                    .And.Property(nameof(TradeInfo.TradeFairness))
                                    .Not.EqualTo(TradeInfo.Fairness.InvalidTrade)
                                    );
        }
        
        
        [Test(Description = "Tests domain requirement D.1")]
        public void ReturnedFairnessResultRefersToFirstTrader()
        {
            // Arrange
            DefineRandomTraders(out var firstTrader, out var secondTrader);


            // Act
            var fairness = Trader.Check(firstTrader, secondTrader);

            // Assert
            Assert.That(fairness, Is.InstanceOf<TradeInfo>()
                                    .And.Property(nameof(TradeInfo.First)).EqualTo(firstTrader)
                                    .And.Property(nameof(TradeInfo.Second)).EqualTo(secondTrader));
        }
        [Test(Description = "Tests domain requirement D.1")]
        public void ReturnedFairnessResultRefersToSecondTrader()
        {
            // Arrange
            DefineRandomTraders(out var firstTrader, out var secondTrader);


            // Act
            var fairness = Trader.Check(firstTrader, secondTrader);

            // Assert
            Assert.That(fairness, Is.InstanceOf<TradeInfo>()
                                    .And.Property(nameof(TradeInfo.First)).EqualTo(firstTrader)
                                    .And.Property(nameof(TradeInfo.Second)).EqualTo(secondTrader));
        }
        public void ReturnsSomeFairnessResult()
        {
            // Arrange
            DefineRandomTraders(out var firstTrader, out var secondTrader);
            var before = DateTime.Now;


            // Act
            var trade = Trader.MakeTrade(firstTrader, secondTrader);
            var after = DateTime.Now;


            // Assert
            Assert.That(trade, Is.InstanceOf<Trade>()
                                    .And.Property(nameof(Trade.TradeDate))
                                        .AtLeast(before).And.AtMost(after));
        }

        [Test(Description = "Tests domain requirement D.3")]
        public void ReturnsAllTradesMade()
        {
            // Arrange
            var trade1 = MakeRandomTrade();
            var trade2 = MakeRandomTrade();
            var trade3 = MakeRandomTrade();
            var trade4 = MakeRandomTrade();

            // Act
            var history = Trader.GetHistory();


            // Assert
            CollectionAssert.IsSupersetOf(history, new[] { trade1, trade2, trade3, trade4 });

        }
        
        [Test(Description = "Tests domain requirement D.4")]
        public void ReturnsAllTradesMadeByPlayerAndNotTradesMadeByOtherPlayers()
        {
            // Arrange
            var firstTrader = RandomTrader();
            var trade1 = MakeRandomTrade(firstTrader);
            var trade2 = MakeRandomTrade(firstTrader);
            var trade3 = MakeRandomTrade(firstTrader);
            var trade4 = MakeRandomTrade(firstTrader);
            var tradeWithOtherTrader = MakeRandomTrade();

            // Act
            var history = Trader.GetHistory(firstTrader.Trader);

            // Assert
            CollectionAssert.IsSupersetOf(history, new[] { trade1, trade2, trade3, trade4 });
            CollectionAssert.IsNotSupersetOf(history, new[] { tradeWithOtherTrader });
        }

        [Test(Description = "Tests domain requirement D.5")]
        public void TradeRefersToInfo()
        {
            // Arrange
            DefineRandomTraders(out var firstTrader, out var secondTrader);
            var checkResult = Trader.Check(firstTrader, secondTrader);

            // Act
            var fairness = Trader.MakeTrade(checkResult);

            // Assert
            Assert.That(fairness, Is.InstanceOf<Trade>()
                                    .And.Property(nameof(Trade.Info))
                                    .EqualTo(checkResult));
        }
#endregion

 
        protected static void DefineRandomTraders(out TradeParticipant firstTrader, out TradeParticipant secondTrader)
        {
            firstTrader = RandomTrader();
            secondTrader = RandomTrader();
        }

        protected virtual Trade MakeRandomTrade(TradeParticipant? firstTrader = null, TradeParticipant? secondTrader = null)
        {
            firstTrader ??= RandomTrader();
            secondTrader ??= RandomTrader();
            return Trader.MakeTrade(firstTrader, secondTrader);
        }

    }
}