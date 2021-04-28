namespace PokeTrader.Core.Trader.Models
{
    public struct TradeInfo
    {
        public int Id { get; init; }
        public Fairness TradeFairness { get; init; }

        public TradeParticipant First { get; init; }
        public int FirstScore {get;init;}

        public TradeParticipant Second { get; init; }
        public int SecondScore {get;init;}

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