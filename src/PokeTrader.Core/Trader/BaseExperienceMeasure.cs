using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Abstractions;

namespace PokeTrader.Core.Trader
{
    class BaseExperienceMeasure : ICollectionMeasurer<Pokemon>
    {
        public int Measure(Pokemon[] measurable)
        {
            return 0;
        }
    }

}