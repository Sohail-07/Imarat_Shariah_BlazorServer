namespace Imarat_Shariah.Data.Entities
{
    public class Siyajat : BaseEntity
    {
        public int Id { get; set; }
        public int FormNo { get; set; }
        public int QazatNo { get; set; }
        public string? FormType { get; set; }
        public DateTime NikahDate { get; set; } = DateTime.Now;
        public string? GroomName { get; set; }
        public string? BrideName { get; set; }
        public string? QariName { get; set; }
    }
}
