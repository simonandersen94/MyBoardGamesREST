namespace BoardGames.Model {
    public class Game {
        public int GameId { get; set; }
        public string GameName { get; set; }

        public Game() { }

        public Game(int gameId, string gameName) {
            GameId = gameId;
            GameName = gameName;
        }
    }
}

