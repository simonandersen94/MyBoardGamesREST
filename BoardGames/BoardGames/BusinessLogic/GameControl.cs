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
            while (i <= no) {
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

        //public MergedGame GetGameByNoOfPlayers(int no) {
        //    List<Game>? foundGames = null;              //
        //    List<GameVersion>? foundVersions = null;    // LIST AF GAMES, VERSIONER OG KARAKTERER
        //    List<GameCharacter>? foundCharacters = null;//

        //    int randomIndex = -1;

        //    Game gameToMerge = null;                                            // FUNDET GAME SOM SKAL MERGES TIL MergedGame
        //    GameVersion versionToMerge = null;                                  // FUNDET VERSION SOM SKAL MERGES TIL MergedGame
        //    List<GameCharacter> charactersToMerge = new List<GameCharacter>();  // FUNDET KARAKTERER SOM SKAL MERGES TIL MergedGame

        //    GameCharacter tempCharacter = null;             // TEMP KARAKTER FUNDET TILFÆLDIGT

        //    MergedGame foundMergedGame = new MergedGame();

        //    if (no > 0) {
        //        foundGames = _gameAccess.GetGameByNoOfPlayers(no);  //
        //        randomIndex = new Random().Next(foundGames.Count);  // DETTE STYKKE FINDER
        //        if (randomIndex >= 0) {                              // ET TILFÆLDIGT SPIL
        //            gameToMerge = foundGames.ElementAt(randomIndex);//

        //            randomIndex = -1;

        //            foundVersions = _versionAccess.GetVersionByGameId(gameToMerge.GameId);  //
        //            randomIndex = new Random().Next(foundVersions.Count);                   // DETTE STYKKE FINDER
        //            if (randomIndex >= 0) {                                                  // EN TILFÆLDIG VERSION
        //                versionToMerge = foundVersions.ElementAt(randomIndex);              //

        //                randomIndex = -1;

        //                foundCharacters = _characterAccess.GetCharacterByVersionId(versionToMerge.VersionId);
        //                if (foundCharacters.Count > 0) {
        //                    int i = 1;
        //                    while (i <= no) {
        //                        randomIndex = new Random().Next(foundCharacters.Count);
        //                        if (randomIndex >= 0) {
        //                            tempCharacter = foundCharacters.ElementAt(randomIndex);
        //                            if (tempCharacter != null && !charactersToMerge.Contains(tempCharacter)) {
        //                                charactersToMerge.Add(tempCharacter);
        //                                randomIndex = -1;
        //                                i++;
        //                            }
        //                        }
        //                    }
        //                    foundMergedGame.Game = gameToMerge;
        //                    foundMergedGame.GameVersion = versionToMerge;
        //                    foundMergedGame.GameCharacter = charactersToMerge;
        //                }
        //            }
        //        }
        //    }
        //    return foundMergedGame;
        //}

        //public MergedGame GetGameByNoOfPlayers(int no) {
        //    MergedGame? foundMergedGame = new MergedGame();
        //    List<Game>? foundGames = null;
        //    List<GameVersion>? foundGameVersion = null;
        //    List<GameCharacter>? foundGameCharacter = null;
        //    Game gameToReturn;
        //    GameVersion versionToReturn;
        //    List<GameCharacter>? charactersToReturn = new List<GameCharacter>();
        //    int randomGameIndex;
        //    int randomVersionIndex;
        //    int gameIdInt;
        //    int versionIdInt;

        //    if (no > 0) {
        //        foundGames = _gameAccess.GetGameByNoOfPlayers(no);
        //            randomGameIndex = new Random().Next(foundGames.Count);
        //            gameToReturn = foundGames.ElementAt(randomGameIndex);
        //            gameIdInt = gameToReturn.GameId;

        //            foundGameVersion = _versionAccess.GetVersionByGameId(gameIdInt);
        //            randomVersionIndex = new Random().Next(foundGameVersion.Count);

        //            versionToReturn = foundGameVersion.ElementAt(randomVersionIndex);
        //            versionIdInt = versionToReturn.VersionId;

        //            foundGameCharacter = _characterAccess.GetCharacterByVersionId(versionIdInt);
        //            if (foundGameCharacter.Count != 0) {
        //                int i = 1;
        //                while (i <= no) {
        //                    int randomCharacterIndex = new Random().Next(foundGameCharacter.Count);
        //                    GameCharacter characterAtIndex = foundGameCharacter.ElementAt(randomCharacterIndex);
        //                    if (characterAtIndex != null && !charactersToReturn.Contains(characterAtIndex)) {
        //                        charactersToReturn.Add(characterAtIndex);
        //                        i++;
        //                    }
        //                }
        //            }
        //            foundMergedGame.Game = gameToReturn;
        //            foundMergedGame.GameVersion = versionToReturn;
        //            foundMergedGame.GameCharacter = charactersToReturn;
        //    }
        //    return foundMergedGame;
        //}

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
