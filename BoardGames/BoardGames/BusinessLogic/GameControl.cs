using BoardGames.BusinessLogic.Interfaces;
using BoardGames.Data.Interfaces;
using BoardGames.Model;

namespace BoardGames.BusinessLogic {
    public class GameControl : IGameControl {
        private readonly IGameAccess _gameAccess;
        private readonly IVersionAccess _versionAccess;
        private readonly ICharacterAccess _characterAccess;

        public GameControl(IGameAccess gameAccess, IVersionAccess versionAccess, ICharacterAccess characterAccess) {
            _gameAccess = gameAccess;
            _versionAccess = versionAccess;
            _characterAccess = characterAccess;
        }

        public Game Get(int id) {
            Game? foundGame = null;
            if (id > 0) {
                foundGame = _gameAccess.Get(id);
            }
            return foundGame;
        }

    public MergedGame GetGameByNoOfPlayers(int no) {
        List<Game>? foundGames = null;
        List<GameVersion>? foundVersions = null;
        List<GameCharacter>? foundCharacters = null;

        Game gameToMerge = null;
        GameVersion versionToMerge = null;
        List<GameCharacter> charactersToMerge = new List<GameCharacter>();

        GameCharacter tempCharacter = null;
        MergedGame foundMergedGame = new MergedGame();
        int randomIndex = -1;

        try {
            foundGames = _gameAccess.GetGameByNoOfPlayers(no);
            randomIndex = new Random().Next(foundGames.Count);
            gameToMerge = foundGames.ElementAt(randomIndex);

            randomIndex = -1;

            foundVersions = _versionAccess.GetVersionByGameId(gameToMerge.GameId);
            randomIndex = new Random().Next(foundVersions.Count);
            versionToMerge = foundVersions.ElementAt(randomIndex);

            randomIndex = -1;

            foundCharacters = _characterAccess.GetCharacterByVersionId(versionToMerge.VersionId);
            int i = 1;
            while (i <= no && foundCharacters.Count() != 0) {
                randomIndex = new Random().Next(foundCharacters.Count);
                if (randomIndex >= 0) {
                    tempCharacter = foundCharacters.ElementAt(randomIndex);
                    if (tempCharacter != null && !charactersToMerge.Contains(tempCharacter)) {
                        charactersToMerge.Add(tempCharacter);
                        randomIndex = -1;
                        i++;
                    }
                }
            }
            foundMergedGame.Game = gameToMerge;
            foundMergedGame.GameVersion = versionToMerge;
            foundMergedGame.GameCharacter = charactersToMerge;
        } catch {
            Console.WriteLine("No games found");
        }
        return foundMergedGame;
    }

        public List<Game> GetAll() {
            return _gameAccess.GetAll();
        }

        public bool Delete(int id) {
            bool wasDeleted = false;
            if (id > 0) {
                wasDeleted = _gameAccess.Delete(id);
            }
            return wasDeleted;
        }

        public int Create(Game game) {
            return _gameAccess.Create(game);
        }

        public bool Update(int id, Game game) {
            return _gameAccess.Update(id, game);
        }
    }
}
