namespace Domain.Entities
{
    public class Auditoria
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string? NombreTabla { get; set; }
        public int? IdOperacion { get; set; }
        public string? OperacionRealizada { get; set; }
        public string? IdRegistro { get; set; }
        public string? ValorOriginal { get; set; }
        public string? ValorActual { get; set; }
        public string? UserName { get; set; }
    }
}
