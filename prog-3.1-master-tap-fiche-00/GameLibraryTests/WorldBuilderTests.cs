using System;
using System.Collections.Generic;
using NUnit.Framework;
using GameLibrary.Data;
using GameLibrary.Core.Events;
using GameLibrary.Core.Components;
using GameLibrary.Core;

namespace GameLibraryTests
{
    public class WorldBuilderTests
    {
        [Test]
        public void Build_ChildDeclaredBeforeParentIsLinked()
        {
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _mineDataTable = new List<MineData>() };
            EventManager eventManager = new EventManager();
            WorldBuilder builder = new WorldBuilder(game_content_data);

            game_content_data._locationDataTable.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            LocationComponent parent = builder.Build(eventManager);
            LocationComponent child = parent.GetChildLocationAtIndex(0).GetDestinationLocation();

            Assert.AreEqual(parent, child.GetParentLocation());
        }

        [Test]
        public void Build_LinkedLocationsHaveCorrectTimeToTravelInBothDirections()
        {
            int travel_time = 5;
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>(), _shopDataTable = new List<ShopData>(), _mineDataTable = new List<MineData>() };
            EventManager eventManager = new EventManager();
            WorldBuilder builder = new WorldBuilder(game_content_data);


            game_content_data._locationDataTable.Add(new LocationData("DT_W", "Daisy Town", "W", travel_time));
            game_content_data._locationDataTable.Add(new LocationData("W", "World", "", 0));
            LocationComponent parent = builder.Build(eventManager);
            Connection connection = parent.GetChildLocationAtIndex(0);
            LocationComponent child = connection.GetDestinationLocation();

            Assert.AreEqual(1, parent.GetChildLocationCount());
            Assert.AreEqual(travel_time, connection.GetTimeToReachDestination());
            Assert.AreEqual(parent, child.GetParentLocation());
        }

        [Test]
        public void Build_MissingParentArgumentException()
        {
            GameContentData game_content_data = new GameContentData { _locationDataTable = new List<LocationData>()};
            EventManager eventManager = new EventManager();
            WorldBuilder builder = new WorldBuilder(game_content_data);

            game_content_data._locationDataTable.Add(new LocationData("DT_W", "Daisy Town", "W", 5));

            Assert.Throws<KeyNotFoundException>(() => builder.Build(eventManager));
        }
    }
}
