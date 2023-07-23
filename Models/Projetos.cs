namespace Site.Models
{
    public class Projetos
    {
        public int ProjetoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string LinkGithub { get; set; }
        public string LinkWeb { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public List<Tecnologias> Tecnologias { get; set; }
    }
}
