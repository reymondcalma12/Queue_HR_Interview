using System.ComponentModel.DataAnnotations;


namespace Queue.Models
{
    public class Queue_Status
    {
        [Key]
        public int? StatusId { get; set; }

        public string? StatusName { get; set; }

    }
}
