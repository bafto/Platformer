using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class Camera
    {
        private Vector2 ViewportSize;

        private float maxOffsetX;
        private float maxOffsetY;

        private float camX;
        private float camY;

        public Camera()
        {

        }

        public void Initialize()
        {
            ViewportSize.X = Main.graphics.GraphicsDevice.Viewport.Width;
            ViewportSize.Y = Main.graphics.GraphicsDevice.Viewport.Height;

            maxOffsetX = 5000 - ViewportSize.X;
            maxOffsetY = 5000 - ViewportSize.Y;
        }

        public void Update()
        {
            camX = Main.player.position.X - ViewportSize.X - Main.player.rect.Width / 2;
            camY = Main.player.position.Y - ViewportSize.Y - Main.player.rect.Height / 2;

            if (camX > maxOffsetX)
                camX = maxOffsetX;
            else if (camX < 0)
                camX = 0;
            if (camY > maxOffsetY)
                camY = maxOffsetY;
            else if (camY < 0)
                camY = 0;
        }

        public Rectangle Translate(Rectangle rect)
        {
            rect.X += -(int)camX;
            rect.Y += -(int)camY;
            return rect;
        }
    }
}
