using GameHost.Render;
using GameLibrary.Core.Components;
using GameLibrary.Core.States;
using Spectre.Console;

namespace GameSpectre.Render
{
    public class SpectreShopClosedRenderer : IScreenRenderer
    {
        private readonly SpectreRenderManager _render_manager;

        public SpectreShopClosedRenderer(SpectreRenderManager render_manager)
        {
            _render_manager = render_manager;
        }

        public void Render(IState state)
        {
            AnsiConsole.Clear();

            ShopClosedState shopState = (ShopClosedState)state;
            ShopComponent shop = shopState.GetShopComponent();

            LocationComponent location =
                shop.GetGameObject().GetComponent<LocationComponent>();

            Panel shopPanel = new Panel(new Rows(
                new Markup($"Time: {shop.GetHour():00}:{shop.GetMinute():00}"),
                new Markup($"[red]Closed. Opens at " + $"{shop.GetShopData()._openingHour:00}:00[/]"),
                new Markup("Press Esc to return.")))
            {
                Border = BoxBorder.Rounded,
                Header = new PanelHeader(Markup.Escape(location.GetName())),
                BorderStyle = new Style(Color.White)
            }.Expand();

            Grid layout = new Grid();
            layout.AddColumn();
            layout.AddColumn();
            layout.AddRow(shopPanel, SpectreHeroPanel.Create(shopState.GetHeroObject()));

            AnsiConsole.Write(layout);
        }
    }
}