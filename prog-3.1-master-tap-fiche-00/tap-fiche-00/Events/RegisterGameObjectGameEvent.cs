using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace activity_00_tap_26_27.Events
{
    public class RegisterGameObjectGameEvent : IGameEvent
    {
        public GameObject _gameObject;
        public RegisterGameObjectGameEvent(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
    }
}
