using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;
using System.ComponentModel.DataAnnotations;
using static PokeTrader.Core.Trader.Models.TradeInfo;

namespace PokeTrader.Dto.Trader
{
    public class TradeInfoDto : IDto<TradeInfo>
    {
        [Key]
        public int Id { get; set; }
        public Fairness TradeFairness { get; set; }

        public TradeParticipantDto First { get; set; }

        public int FirstScore { get; set; }

        public TradeParticipantDto Second { get; set; }

        public int SecondScore { get; set; }

        public TradeInfo ToModel()
        => new()
        {
            First = First.ToModel(),
            Second = Second.ToModel(),
            TradeFairness = TradeFairness,
            FirstScore = FirstScore,
            SecondScore = SecondScore,
            Id = Id
        };

        public TradeInfoDto(TradeInfo model)
        {
            Id = model.Id;
            First = model.First;
            Second = model.Second;
            TradeFairness = model.TradeFairness;
            FirstScore = model.FirstScore;
            SecondScore = model.SecondScore;
        }

        public TradeInfoDto()
        {
        }

        public static implicit operator TradeInfoDto(TradeInfo model)
            => new(model);
    }

}