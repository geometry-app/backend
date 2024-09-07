using System.Threading.Tasks;

namespace GeometryApp.Explorer;

public interface IRequestExecutor
{
    Task<ExploreResult> Execute<T>(ExploreRequest<T> request) where T : IExploreRequest;
}
