using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCloud.Entities
{
    public class FileInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        [ForeignKey("CloudInfo")]
        public Guid CloudId { get; set; }
    }
}
