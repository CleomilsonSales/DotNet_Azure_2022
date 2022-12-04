using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Manager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //configuração do Azure
                .ConfigureAppConfiguration((context, config) => {
                    if (context.HostingEnvironment.IsProduction()){
                        var buildConfig = config.Build();

                        config.AddAzureKeyVault(
                            buildConfig["AzureKeyVault:Vault"],
                            buildConfig["AzureKeyVault:ClientId"],
                            buildConfig["AzureKeyVault:ClientSecret"]); //configurar essas chaves no appsettings.json
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
