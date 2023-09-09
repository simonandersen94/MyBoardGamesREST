using BoardGames.Model;

namespace BoardGames.BusinessLogic.Interfaces {
    public interface ICharacterControl {
        GameCharacter Get(int id);
        List<GameCharacter> GetCharacterByVersionId(int id);
        List<GameCharacter> GetAll();
        bool Delete(int id);
        int Create(GameCharacter character);
        bool Update(int id, GameCharacter character);
    }
}
