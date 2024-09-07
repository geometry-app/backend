using System.Linq;
using System.Net;
using Cassandra;
using GeometryApp.Common.Configs;

namespace GeometryApp.Repositories.Cassandra
{
    public class GeometryAppCassandra
    {
        private readonly Cluster cluster;
        private readonly ISession session;

        public GeometryAppCassandra(CassandraConfig config)
        {
            cluster = Cluster.Builder()
                .AddContactPoints(new IPEndPoint[] { new(IPAddress.Parse(config.ContactPoints.FirstOrDefault()), 9041)})
                .WithAuthProvider(new PlainTextAuthProvider(config.User, config.Password))
                .Build();

            session = cluster.Connect();
        }

        public ISession GetSession()
        {
            return session;
        }
    }
}
