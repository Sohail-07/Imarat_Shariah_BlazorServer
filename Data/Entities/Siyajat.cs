using System.ComponentModel.DataAnnotations;

namespace Imarat_Shariah.Data.Entities
{
    public class Siyajat : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string FormNo { get; set; }
        
        [Required]
        public string QazatNo { get; set; }
        
        [Required]
        public string FormType { get; set; }
        
        [Required]
        public DateTime NikahDate { get; set; } = DateTime.Now;
        
        [Required]
        public string GroomName { get; set; }
        
        [Required]
        public string BrideName { get; set; }
        
        [Required]
        public string QariName { get; set; }
    }
}
