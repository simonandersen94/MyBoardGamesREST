using BoardGames.Data.Interfaces;
using BoardGames.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace BoardGames.Data {
    public class CharacterAccess : ICharacterAccess {
        private readonly string? _connectionString;

        public CharacterAccess(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DBAccessConnection");
        }

        public GameCharacter Get(int id) {
            GameCharacter? foundCharacter = null;
            string sql =
                @"SELECT ch.characterId, ch.versionId, ch.characterName,
                gv.versionId, gv.gameId, gv.versionName,
                gv.minNoOfPlayers, gv.maxNoOfPlayers,
                g.gameId, g.gameName
                FROM Characters ch
                INNER JOIN Gameversion gv ON ch.versionId = gv.versionId
                LEFT JOIN Game g ON gv.gameId = g.gameId
                WHERE ch.characterId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundCharacter = connection.Query<GameCharacter, GameVersion, Game, GameCharacter>(sql, (gameCharacter, gameVersion, game) => {
                    gameCharacter.GameVersion = gameVersion;
                    gameVersion.Game = game;
                    return gameCharacter;
                }, new { id = id }, splitOn: "versionId, gameId").FirstOrDefault();
            }
            return foundCharacter;
        }

        //This method is to be find in the VersionController
        public List<GameCharacter> GetCharacterByVersionId(int id) {
            List<GameCharacter>? foundCharacters = null;
            string sql =
                @"SELECT ch.characterId, ch.versionId, ch.characterName,
                gv.versionId, gv.gameId, gv.versionName,
                gv.minNoOfPlayers, gv.maxNoOfPlayers,
                g.gameId, g.gameName
                FROM Characters ch
                INNER JOIN Gameversion gv ON ch.versionId = gv.versionId
                LEFT JOIN Game g ON gv.gameId = g.gameId
                WHERE ch.versionId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundCharacters = connection.Query<GameCharacter, GameVersion, Game, GameCharacter>(sql, (gameCharacter, gameVersion, game) => {
                    gameCharacter.GameVersion = gameVersion;
                    gameVersion.Game = game;
                    return gameCharacter;
                }, new { id = id }, splitOn: "versionId, gameId").ToList();
            }
            return foundCharacters;
        }

        public List<GameCharacter> GetAll() {
            List<GameCharacter>? foundCharacters = null;
            string sql =
                @"SELECT ch.characterId, ch.versionId, ch.characterName,
                gv.versionId, gv.gameId, gv.versionName,
                gv.minNoOfPlayers, gv.maxNoOfPlayers,
                g.gameId, g.gameName
                FROM Characters ch
                INNER JOIN Gameversion gv ON ch.versionId = gv.versionId
                LEFT JOIN Game g ON gv.gameId = g.gameId";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                foundCharacters = connection.Query<GameCharacter, GameVersion, Game, GameCharacter>(sql, (gameCharacter, gameVersion, game) => {
                    gameCharacter.GameVersion = gameVersion;
                    gameVersion.Game = game;
                    return gameCharacter;
                }, splitOn: "versionId, gameId").ToList();
            }
            return foundCharacters;
        }

        public bool Delete(int id) {
            int rowsAffected = -1;
            string sql =
                @"DELETE FROM characters WHERE characterId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                rowsAffected = connection.Execute(sql, new { id });
            }
            return rowsAffected > 0;
        }

        public int Create(GameCharacter character) {
            int insertedId = -1;
            string sql =
                @"INSERT INTO characters(versionId, characterName)
                OUTPUT INSERTED.characterId
                VALUES (@versionId, @characterName)";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                try {
                    insertedId = connection.ExecuteScalar<int>(sql, new { character.GameVersion.VersionId, character.CharacterName });
                } catch {
                    insertedId = -1;
                }
            }
            return insertedId;
        }

        public bool Update(int id, GameCharacter character) {
            int rowsAffected = -1;
            character.CharacterId = id;
            string sql =
                @"UPDATE characters
                SET versionId = @versionId, characterName = @characterName
                WHERE characterId = @characterId";

            using (SqlConnection connection = new SqlConnection(_connectionString)) {
                connection.Open();
                rowsAffected = connection.Execute(sql, new {
                    versionId = character.GameVersion.VersionId,
                    characterName = character.CharacterName,
                    characterId = character.CharacterId
                });
            }
            return rowsAffected > 0;
        }
    }
}
