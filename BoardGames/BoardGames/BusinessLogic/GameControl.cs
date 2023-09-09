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
            MergedGame? foundMergedGame = new MergedGame();
            List<Game>? foundGames = null;
            List<GameVersion>? foundGameVersion = null;
            List<GameCharacter>? foundGameCharacter = null;
            Game gameToReturn;
            GameVersion versionToReturn;
            List<GameCharacter>? charactersToReturn = new List<GameCharacter>();
            int randomGameIndex;
            int randomVersionIndex;
            int gameIdInt;
            int versionIdInt;

            if (no > 0) {
                foundGames = _gameAccess.GetGameByNoOfPlayers(no);  //Finder hele listen af games
                    randomGameIndex = new Random().Next(foundGames.Count);  //Tager et random index på listen
                if (randomGameIndex > 0) {
                    gameToReturn = foundGames.ElementAt(randomGameIndex);  //Finder game på det index
                    gameIdInt = gameToReturn.GameId;  //Assigner det fundne game.GameId til gameIdInt

                    foundGameVersion = _versionAccess.GetVersionByGameId(gameIdInt);  //Finder hele listen af versioner med specifikt GameId
                    randomVersionIndex = new Random().Next(foundGameVersion.Count);  //Tager et random index på listen
                    if (randomVersionIndex > 0) {
                        versionToReturn = foundGameVersion.ElementAt(randomVersionIndex);  //Finder version på det index
                        versionIdInt = versionToReturn.VersionId;  //Assigner det fundne Version.VersionId til versionIdInt

                        foundGameCharacter = _characterAccess.GetCharacterByVersionId(versionIdInt);  //Finder hele listen af characters med specifikt versionId
                        if (foundGameCharacter.Count != 0) {
                            int i = 1;
                            while (i <= no) {
                                int randomCharacterIndex = new Random().Next(foundGameCharacter.Count); //Finder random index på listen
                                GameCharacter characterAtIndex = foundGameCharacter.ElementAt(randomCharacterIndex);  //Finder character på index
                                if (characterAtIndex != null && !charactersToReturn.Contains(characterAtIndex)) { //Går videre hvis den findne character ikke er null og ikke allerede er på listen
                                    charactersToReturn.Add(characterAtIndex); //Tilføjer den fundne character til charactersToReturn
                                    i++;
                                }
                            }
                        }
                        foundMergedGame.Game = gameToReturn;
                        foundMergedGame.GameVersion = versionToReturn;
                        foundMergedGame.GameCharacter = charactersToReturn;
                    }
                }
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
