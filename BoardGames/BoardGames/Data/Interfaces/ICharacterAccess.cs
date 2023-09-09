using BoardGames.Model;

namespace BoardGames.Data.Interfaces {
    public interface ICharacterAccess {
        GameCharacter Get(int id);
        List<GameCharacter> GetCharacterByVersionId(int id);
        List<GameCharacter> GetAll();
        bool Delete(int id);
        int Create(GameCharacter character);
        bool Update(int id, GameCharacter character);
    }
}
