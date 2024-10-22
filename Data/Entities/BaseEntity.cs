namespace Imarat_Shariah.Data.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? PDFPath { get; set; }
        public DateTime? DeletedDate { get; set; } 
        public bool IsActive { get; set; } = true;
    }
}
