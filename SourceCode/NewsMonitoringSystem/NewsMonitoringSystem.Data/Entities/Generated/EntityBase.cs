using System;

namespace NewsMonitoringSystem.Data.Entities.Generated
{
    public class EntityBase
    {
        public DateTime? Inserted { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? LastModified{ get; set; }
        public string ModifiedBy { get; set; }
    }
}