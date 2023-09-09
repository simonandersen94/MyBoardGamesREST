namespace BoardGames.Model {
    public class GameCharacter {
        public int? CharacterId { get; set; }
        public GameVersion GameVersion { get; set; }
        public string CharacterName { get; set; }

        public GameCharacter() { }

        public GameCharacter(int characterId, GameVersion version, string characterName) {
            CharacterId = characterId;
            GameVersion = version;
            CharacterName = characterName;
        }
    }
}
