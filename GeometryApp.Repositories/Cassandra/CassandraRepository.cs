using Cassandra.Data.Linq;
using Cassandra.Mapping;

namespace GeometryApp.Repositories.Cassandra
{
    public class CassandraRepository
    {
        private readonly GeometryAppCassandra context;
        private readonly MappingConfiguration config;
        private readonly Mapper mapper;

        public CassandraRepository(GeometryAppCassandra context, MappingConfiguration config = null)
        {
            this.context = context;
            this.config = config;
            var session = context.GetSession();

            mapper = new Mapper(session, config);
        }

        protected void CreateIfNotExists<TTable>()
        {
            var requestsTable = new Table<TTable>(context.GetSession(), config);
            requestsTable.CreateIfNotExists();
        }

        protected Mapper GetMapper() => mapper;
    }
}
