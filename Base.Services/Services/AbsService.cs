using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Services
{
    public abstract class AbsService
    {
        private List<string> _error = new List<string>();
        private List<string> _message = new List<string>();

        public AbsService() { }

        public String GetError()
        {
            return String.Join(". ", this._error);
        }

        public string GetMessage()
        {
            return string.Join(". ", this._message);
        }

        public void SetError(String Error)
        {
            if (_error == null)
                _error = new List<string>();

            _error.Add(Error);
        }

        public void SetMessage(string message)
        {
            if (_message == null)
                _message = new List<string>();

            _message.Add(message);
        }

        public void ResetMessage()
        {
            _message.Clear();
        }

        public void ResetError()
        {
            _error = new List<string>();
        }
    }


}
