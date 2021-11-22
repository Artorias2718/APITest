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
using System.Threading.Tasks;
using APITest.Utilities;

namespace APITest
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private static int Main(string[] args)
        {
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
                    var oResponse1 = APIManager.QueryRest("https://jsonplaceholder.typicode.com", "todos", Method.GET);
                    var oResponse2 = APIManager.QueryGraphQL("https://graphqlzero.almansi.me/api", Utility.JsonSerializeObject(new
                    {
                        query = @"{
                            user(id: 1){
                                id
                                name
                            }
                        }",
                        variables = new
                        {

                        }
                    }));
                    Console.WriteLine(Utility.JsonSerializeObject(oResponse1.Content, true));
                    Console.WriteLine();
                    Console.WriteLine(Utility.JsonSerializeObject(oResponse2.Content, true));
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
