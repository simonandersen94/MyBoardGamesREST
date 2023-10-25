using BoardGames.BusinessLogic;
using BoardGames.BusinessLogic.Interfaces;
using BoardGames.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BoardGames.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase {
        private readonly ICharacterControl _characterControl;

        public CharactersController(ICharacterControl characterControl) {
            _characterControl = characterControl;
        }

        [HttpGet("{id}")]
        public ActionResult<GameCharacter> Get(int id) {
            ActionResult<GameCharacter> foundReturn;

            GameCharacter foundCharacter = _characterControl.Get(id);
            if (foundCharacter != null) {
                foundReturn = Ok(foundCharacter);
            } else {
                foundReturn = new StatusCodeResult(204);
            }
            return foundReturn;
        }

        [HttpGet]
        public ActionResult<List<GameCharacter>> GetAll() {
            ActionResult<List<GameCharacter>> foundReturn;

            List<GameCharacter> foundCharacters = _characterControl.GetAll();
            if (foundCharacters != null) {
                foundReturn = Ok(foundCharacters);
            } else {
                foundReturn = new StatusCodeResult(204);
            }
            return foundReturn;
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            ActionResult result;

            bool wasDeleted = _characterControl.Delete(id);
            if (wasDeleted) {
                result = StatusCode(200);
            } else {
                result = StatusCode(500);
            }
            return result;
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Create([FromBody] GameCharacter character) {
            ActionResult foundResult;
            int insertedId = _characterControl.Create(character);
            if (insertedId == -1) {
                foundResult = StatusCode(500);
            } else {
                character.CharacterId = insertedId;
                foundResult = CreatedAtAction(nameof(Get), new { id = insertedId }, character);
            }
            return foundResult;
        }

        //[Authorize]
        [HttpPut("{id}")]
        public ActionResult<bool> Update(int id, GameCharacter character) {
            ActionResult<bool> result;
            bool wasUpdated = false;
            if (character != null) {
                wasUpdated = _characterControl.Update(id, character);
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
