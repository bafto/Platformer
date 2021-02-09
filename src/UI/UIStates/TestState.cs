using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Platformer.src.UI.UIElements;

namespace Platformer.src.UI.UIStates
{
    class TestState : UIState
    {
        public override void Initialize()
        {
            var panel = new UIPanel(400, 100, Color.Red);
            Append(panel);
            var test = new UIText("yo wässup", Color.Black);
            panel.Append(test);
            base.Initialize();
        }
    }
}
