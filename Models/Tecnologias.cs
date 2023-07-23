namespace Site.Models
{
    public class Tecnologias
    {
        public int TecnologiaId { get; set; }

        public string NomeTecnologia { get; set; }

        public int TipoTecnologiaId { get; set; }

        public string NomeTipoTecnologia { get; set; }

        public string NomeArquivo { get; set; }

        // Aqui você pode adicionar uma propriedade para referenciar o projeto que possui essa tecnologia
        public Projetos Projeto { get; set; }
    }
}
