using System.Threading.Tasks;

namespace GeometryApp.Repositories;

public interface ITopologyProvider
{
    Task<T> Get<T>();
}
