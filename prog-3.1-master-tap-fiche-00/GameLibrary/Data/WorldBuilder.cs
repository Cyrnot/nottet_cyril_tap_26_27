using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.Events;

namespace GameLibrary.Data
{
    public class WorldBuilder
    {
        private GameContentData _gameContentData;

        public WorldBuilder(GameContentData game_content_data)
        {
            _gameContentData = game_content_data;
        }

        public LocationComponent Build(EventManager event_manager)
        {
            LocationComponent location = null;
            Dictionary<string, GameObject> game_object_table = new Dictionary<string, GameObject>();
            Dictionary<string, LocationComponent> location_component_table = new Dictionary<string, LocationComponent>();

            for (int location_index = 0; location_index < _gameContentData._locationDataTable.Count; location_index++)
            {
                LocationData location_data = _gameContentData._locationDataTable[location_index];
                GameObject location_game_object = new GameObject(location_data._id);
                LocationComponent location_component = new LocationComponent(location_data._name, location_game_object);

                location_game_object.AddComponent(location_component);

                location_component_table.Add(location_data._id, location_component);
                game_object_table.Add(location_data._id, location_game_object);

                event_manager.TriggerEvent(new RegisterGameObjectGameEvent(location_game_object));
                location_game_object.SetIsActive(true);
            }

            for (int location_index = 0; location_index < _gameContentData._locationDataTable.Count; location_index++)
            {
                LocationData location_data = _gameContentData._locationDataTable[location_index];
                LocationComponent location_component = location_component_table[location_data._id];

                if (!string.IsNullOrEmpty(location_data._parent))
                {
                    LocationComponent parent_location = location_component_table[location_data._parent];

                    parent_location.AddLocation(location_component, location_data._timeToTravel);
                }
                else
                {
                    location = location_component;
                }
            }

            for (int shop_index = 0; shop_index < _gameContentData._shopDataTable.Count; shop_index++)
            {
                GameObject shop_game_object = game_object_table[_gameContentData._shopDataTable[shop_index]._locationId];
                InventoryComponent inventory_component = new InventoryComponent(shop_game_object);

                for (int inventory_data_index = 0; inventory_data_index < _gameContentData._inventoryDataTable.Count; inventory_data_index++)
                {
                    InventoryEntryData current_inventory_data = _gameContentData._inventoryDataTable[inventory_data_index];

                    if (_gameContentData._shopDataTable[shop_index]._locationId == current_inventory_data._ownerId)
                    {
                        inventory_component.Add(_gameContentData._itemDataTable[current_inventory_data._itemId], current_inventory_data._quantity);
                    }
                }

                shop_game_object.AddComponent(new ShopComponent(_gameContentData._shopDataTable[shop_index], shop_game_object));
                shop_game_object.AddComponent(inventory_component);
            }

            return location;
        }
    }
}
