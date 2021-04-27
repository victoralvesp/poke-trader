using System;
using System.Linq.Expressions;
using PokeTrader.Core.Filters.Abstractions;

namespace PokeTrader.Core.Filters
{
    internal class LambdaFilter<T> : IFilter<T> where T : notnull
    {
        private readonly Func<T, bool> _passingFuncition;

        public LambdaFilter(Func<T, bool> passingFuncition)
        {
            _passingFuncition = passingFuncition;
        }

        public bool Pass(T target)
        {
            return _passingFuncition(target);
        }

        public static explicit operator LambdaFilter<T>(Func<T, bool> passingFuncition) =>
        new(passingFuncition);
    }
}