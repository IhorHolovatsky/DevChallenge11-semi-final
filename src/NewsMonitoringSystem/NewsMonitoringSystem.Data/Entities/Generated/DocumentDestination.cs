using System;
using System.ComponentModel.DataAnnotations;

namespace NewsMonitoringSystem.Data.Entities.Generated
{
    public partial class DocumentDestination : EntityBase
    {
        [Key]
        public Guid DocumentDestinationId { get; set; }

        public string Description { get; set; }
    }
}