using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

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
        public static Vector2 ToWorldCoords(this Vector2 pos)
        {
            return Main.camera.InverseTranslate(pos / Main.GameScale);
        }
        public static Vector2 ToWorldCoords(this Point pos)
        {
            return Main.camera.InverseTranslate(pos.ToVector2() / Main.GameScale);
        }
        public static bool IsValid(this char c)
        {
            char[] validChars = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ä', 'Ö', 'Ü',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'ä', 'ö', 'ü',
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_', '-', '.', ',', ';', ':', '!', '$', '%', '&', '/', '(', ')', '=', '?', '{', '}', '[', ']',
                 '@', '€', '°', '^', '<', '>', '|', '\'', '#', '+', '*', '~', '\"', '\\', ' ', '|' };
            for (int i = 0; i < validChars.Length; i++)
            {
                if (validChars.Contains(c))
                {
                    return true;
                }
            }
            return false;
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
