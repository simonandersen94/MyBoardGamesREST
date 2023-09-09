using BoardGames.BusinessLogic.Interfaces;
using BoardGames.Data;
using BoardGames.Data.Interfaces;
using BoardGames.Model;

namespace BoardGames.BusinessLogic {
    public class VersionControl : IVersionControl {
        private readonly IVersionAccess _versionAccess;

        public VersionControl(IVersionAccess versionAccess) {
            _versionAccess = versionAccess;
        }

        public GameVersion Get(int id) {
            GameVersion? foundVersion = null;
            if (id > 0) {
                foundVersion = _versionAccess.Get(id);
            }
            return foundVersion;
        }

        public List<GameVersion> GetVersionByGameId(int id) {
            List<GameVersion>? foundVersions = null;
            if (id > 0) {
                foundVersions = _versionAccess.GetVersionByGameId(id);
            }
            return foundVersions;
        }

        public List<GameVersion> GetAll() {
            return _versionAccess.GetAll();
        }

        public bool Delete(int id) {
            bool wasDeleted = false;
            if (id > 0) {
                wasDeleted = _versionAccess.Delete(id);
            }
            return wasDeleted;
        }

        public int Create(GameVersion version) {
            return _versionAccess.Create(version);
        }

        public bool Update(int id, GameVersion gameVersion) {
            return _versionAccess.Update(id, gameVersion);
        }
    }
}
