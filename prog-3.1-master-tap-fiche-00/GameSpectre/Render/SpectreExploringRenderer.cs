using GameHost.Render;
using GameLibrary.Core;
using GameLibrary.Core.Components;
using GameLibrary.Core.States;
using Spectre.Console;

namespace GameSpectre.Render
{
    public class SpectreExploringRenderer : IScreenRenderer
    {
        private readonly SpectreRenderManager _render_manager;

        public SpectreExploringRenderer(SpectreRenderManager render_manager)
        {
            _render_manager = render_manager;
        }

        public void Render(IState state)
        {
            AnsiConsole.Clear();

            ExploringState exploringState = (ExploringState)state;
            LocationComponent currentLocation = exploringState.GetCurrentLocation();

            GameObject hero = exploringState.GetHeroObject();
            HeroComponent heroComponent = hero.GetComponent<HeroComponent>();
            HealthComponent healthComponent = hero.GetComponent<HealthComponent>();
            WalletComponent walletComponent = hero.GetComponent<WalletComponent>();
            InventoryComponent inventoryComponent = hero.GetComponent<InventoryComponent>();

            string gameText = "Game in progress...";

            if (exploringState.GetIsReplaying())
            {
                gameText += $" - REPLAY: {exploringState.GetReplaySpeed()}x";
            }

            Panel gamePanel = new Panel(new Markup(Markup.Escape(gameText)))
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader("Game"),
                BorderStyle = new Style(Color.White)
            }.Expand();


            Table inventoryTable = new Table
            {
                Border = TableBorder.None,
                ShowHeaders = false,
                Expand = true
            };

            inventoryTable.AddColumn("");

            for (int itemIndex = 0; itemIndex < inventoryComponent.GetEntryCount(); itemIndex++)
            {
                string itemName = Markup.Escape(inventoryComponent.GetEntryAtIndex(itemIndex).GetItem()._name);

                inventoryTable.AddRow(new Markup($"[darkblue on blue]{itemName}[/]"));
            }

            Panel inventoryPanel = new Panel(inventoryTable)
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader("Inventory"),
                BorderStyle = new Style(Color.White)
            };

            Panel playerPanel = SpectreHeroPanel.Create(hero);
            LocationComponent rootLocation = currentLocation;

            while (rootLocation.GetParentLocation() != null)
            {
                rootLocation = rootLocation.GetParentLocation();
            }

            Tree locationsTree = new Tree(GetSelectedLocation(rootLocation, currentLocation, false));

            int selectedDestinationIndex = exploringState.GetSelectedDestinationIndex();

            for (int childIndex = 0;childIndex < rootLocation.GetChildLocationCount();childIndex++)
            {
                Connection connection = rootLocation.GetChildLocationAtIndex(childIndex);
                LocationComponent destination = connection.GetDestinationLocation();

                bool isSelectedDestination = rootLocation == currentLocation && childIndex == selectedDestinationIndex;

                TreeNode childNode = locationsTree.AddNode(GetSelectedLocation(destination, currentLocation, isSelectedDestination));

                AddLocationChildren(childNode,destination,currentLocation,selectedDestinationIndex);
            }

            Panel locationsPanel = new Panel(locationsTree)
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader("Locations"),
                BorderStyle = new Style(Color.White)
            }.Expand();

            Panel viewPanel = new Panel(new Text(GetLocationView(currentLocation)))
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader("View"),
                BorderStyle = new Style(Color.White)
            }.Expand();

            Grid layout = new Grid();
            layout.AddColumn();
            layout.AddColumn();
            layout.AddRow(new Rows(gamePanel, playerPanel, locationsPanel), viewPanel);

            AnsiConsole.Write(layout);
        }
        private static string GetLocationView(LocationComponent location)
        {
            if (_locationViews.TryGetValue(location.GetName(), out string view))
            {
                return view;
            }

            return "No illustration for this location yet.";
        }

        private static void AddLocationChildren(TreeNode parentNode,LocationComponent parentLocation,LocationComponent currentLocation,int selectedDestinationIndex)
        {
            for (int childIndex = 0;childIndex < parentLocation.GetChildLocationCount();childIndex++)
            {
                Connection connection =parentLocation.GetChildLocationAtIndex(childIndex);
                LocationComponent destination =connection.GetDestinationLocation();

                bool isSelectedDestination =parentLocation == currentLocation&& childIndex == selectedDestinationIndex;

                TreeNode childNode = parentNode.AddNode(GetSelectedLocation(destination,currentLocation,isSelectedDestination));

                AddLocationChildren(childNode,destination,currentLocation,selectedDestinationIndex);
            }
        }

        private static string GetSelectedLocation(LocationComponent location, LocationComponent currentLocation, bool isSelectedDestination)
        {
            string name = Markup.Escape(location.GetName());

            if (isSelectedDestination || location == currentLocation)
            {
                return $"[black on red]{name}[/]";
            }

            return $"[red]{name}[/]";
        }



        private static readonly Dictionary<string, string> _locationViews = new()
        {
            ["World"] = """
                 \  |  /
                  \ | /
               .--- O ---.
                  / | \
             /\             /\
            /  \____   ____/  \
           /        \_/        \
        __/_____________________\__
    """,

            ["Daisy Town"] = """
          ____________________
         |      DAISY TOWN    |
     ____|____________________|____
         |  []  |      |  []  |
         |______|______|______|
            ||            ||
    """,

            ["Laughing Horse Inn"] = """
              ______________
          ___/   LAUGHING   \___
         |      HORSE  INN      |
         |   []  |      |  []   |
         |_______|______|_______|
             ||            ||
    """,

            ["Gun Shop"] = """
          __________________
         |      GUN SHOP    |
         |   _`_________,   |
         |   /  _______|    |
         |  /__/°           |
         |__________________|
              ||      ||
    """,

            ["Rose Town"] = """
             @        @
            /|\      /|\
          _/ | \____/ | \_
         |   ROSE TOWN    |
     ____|________________|____
         |  []   []   []  |
         |________________|
    """,

            ["Fallen Cow Inn"] = """
          __________________
         |  FALLEN COW INN  |
         |       (__)       |
         |       (oo)       |
         |_______|  |_______|
             []       []
    """,

            ["Tanner Shop"] = """
          ____________________
         |    TANNER SHOP     |
         |   .---------.      |
         |   |  HIDE   |      |
         |   '---------'      |
         |____________________|
    """,

            ["Silver Mine Dungeon"] = """
                 /\ 
            ____/  \____
           / SILVER MINE \
          /       __      \
         |_______/  \______|
             ==[__]==== rails
    """,

            ["Dungeon Level 1"] = """
          ____________________
         |   DUNGEON LEVEL 1  |
         |  ___       _____   |
         | |   |_____|     |  |
         |_|_______________|__|
             *        *
    """,

            ["Dungeon Level 2"] = """
          ____________________
         |   DUNGEON LEVEL 2  |
         |       .--.         |
         |  _____|  |_____    |
         |_/              \___|
            ~  ~  ~  ~
    """,

            ["Dungeon Level 3"] = """
          ____________________
         |   DUNGEON LEVEL 3  |
         |   .          .     |
         |  / \________/ \    |
         |_/              \___|
             *  *  *  *
    """,

            ["Dead Forest"] = """
          .       .       .
         /|\     /|\     /|\
        / | \   / | \   / | \
       /__|__\ /__|__\ /__|__\
       ___|_______|_______|___
          .   .       .   .
    """
        };
    }
}