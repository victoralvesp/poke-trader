using System.Collections.Generic;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;

namespace PokeTrader.Core.Trader
{
    public class DefaultTrader : ITrader
    {
        public TradeInfo Check(TradeParticipant firstTrader, TradeParticipant secondTrader)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Trade> GetHistory()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Trade> GetHistory(Player player)
        {
            throw new System.NotImplementedException();
        }

        public Trade MakeTrade(TradeParticipant firstTrader, TradeParticipant secondTrader)
        {
            throw new System.NotImplementedException();
        }

        public Trade MakeTrade(TradeInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}