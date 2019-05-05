using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaSheduler.Exceptions
{
    class SettingsFileReadException : Exception
    {
        public SettingsFileReadException() { }
        public SettingsFileReadException(string message) : base(message) { }
        public SettingsFileReadException(string message, Exception inner) : base(message, inner) { }
    }
}
