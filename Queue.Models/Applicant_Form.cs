using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Queue.Models
{
    public class Applicant_Form
    {
        [Key]
        public int? ApplicantId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        [Column(TypeName = "nvarchar(20)")]
        public string? ContactNumber { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        public string? Position { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime Created_At { get; set; }
    }
}