using Animes.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Animes;

class Program
{
    static void Main(string[] args)
    {
        const string connectionString = "Data Source=DESKTOP-FVOS0IE\\SQLEXPRESS;Initial Catalog=qwerty;Integrated Security=True;TrustServerCertificate=True";

        using (var connection = new SqlConnection(connectionString))
        {
            //CreateAnime(connection);
            UpdateAnime(connection);
            GetAnimes(connection);
        }
    }

    static void GetAnimes(SqlConnection connection)
    {
        var animes = connection.Query<Anime>("SELECT [Id], [AnimeName] FROM Anime").ToList();

        foreach (var item in animes)
        {
            Console.WriteLine($"{item.Id} - {item.AnimeName}");
        }
    }

    static void CreateAnime(SqlConnection connection)
    {
        var anime = new Anime();
        anime.AnimeName = "Naruto";

        var query = "INSERT INTO [Anime] VALUES (@AnimeName)";

        connection.Execute(query, new { anime.AnimeName });
    }

    static void UpdateAnime(SqlConnection connection)
    {
        var query = "UPDATE [Anime] SET [AnimeName]=@AnimeName WHERE [Id]=@id";

        connection.Execute(query, new { id = 9,  AnimeName = "Boruto"});
    }
}