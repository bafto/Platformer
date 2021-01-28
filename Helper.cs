using Microsoft.Xna.Framework;

namespace Platformer
{
    static class Helper
    {
        // for extention or helper methods
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
