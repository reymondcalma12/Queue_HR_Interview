using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Queue.Models
{
    public class Table
    {
        [Key]
        [Required(ErrorMessage = "Please select a table.")]
        public int TableId { get; set; }

        [Required]
        [ValidateNever]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please select a stage.")]
        public int StageId { get; set; }
        [ForeignKey("StageId")]
        [ValidateNever]
        [JsonIgnore]
        public Stage? Stage { get; set; }

    }
}