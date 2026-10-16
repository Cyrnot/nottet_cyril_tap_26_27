using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core.Events
{
    internal class GameActionGameEvent : IGameEvent
    {
        public GameActionType _gameActionType;

        public GameActionGameEvent(GameActionType gameActionType)
        {
            _gameActionType = gameActionType;
        }
    }
}
