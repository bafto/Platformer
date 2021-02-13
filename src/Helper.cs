using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Platformer.src
{
    static class Extentions
    {
        // for extention or helper methods

        /// <returns><b>true</b> if the key has just been pressed</returns>
        public static bool JustPressed(this KeyboardState s, Keys key)
        {
            return Main.lastKeyboard.IsKeyUp(key) && Main.keyboard.IsKeyDown(key);
        }

        /// <returns>The X and Y components of a <b>Vector3</b> as a <b>Vector2</b></returns>
        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }

        /// <returns> The Size of a Rectangle as a <b>Vector2</b></returns>
        public static Vector2 VectorSize(this Rectangle r)
        {
            return r.Size.ToVector2();
        }

        /// <returns><b>true</b> if the character is valid (specified in an array)</returns>
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
        /// <returns><b>true</b> if a clamp has occured</returns>
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

        public static Rectangle[] CreatePatches(Rectangle rectangle)
        {
            int padding = 10;
            var x = rectangle.X;
            var y = rectangle.Y;
            var w = rectangle.Width;
            var h = rectangle.Height;
            var middleWidth = w - padding - padding;
            var middleHeight = h - padding - padding;
            var bottomY = y + h - padding;
            var rightX = x + w - padding;
            var leftX = x + padding;
            var topY = y + padding;
            var patches = new[]
            {
                new Rectangle(x,      y,        padding,        padding),       // top left
                new Rectangle(leftX,  y,        middleWidth,    padding),       // top middle
                new Rectangle(rightX, y,        padding,        padding),       // top right
                new Rectangle(x,      topY,     padding,        middleHeight),  // left middle
                new Rectangle(leftX,  topY,     middleWidth,    middleHeight),  // middle
                new Rectangle(rightX, topY,     padding,        middleHeight),  // right middle
                new Rectangle(x,      bottomY,  padding,        padding),       // bottom left
                new Rectangle(leftX,  bottomY,  middleWidth,    padding),       // bottom middle
                new Rectangle(rightX, bottomY,  padding,        padding)        // bottom right
            };
            return patches;
        }
    }
}
