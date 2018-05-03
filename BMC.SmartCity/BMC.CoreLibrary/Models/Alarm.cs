using System;
using System.Collections.Generic;
using System.Text;

namespace BMC.CoreLibrary.Models
{
    public class Alarm
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ResolveDate { get; set; }
        public long Id { get; set; }
        public long RuleId { get; set; }
        public string Message { get; set; }
        public string DelegatedTo { get; set; }
        public string Status { get; set; }
    }
}
