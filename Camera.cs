using Microsoft.Xna.Framework;

namespace Platformer
{
    public class Camera
    {
        private Vector2 ViewportSize;

        private Vector2 maxOffset;

        //the offset that will be applied to everything
        private Vector2 camOffset;

        public Camera()
        {

        }

        public void Initialize()
        {
            ViewportSize.X = Main.graphics.GraphicsDevice.Viewport.Width;
            ViewportSize.Y = Main.graphics.GraphicsDevice.Viewport.Height;

            maxOffset.X = 5000 - ViewportSize.X;
            maxOffset.Y = 5000 - ViewportSize.Y;
        }

        public void Update()
        {
            //calculate the offset
            camOffset.X = Main.player.position.X - ViewportSize.X - Main.player.rect.Width / 2;
            camOffset.Y = Main.player.position.Y - ViewportSize.Y - Main.player.rect.Height / 2;

            //clamp it to world coordinates
            camOffset = Vector2.Clamp(camOffset, Vector2.Zero, maxOffset);
        }

        public Rectangle Translate(Rectangle rect)
        {
            rect.X += -(int)camOffset.X;
            rect.Y += -(int)camOffset.Y;
            return rect;
        }
    }
}
