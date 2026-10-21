using GameLibrary.Core.Events;
using GameLibrary.Core.Menu;
namespace GameLibrary.Core.States
{
    public class MainMenuState : IState
    {

        private StateMachine _stateMachine;
        private GameManager _gameManager;
        private EventManager _eventManager;

        private int _selectedItemIndex = -1;
        private List<IMenuEntry> _menuItemTable = new List<IMenuEntry>();

        public MainMenuState(StateMachine state_machine, GameManager game_manager, EventManager event_manager)
        {
            _eventManager = event_manager;
            _gameManager = game_manager;
            _stateMachine = state_machine;
        }

        public void Enter()
        {
            _eventManager.RegisterToEvent<GameActionGameEvent>(OnGameActionGameEvent);

            _menuItemTable.Add(new NewGameMenuEntry(_gameManager, _stateMachine, _eventManager));

            foreach ((string save_name, string save_data) in _gameManager.GetSaveTable())
            {
                AddSaveEntry(save_name);
            }

            _menuItemTable.Add(new ReplaySpeedMenuEntry(_gameManager));
            _menuItemTable.Add(new QuitMenuEntry(_gameManager));
        }

        private void OnGameActionGameEvent(IGameEvent game_event)
        {
            GameActionGameEvent game_action_game_event = game_event as GameActionGameEvent;

            if (game_action_game_event._gameActionType == GameActionType.CONFIRM)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                if ((_selectedItemIndex >= 0) && (_selectedItemIndex < _menuItemTable.Count))
                {
                    _menuItemTable[_selectedItemIndex].Execute();
                }
            }
            else if (game_action_game_event._gameActionType == GameActionType.QUIT)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent($"{game_action_game_event._gameActionType}"));

                _gameManager.RequestQuit();
            }
            else if (game_action_game_event._gameActionType == GameActionType.NAVIGATE_UP)
            {
                _selectedItemIndex = (_selectedItemIndex - 1 + _menuItemTable.Count) % _menuItemTable.Count;
            }
            else if (game_action_game_event._gameActionType == GameActionType.NAVIGATE_DOWN)
            {
                _selectedItemIndex = (_selectedItemIndex + 1) % _menuItemTable.Count;
            }
        }

        public List<IMenuEntry> GetMenuItemTable()
        {
            return _menuItemTable;
        }

        public void AddSaveEntry(string save_name)
        {
            int replay_speed_index = _menuItemTable.FindIndex(menu_entry => menu_entry is ReplaySpeedMenuEntry);
            LoadSaveMenuEntry save_menu_entry = new LoadSaveMenuEntry(save_name, _stateMachine, _gameManager, _eventManager);

            if (replay_speed_index >= 0)
            {
                _menuItemTable.Insert(replay_speed_index, save_menu_entry);
            }
            else
            {
                _menuItemTable.Add(save_menu_entry);
            }
        }

        public int GetSelectedItemIndex()
        {
            return _selectedItemIndex;
        }

        public void Exit()
        {
            _eventManager.UnregisterFromEvent<GameActionGameEvent>(OnGameActionGameEvent);
        }

        public void FixedUpdate(float fixed_elapsed_time)
        {

        }

        public void Update(float elapsed_time)
        {

        }
    }
}
