using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsMonitoringSystem.Data.Entities.Generated
{
    public partial class Document : EntityBase
    {
        [Key]
        [Column(Order = 0)]
        public Guid DocumentId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int Revision { get; set; }
        
        public string DocumentIdentifier { get; set; }

        public string Title { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string Link { get; set; }

        public string HtmlContent { get; set; }

        public string TextContent { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }
    }
}