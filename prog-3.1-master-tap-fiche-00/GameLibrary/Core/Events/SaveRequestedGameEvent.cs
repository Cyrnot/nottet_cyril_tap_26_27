using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Events
{
    public class SaveRequestedGameEvent : IGameEvent
    {
        public readonly string _saveText;

        public SaveRequestedGameEvent(string save_text)
        {
            _saveText = save_text;
        }
    }
}
