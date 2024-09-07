using GeometryApp.Common.Configs;

namespace GeometryApp.Common;

public static class KafkaExtensions
{
    public static string GetName(this KafkaTopology topology, string topic) => $"{topology.Prefix}_{topic}";
}
