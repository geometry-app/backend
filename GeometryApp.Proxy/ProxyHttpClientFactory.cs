using System;
using System.Net;
using System.Net.Http;
using GeometryDashAPI.Factories;

namespace GeometryApp.Proxy;

public class ProxyHttpClientFactory : IFactory<HttpClient>
{
    private readonly string proxy;
    private readonly string login;
    private readonly string password;

    public ProxyHttpClientFactory(string proxy, string login, string password)
    {
        if (string.IsNullOrEmpty(login) ^ string.IsNullOrEmpty(password))
            throw new ArgumentException($"login: {login ?? "null"}. Password: {password ?? "null"}");
        this.proxy = proxy;
        this.login = login;
        this.password = password;
    }
    
    public HttpClient Create()
    {
        ICredentials credentials = null;
        if (!string.IsNullOrEmpty(login))
            credentials = new NetworkCredential(login, password);
        return new HttpClient(new HttpClientHandler()
        {
            Proxy = new WebProxy(proxy)
            {
                Credentials = credentials
            },
            UseProxy = true
        })
        {
            Timeout = TimeSpan.FromSeconds(60)
        };
    }
}
