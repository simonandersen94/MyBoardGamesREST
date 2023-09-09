using BoardGames.Model;

namespace BoardGames.BusinessLogic.Interfaces {
    public interface IGameControl {
        Game Get(int id);
        MergedGame GetGameByNoOfPlayers(int no);
        List<Game> GetAll();
        bool Delete(int id);
        int Create(Game game);
        bool Update(int id, Game game);
    }
}
