using Folleach.Vostok.Logging.Kafka;
using Folleach.Vostok.Logging.Kafka.Options;
using GeometryApp.Common;
using GeometryApp.Common.Configs;
using Microsoft.Extensions.DependencyInjection;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;

namespace GeometryApp.API.Extensions;

public static class DiExtensions
{
    public static IServiceCollection AddLog(this IServiceCollection builder, bool consoleLogEnabled)
    {
        builder.AddSingleton(c =>
        {
            var consoleLog = new SynchronousConsoleLog();
            var server = c.GetService<KafkaConfig>().Server;
            var topic = c.GetService<KafkaTopology>().GetName("logs2");
            var kafkaLog = new KafkaLog(new KafkaOptions(topic, [server])
            {
                KeySelector = x => $"{x.Timestamp:O}-{x.Lt}-{c.GetService<ServiceInfo>().Service}",
            });

            ILog log = consoleLogEnabled ? new CompositeLog(consoleLog, kafkaLog) : consoleLog;
            LogProvider.Configure(log, true);
            return log;
        });
        return builder;
    }
}
