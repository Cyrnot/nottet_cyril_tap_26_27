using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.Replay;
using GameLibrary.Core.States;
using GameLibrary.Csv;
using GameLibrary.Data;
using GameLibrary.Replay;

namespace GameLibrary.Core;

public class GameManager : IFixedStepClock
{
    private readonly List<GameObject> _gameObjectTable = new List<GameObject>();
    private readonly Dictionary<string, string> _saveTable = new Dictionary<string, string>();
    private readonly EventManager _eventManager;
    private readonly StateMachine _gameFlowStateStateMachine;
    private readonly WorldBuilder _worldBuilder;
    private LocationComponent _currentLocation;

    private ReplayRecorder _replayRecorder;
    private ReplayPlayer _replayPlayer;

    private Random _random;

    private int _selectedChildLocationIndex = -1;

    private bool _shouldQuit = false;
    private bool _isMenuStateActive = true;

    private int _fixedStepIndex = 0;
    private int _replaySpeed;

    public GameManager(EventManager event_manager, GameContentData game_content_data)
    {
        _eventManager = event_manager;
        _worldBuilder = new WorldBuilder(game_content_data);

        _gameFlowStateStateMachine = new StateMachine(_eventManager);
        _gameFlowStateStateMachine.SetInitialState(new MainMenuState(_gameFlowStateStateMachine, this, _eventManager));

        GameObject game_object_test = new GameObject("game_object_test");

        _eventManager.RegisterToEvent<RegisterGameObjectGameEvent>(OnRegisterGameObjectGameEvent);
        _eventManager.RegisterToEvent<UnregisterGameObjectGameEvent>(OnUnregisterGameObjectGameEvent);  

        _eventManager.TriggerDelayedEvent(new RegisterGameObjectGameEvent(game_object_test));
    }

    public void StartNewGame(int seed)
    {
        if (_replayRecorder != null)
        {
            _replayRecorder.StopRecording();
            _gameObjectTable.Clear();
        }

        _eventManager.TriggerEvent(new LogMessageGameEvent($"seed : {seed}"));

        _random = new Random(seed);
        _replayRecorder = new ReplayRecorder(seed, _eventManager, this);
        _replayPlayer = null;
        _fixedStepIndex = 0;
        LocationComponent location = _worldBuilder.Build(_eventManager);

        _currentLocation = location;   
    }

    public void QuitGame()
    {
        _replayRecorder.StopRecording();
        SetCurrentLocation(null);

        string save_text = _replayRecorder.GetCsvTextEnd(_fixedStepIndex);

        _eventManager.TriggerEvent(new SaveRequestedGameEvent(save_text));
    }

    private void OnRegisterGameObjectGameEvent(IGameEvent game_event)
    {
        RegisterGameObjectGameEvent register_game_object_game_event = game_event as RegisterGameObjectGameEvent;

        _gameObjectTable.Add(register_game_object_game_event._gameObject);
    }

    private void OnUnregisterGameObjectGameEvent(IGameEvent game_event)
    {
        UnregisterGameObjectGameEvent unregister_game_object_game_event = game_event as UnregisterGameObjectGameEvent;

        _gameObjectTable.Remove(unregister_game_object_game_event._gameObject);
    }

    public void Update(float elapsed_time)
    {
        for (int object_index = 0; object_index < _gameObjectTable.Count; object_index++)
        {
            GameObject game_object = _gameObjectTable[object_index];

            if (game_object.GetIsActive())
            {
                game_object.Update(elapsed_time);
            }
        }
    }

    public void FixedUpdate(float fixed_elapsed_time)
    {
        for (int object_index = 0; object_index < _gameObjectTable.Count; object_index++)
        {
            GameObject game_object = _gameObjectTable[object_index];

            if (game_object.GetIsActive())
            {
                game_object.FixedUpdate(fixed_elapsed_time);
            }
        }

        if (_replayPlayer != null)
        {
            _replayPlayer.FixedUpdate(fixed_elapsed_time);
        }

        _gameFlowStateStateMachine.FixedUpdate(fixed_elapsed_time);

        _fixedStepIndex++;
    }

    public bool GetShouldQuit()
    {
        return _shouldQuit;
    }

    public void SetShouldQuit(bool should_quit)
    {
        _shouldQuit = should_quit;
    }

    public void RequestQuit()
    {
        _shouldQuit = true;
    }

    public LocationComponent GetCurrentLocation()
    {
        return _currentLocation;
    }

    public int GetSelectedDestinationIndex()
    {
        return _selectedChildLocationIndex;
    }

    public void SetSelectedDestinationIndex(int selected_destination_index)
    {
        _selectedChildLocationIndex = selected_destination_index;
    }

    public void SetCurrentLocation(LocationComponent current_location)
    {
        _currentLocation = current_location;
        SetSelectedChildLocationIndex(-1);
    }

    public void SetSelectedChildLocationIndex(int selected_child_location_index)
    {
        _selectedChildLocationIndex = selected_child_location_index;
    }

    public IState GetGameFlowStateMachine()
    {
        return _gameFlowStateStateMachine.GetCurrentState();
    }

    public void AddSave(string file_name, string save)
    {
        ContentReport content_report = new ContentReport();
        CsvTable csv_table = CsvReader.Read(file_name, save, content_report);
        ReplayData replay_save_data = ReplayReader.Read(csv_table, content_report);

        if (content_report.GetErrorCount() > 0)
        {
            file_name += " (Damaged) ";

            for (int error_index = 0; error_index < content_report.GetErrorCount(); error_index++)
            {
                _eventManager.TriggerEvent(new LogMessageGameEvent(content_report.GetErrorTextAtIndex(error_index)));
            }
        }

        _saveTable.Add(file_name, save);

        if (_gameFlowStateStateMachine.GetCurrentState() is MainMenuState main_menu_state)
        {
            main_menu_state.AddSaveEntry(file_name);
        }
    }

    public string GetSaveText()
    {
        string save_text = "";

        if (_replayRecorder != null)
        {
            save_text = _replayRecorder.GetCsvTextWithoutEnd();
        }

        return save_text;
    }

    public EventManager GetEventManager()
    {
        return _eventManager;
    }

    public int GetFixedStepIndex()
    {
        return _fixedStepIndex;
    }

    public void LoadReplay(ReplayData replay_data)
    {
        StartNewGame(replay_data._seed);

        _replayPlayer = new ReplayPlayer(replay_data, _eventManager, this);
    }

    public Dictionary<string,string> GetSaveTable()
    {
        return _saveTable;
    }

    public void SetReplaySpeed(int replay_speed)
    {
        _replaySpeed = replay_speed;
    }

    public int GetReplaySpeed()
    {
        return _replaySpeed;
    }

    public bool GetIsReplaying()
    {
        return _replayPlayer != null && !_replayPlayer.GetIsFinished(_fixedStepIndex);
    }
}
