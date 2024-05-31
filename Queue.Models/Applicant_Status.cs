using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Queue.Models
{
    public class Applicant_Status
    {
        public int ApplicantId { get; set; }
        [ForeignKey("ApplicantId")]
        [ValidateNever]
        public Applicant_Form? Applicant { get; set; }

        public string? Stage_1 { get; set; }
        public string? Stage_2 { get; set; }
        public string? Stage_3 { get; set; }

        public string? Stage1_Table { get; set; }
        public string? Stage2_Table { get; set; }
        public string? Stage3_Table { get; set; }

    }
}
