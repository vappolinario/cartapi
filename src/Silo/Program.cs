using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Silo
{
    public class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        static async Task Main(string[] args)
        {
          //var path = Directory.GetCurrentDirectory();
          var path = AppContext.BaseDirectory;
          Console.WriteLine(path);
            var configuration = new ConfigurationBuilder()
              .SetBasePath(path)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

            var ip = Dns.GetHostEntry(Dns.GetHostName())
              .AddressList
              .First(i => i.AddressFamily == AddressFamily.InterNetwork);

            var siloBuilder = new SiloHostBuilder()
              .Configure<ClusterOptions>(options =>
                  {
                      options.ClusterId = configuration.GetValue<string>("Orleans:ClusterId");
                      options.ServiceId = configuration.GetValue<string>("Orleans:ServiceId");
                  })
            .UseDevelopmentClustering(new IPEndPoint(ip, configuration.GetValue<int>("Orleans:SiloPort")))
              .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = ip)
              .AddAdoNetGrainStorage("CartStorage", options =>
                  {
                      options.Invariant = configuration.GetValue<string>("Orleans:Invariant");
                      options.ConnectionString = configuration.GetValue<string>("Orleans:ConnectionString");
                      options.UseJsonFormat = true;
                  })
            .UseAdoNetClustering(options =>
                {
                    options.Invariant = configuration.GetValue<string>("Orleans:Invariant");
                    options.ConnectionString = configuration.GetValue<string>("Orleans:ConnectionString");
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
