using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAppService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine(Environment.GetEnvironmentVariable("PORT"));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder
                    .UseKestrel()
                    .UseUrls(String.Concat("http://*:", Environment.GetEnvironmentVariable("PORT")))
                    .UseStartup<Startup>(); });
    }
}