using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class Player
    {
        public Rectangle rect;
        public Vector2 position;
        public Vector2 lastPosition;
        public Color color;
        public float maxSpeed;
        public Vector2 Velocity;
        private float moveTimer;
        public float acceleration;
        public Player()
        {

        }
        public void Initialize()
        {
            position = new Vector2(0, 0);
            rect = new Rectangle(0, 0, 50, 50);
            color = Color.Red;
            maxSpeed = 10f;
            acceleration = 2f;
        }

        public void Update(GameTime gameTime)
        {
            Velocity.Y += 0.1f; // gravity
            moveTimer += 0.2f; // increment timer. value represents how fast the player will reach maxSpeed
            if (Main.keyboard.IsKeyDown(Keys.A))
            {
                Velocity.X -= MathHelper.Lerp(Velocity.X, maxSpeed, moveTimer) * acceleration * Main.deltaTime;
            }
            if (Main.keyboard.IsKeyDown(Keys.D))
            {
                Velocity.X += MathHelper.Lerp(Velocity.X, maxSpeed, moveTimer) * acceleration * Main.deltaTime;
            }
            if (Main.keyboard.IsKeyDown(Keys.W))
            {
                Velocity.Y -= MathHelper.Lerp(Velocity.Y, maxSpeed, moveTimer) * acceleration * Main.deltaTime;
            }
            if (Main.keyboard.IsKeyDown(Keys.S))
            {
                Velocity.Y += MathHelper.Lerp(Velocity.Y, maxSpeed, moveTimer) * acceleration * Main.deltaTime;
            }
            if (moveTimer >= 1)
            {
                moveTimer = 0;
            }
            // Clamp Velocity
            Velocity = Vector2.Clamp(Velocity, new Vector2(-maxSpeed), new Vector2(maxSpeed));

            // set position
            lastPosition = position;
            position += Velocity;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            spriteBatch.Draw(Main.solid, rect, color);
        }
    }
}
