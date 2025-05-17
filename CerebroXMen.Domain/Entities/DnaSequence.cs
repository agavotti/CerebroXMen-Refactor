namespace CerebroXMen.Domain.Entities
{
    public class DnaSequence
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string[] Sequence { get; set; } = Array.Empty<string>();
        public bool IsMutant { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
