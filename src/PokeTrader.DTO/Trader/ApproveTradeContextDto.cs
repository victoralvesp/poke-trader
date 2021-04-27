using PokeTrader.Core.Trader.Models;

namespace PokeTrader.Dto.Trader
{
    public class ApproveTradeContextDto
    {
        public TradeParticipantDto First { get; set; }

        public TradeParticipantDto Second { get; set; }

        public (TradeParticipant first, TradeParticipant second) ToModel()
        {
            return (First.ToModel(), Second.ToModel());
        }
    }
}
