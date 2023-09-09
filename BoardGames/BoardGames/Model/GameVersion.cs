namespace BoardGames.Model {
    public class GameVersion {
        public int VersionId { get; set; }
        public Game Game { get; set; }
        public string VersionName { get; set; }
        public int MinNoOfPlayers { get; set; }
        public int MaxNoOfPlayers { get; set; }

        public GameVersion() { }

        public GameVersion(int versionId, Game game, string versionName, int minNoOfPlayers, int maxNoOfPlayers) {
            VersionId = versionId;
            Game = game;
            VersionName = versionName;
            MinNoOfPlayers = minNoOfPlayers;
            MaxNoOfPlayers = maxNoOfPlayers;
        }
    }
}
