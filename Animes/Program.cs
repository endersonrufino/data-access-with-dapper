using Animes.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Animes;

class Program
{
    static void Main(string[] args)
    {
        const string connectionString = "Data Source=DESKTOP-FVOS0IE\\SQLEXPRESS;Initial Catalog=qwerty;Integrated Security=True;TrustServerCertificate=True";

        using(var connection = new SqlConnection(connectionString)){
            var animes = connection.Query<Anime>("SELECT [Id], [AnimeName] FROM Anime").ToList();
        }
    }
}