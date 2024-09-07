using System;
using System.Threading.Tasks;
using Cassandra.Data.Linq;
using GeometryApp.Repositories.Entities.Requests;

namespace GeometryApp.Repositories.Cassandra
{
    public class RequestCassandraRepository : IRequestRepository
    {
        public RequestCassandraRepository(GeometryAppCassandra context)
        {
            var session = context.GetSession();

            var requestsTable = new Table<RequestListEntity>(session);
            requestsTable.CreateIfNotExists();
        }
        
        public Task<RequestListEntity> Create(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}