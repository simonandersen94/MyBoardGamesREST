using Microsoft.AspNetCore.Mvc;
using BoardGames.Model;
using BoardGames.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BoardGames.Controllers {
    [Route("api/[controller]")]
    public class GamesController : ControllerBase {
        private readonly IGameControl _gamesControl;
        private readonly IVersionControl _versionControl;

        public GamesController(IGameControl gamesControl, IVersionControl versionControl) {
            _versionControl = versionControl;
            _gamesControl = gamesControl;
        }

        [HttpGet("{id}")]
        public ActionResult<Game> Get(int id) {
            ActionResult<Game>? foundReturn = null;

            Game foundGame = _gamesControl.Get(id);
            if (foundGame != null) {
                foundReturn = Ok(foundGame);
            } else {
                foundReturn = new StatusCodeResult(204);
            }
            return foundGame;
        }

        [HttpGet("{id}/versions")]
        public ActionResult<List<GameVersion>> GetVersionByGameId(int id) {
            ActionResult<List<GameVersion>>? foundReturns;

            List<GameVersion> foundVersions = _versionControl.GetVersionByGameId(id);
            if (foundVersions != null) {
                foundReturns = Ok(foundVersions);
            } else {
                foundReturns = new StatusCodeResult(204);
            }
            return foundReturns;
        }

        [HttpGet("players/{no}")]
        public ActionResult<MergedGame> GetGamesByNoOfPlayers(int no) {
            ActionResult<MergedGame>? foundReturn = null;
            MergedGame foundGame = _gamesControl.GetGameByNoOfPlayers(no);
            if (foundGame != null) {
                foundReturn = Ok(foundGame);
            } else {
                foundReturn = new StatusCodeResult(204);
            }
            return foundReturn;
        }

        [HttpGet]
        public ActionResult<List<Game>> GetAll() {
            ActionResult<List<Game>>? foundReturns;

            List<Game> foundGames = _gamesControl.GetAll();
            if (foundGames != null) {
                foundReturns = Ok(foundGames);
            } else {
                foundReturns = new StatusCodeResult(204);
            }
            return foundReturns;
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            ActionResult result;

            bool wasDeleted = _gamesControl.Delete(id);
            if (wasDeleted) {
                result = StatusCode(200);
            } else {
                result = StatusCode(500);
            }
            return result;
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Create([FromBody] Game game) {
            ActionResult foundResult;
            int insertedId = _gamesControl.Create(game);
            if (insertedId == -1) {
                foundResult = BadRequest();
            } else {
                game.GameId = insertedId;
                foundResult = CreatedAtAction(nameof(Get), new { id = insertedId }, game);
            }
            return foundResult;
        }

        //[Authorize]
        [HttpPut("{id}")]
        public ActionResult<bool> Update(int id, [FromBody] Game game) {
            ActionResult<bool> result;
            bool wasUpdated = false;
            if (game != null) {
                wasUpdated = _gamesControl.Update(id, game);
                if (wasUpdated == true) {
                    result = Ok(wasUpdated);
                } else {
                    result = BadRequest();
                }
            } else {
                result = BadRequest();
            }
            return result;
        }

        [HttpGet("amount")]
        public ActionResult<int> GetAmount() {
            ActionResult<int> result;
            int amount = _gamesControl.GetAmount();
            if (amount != 0) {
                result = Ok(amount);
            } else {
                result = BadRequest();
            }
            return result;
        }
    }
}
