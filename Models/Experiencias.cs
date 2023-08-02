namespace Site.Models
{
    public class Experiencias
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
