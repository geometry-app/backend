using System;
using System.IO;
using System.Text.Json;
using GeometryApp.Common.Configs;

namespace GeometryApp.Common
{
    public static class ConfigProvider
    {
        private static string? currentPath;

        public static ServiceConfig GetService() => GetConfig<ServiceConfig>("service.json");

        public static CassandraConfig GetCassandra() => GetConfig<CassandraConfig>("cassandra.json");

        public static KafkaConfig GetKafka() => GetConfig<KafkaConfig>("kafka.json");

        public static KafkaTopology GetKafkaTopology() => GetConfig<KafkaTopology>("kafka_topology.json");

        public static ElasticConfig GetElastic() => GetConfig<ElasticConfig>("elastic.json");

        public static ElasticTopology GetElasticTopology() => GetConfig<ElasticTopology>("elastic_topology.json");

        public static CassandraTopology GetCassandraTopology() => GetConfig<CassandraTopology>("cassandra_topology.json");

        private static T? GetConfig<T>(string variable) where T : class
        {
            var directory = GetDirectoryPath();
            if (string.IsNullOrEmpty(directory))
                return null;
            var path = Path.Combine(directory, variable);
            Console.WriteLine($"getting config: '{path}'");
            if (variable.EndsWith(".json"))
                return JsonSerializer.Deserialize<T>(new FileStream(path, FileMode.Open, FileAccess.Read));
            return JsonSerializer.Deserialize<T>(variable);
        }

        public static string? GetDirectoryPath()
        {
            return currentPath ?? Environment.GetEnvironmentVariable("GEOMETRYAPP_CONFIG") ?? GetFromUser();
        }

        public static void OverridePath(string path)
        {
            currentPath = path;
        }

        private static string? GetFromUser()
        {
            return Environment.GetEnvironmentVariable("GEOMETRYAPP_CONFIG", EnvironmentVariableTarget.User);
        }
    }
}
