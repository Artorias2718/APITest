//using APITest.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace APITest
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private static int Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                // Start!
                // Create service collection
                Console.WriteLine("Creating service collection");
                ServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                // Create service provider
                Console.WriteLine("Building service provider");
                IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

                try
                {
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error running service {ex.Message}");
                }
                finally
                {
                }

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging();

            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(Configuration);
        }
    }
}
