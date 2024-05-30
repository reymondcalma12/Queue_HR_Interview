using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Queue.Models
{
    public class Applicant_Reject
    {
        public int ApplicantId { get; set; }
        [ForeignKey("ApplicantId")]
        [ValidateNever]
        public Applicant_Form? Applicant { get; set; }

        public int StageId { get; set; }
        [ForeignKey("StageId")]
        [ValidateNever]
        public Stage? Stage { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime Rejected_At { get; set; }

    }
}
