using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GenealogyWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            //return WebHost.CreateDefaultBuilder();
            var builder = new WebHostBuilder()
                .UseKestrel((builderContext, options) =>
                {
                    options.Configure(builderContext.Configuration.GetSection("Kestrel"));
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseHealthChecks("/health", TimeSpan.FromSeconds(3)) // Or to host on a separate port: .UseHealthChecks(port)
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
                .UseStartup<Startup>();

            if (args != null)
            {
                builder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
            }

            return builder;

        }
    }
}
