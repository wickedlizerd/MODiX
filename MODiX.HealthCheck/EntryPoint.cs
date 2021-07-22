using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Modix.HealthCheck
{
    public static class EntryPoint
    {
        public static async Task<int> Main(string[] args)
        {
            if (args.Length is 0)
            {
                Console.WriteLine("No health-check paths were given");
                return 1;
            }

            using var client = new HttpClient();

            foreach (var path in args)
            {
                var response = await client.GetAsync(args[0]);

                Console.WriteLine(await response.Content.ReadAsStringAsync());

                if (response.StatusCode is not HttpStatusCode.OK)
                    return 1;
            }

            return 0;
        }
    }
}
