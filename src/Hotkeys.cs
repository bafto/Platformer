using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer.src
{
    public class Hotkeys
    {
        private static bool frameStep;

        public static void Update()
        {
            if (frameStep)
            {
                Main.freeze = true;
            }

            Main.GameScale -= Input.scrollwheel;
            if (Input.keyboard.JustPressed(Keys.F))
            {
                frameStep = true;
                Main.freeze = false;
            }
            if (Input.keyboard.JustPressed(Keys.G))
            {
                frameStep = false;
                Main.freeze = false;
            }
            if (Input.keyboard.JustPressed(Keys.U))
            {
                Main.UIActive = !Main.UIActive;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Main.instance.Exit();
            }
        }
    }
}
