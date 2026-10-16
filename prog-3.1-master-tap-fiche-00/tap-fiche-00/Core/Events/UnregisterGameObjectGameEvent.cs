using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core.Events
{
    internal class UnregisterGameObjectGameEvent : IGameEvent
    {
        public GameObject _gameObject;

        public UnregisterGameObjectGameEvent(GameObject game_object)
        {
            _gameObject = game_object;
        }
    }
}
