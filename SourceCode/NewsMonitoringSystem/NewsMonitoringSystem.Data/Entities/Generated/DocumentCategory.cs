using System;
using System.ComponentModel.DataAnnotations;

namespace NewsMonitoringSystem.Data.Entities.Generated
{
    public partial class DocumentCategory : EntityBase
    {
        [Key]
        public Guid DocumentCategoryId { get; set; }

        public int Value { get; set; }

        public string Description { get; set; }
    }
}