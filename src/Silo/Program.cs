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
                .UseLocalhostClustering(serviceId: "orleans-cartapi")
                .AddAdoNetGrainStorage("CartStorage", options =>
                    {
                        options.Invariant = "MySql.Data.MySqlClient";
                        options.ConnectionString = "host=127.0.0.1;port=3306;user id=root;password=admin;database=CartApi";
                        options.UseJsonFormat = true;
                    })
                .ConfigureLogging(logging => logging.AddConsole());

            using (var host = siloBuilder.Build())
            {
                await host.StartAsync();
                Console.ReadLine();
            }
        }
    }
}
