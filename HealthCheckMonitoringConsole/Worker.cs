using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net;

namespace HealthCheckMonitoringConsole
{
    internal class Worker
    {
        private readonly IConfiguration configuration;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void DoWork()
        {
            Log.Information(configuration.GetSection("HealthCheckEndpoint")["DB2Endpoint"]);
            while (1 == 1)
            {
                try
                {
                    var response = Get(configuration.GetSection("HealthCheckEndpoint")["DB2Endpoint"]);
                    Log.Information(response);
                }
                catch (Exception ex)
                {
                    int i = 1;
                    Log.Error(ex.Message);
                    //var success = false;
                    //while (!success && i <= 3)
                    //{
                    //    try
                    //    {
                    //        Log.Information("Retry On Error. Attempt : " + i);
                    //        var response = Get(configuration.GetSection("HealthCheckEndpoint")["DB2Endpoint"]);
                    //        success = true;
                    //    }
                    //    catch {
                    //        Log.Error(ex.ToString());
                    //    }
                    //    i++;
                    //}                    
                }
                Thread.Sleep(60000);
            }
        }

        public string Get(string uri)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
