using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Menu
{
    public class NewGameMenuEntry : IMenuEntry
    {
        private const string TEXT = "Start a new game";

        private readonly GameManager _gameManager;
        private readonly StateMachine _stateMachine;
        private readonly EventManager _eventManager;

        public NewGameMenuEntry(GameManager gameManager, StateMachine stateMachine, EventManager eventManager)
        {
            _gameManager = gameManager;
            _stateMachine = stateMachine;
            _eventManager = eventManager;
        }

        public void Execute()
        {
            _gameManager.StartNewGame((int)DateTime.Now.Ticks);

            _stateMachine.ChangeState(new ExploringState(_stateMachine, _gameManager, _eventManager));
        }

        public string GetText()
        {
            return TEXT;
        }
    }
}
