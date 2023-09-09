using BoardGames.Model;

namespace BoardGames.BusinessLogic.Interfaces {
    public interface IVersionControl {
        GameVersion Get(int id);
        List<GameVersion> GetVersionByGameId(int id);
        List<GameVersion> GetAll();
        bool Delete(int id);
        int Create(GameVersion version);
        bool Update(int id, GameVersion version);
    }
}
