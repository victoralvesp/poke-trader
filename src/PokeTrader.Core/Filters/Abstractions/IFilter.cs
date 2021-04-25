namespace PokeTrader.Core.Filters.Abstractions
{
    public interface IFilter<T> where T : notnull
    {
        bool Pass(T target);
    } 

}