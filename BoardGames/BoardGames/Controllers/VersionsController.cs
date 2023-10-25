using BoardGames.BusinessLogic;
using BoardGames.BusinessLogic.Interfaces;
using BoardGames.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardGames.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class VersionsController : ControllerBase {
        private readonly IVersionControl _versionControl;
        private readonly ICharacterControl _characterControl;

        public VersionsController(IVersionControl versionControl, ICharacterControl characterControl) {
            _versionControl = versionControl;
            _characterControl = characterControl;
        }

        [HttpGet("{id}")]
        public ActionResult<GameVersion> Get(int id) {
            ActionResult<GameVersion>? foundReturn = null;

            GameVersion foundVersion = _versionControl.Get(id);
            if (foundVersion != null) {
                foundReturn = Ok(foundVersion);
            } else {
                foundReturn = new StatusCodeResult(204);
            }
            return foundReturn;
        }

        [HttpGet("{id}/characters")]
        public ActionResult<List<GameCharacter>> GetCharacterByVersionId(int id) {
            ActionResult<List<GameCharacter>>? foundReturns = null;

            List<GameCharacter> foundCharacters = _characterControl.GetCharacterByVersionId(id);
            if (foundCharacters != null) {
                foundReturns = Ok(foundCharacters);
            } else {
                foundReturns = new StatusCodeResult(204);
            }
            return foundReturns;
        }

        [HttpGet]
        public ActionResult<List<GameVersion>> GetAll() {
            ActionResult<List<GameVersion>>? foundReturn;
            
            List<GameVersion> foundVersions = _versionControl.GetAll();
            if (foundVersions != null) {
                foundReturn= Ok(foundVersions);
            } else {
                foundReturn = new StatusCodeResult(204);
            }
            return foundReturn;
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            ActionResult result;

            bool wasDeleted = _versionControl.Delete(id);
            if (wasDeleted) {
                result = StatusCode(200);
            } else {
                result = StatusCode(500);
            }
            return result;
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Create([FromBody] GameVersion version) {
            ActionResult foundResult;
            int insertedId = _versionControl.Create(version);
            if (insertedId == -1) {
                foundResult = StatusCode(500);
            } else {
                version.VersionId = insertedId;
                foundResult = CreatedAtAction(nameof(Get), new { id = insertedId }, version);
            }
            return foundResult;
        }

        //[Authorize]
        [HttpPut("{id}")]
        public ActionResult<bool> Update(int id, [FromBody] GameVersion version) {
            ActionResult<bool> result;
            bool wasUpdated = false;
            if (version != null) {
                wasUpdated = _versionControl.Update(id, version);
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
    }
}
