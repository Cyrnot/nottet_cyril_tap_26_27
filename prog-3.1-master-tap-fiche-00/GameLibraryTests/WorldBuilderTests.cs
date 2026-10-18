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
            List<LocationData> location_data_table = new List<LocationData>();
            EventManager eventManager = new EventManager();
            WorldBuilder builder = new WorldBuilder(location_data_table);

            location_data_table.Add(new LocationData("DT_W", "Daisy Town", "W", 5));
            location_data_table.Add(new LocationData("W", "World", "", 0));
            LocationComponent parent = builder.Build(eventManager);
            LocationComponent child = parent.GetChildLocationAtIndex(0).GetDestinationLocation();

            Assert.AreEqual(parent, child.GetParentLocation());
        }

        [Test]
        public void Build_LinkedLocationsHaveCorrectTimeToTravelInBothDirections()
        {
            int travel_time = 5;
            List<LocationData> location_data_table = new List<LocationData>();
            EventManager eventManager = new EventManager();
            WorldBuilder builder = new WorldBuilder(location_data_table);


            location_data_table.Add(new LocationData("DT_W", "Daisy Town", "W", travel_time));
            location_data_table.Add(new LocationData("W", "World", "", 0));
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
            List<LocationData> location_data_table = new List<LocationData>();
            EventManager eventManager = new EventManager();
            WorldBuilder builder = new WorldBuilder(location_data_table);

            location_data_table.Add(new LocationData("DT_W", "Daisy Town", "W", 5));

            Assert.Throws<KeyNotFoundException>(() => builder.Build(eventManager));
        }
    }
}
