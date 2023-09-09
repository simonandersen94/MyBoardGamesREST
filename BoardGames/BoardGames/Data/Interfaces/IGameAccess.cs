using BoardGames.Model;

namespace BoardGames.Data.Interfaces {
    public interface IGameAccess {
        Game Get(int id);
        List<Game> GetGameByNoOfPlayers(int no);
        List<Game> GetAll();
        bool Delete(int id);
        int Create(Game game);
        bool Update(int id, Game game);
    }
}
