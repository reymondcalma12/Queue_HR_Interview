using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Queue.Models
{
    public class Serving
    {
        public int TableId { get; set; }
        [ForeignKey("TableId")]
        [ValidateNever]
        public Table? Table { get; set; }

        public int ApplicantId { get; set; }
        [ForeignKey("ApplicantId")]
        [ValidateNever]
        public Applicant_Form? Applicant { get; set; }

        public int StageId { get; set; }
        [ForeignKey("StageId")]
        [ValidateNever]
        public Stage? Stage { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime Served_At { get; set; }

    }
}
