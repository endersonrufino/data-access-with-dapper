using Animes.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Animes;

class Program
{
    static void Main(string[] args)
    {
        const string connectionString = "Data Source=DESKTOP-FVOS0IE\\SQLEXPRESS;Initial Catalog=qwerty;Integrated Security=True;TrustServerCertificate=True";

        using (var connection = new SqlConnection(connectionString))
        {
            //CreateAnime(connection);
            //UpdateAnime(connection);
            //GetAnimes(connection);
            //ExecuteProcedure(connection);
            //ExecuteReadProcedure(connection);
            //ExecuteScalar(connection);
            //ReadView(connection);
            //OneToOne(connection);
            //OneToMany(connection);
            //QueryMultiple(connection);
            //SelectIn(connection);
            //Like(connection, "zo");
            Transaction(connection);
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

        connection.Execute(query, new { id = 9, AnimeName = "Boruto" });
    }

    static void ExecuteProcedure(SqlConnection connection)
    {
        var procedure = "DELETE_ANIME";
        var parameters = new { Id = 9 };

        connection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
    }

    static void ExecuteReadProcedure(SqlConnection connection)
    {
        var procedure = "SELECT_ANIME";

        var animes = connection.Query(procedure, commandType: CommandType.StoredProcedure);

        foreach (var anime in animes)
        {
            Console.WriteLine(anime.AnimeName);
        }
    }

    static void ExecuteScalar(SqlConnection connection)
    {
        var anime = new Anime();
        anime.AnimeName = "Naruto";

        var query = "INSERT INTO [Anime] VALUES (@AnimeName) SELECT SCOPE_IDENTITY()";

        var id = connection.ExecuteScalar<int>(query, new { anime.AnimeName });

        Console.WriteLine(id);
    }

    static void ReadView(SqlConnection connection)
    {

        var query = "SELECT * FROM [vwAnimes]";

        var animes = connection.Query(query);

        foreach (var anime in animes)
        {
            Console.WriteLine($"{anime.Id} - {anime.AnimeName}");
        }
    }

    static void OneToOne(SqlConnection connection)
    {

        var query = "SELECT * FROM [Anime] A INNER JOIN [Character] C ON A.Id = C.AnimeId";

        var characters = connection.Query<Anime, Character, Character>(query,
        (anime, character) =>
        {
            character.Anime = anime;
            return character;
        }, splitOn: "Id");

        foreach (var character in characters)
        {
            Console.WriteLine($"Anime: {character.Anime.AnimeName} - Personagem: {character.Name}");
        }
    }

    static void OneToMany(SqlConnection connection)
    {

        var query = @"SELECT [Anime].[Id], [Anime].[AnimeName], [Character].[Id], [Character].[Name], [Character].[AnimeId]
                      FROM [Anime] INNER JOIN [Character] 
                      ON [Anime].[Id] = [Character].[AnimeId] 
                      ORDER BY [Character].[Name]"
        ;

        var animes = new List<Anime>();

        var items = connection.Query<Anime, Character, Anime>(query,
        (anime, character) =>
        {
            var ani = animes.FirstOrDefault(x => x.Id == anime.Id);
            if (ani == null)
            {
                ani = anime;
                ani.Characters.Add(character);
                animes.Add(ani);
            }
            else
            {
                ani.Characters.Add(character);
            }

            return anime;
        }, splitOn: "Id");

        foreach (var anime in animes)
        {
            Console.WriteLine($"*************** Anime: {anime.AnimeName} ***************");
            Console.WriteLine($"*************** Personangens***************");

            foreach (var character in anime.Characters)
            {
                Console.WriteLine(character.Name);
            }
        }
    }

    static void QueryMultiple(SqlConnection connection)
    {
        var query = "SELECT * FROM [Anime]; SELECT * FROM [Character]";

        using (var multi = connection.QueryMultiple(query))
        {
            var animes = multi.Read<Anime>();
            var characters = multi.Read<Character>();

            foreach (var anime in animes)
            {
                Console.WriteLine(anime.AnimeName);
            }

            foreach (var character in characters)
            {
                Console.WriteLine(character.Name);
            }
        }
    }

    static void SelectIn(SqlConnection connection)
    {
        var query = "select * from [Character] where Id in @Id";

        var characters = connection.Query<Character>(query, new
        {
            Id = new[]{
                1, 2
            }
        });

        foreach (var character in characters)
        {
            Console.WriteLine(character.Name);
        }
    }

    static void Like(SqlConnection connection, string term)
    {
        var query = "select * from [Character] where Name like @exp";

        var characters = connection.Query<Character>(query, new
        {
            exp = $"%{term}%"
        });

        foreach (var character in characters)
        {
            Console.WriteLine(character.Name);
        }
    }

    static void Transaction(SqlConnection connection)
    {
        var anime = new Anime();
        anime.AnimeName = "Naruto";

        var query = "INSERT INTO [Anime] VALUES (@AnimeName)";

        using (var transaction = connection.BeginTransaction())
        {
            connection.Execute(query, new { anime.AnimeName }, transaction);

            transaction.Commit();
        }        
    }
}