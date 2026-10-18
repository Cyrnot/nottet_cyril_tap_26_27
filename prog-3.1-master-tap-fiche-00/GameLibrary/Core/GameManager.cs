using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;
using GameLibrary.Data;

namespace GameLibrary.Core;

public class GameManager
{
    private readonly List<GameObject> _gameObjectTable = new List<GameObject>();
    private readonly EventManager _eventManager;
    private readonly StateMachine _gameFlowStateStateMachine;
    private readonly WorldBuilder _worldBuilder;
    private LocationComponent _currentLocation;

    private int _selectedChildLocationIndex = -1;

    private bool _shouldQuit = false;
    private bool _isMenuStateActive = true;



    public GameManager(EventManager event_manager, WorldBuilder world_builder)
    {
        _eventManager = event_manager;
        _worldBuilder = world_builder;

        _gameFlowStateStateMachine = new StateMachine(_eventManager);
        _gameFlowStateStateMachine.SetInitialState(new MainMenuState(_gameFlowStateStateMachine, this, _eventManager));

        GameObject game_object_test = new GameObject("game_object_test");

        _eventManager.RegisterToEvent<RegisterGameObjectGameEvent>(OnRegisterGameObjectGameEvent);
        _eventManager.RegisterToEvent<UnregisterGameObjectGameEvent>(OnUnregisterGameObjectGameEvent);

        _eventManager.TriggerDelayedEvent(new RegisterGameObjectGameEvent(game_object_test));
    }

    public void BeginExploration()
    {
        LocationComponent location = _worldBuilder.Build(_eventManager);

        _currentLocation = location;
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
    }

    public bool GetIsMenuStateActive()
    {
        return _isMenuStateActive;
    }

    public void SetIsMenuStateActive(bool is_menu_state_active)
    {
        _isMenuStateActive = is_menu_state_active;
    }
}
