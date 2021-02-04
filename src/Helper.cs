using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer.src
{
    static class Extentions
    {
        // for extention or helper methods
        /// <summary>
        /// returns true if the key has just been pressed
        /// </summary>
        public static bool JustPressed(this KeyboardState s, Keys key)
        {
            return Main.lastKeyboard.IsKeyUp(key) && Main.keyboard.IsKeyDown(key);
        }
        public static Vector2 ToWorldCoords(this MouseState s)
        {
            return Main.camera.InverseTranslate(s.Position.ToVector2());
        }
    }
    static class Helper
    {
        /// <summary>
        /// Checks if a Clamp would occur
        /// </summary>
        /// <returns>if a clamp has occured</returns>
        public static bool IsClamp(Vector2 value, Vector2 min, Vector2 max)
        {
            if (value.X < min.X || value.Y < min.Y)
            {
                return true;
            }
            if (value.X > max.X || value.Y > max.Y)
            {
                return true;
            }
            return false;
        }
    }
}
