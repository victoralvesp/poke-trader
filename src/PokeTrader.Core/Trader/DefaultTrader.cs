using System;
using System.Collections.Generic;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Exceptions;
using PokeTrader.Core.Trader.Models;

namespace PokeTrader.Core.Trader
{
    public class DefaultTrader : ITrader
    {
        readonly ICollectionMeasurer<Pokemon> _comparer;
        readonly IHistory<Trade> _history;

        public DefaultTrader(ICollectionMeasurer<Pokemon> comparer, IHistory<Trade> manager)
        {
            _comparer = comparer;
            _history = manager;
        }

        /// <summary>
        /// Expecify the maximum distance between trade offers of a trade to consider it fair 
        /// </summary>
        public int FairnessTierSize { get; init; }

        public TradeInfo Check(TradeParticipant firstTrader, TradeParticipant secondTrader)
        {
            var firstOfferValue = _comparer.Measure(firstTrader.TradeOffer);
            var secondOfferValue = _comparer.Measure(secondTrader.TradeOffer);

            return CreateInfo(firstTrader, secondTrader, firstOfferValue, secondOfferValue);
        }

        private TradeInfo CreateInfo(TradeParticipant firstTrader, TradeParticipant secondTrader, int firstOfferValue, int secondOfferValue)
        {
            var measureDistance = firstOfferValue - secondOfferValue;
            return new TradeInfo
            {
                First = firstTrader,
                FirstScore = firstOfferValue,
                Second = secondTrader,
                SecondScore = secondOfferValue,
                TradeFairness = DefineFairness(measureDistance)
            };
        }

        private TradeInfo.Fairness DefineFairness(int measureDistance) 
        => measureDistance switch
        {
            var x when Math.Abs(x) <= FairnessTierSize => TradeInfo.Fairness.Fair,
            var x when x <= 2 * FairnessTierSize => TradeInfo.Fairness.SlightlyFavorsFirst,
            var x when x >= 2 * FairnessTierSize => TradeInfo.Fairness.FavorsFirst,
            var x when x >= -2 * FairnessTierSize => TradeInfo.Fairness.SlightlyFavorsSecond,
            var x when x <= -2 * FairnessTierSize => TradeInfo.Fairness.FavorsSecond,
            _ => throw new InvalidMeasureException()
        };

        public IEnumerable<Trade> GetHistory()
        => _history.Get();

        public IEnumerable<Trade> GetHistory(Player player)
        {
            var filter = new Filters.LambdaFilter<Trade>((trd) => (trd.Info.First.Trader == player) || trd.Info.Second.Trader == player);
            return _history.Get(filter);
        }

        public Trade MakeTrade(TradeParticipant firstTrader, TradeParticipant secondTrader)
        {
            var info = Check(firstTrader, secondTrader);
            return MakeTrade(info);
        }

        public Trade MakeTrade(TradeInfo info)
        {
            var tradeDate = DateTime.Now;
            var trade = new Trade
            {
                Info = info,
                TradeDate = tradeDate
            };
            _history.Add(trade);
            return trade;
        }
    }
}