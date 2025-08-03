using System.Linq.Expressions;

namespace DemoBackend.Common.Mapping;

public class Mapper : IMapper
{
    private readonly Dictionary<(Type, Type), Delegate> _compiledFunctions = new();
    private readonly Dictionary<(Type, Type), Expression> _expressions = new();


    public TDest Map<TSource, TDest>(TSource source)
    {
        var func = GetExpression<TSource, TDest>().Compile();
        return func(source);
    }

    public IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> source)
    {
        var func = GetFunction<TSource, TDest>();
        return source.Select(s => func(s));
    }

    public IQueryable<TDest> Map<TSource, TDest>(IQueryable<TSource> source)
    {
        var expr = GetExpression<TSource, TDest>();
        return source.Select(expr);
    }

    public void Register<TSource, TDest>(Expression<Func<TSource, TDest>> expression)
    {
        _expressions[(typeof(TSource), typeof(TDest))] = expression;
        _compiledFunctions[(typeof(TSource), typeof(TDest))] = expression.Compile();
    }

    private Expression<Func<TSource, TDest>> GetExpression<TSource, TDest>()
    {
        var value = _expressions.GetValueOrDefault((typeof(TSource), typeof(TDest)));
        if (value == null)
            throw new InvalidOperationException(
                $"No map found for type {typeof(TSource)} to {typeof(TDest)}");

        return (Expression<Func<TSource, TDest>>)value;
    }

    private Func<TSource, TDest> GetFunction<TSource, TDest>()
    {
        var value = _compiledFunctions.GetValueOrDefault((typeof(TSource), typeof(TDest)));
        if (value == null)
            throw new InvalidOperationException(
                $"No map found for type {typeof(TSource)} to {typeof(TDest)}");

        return (Func<TSource, TDest>)value;
    }
}