using Microsoft.Xna.Framework;

namespace Platformer.src
{
    public class Camera
    {
        //basically screen size for our purposes
        private Vector2 ViewportSize;

        //maximum offset (basically the world size)
        private Vector2 MaxOffset => new Vector2(Main.tilemap.width * Tile.TileSize.X, Main.tilemap.height * Tile.TileSize.Y) - ViewportSize;

        //the offset that will be applied to everything
        private Vector2 camOffset;

        public Camera()
        {
            ViewportSize = new Vector2(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height);
        }

        public void Update()
        {
            //calculate the offset
            camOffset = Main.player.position - ViewportSize - Main.player.rect.size / 2;

            //clamp it to world coordinates
            camOffset = Vector2.Clamp(camOffset, Vector2.Zero, MaxOffset);
        }

        /// <summary>
        /// Apply Camera offset to the Rectangle
        /// </summary>
        /// <param name="rect">Rectangle to apply to</param>
        /// <returns>new Rectangle</returns>
        public Rectangle Translate(Rectangle rect)
        {
            return new Rectangle(rect.X - (int)camOffset.X, rect.Y - (int)camOffset.Y, rect.Width, rect.Height);
        }
        public Rectangle Translate(RectangleF rect)
        {
            return new RectangleF(rect.position.X - (int)camOffset.X, rect.position.Y - (int)camOffset.Y, rect.size.X, rect.size.Y).toIntRect();
        }
        public Vector2 Translate(Vector2 vec)
        {
            return vec - camOffset;
        }
        public Rectangle InverseTranslate(Rectangle rect)
        {
            return new Rectangle(rect.X + (int)camOffset.X, rect.Y + (int)camOffset.Y, rect.Width, rect.Height);
        }
        public RectangleF InverseTranslate(RectangleF rect)
        {
            return new RectangleF(rect.position.X + camOffset.X, rect.position.Y + camOffset.Y, rect.size.X, rect.size.Y);
        }
        public Vector2 InverseTranslate(Vector2 vec)
        {
            return vec + camOffset;
        }
    }
}
