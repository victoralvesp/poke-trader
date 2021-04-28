using System.ComponentModel.DataAnnotations;
using PokeTrader.Core.Trader.Models;

namespace PokeTrader.Dto.Trader
{
    public class ApproveTradeContextDto
    {
        [Required]
        public TradeParticipantDto First { get; set; }

        [Required]
        public TradeParticipantDto Second { get; set; }

        public (TradeParticipant first, TradeParticipant second) ToModel()
        {
            return (First.ToModel(), Second.ToModel());
        }
    }
}
