using BoardGames.Data.Interfaces;
using BoardGames.Model;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BoardGames.Data {
    public class GameAccess : IGameAccess {
        private readonly string? _connectionString;

        public GameAccess(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DBAccessConnection");
        }

        public Game Get(int id) {
            Game? foundGame = null;
            string sql =
                @"SELECT g.gameId, g.gameName
                FROM game g
                WHERE gameId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundGame = connection.Query<Game>(sql, new { id = id }).FirstOrDefault();
            }
            return foundGame;
        }

        public List<Game> GetGameByNoOfPlayers(int no) {
            List<Game>? foundGames = null;
            string sql =
                @"SELECT DISTINCT g.gameId, g.gameName
                FROM game g
                INNER JOIN gameVersion gv ON g.gameId = gv.gameId
                WHERE @no BETWEEN gv.minNoOfPlayers AND gv.maxNoOfPlayers";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundGames = connection.Query<Game>(sql, new { no = no }).ToList();
            }
            return foundGames;
        }

        public List<Game> GetAll() {
            List<Game> foundGames = null;
            string sql =
                @"SELECT gameId, gameName
                FROM Game";
            
            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundGames = connection.Query<Game>(sql).ToList();
            }
            return foundGames;
        }

        public bool Delete(int id) {
            int rowsAffected = -1;
            string sql =
                @"DELETE FROM game WHERE gameId = @id 
                DELETE FROM gameversion WHERE gameId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                rowsAffected = connection.Execute(sql, new { id });
            }
            return rowsAffected > 0;
        }

        public int Create(Game game) {
            int insertedId = -1;
            string sql =
                @"INSERT INTO game (gameName)
                OUTPUT INSERTED.gameId
                VALUES (@gameName)";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                try {
                    insertedId = connection.ExecuteScalar<int>(sql, game);
                } catch {
                    insertedId = -1;
                }
            }
            return insertedId;
        }

        public bool Update(int id, Game game) {
            int rowsAffected = -1;
            game.GameId = id;
            string sql =
                @"UPDATE game
                SET gameName = @gameName
                WHERE gameId = @gameId";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                rowsAffected = connection.Execute(sql, new {
                    gameName = game.GameName,
                    gameId = game.GameId
                });
            }
            return rowsAffected > 0;
        }

        public int GetAmount() {
            int amountOfGames = 0;
            string sql =
                @"SELECT COUNT(versionid)
                FROM GameVersion";
            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                amountOfGames = (int)command.ExecuteScalar();
            }
            return amountOfGames;
        }
    }
}
