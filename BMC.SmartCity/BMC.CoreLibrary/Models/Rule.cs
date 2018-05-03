using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMC.CoreLibrary.Models
{
    public class Rule : IComparable<Rule>
    {
        private const string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";
        [JsonIgnore] //comes from the StorageAdapter document and not the serialized rule
        public string ETag { get; set; } = string.Empty;
        [JsonIgnore] //comes from the StorageAdapter document and not the serialized rule
        public long Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTimeOffset.UtcNow.Date;//.ToString(DATE_FORMAT);
        public DateTime DateModified { get; set; } = DateTimeOffset.UtcNow.Date;//.ToString(DATE_FORMAT);
        public bool Enabled { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        public int GroupId { get; set; } = 0;// string.Empty;
        public string Severity { get; set; } = string.Empty;
        public IList<Condition> Conditions { get; set; } = new List<Condition>();

        public Rule() { }

        public int CompareTo(Rule other)
        {
            if (other == null) return 1;

            return other.DateCreated.CompareTo(this.DateCreated);
                //DateTimeOffset.Parse(other.DateCreated).CompareTo(DateTimeOffset.Parse(this.DateCreated));
        }
    }
}
