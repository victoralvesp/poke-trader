using System;

namespace PokeTrader.Core.Resources
{
    public static class TextResources
    {

        private static string s_unknown = "Desconhecido";
        private static string s_TraderDefaultText = "Jogador";

        internal static string UnknownPlayer()
            => s_unknown;

        internal static string TraderName(int traderOrder) => $"{s_TraderDefaultText} {traderOrder}";
        internal static string UnknownPokemon() => s_unknown;
    }
}