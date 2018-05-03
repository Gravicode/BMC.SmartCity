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
using BMC.CoreLibrary.Models;
using BMC.CoreLibrary.Helpers;

namespace BMC.Simulator
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Data Sensor Simulator\r\nPress Any Key to Stop.");
            Setup();
            //Rule rule = new Rule() { Id=1, Description="Check water flow threshold", Enabled=true, GroupId=1, Name="Rule Water Flow", Severity="High", Conditions=new List<Condition>()  { new Condition() { Field="Water Flow Sensor", Operator=ConditionOperators.GreaterThan, Value=60 } } };
            //Rule rule2 = new Rule() { Id = 2, Description = "Check water height threshold", Enabled = true, GroupId = 2, Name = "Rule Water Height", Severity = "High", Conditions = new List<Condition>() { new Condition() { Field = "Water Height Sensor", Operator = ConditionOperators.GreaterEqualThan, Value = 90 } } };
            //RuleFactory.ConvertRuleToJson(new List<Rule>() { rule, rule2 }, @"c:\temp\rules.json");
            
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
            SensorData data2 = new SensorData();
            Random rnd = new Random();
            data.DeviceID = 99;
            data.DeviceName = "Water Flow Sensor";
            data.SensorType = "FlowSensor";
            data.Status = "Active";

            data2.DeviceID = 98;
            data2.DeviceName = "Water Height Sensor";
            data2.SensorType = "DistanceSensor";
            data2.Status = "Active";
            long counter = 0;
            while (true)
            {
                try
                {
                    //sensor 1
                    data.CreatedDate = DateTime.Now;
                    data.DataValue = rnd.Next(1, 100);
                    byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
                    // send data
                    client.Send(bytes, bytes.Length);
                    //sensor 2
                    data2.CreatedDate = DateTime.Now;
                    data2.DataValue = rnd.Next(50, 120);
                    byte[] bytes2 = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data2));
                    // send data
                    client.Send(bytes2, bytes2.Length);

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

    
}
