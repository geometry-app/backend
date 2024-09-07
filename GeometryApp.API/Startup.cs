using System;
using Folleach.Properties;
using GeometryApp.API.Extensions;
using GeometryApp.API.Middlewares;
using GeometryApp.API.Services;
using GeometryApp.API.Services.Filters;
using GeometryApp.Common;
using GeometryApp.Common.Configs;
using GeometryApp.Repositories;
using GeometryApp.Repositories.Cassandra;
using GeometryApp.Repositories.Elastic;
using GeometryApp.Services.Roulette;
using GeometryApp.Services.Roulette.Generators;
using GeometryApp.Services.Roulette.Repositories;
using GeometryApp.Services.Roulette.SessionGenerator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Vostok.Logging.Abstractions;

namespace GeometryApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static string DevCors = "_allowDevCors";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new ServiceInfo("backend-api"));
            services.AddLog(true);

            services.AddLogging(c =>
            {
                c.ClearProviders();
            });
            var environment = Configuration.GetSection("Application").GetSection("env").Value;
            Console.WriteLine($"Environment: {environment}");
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "GeometryApp.API", Version = "v1"});
            });
            services.AddSingleton<IAppEnvironment, AppEnvironment>();
            services.AddSingleton<IBackendVersionService>(c => new FileBackendVersionService("version.txt", c.GetRequiredService<IAppEnvironment>()));
            services.AddSingleton(ConfigProvider.GetCassandraTopology());
            services.AddSingleton(ConfigProvider.GetCassandra());
            services.AddSingleton(ConfigProvider.GetElasticTopology());
            services.AddSingleton(ConfigProvider.GetElastic());
            services.AddSingleton(ConfigProvider.GetKafkaTopology());
            services.AddSingleton(ConfigProvider.GetKafka());
            services.AddSingleton<GeometryAppCassandra>();
            services.AddSingleton<ElasticApp>();
            services.AddSingleton<IPropertiesProvider>(x => new CassandraPropertiesProvider(
                x.GetService<GeometryAppCassandra>()?.GetSession() ?? throw new InvalidOperationException($"Service {nameof(GeometryAppCassandra)} is not registered"),
                x.GetService<CassandraTopology>()?.MainKeyspace ?? throw new InvalidOperationException($"Service {nameof(CassandraTopology)} is not registered"),
                "properties"));
            services.AddSingleton<ITopologyProvider, TopologyProvider>();
            services.AddSingleton<IRouletteOwnerRepository, CassandraRouletteOwnerRepository>();
            services.AddSingleton<IRouletteRepository, CassandraRouletteRepository>();
            services.AddSingleton<IRouletteSessionGenerator, SessionGenerator>();
            services.AddSingleton<IRouletteGenerator, RouletteGenerator>();
            services.AddSingleton<IProgressRepository, CassandraProgressRepository>();
            services.AddSingleton<RouletteService>();
            services.RegisterFilters();
            services.AddSingleton<LogRequestMiddleware>();
            services.AddSingleton<IIndexRepository, IndexElasticRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: DevCors,
                    policy  =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILog log)
        {
            app.ApplicationServices
                .GetService<ElasticApp>()
                .ApplyIndices(app.ApplicationServices.GetService<ITopologyProvider>())
                .GetAwaiter().GetResult();
            app.UseMiddleware<LogRequestMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeometryApp.API v1"));
                app.UseCors(c => c.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
                
                app.UseCors(DevCors);
            }
            else
            {
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build());
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            log.Info("api started successfully");
        }
    }
}
