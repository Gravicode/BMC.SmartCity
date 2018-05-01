using System;
namespace BMC.UdpDataReceiver
{
    public class GlobalConfig
    {
        public GlobalConfig()
        {
            

        }
        public static string InfluxDBUrl
        {
            get;
            set;
        }
        public static string InfluxDBName
        {
            get;
            set;
        }

        public static string InfluxDBPort
        {
            get;
            set;
        }
        public static string InfluxDBUser
        {
            get;
            set;
        }
        public static string InfluxDBPassword
        {
            get;
            set;
        }
        public static string UdpPort
        {
            get;
            set;
        }
    }
}
