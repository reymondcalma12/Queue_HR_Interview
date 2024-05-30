using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Queue.Models
{
    public class Table_Serve
    {
        public int TableId { get; set; }
        [ForeignKey("TableId")]
        [ValidateNever]
        public Table? Table { get; set; }

        public int? TotalPassed { get; set; }

        public int? TotalPooled { get; set; }

        public int? TotalFailed { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime Served_At { get; set; }

    }
}