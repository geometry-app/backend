using System;
using GeometryApp.Explorer.Implementations.ImpossibleList;
using GeometryApp.Explorer.Implementations.Level;
using GeometryApp.Explorer.Implementations.PointCreate;
using Silverback.Messaging.Configuration.Kafka;

namespace GeometryApp.Explorer.Implementations;

public static class KafkaExtensions
{
    public static IKafkaEndpointsConfigurationBuilder AddKafkaRequestOutbound(
        this IKafkaEndpointsConfigurationBuilder builder, string prefix)
    {
        var x = builder
            .AddToProduce<LevelSearchRequest>(prefix)
            .AddToProduce<PointCreateRequest>(prefix)
            .AddToProduce<ImpossibleListRequest>(prefix);
        return x;
    }

    private static IKafkaEndpointsConfigurationBuilder AddToProduce<T>(
        this IKafkaEndpointsConfigurationBuilder builder, string prefix)
        where T : IExploreRequest
        => builder.AddOutbound(typeof(ExploreRequest<T>), EndpointBuilderAction<T>(prefix));

    private static Action<IKafkaProducerEndpointBuilder> EndpointBuilderAction<T>(string prefix) where T : IExploreRequest
    {
        return endpoint => endpoint
            .ProduceTo(_ => WithPrefix(prefix, "explore_requests"))
            .WithKafkaKey<ExploreRequest<T>>(x => x.Message!.RequestId);
    }

    private static string WithPrefix(string prefix, string topic)
    {
        return $"{prefix}_{topic}";
    }
}
