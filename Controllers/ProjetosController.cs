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
                string sqlQuery = "SELECT id, nome, descricao, linkgithub, linkweb, login, senha FROM Projetos";

                SqlCommand command = new SqlCommand(sqlQuery, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Projetos projeto = new Projetos
                    {
                        id = (int)reader["id"],
                        nome = reader["nome"].ToString(),
                        descricao = reader["descricao"].ToString(),
                        linkgithub = reader["linkgithub"].ToString(),
                        linkweb = reader["linkweb"].ToString(),
                        login = reader["login"].ToString(),
                        senha = reader["senha"].ToString()
                    };

                    projetos.Add(projeto);
                }

                reader.Close();
            }

            return View(projetos);
        }
    }
}
