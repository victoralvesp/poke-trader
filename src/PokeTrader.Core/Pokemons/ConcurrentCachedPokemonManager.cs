using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Pokemons.Manager.Abstractions;
using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Repositories.Abstractions;

namespace PokeTrader.Core.Pokemons.Manager
{
    internal class ConcurrentCachedPokemonManager : IPokemonManager
    {
        ConcurrentBag<Pokemon> _cache = new();
        IPokemonRepository _repo;
        private bool _disposed = false;
        internal static readonly TimeSpan _cacheTime = TimeSpan.FromSeconds(.3);

        readonly Subject<bool> _syncSubject = new();

        public ConcurrentCachedPokemonManager(IPokemonRepository repo)
        {
            _repo = repo;
            InitializeSyncCycle();
        }

        private void InitializeSyncCycle()
        {
            _ = Task.Run(SyncCycle);
        }

        private async void SyncCycle()
        {
            while (!_disposed)
            {
                var persisted = await _repo.Get();
                var notCached = persisted.Where(t => !_cache.Any(u => u.Equals(t))).ToArray();
                if (notCached.Any())
                {
                    _syncSubject.OnNext(false);

                    _cache = new(_cache.ToArray().Concat(notCached).ToArray());
                }
                _syncSubject.OnNext(true);
                await Task.Delay(_cacheTime);
            }
        }


        public IEnumerable<Pokemon> Get()
        {
            return _cache.ToArray();
        }

        public IEnumerable<Pokemon> Get(IFilter<Pokemon> filter)
        {
            return _cache.ToArray();
        }

        public Pokemon? Get(int id)
        {
            var pokemon = _cache.SingleOrDefault(poke => poke.Id == id);
            if (pokemon == null)
            {
                EnsureSynchronized();
                pokemon = _cache.SingleOrDefault(poke => poke.Id == id);
            }

            return pokemon;
        }

        private void EnsureSynchronized()
        {
            var synched = false;
            _syncSubject.Subscribe((sync) => synched = sync);
            while (!synched)
            {
                Task.Delay(TimeSpan.FromMilliseconds(100)).Wait();
            }
        }

    }
}