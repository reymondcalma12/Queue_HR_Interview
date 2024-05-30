using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Queue.Models
{
    public class Queue_Stage_1
    {
        public int ApplicantId { get; set; }
        [ForeignKey("ApplicantId")]
        [ValidateNever]
        public Applicant_Form? Applicant { get; set; }

        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        [ValidateNever]
        public Queue_Status? Status { get; set; }

        public int StageId { get; set; }
        [ForeignKey("StageId")]
        [ValidateNever]
        public Stage? Stage { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? TemporaryRejected_At { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime Generated_At { get; set; }
    }
}
