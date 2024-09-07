namespace GeometryApp.API.Services;

public interface IBackendVersionService
{
    (string version, string environment) Get();
}
