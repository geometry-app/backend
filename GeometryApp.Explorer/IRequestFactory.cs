namespace GeometryApp.Explorer;

public interface IRequestFactory
{
    ExploreRequest<T> Create<T>(T parameters) where T : IExploreRequest;
}
