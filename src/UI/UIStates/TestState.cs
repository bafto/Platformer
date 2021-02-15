using Microsoft.Xna.Framework;
using Platformer.src.UI.UIElements;

namespace Platformer.src.UI.UIStates
{
    public class TestState : UIState
    {
        private UIText playerInfo;

        public override void Initialize()
        {
            var panel = new UIPanel(400, 200, Color.Red);
            Append(panel);
            playerInfo = new UIText("", Color.Black);
            panel.Append(playerInfo);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            playerInfo.Text = $"Player Position: {Main.player.position}\nPlayer Velocity: {Main.player.velocity}\nIs player grounded?: {Main.player.grounded}\nPlayer Health: {Main.player.health}\nZoom: {Main.GameScale}\nMouseWorld: {Main.MouseWorld}\nMouseScreen: {Main.mouse.Position}\nScreen Position: {Camera.camOffset}";
            base.Update(gameTime);
        }
    }
}
