namespace BoardGames.Model {
    public class MergedGame {
        public Game Game { get; set; }
        public GameVersion GameVersion { get; set; }
        public List<GameCharacter> GameCharacter { get; set; }

        public MergedGame()
        {
            
        }

        public MergedGame(Game game, GameVersion gameVersion, List<GameCharacter> gameCharacter)
        {
            Game = game;
            GameVersion = gameVersion;
            GameCharacter = gameCharacter;
        }
    }
}
