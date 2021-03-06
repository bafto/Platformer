﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.src
{
    public class Entity
    {
        public RectangleF rect;
        public Vector2 position;
        public Vector2 nextPosition;
        public Vector2 velocity;
        public Color color = Color.Pink;
        public bool noGravity = false;

        public Entity()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            rect = new RectangleF(new Vector2(0, 0), new Vector2(50, 50));
            position = Vector2.Zero;
        }

        public virtual void Update()
        {
            if (!noGravity)
            {
                velocity.Y += Main.level.gravity * Main.DeltaTime;
            }
            AI();
            HandleCollision();
        }

        /// <summary>
        /// Movement
        /// </summary>
        protected virtual void AI()
        {

        }

        protected virtual void HandleCollision()
        {
            // Handle collision by checking X first then Y then both and resolving it accordingly
            RectangleF nextRect = new RectangleF(nextPosition, rect.Size);
            for (int i = 0; i < Main.level.tilemap.hitboxes.Count; i++)
            {
                if (nextRect.Intersects(Main.level.tilemap.hitboxes[i]))
                {
                    // try only x intersection
                    nextRect.Position.X -= velocity.X;
                    // still intersects?
                    if (nextRect.Intersects(Main.level.tilemap.hitboxes[i]))
                    {
                        nextRect.Position.X += velocity.X;
                        // try only y intersection
                        nextRect.Position.Y -= velocity.Y;
                        // still intersects?
                        if (nextRect.Intersects(Main.level.tilemap.hitboxes[i]))
                        {
                            // revert it cause it must be X and Y
                            nextRect.Position.Y += velocity.Y;
                        }
                        else // no? set the position accordingly
                        {
                            if (nextRect.Position.Y > Main.level.tilemap.hitboxes[i].Bottom)
                            {
                                nextRect.Position.Y = Main.level.tilemap.hitboxes[i].Bottom;
                                velocity.Y = 0;
                            }
                            else if (nextRect.Position.Y + nextRect.Size.Y < Main.level.tilemap.hitboxes[i].Top)
                            {
                                nextRect.Position.Y = Main.level.tilemap.hitboxes[i].Top - nextRect.Size.Y;
                                velocity.Y = 0;
                            }
                            else
                            {
                                velocity.Y = 0;
                            }
                        }
                    }
                    else // no? set the position accordingly
                    {
                        if (nextRect.Position.X > Main.level.tilemap.hitboxes[i].Right)
                        {
                            nextRect.Position.X = Main.level.tilemap.hitboxes[i].Right;
                            velocity.X = 0;
                        }
                        else if (nextRect.Position.X + nextRect.Size.X < Main.level.tilemap.hitboxes[i].Left)
                        {
                            nextRect.Position.X = Main.level.tilemap.hitboxes[i].Left - nextRect.Size.X;
                            velocity.X = 0;
                        }
                        else
                        {
                            velocity.X = 0;
                        }
                    }
                }

                // Set the new position
                position = nextRect.Position;
                rect.Position = position;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            rect.Position = position;
            spriteBatch.Draw(Main.solid, rect.ToIntRect(), color);
        }

        public override string ToString()
        {
            return $"Position: {position} Velocity: {velocity}";
        }
    }
}
