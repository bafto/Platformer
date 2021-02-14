using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Platformer.src
{
    public class Player : Entity
    {
        public Vector2 lastPosition;
        public bool grounded;
        public float drag;
        public float acceleration;
        public float jumpspeed;
        public float maxJumpSpeed;
        public float maxFallSpeed;
        public float maxWalkSpeed;
        public List<Vector2> trail = new List<Vector2>();

        protected override void Initialize()
        {
            position = Vector2.Zero;
            nextPosition = Vector2.Zero;
            rect = new RectangleF(0, 0, 50, 50);
            color = Color.Red;
            maxWalkSpeed = 10f;
            maxJumpSpeed = 200f;
            maxFallSpeed = 15f;
            acceleration = 0.5f;
            drag = 5;
            jumpspeed = 13f;
        }

        public override void Update()
        {
            lastPosition = position;
            trail.Add(position);
            if (Main.globalTimer > 60) trail.RemoveAt(0);

            // gravity
            if (!grounded)
            {
                velocity.Y += Main.level.gravity * Main.DeltaTime;
            }

            grounded = Main.level.tilemap.Collides(new RectangleF(position.X, position.Y + 2, rect.Size.X, rect.Size.Y));

            // Mainly handles jumping and movement
            HandleInput();

            // Handle collision and set position
            base.HandleCollision();
        }

        /// <summary>
        /// Jumping and Movement
        /// </summary>
        private void HandleInput()
        {
            // Handle input
#if DEBUG
            if (Main.LeftClick)
            {
                position = Main.MouseWorld - rect.Size / 2;
            }
#endif
            if (Main.keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= 10 * Math.Abs(velocity.X) * Main.DeltaTime + acceleration;
            }
            if (Main.keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += 10 * Math.Abs(velocity.X) * Main.DeltaTime + acceleration;
            }
            if (grounded && Main.keyboard.JustPressed(Keys.W))
            {
                velocity.Y -= jumpspeed;
            }
            // Drag so the player slows on X movement
            velocity.X -= velocity.X / 15;

            // Clamp Velocity
            velocity = Vector2.Clamp(velocity, new Vector2(-maxWalkSpeed, -maxJumpSpeed), new Vector2(maxWalkSpeed, maxFallSpeed));

            nextPosition = position + velocity;
        }

        public override void Draw()
        {
            rect.Position = position;
            Main.spriteBatch.Draw(Main.solid, rect.toIntRect(), color);
#if DEBUG
            Main.spriteBatch.Draw(Main.solid, new Rectangle(lastPosition.ToPoint(), rect.Size.ToPoint()), Color.Green * 0.3f);
            Main.spriteBatch.Draw(Main.solid, new Rectangle((position + velocity).ToPoint(), rect.Size.ToPoint()), Color.Blue * 0.3f);
            for(int i = 0; i < trail.Count; i++)
                Main.spriteBatch.Draw(Main.solid, new Rectangle(trail[i].ToPoint() + (rect.Size / 2).ToPoint(), new Point(4, 4)), Color.Red);

            var groundedCheck = new Rectangle((int)position.X, (int)position.Y + 2, 50, 50);
            Main.spriteBatch.Draw(Main.solid, new Rectangle((int)position.X, groundedCheck.Bottom, 50, 2), Color.Yellow);
#endif
        }
        public override string ToString()
        {
            return $"pos: {position}, vel: {velocity}";
        }
    }
}
