using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Site.Models;

namespace Site.Controllers
{
    public class ProjetosController : Controller
    {
        private readonly string _connectionString; // Substitua aqui pela sua string de conexão com o banco de dados

        public ProjetosController()
        {
            _connectionString = "Data Source=SQL5085.site4now.net;Initial Catalog=db_a9c2c8_tarefas;User ID=db_a9c2c8_tarefas_admin;Password=Vitoriade10.;";
        }

        public IActionResult Index()
        {
            List<Projetos> projetos = new List<Projetos>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Consulta para buscar os projetos
                string projetosQuery = "SELECT * FROM vwProjetos";
                SqlCommand projetosCommand = new SqlCommand(projetosQuery, connection);

                connection.Open();
                SqlDataReader projetosReader = projetosCommand.ExecuteReader();

                while (projetosReader.Read())
                {
                    Projetos projeto = new Projetos
                    {
                        id = (int)projetosReader["ID"],
                        nome = projetosReader["NOME"].ToString(),
                        descricao = projetosReader["descricao"].ToString(),
                        linkgithub = projetosReader["linkgithub"].ToString(),
                        linkweb = projetosReader["linkweb"].ToString(),
                        login = projetosReader["login"].ToString(),
                        senha = projetosReader["senha"].ToString(),
                        tecnologias = new List<string>()
                    };

                    projetos.Add(projeto);
                }

                projetosReader.Close();

                // Consulta para buscar as tecnologias relacionadas a cada projeto
                string tecnologiasQuery = "SELECT PROJETO_ID, NOME_ARQUIVO FROM vwTecnologiasProjeto";
                SqlCommand tecnologiasCommand = new SqlCommand(tecnologiasQuery, connection);

                SqlDataReader tecnologiasReader = tecnologiasCommand.ExecuteReader();

                while (tecnologiasReader.Read())
                {
                    int projetoId = (int)tecnologiasReader["PROJETO_ID"];
                    string nomeArquivo = tecnologiasReader["NOME_ARQUIVO"].ToString();

                    // Associar a tecnologia ao projeto correspondente
                    var projeto = projetos.Find(p => p.id == projetoId);
                    if (projeto != null && !string.IsNullOrEmpty(nomeArquivo))
                    {
                        projeto.tecnologias.Add(nomeArquivo);
                    }
                }

                tecnologiasReader.Close();
            }

            return View(projetos);
        }

    }
}
