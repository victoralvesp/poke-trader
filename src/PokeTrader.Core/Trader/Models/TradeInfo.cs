using System;

namespace PokeTrader.Core.Trader.Models
{
    public record Trade
    {
        public TradeInfo Info { get; init; } = new();
        public DateTime TradeDate { get; init; } = DateTime.MaxValue;
    }
    public struct TradeInfo
    {
        public Fairness TradeFairness { get; init; }

        public TradeParticipant First { get; init; }

        public TradeParticipant Second { get; init; }


        public enum Fairness
        {
            InvalidTrade,
            Fair,
            SlightlyFavorsFirst,
            SlightlyFavorsSecond,
            FavorsFirst,
            FavorsSecond
        }
        public enum FavoredPlayer
        {
            None,
            First,
            Second
        }
    }

}