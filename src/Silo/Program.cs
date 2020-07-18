using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;

namespace Silo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var siloBuilder = new SiloHostBuilder()
                .UseLocalhostClustering(serviceId:"orleans-cartapi")
                .ConfigureLogging(logging => logging.AddConsole());

            using (var host = siloBuilder.Build())
            {
                await host.StartAsync();
                Console.ReadLine();
            }
        }
    }
}
