using System.Threading.Tasks;

namespace GeometryApp.Common;

public class MultiTask
{
    public static async Task<(T1, T2)> ExecuteAll<T1, T2>(Task<T1> t1, Task<T2> t2)
    {
        await Task.WhenAll(t1, t2).ConfigureAwait(false);
        return (t1.GetAwaiter().GetResult(), t2.GetAwaiter().GetResult());
    }
}
