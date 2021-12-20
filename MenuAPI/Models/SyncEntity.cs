using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuAPI.Models
{
    public class SyncEntity
    {
        public Guid Id { get; set; }
        public DateTime LastChangedAt { get; set; }
        public string JsonData { get; set; }
        public string SyncType { get; set; }
        public string ObjectType { get; set; }
        public string Origin { get; set; }
    }
}
