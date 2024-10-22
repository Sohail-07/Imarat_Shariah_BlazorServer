using System.ComponentModel.DataAnnotations;

namespace Imarat_Shariah.Data.Entities
{
    public class Khula : BaseEntity
    {
        public int Id { get; set; }
        
        [Required]
        public string? FormNumber { get; set; }

        [Required]
        public DateTime NikahDate { get; set; } = DateTime.Now;
        
        [Required]
        public string? GroomName { get; set; }
        
        [Required]
        public string? BrideName { get; set; }
    }
}
