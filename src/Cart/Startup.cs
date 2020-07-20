using System;
using System.Linq;
using System.Net;
using GrainInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Cart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(CreateClusterClient);
            services.AddControllers();
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var gateways = System.Net.Dns
              .GetHostAddresses(Configuration.GetValue<string>("Orleans:SiloHost"))
              .Select(ip => new IPEndPoint(
                    ip,
                    Configuration.GetValue<int>("Orleans:SiloPort")
                    ))
              .ToArray();

            var client = new ClientBuilder()
              .UseAdoNetClustering(options =>
                  {
                      options.Invariant = Configuration.GetValue<string>("Orleans:Invariant");
                      options.ConnectionString = Configuration.GetValue<string>("Orleans:ConnectionString");
                  })
                 .Configure<ClusterOptions>(options =>
                     {
                         options.ClusterId = Configuration.GetValue<string>("Orleans:ClusterId");
                         options.ServiceId = Configuration.GetValue<string>("Orleans:ServiceId");
                     })
                 .ConfigureApplicationParts(p => p.AddApplicationPart(typeof(ICartGrain).Assembly).WithReferences())
                 .ConfigureLogging(logging => logging.AddConsole())
                 .Build();

            client.Connect().Wait();
            return client;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
