using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OCloud.Entities
{
    public class CloudInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }

        public string ProviderName { get; set; }

        public string RefreshToken { get; set; }

        public string SpecificData { get; set; }
    }
}
