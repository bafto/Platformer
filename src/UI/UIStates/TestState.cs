using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Platformer.src.UI.UIElements;

namespace Platformer.src.UI.UIStates
{
    class TestState : UIState
    {
        private UIText playerInfo;
        public override void Initialize()
        {
            var panel = new UIPanel(400, 100, Color.Red);
            Append(panel);
            playerInfo = new UIText("", Color.Black);
            panel.Append(playerInfo);
            base.Initialize();
        }
        protected override void Update(GameTime gameTime)
        {
            playerInfo.Text = $"Player Position: {Main.player.position}\nPlayer Velocity: {Main.player.velocity}\n Is player grounded?: {Main.player.grounded}\nZoom: {Main.GameScale}\n MousePos: {Main.mouse.Position} alt: {Main.InvertTranslate(Main.mouse.Position.ToVector2())}";
            base.Update(gameTime);
        }
    }
}
