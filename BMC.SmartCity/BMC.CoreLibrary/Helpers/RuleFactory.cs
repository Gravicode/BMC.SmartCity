using BMC.CoreLibrary.Models;
using BMC.CoreLibrary.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BMC.CoreLibrary.Helpers
{
    public class RuleFactory
    {
        public List<Rule> Rules { get; set; }

        public RuleFactory(bool GetFromResources, string FilePath=null)
        {
            var JsonRule = "";
            if (GetFromResources)
            {
                JsonRule = Resources.rules;
            }
            Rules = JsonConvert.DeserializeObject<List<Rule>>(JsonRule);

        }

        public List<Alarm> ValidateRules(List<SensorData> DataSensor)
        {
            var alarms = new List<Alarm>();
            foreach(var item in DataSensor)
            {
                var Finds = ValidateRule(item);
                if (Finds.Count > 0)
                {
                    alarms.AddRange(Finds);
                }
            }
            return alarms;
        }
        public List<Alarm> ValidateRule(SensorData DataSensor)
        {
            int counter = 0;
            var alarms = new List<Alarm>();
            foreach (var rule in Rules)
            {
                if (rule.Enabled )
                {
                    foreach (var condition in rule.Conditions)
                    {
                        if (DataSensor.DeviceName == condition.Field)
                        {
                            switch (condition.Operator)
                            {
                                case ConditionOperators.Equal:
                                    if (DataSensor.DataValue == condition.Value)
                                    {
                                        alarms.Add(new Alarm() { Id = counter++, CreatedDate = DateTime.Now, DelegatedTo = "", Message = $"Sensor {DataSensor.DeviceName} value is equal to ({condition.Value})", RuleId = rule.Id, Status = "Open" });
                                    }
                                    break;
                                case ConditionOperators.GreaterEqualThan:
                                    if (DataSensor.DataValue >= condition.Value)
                                    {
                                        alarms.Add(new Alarm() { Id = counter++, CreatedDate = DateTime.Now, DelegatedTo = "", Message = $"Sensor {DataSensor.DeviceName} value is greater or equal than ({condition.Value})", RuleId = rule.Id, Status = "Open" });
                                    }
                                    break;

                                case ConditionOperators.GreaterThan:
                                    if (DataSensor.DataValue > condition.Value)
                                    {
                                        alarms.Add(new Alarm() { Id = counter++, CreatedDate = DateTime.Now, DelegatedTo = "", Message = $"Sensor {DataSensor.DeviceName} value is greater than ({condition.Value})", RuleId = rule.Id, Status = "Open" });
                                    }
                                    break;

                                case ConditionOperators.LowerEqualThan:
                                    if (DataSensor.DataValue <= condition.Value)
                                    {
                                        alarms.Add(new Alarm() { Id = counter++, CreatedDate = DateTime.Now, DelegatedTo = "", Message = $"Sensor {DataSensor.DeviceName} value is lower or equal than ({condition.Value})", RuleId = rule.Id, Status = "Open" });
                                    }
                                    break;

                                case ConditionOperators.LowerThan:
                                    if (DataSensor.DataValue < condition.Value)
                                    {
                                        alarms.Add(new Alarm() { Id = counter++, CreatedDate = DateTime.Now, DelegatedTo = "", Message = $"Sensor {DataSensor.DeviceName} value is lower than ({condition.Value})", RuleId = rule.Id, Status = "Open" });
                                    }
                                    break;

                            }
                        }
                    }
                }
            }
            return alarms;
        }
        public static void ConvertRuleToJson(List<Rule> datas, string PathFile)
        {
            FileInfo info = new FileInfo(PathFile);
            if (!info.Directory.Exists)
            {
                info.Directory.Create();
            }
            var Json = JsonConvert.SerializeObject(datas);
            File.WriteAllText(PathFile, Json);
        }
    }
}
