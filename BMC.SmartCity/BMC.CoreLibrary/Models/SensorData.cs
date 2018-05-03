using System;
using System.Collections.Generic;
using System.Text;

namespace BMC.CoreLibrary.Models
{
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
