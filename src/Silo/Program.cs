using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Silo
{
    public class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        static async Task Main(string[] args)
        {
            var ip = Dns.GetHostEntry(Dns.GetHostName())
              .AddressList
              .First(i => i.AddressFamily == AddressFamily.InterNetwork);

            var siloBuilder = new SiloHostBuilder()
              .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "DefaultCluster-01";
                    options.ServiceId = "orleans-cartapi";
                })
              .UseDevelopmentClustering(new IPEndPoint(ip, 30000))
              .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = ip)
              .AddAdoNetGrainStorage("CartStorage", options =>
                  {
                      options.Invariant = "MySql.Data.MySqlClient";
                      options.ConnectionString = "host=mariadb;port=3306;user id=root;password=admin;database=CartApi";
                      options.UseJsonFormat = true;
                  })
              .UseAdoNetClustering(options =>
                  {
                      options.Invariant = "MySql.Data.MySqlClient";
                      options.ConnectionString = "host=mariadb;port=3306;user id=root;password=admin;database=CartApi";
                  })
              .ConfigureLogging(logging => logging.AddConsole());


            using (var host = siloBuilder.Build())
            {
                await host.StartAsync();
                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);
                _closing.WaitOne();
                await host.StopAsync();
            }
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs e)
        {
            _closing.Set();
        }
    }
}
