using System.Threading.Tasks;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer;

public interface IExplorer<T> where T : IExploreRequest
{
    Task<ExploreResult> Explore(ExploreRequest<T> request, ILog log);
}
