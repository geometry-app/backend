using System.Threading.Tasks;

namespace GeometryApp.Common;

public interface IReadContext<out T>
{
    T Value { get; }
    Task Complete();
}
