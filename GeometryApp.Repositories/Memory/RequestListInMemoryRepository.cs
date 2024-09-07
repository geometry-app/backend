using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeometryApp.Repositories.Entities.Requests;

namespace GeometryApp.Repositories.Memory
{
    public class RequestListInMemoryRepository
    {
        private Dictionary<Guid, RequestListEntity> lists = new();
        
        public async Task<RequestListEntity> Create(Guid userId)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            var entity = new RequestListEntity()
            {
                Id = Guid.NewGuid(),
                Owner = userId
            };
            lists.Add(entity.Id, entity);
            return entity;
        }
    }
}