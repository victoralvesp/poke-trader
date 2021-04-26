using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;
using System;

namespace PokeTrader.Dto.Trader
{
    public record TradeDto : IDto<Trade>
    {
        public TradeInfoDto Info { get; set; }
        public DateTime TradeDate { get; set; } = DateTime.MaxValue;

        public Trade ToModel()
        => new()
        {
            Info = Info.ToModel(),
            TradeDate = TradeDate
        };

        public TradeDto(Trade model)
        {
            Info = model.Info;
            TradeDate = model.TradeDate;
        }

        public TradeDto()
        {

        }

        public static implicit operator TradeDto(Trade model)
            => new(model);
    }

}