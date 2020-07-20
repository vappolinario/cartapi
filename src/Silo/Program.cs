using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Silo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var ip = Dns.GetHostEntry(Dns.GetHostName())
              .AddressList
              .First(i => i.AddressFamily == AddressFamily.InterNetwork);

            var siloBuilder = new SiloHostBuilder()
              .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "DefaultCluster";
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
              .ConfigureLogging(logging => logging.AddConsole());

            using (var host = siloBuilder.Build())
            {
                await host.StartAsync();
                await Task.Run(() => System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite));
            }
        }
    }
}
