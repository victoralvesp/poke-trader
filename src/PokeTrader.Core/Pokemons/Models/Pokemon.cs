using System.ComponentModel.DataAnnotations;

namespace PokeTrader.Core.Pokemons.Models
{
    public record Pokemon
    {
        [Required]
        public int Id { get; init; }

        public string Name { get; init; } = Resources.TextResources.UnknownPokemon();

        public int BaseExperience { get; init; }

        public override string? ToString()
        {
            return Name;
        }
    }
}