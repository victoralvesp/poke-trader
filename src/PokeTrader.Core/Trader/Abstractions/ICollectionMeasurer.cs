using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Models;

namespace PokeTrader.Core.Trader.Abstractions
{
    public interface ICollectionMeasurer<T>
    {
        int Measure(T[] measurable);
    }
}