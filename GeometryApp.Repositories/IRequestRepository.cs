using System;
using System.Threading.Tasks;
using GeometryApp.Repositories.Entities.Requests;

namespace GeometryApp.Repositories
{
    public interface IRequestRepository
    {
        Task<RequestListEntity> Create(Guid userId);
    }
}
