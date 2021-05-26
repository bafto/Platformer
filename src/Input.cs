using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer.src
{
    public class Input
    {
        /// <summary>
        /// The mouse relative to the screen
        /// </summary>
        public static MouseState mouse = Mouse.GetState();
        public static MouseState lastmouse;
        public static KeyboardState keyboard;
        public static KeyboardState lastKeyboard;
        /// <summary>
        /// How much the scroll wheel has changed since the last frame
        /// </summary>
        public static float scrollwheel;
        public static bool LeftHeld;
        public static bool RightHeld;
        public static bool LeftReleased;
        public static bool RightReleased;
        public static bool LeftClick;
        public static bool RightClick;
        /// <summary>
        /// How much the mouse has moved since the last frame
        /// </summary>
        public static bool mouseMoved;
        public static Vector2 MouseWorld => Camera.InvertTranslate(mouse.Position);

        /// <summary>
        /// Updates input variables
        /// </summary>
        internal static void UpdateInput()
        {
            lastmouse = mouse;
            mouse = Mouse.GetState();
            lastKeyboard = keyboard;
            keyboard = Keyboard.GetState();
            mouseMoved = mouse.Position != lastmouse.Position;
            scrollwheel = (lastmouse.ScrollWheelValue - mouse.ScrollWheelValue) / 8000f;

            LeftHeld = mouse.LeftButton == ButtonState.Pressed;
            RightHeld = mouse.RightButton == ButtonState.Pressed;
            LeftReleased = mouse.LeftButton == ButtonState.Released;
            RightReleased = mouse.RightButton == ButtonState.Released;
            LeftClick = LeftReleased && lastmouse.LeftButton == ButtonState.Pressed;
            RightClick = RightReleased && lastmouse.RightButton == ButtonState.Pressed;
        }
    }
}
