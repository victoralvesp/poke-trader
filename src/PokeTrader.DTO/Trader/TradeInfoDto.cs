using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;
using static PokeTrader.Core.Trader.Models.TradeInfo;

namespace PokeTrader.Dto.Trader
{
    public class TradeInfoDto : IDto<TradeInfo>
    {
        public Fairness TradeFairness { get; set; }

        public TradeParticipantDto First { get; set; }

        public TradeParticipantDto Second { get; set; }

        public TradeInfo ToModel()
        => new()
        {
            First = First.ToModel(),
            Second = Second.ToModel(),
            TradeFairness = TradeFairness
        };

        public TradeInfoDto(TradeInfo model)
        {
            First = model.First;
            Second = model.Second;
            TradeFairness = model.TradeFairness;
        }

        public static implicit operator TradeInfoDto(TradeInfo model)
            => new(model);
    }

}