using BoardGames.Model;

namespace BoardGames.Data.Interfaces {
    public interface IVersionAccess {
        GameVersion Get(int id);
        List<GameVersion> GetVersionByGameId(int id);
        List<GameVersion> GetAll();
        bool Delete(int id);
        int Create(GameVersion version);
        bool Update(int id, GameVersion gameVerson);
    }
}
