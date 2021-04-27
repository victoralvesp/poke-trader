using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace PokeTrader.Dto.Trader
{
    public record TradeDto : IDto<Trade>
    {
        [Key]
        public int Id { get; set; }
        public TradeInfoDto Info { get; set; }
        public DateTime TradeDate { get; set; } = DateTime.MaxValue;

        public Trade ToModel()
        => new()
        {
            Info = Info.ToModel(),
            TradeDate = TradeDate,
            Id = Id
        };

        public TradeDto(Trade model)
        {
            Info = model.Info;
            TradeDate = model.TradeDate;
            Id = model.Id;
        }

        public TradeDto()
        {

        }

        public static implicit operator TradeDto(Trade model)
            => new(model);
    }

}