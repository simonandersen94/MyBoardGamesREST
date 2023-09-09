using BoardGames.BusinessLogic.Interfaces;
using BoardGames.Data;
using BoardGames.Data.Interfaces;
using BoardGames.Model;
using System;

namespace BoardGames.BusinessLogic {
    public class CharacterControl : ICharacterControl {
        private readonly ICharacterAccess _characterAccess;

        public CharacterControl(ICharacterAccess characterAccess) {
            _characterAccess = characterAccess;
        }

        public GameCharacter Get(int id) {
            GameCharacter? foundCharacter = null;
            if (id > 0) {
                foundCharacter = _characterAccess.Get(id);
            }
            return foundCharacter;
        }

        public List<GameCharacter> GetCharacterByVersionId(int id) {
            List<GameCharacter>? foundCharacter = null;
            if (id > 0) {
                foundCharacter = _characterAccess.GetCharacterByVersionId(id);
            }
            return foundCharacter;
        }

        public List<GameCharacter> GetAll() {
            return _characterAccess.GetAll();
        }

        public bool Delete(int id) {
            bool wasDeleted = false;
            if (id > 0) {
                wasDeleted = _characterAccess.Delete(id);
            }
            return wasDeleted;
        }

        public int Create(GameCharacter character) {
            return _characterAccess.Create(character);
        }

        public bool Update(int id, GameCharacter character) {
            return _characterAccess.Update(id, character);
        }
    }
}
