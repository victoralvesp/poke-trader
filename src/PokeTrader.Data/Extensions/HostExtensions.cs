using Microsoft.Extensions.DependencyInjection;
using PokeApiNet;
using PokeTrader.Core.Pokemons.Manager;
using PokeTrader.Core.Pokemons.Manager.Abstractions;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Data.Persistence;
using PokeTrader.Data.Services;

using Pokemon = PokeTrader.Core.Pokemons.Models.Pokemon;

namespace PokeTrader.Core.Extensions
{
    public static class HostExtensions
    {
        public static IServiceCollection AddEfPokeTrader(this IServiceCollection services)
        {
            services.AddScoped<ITrader, DefaultTrader>()
                    .AddScoped<ICollectionMeasurer<Pokemon>, BaseExperienceMeasure>()
                    .AddScoped<IHistory<Trade>, ConcurrentCachedHistory<Trade>>()
                    .AddScoped<IHistoryRepository<Trade>, DefaultTradeHistoryRepository>()
                    .AddScoped<IPokemonManager, DefaultPokemonManager>()
                    .AddScoped<IPlayerManager, DefaultPlayerManager>()
                    .AddScoped<IPlayerRepository, DefaultPlayerRepository>()
                    .AddScoped<IPokemonRepository, DefaultPokemonRepository>()
                    .AddSingleton<PokeApiClient>();

            return services;
            
        }
    }
}
