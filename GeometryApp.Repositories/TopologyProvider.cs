using System.Threading.Tasks;
using Folleach.Properties;
using GeometryApp.Common;
using GeometryApp.Common.Configs;
using GeometryApp.Repositories.Cassandra;

namespace GeometryApp.Repositories;

public class TopologyProvider : ITopologyProvider
{
    private readonly ServiceInfo serviceInfo;
    private readonly CassandraPropertiesProvider props;

    public TopologyProvider(GeometryAppCassandra cassandra, CassandraTopology topology, ServiceInfo serviceInfo)
    {
        this.serviceInfo = serviceInfo;
        props = new CassandraPropertiesProvider(cassandra.GetSession(), topology.MainKeyspace, "topologies");
    }
    
    public async Task<T> Get<T>()
    {
        return await props.Get<T>(serviceInfo.Service, typeof(T).Name);
    }
}
