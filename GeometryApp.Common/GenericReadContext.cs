using System;
using System.Threading.Tasks;

namespace GeometryApp.Common;

public class GenericReadContext<T, TComplete> : IReadContext<T> where TComplete : class
{
    private readonly Func<TComplete, Task>? complete;
    private readonly TComplete? result;
    public T Value { get; }

    public GenericReadContext(T request, TComplete? result = null, Func<TComplete, Task>? complete = null)
    {
        if (complete == null ^ result == null)
            throw new ArgumentException($"one of {nameof(this.complete)} and {nameof(result)} is null");
        this.complete = complete;
        this.result = result;
        Value = request;
    }

    public async Task Complete()
    {
        if (complete != null)
            await complete(result!);
    }
}
