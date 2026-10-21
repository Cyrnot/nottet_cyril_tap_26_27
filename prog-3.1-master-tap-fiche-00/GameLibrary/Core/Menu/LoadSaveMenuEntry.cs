using GameLibrary.Core.Events;
using GameLibrary.Core.Replay;
using GameLibrary.Core.States;
using GameLibrary.Csv;
using GameLibrary.Replay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Menu
{
    public class LoadSaveMenuEntry : IMenuEntry
    {
        private const string TEXT = "Load save: ";
        private string _saveName;

        private StateMachine _stateMachine;
        private GameManager _gameManager;
        private EventManager _eventManager;

        public LoadSaveMenuEntry(string save_name, StateMachine state_machine, GameManager game_manager, EventManager event_manager)
        {
            _saveName = save_name;
            _stateMachine = state_machine;
            _gameManager = game_manager;
            _eventManager = event_manager;
        }

        public void Execute()
        {
            if (!_saveName.Contains("(Damaged)"))
            {
                ContentReport error_report = new ContentReport();
                CsvTable csv_table = CsvReader.Read(_saveName, _gameManager.GetSaveTable()[_saveName], error_report);
                ReplayData replay_data = ReplayReader.Read(csv_table, error_report);

                _gameManager.LoadReplay(replay_data);
                _stateMachine.ChangeState(new ExploringState(_stateMachine, _gameManager, _eventManager));
            }
        }

        public string GetText()
        {
            return $"{TEXT}{_saveName}";
        }
    }
}
