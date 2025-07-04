using System.Linq.Expressions;

namespace DemoBackend.Common.Mapping;

public interface IMapper
{
    TDest Map<TSource, TDest>(TSource source);
    IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> source);
    IQueryable<TDest> Map<TSource, TDest>(IQueryable<TSource> source);
    void Register<TSource, TDest>(Expression<Func<TSource, TDest>> expression);
}