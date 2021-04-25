using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PokeTrader.Core.Trader.Models;

[assembly: InternalsVisibleToAttribute("PokeTrader.Tests")]
namespace PokeTrader.Core.Trader.Abstractions
{
    public interface ITrader
    {
        TradeInfo Check(TradeParticipant firstTrader, TradeParticipant secondTrader);
        Trade MakeTrade(TradeParticipant firstTrader, TradeParticipant secondTrader);
        Trade MakeTrade(TradeInfo info);
        IEnumerable<Trade> GetHistory();
        IEnumerable<Trade> GetHistory(Player player);

    }

}