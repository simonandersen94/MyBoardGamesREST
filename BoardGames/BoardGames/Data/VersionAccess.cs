using BoardGames.Data.Interfaces;
using BoardGames.Model;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BoardGames.Data {
    public class VersionAccess : IVersionAccess {

        private readonly string? _connectionString;

        public VersionAccess(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DBAccessConnection");
        }

        public GameVersion Get(int id) {
            GameVersion? foundVersion = null;
            string sql =
                @"SELECT gv.versionId, gv.versionName, gv.minNoOfPlayers, gv.maxNoOfPlayers,
                g.gameId, g.gameName
                FROM gameVersion gv
                INNER JOIN game g ON gv.gameId = g.gameId
                WHERE gv.versionId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundVersion = connection.Query<GameVersion, Game, GameVersion>(sql, (gameVersion, game) => {
                    gameVersion.Game = game;
                    return gameVersion;
                }, new { id = id }, splitOn: "gameId").FirstOrDefault();
            }
            return foundVersion;
        }

        //This method is to be find in the GameController
        public List<GameVersion> GetVersionByGameId(int id) {
            List<GameVersion>? foundVersions = null;
            string sql =
                @"SELECT gv.versionId, gv.versionName, gv.minNoOfPlayers, gv.maxNoOfPlayers,
                g.gameId, g.gameName
                FROM gameVersion gv
                INNER JOIN game g ON gv.gameId = g.gameId
                WHERE gv.gameId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundVersions = connection.Query<GameVersion, Game, GameVersion>(sql, (gameVersion, game) => {
                    gameVersion.Game = game;
                    return gameVersion;
                }, new { id = id }, splitOn: "gameId").ToList();
            }
            return foundVersions;
        }

        public List<GameVersion> GetAll() {
            List<GameVersion>? foundVersions = null;
            string sql =
                @"SELECT gv.versionId, gv.gameId, gv.versionName, gv.minNoOfPlayers, gv.maxNoOfPlayers, g.gameId, g.gameName
                FROM gameversion gv
                INNER JOIN game g ON gv.gameId = g.gameId";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundVersions = connection.Query<GameVersion, Game, GameVersion>(sql, (gameVersion, game) => {
                    gameVersion.Game = game;
                    return gameVersion;
                }, splitOn: "gameId").ToList();
            }
            return foundVersions;
        }

        public bool Delete(int id) {
            int rowsAffected = -1;
            string sql =
                @"DELETE FROM gameVersion WHERE versionId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                rowsAffected = connection.Execute(sql, new { id });
            }
            return rowsAffected > 0;
        }

        public int Create(GameVersion version) {
            int insertedId = -1;
            string sql =
                @"INSERT INTO gameVersion (gameId, versionName, minNoOfPlayers, maxNoOfPlayers)
                OUTPUT INSERTED.versionId
                VALUES (@gameId, @versionName, @minNoOfPlayers, @maxNoOfPlayers)";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                try {
                    insertedId = connection.ExecuteScalar<int>(sql, new { version.Game.GameId, version.VersionName, version.MinNoOfPlayers, version.MaxNoOfPlayers });
                } catch {
                    insertedId = -1;
                }
            }
            return insertedId;
        }

        public bool Update(int id, GameVersion version) {
            int rowsAffected = -1;
            version.VersionId = id;
            string sql =
                @"UPDATE gameversion
                SET gameid = @gameId, versionName = @versionName, minNoOfPlayers = @minNoOfPlayers, maxNoOfPlayers = @maxNoOfPlayers
                WHERE versionId = @versionId";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                rowsAffected = connection.Execute(sql, new {
                    gameId = version.Game.GameId,
                    versionName = version.VersionName,
                    minNoOfPlayers = version.MinNoOfPlayers,
                    maxNoOfPlayers = version.MaxNoOfPlayers,
                    versionId = version.VersionId
                });
            }
            return rowsAffected > 0;
        }
    }
}
