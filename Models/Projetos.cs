using System.ComponentModel.DataAnnotations;

namespace Site.Models
{

    public class Projetos
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        public string descricao { get; set; }

        public string linkgithub { get; set; }

        public string linkweb { get; set; }

        public string login { get; set; }

        public string senha { get; set; }

        // Outros campos relevantes para a exibição no card
    }

}
