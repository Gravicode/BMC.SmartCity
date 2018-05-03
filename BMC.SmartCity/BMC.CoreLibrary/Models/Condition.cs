using System;
using System.Collections.Generic;
using System.Text;

namespace BMC.CoreLibrary.Models
{
    public enum ConditionOperators { GreaterThan,GreaterEqualThan, LowerThan,LowerEqualThan, Equal };
    public class Condition
    {
        public string Field { get; set; } = string.Empty;
        public ConditionOperators Operator { get; set; } = ConditionOperators.Equal;
        public double Value { get; set; } = 0;

        public Condition() { }
    }
}
