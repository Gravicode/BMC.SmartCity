using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace BMC.Simulator
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Data Sensor Simulator\r\nPress Any Key to Stop.");
            Setup();
            Task loop = new Task(new Action(Loop));
            loop.Start();
            Console.ReadLine();
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
            GlobalConfig.UdpServer = Configuration.GetSection("server").GetSection("udp-server").Value;



        }

        static void Loop()
        {

            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(GlobalConfig.UdpServer), int.Parse(GlobalConfig.UdpPort)); // endpoint where server is listening
            client.Connect(ep);
            SensorData data = new SensorData();
            Random rnd = new Random();
            data.DeviceID = 99;
            data.DeviceName = "Water Flow Sensor";
            data.SensorType = "FlowSensor";
            data.Status = "Active";
            long counter = 0;
            while (true)
            {
                try
                {
                    data.CreatedDate = DateTime.Now;
                    data.DataValue = rnd.Next(1, 100);
                    byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
                    // send data
                    client.Send(bytes, bytes.Length);
                    Console.WriteLine($"Send Data Count : {counter++}");
                    // then receive data
                    //var receivedData = client.Receive(ref ep);

                    //Console.Write("receive data from " + ep.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "_" + ex.StackTrace);
                }
                Thread.Sleep(2000);
            }


        }
    }

    public class SensorData
    {
        public DateTime CreatedDate { get; set; }
        public double DataValue { get; set; }
        public string DeviceName { get; set; }
        public long DeviceID { get; set; }
        public string SensorType
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
    }
}
