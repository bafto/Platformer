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
        public float drag;

        public Player()
        {

        }
        public void Initialize()
        {
            position = new Vector2(100, 100);
            rect = new Rectangle(0, 0, 50, 50);
            color = Color.Red;
            maxSpeed = 10f;
            acceleration = 2f;
            drag = 20;
        }

        public void Update()
        {
            Velocity.Y += 8f * Main.deltaTime; // gravity
            if (Velocity.X != 0)
                Velocity.X += 0.1f * -(Velocity.X / Math.Abs(Velocity.X)); //very bad slowdown sideways. needs improvement
            moveTimer += 0.15f * Main.deltaTime; // increment timer. value represents how fast the player will reach maxSpeed

            // Handle input
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

            // Keep player in level bounds
            position = Vector2.Clamp(position, Vector2.Zero, new Vector2(Main.screen.Width - 50, Main.screen.Height - 90));
            if (Helper.isClamp(position, Vector2.Zero, new Vector2(Main.screen.Width - 50, Main.screen.Height - 90)))
            {
                Velocity = Vector2.Zero;
            }

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
