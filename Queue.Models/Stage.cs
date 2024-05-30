using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Queue.Models
{
    public class Stage
    {
        [Key]
        public int? StageId { get; set; }

        public string? StageName { get; set; }

        public virtual ICollection<Table> tables { get; set; }

    }
}
