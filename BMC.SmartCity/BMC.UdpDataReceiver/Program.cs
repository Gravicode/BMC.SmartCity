using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Http;
using System.Text;
using InfluxDB.Collector;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using BMC.CoreLibrary.Models;
using BMC.CoreLibrary.Helpers;

namespace BMC.UdpDataReceiver
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {

            Console.WriteLine("Start receiving UDP from Node Device..");
          
            Setup();
            Task loop = new Task(new Action(Loop));
            loop.Start();
            Console.ReadLine();
        }

        static void Loop()
        {
            UdpClient udpServer = new UdpClient(int.Parse(GlobalConfig.UdpPort));
            var Validator = new RuleFactory(true);

            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, int.Parse(GlobalConfig.UdpPort));
                var data = udpServer.Receive(ref remoteEP); // listen on port 8888

                var datastr = System.Text.Encoding.Default.GetString(data);
                Console.WriteLine("receive data from " + remoteEP.ToString());
                Console.WriteLine("data: " + datastr);
                var obj = JsonConvert.DeserializeObject<SensorData>(datastr);
                if (obj != null)
                {
                    obj.CreatedDate = DateTime.Now;
                    //call power bi api
                    //SendToPowerBI(sensorValue);
                    SendToInfluxDB(obj);

                    //check rule
                    var violated = Validator.ValidateRule(obj);
                    if (violated.Count > 0)
                    {
                       foreach(var vItem in violated)
                        {
                            Console.WriteLine($"--> alarm triggered [{vItem.CreatedDate}] : {vItem.Message}");
                        }
                    }
                    Thread.Sleep(2000);
                }



            }


        }
        private static HttpClient _client;

        public static HttpClient client
        {
            get
            {
                if (_client == null) _client = new HttpClient();
                return _client;
            }

        }
        static void Setup()
        {
            Console.OutputEncoding = Encoding.UTF8;

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (String.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");

            Console.WriteLine("Environment: {0}", environment);


            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);
            if (environment == "Development")
            {

                builder
                    .AddJsonFile(
                        Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
                        optional: true
                    );
            }
            else
            {
                builder
                    .AddJsonFile($"appsettings.{environment}.json", optional: true);
            }
            //add env vars
            builder.AddEnvironmentVariables();
            //get config
            Configuration = builder.Build();

            //rediscon = Configuration.GetConnectionString("RedisCon");
            GlobalConfig.InfluxDBUrl = Configuration.GetSection("server").GetSection("influxdb-url").Value;
            GlobalConfig.InfluxDBPort = Configuration.GetSection("server").GetSection("influxdb-port").Value;
            GlobalConfig.InfluxDBUser = Configuration.GetSection("server").GetSection("influxdb-user").Value;
            GlobalConfig.InfluxDBPassword = Configuration.GetSection("server").GetSection("influxdb-password").Value;
            GlobalConfig.InfluxDBName = Configuration.GetSection("server").GetSection("influxdb-name").Value;
            GlobalConfig.UdpPort = Configuration.GetSection("server").GetSection("udp-port").Value;


            Metrics.Collector = new CollectorConfiguration()
 .Tag.With("host", Environment.GetEnvironmentVariable("COMPUTERNAME"))
 .Batch.AtInterval(TimeSpan.FromSeconds(2))
                .WriteTo.InfluxDB(GlobalConfig.InfluxDBUrl + ":" + GlobalConfig.InfluxDBPort, GlobalConfig.InfluxDBName)
 .CreateCollector();
        }
        static void SendToInfluxDB(SensorData data)
        {
            Metrics.Increment("iterations");

            Metrics.Write(data.DeviceName,
                new Dictionary<string, object>
                {
                { "value",data.DataValue },
                { "type", data.SensorType },
                { "status", data.Status },
                { "deviceid", data.DeviceID },

                });

            //Metrics.Measure(data.SensorType, data.DataValue);
        }
        static async void SendToPowerBI(SensorData data)
        {
            var url = "https://api.powerbi.com/beta/e4a5cd36-e58f-4f98-8a1a-7a8e545fc65a/datasets/c3152879-fd74-4ba6-94aa-2b9b7111005f/rows?key=AUL%2FVTsGwsmJGzP28v7ah5EInDrjg7rTXx4b1IarBiTTcuB62zXzkG8QGoCZuJwyICzydqAT6ieTzGxsMXMETQ%3D%3D";
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var res = await client.PostAsync(url, content, CancellationToken.None);
            if (res.IsSuccessStatusCode)
            {
                Console.WriteLine("data sent to power bi - " + DateTime.Now);
            }
        }
    }

    
}
