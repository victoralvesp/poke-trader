using PokeTrader.Dto.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrader.Dto.Extensions
{
    public static class DtoExtensions
    {
        public static IEnumerable<T> ToModel<T>(this IEnumerable<IDto<T>> dtos)
            => from dto in dtos
               select dto.ToModel();


    }
}
