using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FeatureToggle.Internal;
using GenealogyWebAPI.Proxies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.Processors;
using Polly;
using Polly.Registry;
using Refit;

namespace GenealogyWebAPI
{
    public class Startup
    {
        private IPolicyRegistry<string> policyRegistry;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                if (appAssembly != null)
                {
                    builder.AddUserSecrets(appAssembly, optional: true);
                }
            }

            Configuration = builder.Build();

            if (env.IsProduction())
            {
                builder.AddAzureKeyVault(Configuration["KeyVaultName"]);
                Configuration = builder.Build();
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Options for particular external services
            services.Configure<GenderizeApiOptions>(Configuration.GetSection("GenderizeApiOptions"));

            ConfigurePolicies(services);
            ConfigureFeatures(services);
            ConfigureHealth(services);
            ConfigureOpenApi(services);
            ConfigureApiOptions(services);
            ConfigureHttpClients(services);
            ConfigureVersioning(services);
            ConfigureApplicationInsights(services);
            ConfigureHSTS(services);
        }

        private void ConfigureHSTS(IServiceCollection services)
        {
            services.AddHsts(
                options =>
                {
                    options.MaxAge = TimeSpan.FromDays(100);
                    options.IncludeSubDomains = true;
                    options.Preload = true;
                });
        }

        private void ConfigureFeatures(IServiceCollection services)
        {
            var provider = new AppSettingsProvider { Configuration = Configuration };
            services.AddSingleton(new AdvancedHealthFeature { ToggleValueProvider = provider });
        }

        private void ConfigureApplicationInsights(IServiceCollection services)
        {
            IHostingEnvironment env = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>();
            services.AddApplicationInsightsTelemetry(options => {
                options.DeveloperMode = env.IsDevelopment();
                options.InstrumentationKey = Configuration["ApplicationInsights:InstrumentationKey"];
            });
        }

        private void ConfigureVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Includes headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });

            // Alternative to attribute based versioning
            //options.Conventions.Controller<GameServerController>()
            //    .HasDeprecatedApiVersion(new ApiVersion(0, 9))
            //    .HasApiVersion(1)
            //    .AdvertisesApiVersion(2)
            //    .Action(a => a.Get(default(int))).MapToApiVersion(1);

        }

        private void ConfigurePolicies(IServiceCollection services)
        {
            policyRegistry = services.AddPolicyRegistry();
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(1500));
            policyRegistry.Add("timeout", timeoutPolicy);
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient("Genderize", options =>
            {
                options.BaseAddress = new Uri(Configuration["GenderizeApiOptions:BaseUrl"]);
                options.Timeout = TimeSpan.FromMilliseconds(15000);
                options.DefaultRequestHeaders.Add("ClientFactory", "Check");
            })
            .AddPolicyHandlerFromRegistry("timeout")
            .AddTransientHttpErrorPolicy(p => p.RetryAsync(3))
            .AddTypedClient(client => RestService.For<IGenderizeClient>(client));
        }

        private void ConfigureApiOptions(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://asp.net/core",
                        Detail = "Please refer to the errors property for additional details."
                    };
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });
        }

        private void ConfigureOpenApi(IServiceCollection services)
        {
            services.AddSwagger();
        }

        private void ConfigureHealth(IServiceCollection services)
        {
            services.AddHealthChecks(checks =>
            {
                checks
                    .AddUrlCheck(Configuration["GenderizeApiOptions:BaseUrl"],
                        response =>
                        {
                            // Custom check for healthy service
                            var status = response.StatusCode == HttpStatusCode.UnprocessableEntity ? CheckStatus.Healthy : CheckStatus.Unhealthy;
                            return new ValueTask<IHealthCheckResult>(HealthCheckResult.FromStatus(status, "Genderize API base URL reachable."));
                        }
                    );

                // Use feature toggle to add this functionality
                var feature = services.BuildServiceProvider().GetRequiredService<AdvancedHealthFeature>();
                if (feature.FeatureEnabled)
                {
                    checks.AddHealthCheckGroup(
                        "memory",
                        group => group
                            .AddPrivateMemorySizeCheck(200000000) // Maximum private memory
                            .AddVirtualMemorySizeCheck(3000000000000)
                            .AddWorkingSetCheck(200000000),
                        CheckStatus.Unhealthy
                    );
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Information);
            loggerFactory.AddEventSourceLogger(); // ETW on Windows, dev/null on other platforms
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Do not expose Swagger interface in production
                app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
                {
                    settings.SwaggerRoute = "/swagger/v2/swagger.json";
                    settings.ShowRequestHeaders = true;
                    settings.DocExpansion = "list";
                    settings.UseJsonEditor = true;
                    settings.PostProcess = document =>
                    {
                        document.BasePath = "/";
                    };
                    settings.GeneratorSettings.Description = "Building Web APIs Workshop Demo Web API";
                    settings.GeneratorSettings.Title = "Genealogy API";
                    settings.GeneratorSettings.Version = "2.0";
                    settings.GeneratorSettings.OperationProcessors.Add(
                        new ApiVersionProcessor() { IncludedVersions = { "2.0" } }
                    );
                });

                app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
                {
                    settings.SwaggerRoute = "/swagger/v1/swagger.json";
                    settings.ShowRequestHeaders = true;
                    settings.DocExpansion = "list";
                    settings.UseJsonEditor = true;
                    settings.PostProcess = document =>
                    {
                        document.BasePath = "/";
                    };
                    settings.GeneratorSettings.Description = "Building Web APIs Workshop Demo Web API";
                    settings.GeneratorSettings.Title = "Genealogy API";
                    settings.GeneratorSettings.Version = "1.0";
                    settings.GeneratorSettings.OperationProcessors.Add(
                        new ApiVersionProcessor() { IncludedVersions = { "1.0" } }
                    );
                });
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
