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
                        ProjetoId = (int)projetosReader["ID"],
                        Nome = projetosReader["NOME"].ToString(),
                        Descricao = projetosReader["descricao"].ToString(),
                        LinkGithub = projetosReader["linkgithub"].ToString(),
                        LinkWeb = projetosReader["linkweb"].ToString(),
                        Login = projetosReader["login"].ToString(),
                        Senha = projetosReader["senha"].ToString(),
                        Tecnologias = new List<Tecnologias>()
                    };

                    projetos.Add(projeto);
                }

                projetosReader.Close();

                // Consulta para buscar as tecnologias relacionadas a cada projeto
                string tecnologiasQuery = "SELECT PROJETO_ID, NOME_ARQUIVO, ID_TECNOLOGIA, NOME_TECNOLOGIA, ID_TIPOTECNOLOGIA, tipo_tecnologia_nome FROM vwTecnologiasProjeto";
                SqlCommand tecnologiasCommand = new SqlCommand(tecnologiasQuery, connection);

                SqlDataReader tecnologiasReader = tecnologiasCommand.ExecuteReader();

                while (tecnologiasReader.Read())
                {
                    int projetoId = (int)tecnologiasReader["PROJETO_ID"];
                    string nomeArquivo = tecnologiasReader["NOME_ARQUIVO"].ToString();
                    int tecnologiaId = (int)tecnologiasReader["ID_TECNOLOGIA"];
                    string nomeTecnologia = tecnologiasReader["NOME_TECNOLOGIA"].ToString();
                    int tipoTecnologiaId = (int)tecnologiasReader["ID_TIPOTECNOLOGIA"];
                    string nomeTipoTecnologia = tecnologiasReader["tipo_tecnologia_nome"].ToString();

                    // Verificar se o projeto já foi adicionado à lista
                    var projeto = projetos.Find(p => p.ProjetoId == projetoId);
                    if (projeto == null)
                    {
                        projeto = new Projetos
                        {
                            ProjetoId = projetoId,
                            Nome = tecnologiasReader["NOME"].ToString(),
                            Descricao = tecnologiasReader["descricao"].ToString(),
                            LinkGithub = tecnologiasReader["linkgithub"].ToString(),
                            LinkWeb = tecnologiasReader["linkweb"].ToString(),
                            Login = tecnologiasReader["login"].ToString(),
                            Senha = tecnologiasReader["senha"].ToString(),
                            Tecnologias = new List<Tecnologias>()
                        };
                        projetos.Add(projeto);
                    }

                    // Associar a tecnologia ao projeto correspondente
                    projeto.Tecnologias.Add(new Tecnologias
                    {
                        TecnologiaId = tecnologiaId,
                        NomeTecnologia = nomeTecnologia,
                        TipoTecnologiaId = tipoTecnologiaId,
                        NomeTipoTecnologia = nomeTipoTecnologia,
                        NomeArquivo = nomeArquivo,
                        Projeto = projeto
                    });
                }

                tecnologiasReader.Close();
            }

            // Buscar as tecnologias existentes no banco de dados para o filtro
            List<Tecnologias> tecnologiasDisponiveis = new List<Tecnologias>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string tecnologiasQuery = "SELECT * FROM vwTecnologias";
                SqlCommand tecnologiasCommand = new SqlCommand(tecnologiasQuery, connection);

                connection.Open();
                SqlDataReader tecnologiasReader = tecnologiasCommand.ExecuteReader();

                while (tecnologiasReader.Read())
                {
                    Tecnologias tecnologia = new Tecnologias
                    {
                        TecnologiaId = (int)tecnologiasReader["id"],
                        NomeTecnologia = tecnologiasReader["nome"].ToString(),
                        TipoTecnologiaId = (int)tecnologiasReader["id_tipotecnologia"],
                        NomeTipoTecnologia = tecnologiasReader["tipo_tecnologia_nome"].ToString(),
                        NomeArquivo = tecnologiasReader["nome_arquivo"].ToString()
                    };

                    tecnologiasDisponiveis.Add(tecnologia);
                }

                tecnologiasReader.Close();
            }

            ViewBag.TecnologiasDisponiveis = tecnologiasDisponiveis;

            return View(projetos);
        }


        // Método para filtrar os projetos por tecnologia
        public IActionResult FiltrarProjetosPorTecnologia(List<int> tecnologiaIds)
        {
            if (tecnologiaIds == null || tecnologiaIds.Count == 0)
            {
                // If no technology IDs were selected, redirect to the main page
                return RedirectToAction("Index");
            }

            List<Projetos> projetosFiltrados = new List<Projetos>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Montar a cláusula WHERE dinamicamente para filtrar por IDs de tecnologias selecionadas
                string whereClause = string.Join(" OR ", tecnologiaIds.Select(t => "ID_TECNOLOGIA = @tecnologia_" + t));

                // Consulta para buscar os projetos que possuem as tecnologias selecionadas
                string projetosQuery = "SELECT * FROM vwProjetos WHERE ID IN (SELECT PROJETO_ID FROM vwTecnologiasProjeto " +
                                       "WHERE " + whereClause + ")";
                SqlCommand projetosCommand = new SqlCommand(projetosQuery, connection);

                // Adicionar os parâmetros das IDs de tecnologias selecionadas à consulta
                for (int i = 0; i < tecnologiaIds.Count; i++)
                {
                    projetosCommand.Parameters.AddWithValue("@tecnologia_" + tecnologiaIds[i], tecnologiaIds[i]);
                }

                connection.Open();
                SqlDataReader projetosReader = projetosCommand.ExecuteReader();

                while (projetosReader.Read())
                {
                    Projetos projeto = new Projetos
                    {
                        ProjetoId = (int)projetosReader["ID"],
                        Nome = projetosReader["NOME"].ToString(),
                        Descricao = projetosReader["descricao"].ToString(),
                        LinkGithub = projetosReader["linkgithub"].ToString(),
                        LinkWeb = projetosReader["linkweb"].ToString(),
                        Login = projetosReader["login"].ToString(),
                        Senha = projetosReader["senha"].ToString(),
                        Tecnologias = new List<Tecnologias>() // Now the list is of Technology objects
                    };

                    projetosFiltrados.Add(projeto);
                }

                projetosReader.Close();

                // Consulta para buscar as tecnologias relacionadas aos projetos filtrados
                string tecnologiasQuery = "SELECT PROJETO_ID, NOME_ARQUIVO, ID_TECNOLOGIA FROM vwTecnologiasProjeto " +
                                          "WHERE PROJETO_ID IN (SELECT ID FROM vwProjetos WHERE ID IN (SELECT PROJETO_ID FROM vwTecnologiasProjeto " +
                                          "WHERE " + whereClause + "))";
                SqlCommand tecnologiasCommand = new SqlCommand(tecnologiasQuery, connection);

                // Adicionar os parâmetros das IDs de tecnologias selecionadas à consulta
                for (int i = 0; i < tecnologiaIds.Count; i++)
                {
                    tecnologiasCommand.Parameters.AddWithValue("@tecnologia_" + tecnologiaIds[i], tecnologiaIds[i]);
                }

                SqlDataReader tecnologiasReader = tecnologiasCommand.ExecuteReader();

                while (tecnologiasReader.Read())
                {
                    int projetoId = (int)tecnologiasReader["PROJETO_ID"];
                    string nomeArquivo = tecnologiasReader["NOME_ARQUIVO"].ToString();
                    int tecnologiaId = (int)tecnologiasReader["ID_TECNOLOGIA"];

                    // Associar a tecnologia ao projeto correspondente
                    var projeto = projetosFiltrados.Find(p => p.ProjetoId == projetoId);
                    if (projeto != null && !string.IsNullOrEmpty(nomeArquivo))
                    {
                        projeto.Tecnologias.Add(new Tecnologias
                        {
                            TecnologiaId = tecnologiaId,
                            NomeArquivo = nomeArquivo
                        });
                    }
                }

                tecnologiasReader.Close();
            }

            // Buscar as tecnologias existentes no banco de dados
            List<Tecnologias> tecnologiasDisponiveis = new List<Tecnologias>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string tecnologiasQuery = "SELECT * FROM Tecnologias";
                SqlCommand tecnologiasCommand = new SqlCommand(tecnologiasQuery, connection);

                connection.Open();
                SqlDataReader tecnologiasReader = tecnologiasCommand.ExecuteReader();

                while (tecnologiasReader.Read())
                {
                    Tecnologias tecnologiaa = new Tecnologias
                    {
                        TecnologiaId = (int)tecnologiasReader["id"],
                        NomeTecnologia = tecnologiasReader["nome"].ToString(),
                        NomeArquivo = tecnologiasReader["nome_arquivo"].ToString()
                    };

                    tecnologiasDisponiveis.Add(tecnologiaa);
                }

                tecnologiasReader.Close();
            }

            ViewBag.TecnologiasDisponiveis = tecnologiasDisponiveis;

            return View("Index", projetosFiltrados);
        }
    }

}

