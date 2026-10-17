using GameLibrary.Core.Components;
using GameLibrary.Core.Events;
using GameLibrary.Core.States;

namespace GameLibrary.Core;

public class GameManager
{
    private readonly List<GameObject> _gameObjectTable = new List<GameObject>();

    private readonly EventManager _eventManager;

    private readonly StateMachine _gameFlowStateStateMachine;

    private LocationComponent _currentLocation;
    private int _selectedChildLocationIndex = -1;

    private bool _shouldQuit = false;
    private bool _isMenuStateActive = true;

    public GameManager(EventManager event_manager)
    {
        _eventManager = event_manager;

        _gameFlowStateStateMachine = new StateMachine(_eventManager);
        _gameFlowStateStateMachine.SetInitialState(new MainMenuState(_gameFlowStateStateMachine, this, _eventManager));

        GameObject game_object_test = new GameObject("game_object_test");

        _eventManager.RegisterToEvent<RegisterGameObjectGameEvent>(OnRegisterGameObjectGameEvent);
        _eventManager.RegisterToEvent<UnregisterGameObjectGameEvent>(OnUnregisterGameObjectGameEvent);

        _eventManager.TriggerDelayedEvent(new RegisterGameObjectGameEvent(game_object_test));        
    }

    public void BeginExploration()
    {
        LocationComponent world_location = CreateLocation("World", _eventManager, null, 0.0f);

        _currentLocation = world_location;

        LocationComponent town_location = CreateLocation("Daisy Town", _eventManager, world_location, 10.0f);
        LocationComponent inn_location = CreateLocation("Laughing Horse Inn", _eventManager, town_location, 5.0f);
        LocationComponent shop_location = CreateLocation("Gun Shop", _eventManager, town_location, 3.0f);

        LocationComponent town_location_2 = CreateLocation("Rose Town", _eventManager, world_location, 10.0f);
        LocationComponent inn_location_2 = CreateLocation("Fallen Cow Inn", _eventManager, town_location_2, 5.0f);
        LocationComponent shop_location_2 = CreateLocation("Tanner Shop", _eventManager, town_location_2, 3.0f);

        LocationComponent dungeon_location = CreateLocation("Silver Mine Dungeon", _eventManager, world_location, 15.0f);
        LocationComponent dungeon_level_one_location = CreateLocation("Dungeon level 1", _eventManager, dungeon_location, 2.0f);
        LocationComponent dungeon_level_two_location = CreateLocation("Dungeon level 2", _eventManager, dungeon_level_one_location, 5.0f);
        LocationComponent dungeon_level_three_location = CreateLocation("Dungeon level 3", _eventManager, dungeon_level_two_location, 10.0f);
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

    private LocationComponent CreateLocation(string location_name, EventManager event_manager, LocationComponent parent_location, float distance_to_parent)
    {
        GameObject game_object = new GameObject(location_name);
        LocationComponent location_component = new LocationComponent(location_name);

        game_object.AddComponent(location_component);

        if (parent_location != null)
        {
            parent_location.AddLocation(location_component, distance_to_parent);
        }

        event_manager.TriggerDelayedEvent(new RegisterGameObjectGameEvent(game_object));

        return location_component;
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
