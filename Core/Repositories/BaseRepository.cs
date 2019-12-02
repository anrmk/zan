using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repositories {
    public interface IBaseRepository {

    }

    public abstract class BaseRepository: IBaseRepository, IDisposable {
        private string _language;

        public string Language {
            get { return _language.Substring(0, 1).ToUpper() + _language.Substring(1, 1).ToLower(); }
            set { this._language = value; }
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}
