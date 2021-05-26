using Microsoft.Xna.Framework;

namespace Platformer.src
{
    public class Camera
    {
        public static Vector2 camOffset;

        public static Matrix Translate(Matrix matrix)
        {
            // Calculate Translation
            Vector2 viewportSize = new Vector2(Main.ViewPort.Width, Main.ViewPort.Height);
            camOffset = Main.player.position - viewportSize / 2 / Main.GameScale;

            // Prevent camera from going offscreen
            Vector2 MaxOffset = Main.level.bounds.VectorSize() - viewportSize;
            camOffset = Vector2.Clamp(camOffset, Vector2.Zero, MaxOffset * Main.GameScale);

            // Apply Translation
            matrix.Translation -= new Vector3(camOffset.X, camOffset.Y, 0) * Main.GameScale;

            return matrix;
        }

        public static Vector2 InvertTranslate(Vector2 vector)
        {
            Matrix invMatrix = Matrix.Invert(Main.InGameMatrix);
            return Vector2.Transform(vector, invMatrix);
        }

        public static Vector2 InvertTranslate(Point point)
        {
            Matrix invMatrix = Matrix.Invert(Main.InGameMatrix);
            return Vector2.Transform(point.ToVector2(), invMatrix);
        }
    }
}
