﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Platformer.src
{
    public class Player
    {
        public RectangleF rect;
        public Vector2 position;
        public Vector2 lastPosition;
        public Vector2 nextPosition;
        public Color color;
        public Vector2 velocity;
        public bool grounded;
        public float drag;
        public float acceleration;
        public float jumpspeed;
        public float maxJumpSpeed;
        public float maxFallSpeed;
        public float maxWalkSpeed;
        public float gravity;

        public Player()
        {
            Initialize();
        }
        public void Initialize()
        {
            position = Vector2.Zero;
            rect = new RectangleF(0, 0, 50, 50);
            color = Color.Red;
            maxWalkSpeed = 10f;
            maxJumpSpeed = 16f; // If the same as jumpspeed it does nothing, if lower it limits jumpspeed, if higher it enables a mechanic
            maxFallSpeed = 15f;
            acceleration = 0.5f;
            drag = 5;
            jumpspeed = 13f;
            gravity = 25f;
        }

        public void Update()
        {
            lastPosition = position;

            grounded = Main.level.tilemap.Collides(new RectangleF(position.X, position.Y + 1, rect.size.X, rect.size.Y));

            // gravity
            if (!grounded)
            {
                velocity.Y += gravity * Main.DeltaTime;
            }

            // Mainly handles jumping and movement
            HandleInput();

            // Resolve collision and set position
            HandleCollision();
        }
        private void HandleInput()
        {
            // Handle input
#if DEBUG
            if (Main.LeftClick)
            {
                position = Main.mouse.Position.ToWorldCoords() - rect.size / 2;
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
            velocity.X -= velocity.X / 30;

            // Clamp Velocity
            velocity = Vector2.Clamp(velocity, new Vector2(-maxWalkSpeed, -maxJumpSpeed), new Vector2(maxWalkSpeed, maxFallSpeed));

            nextPosition = position + velocity;
        }
        private void HandleCollision()
        {
            // Keep player in level bounds
            if (Helper.IsClamp(position, Vector2.Zero, new Vector2(Main.level.tilemap.width * Tile.TileSize.X - 50, Main.level.tilemap.height * Tile.TileSize.Y - 90)))
            {
                position = Main.level.spawnPoint;
                velocity = Vector2.Zero;
            }
            else
            {
                // Handle collision by checking X first then Y then both and resolving it accordingly
                RectangleF nextRect = new RectangleF(nextPosition, rect.size);
                for (int i = 0; i < Main.level.tilemap.hitboxes.Count; i++)
                {
                    if (nextRect.Intersects(Main.level.tilemap.hitboxes[i]))
                    {
                        //bool XorY = false;
                        // try only x intersection
                        nextRect.position.X -= velocity.X;
                        // still intersects?
                        if (nextRect.Intersects(Main.level.tilemap.hitboxes[i]))
                        {
                            nextRect.position.X += velocity.X;
                            // try only y intersection
                            nextRect.position.Y -= velocity.Y;
                            // still intersects?
                            if (nextRect.Intersects(Main.level.tilemap.hitboxes[i]))
                            {
                                // revert it cause it must be X and Y
                                nextRect.position.Y += velocity.Y;
                            }
                            else // no? set the position accordingly
                            {
                                //XorY = true;
                                if (nextRect.position.Y > Main.level.tilemap.hitboxes[i].Bottom())
                                {
                                    nextRect.position.Y = Main.level.tilemap.hitboxes[i].Bottom();
                                    velocity.Y = 0;
                                }
                                else if (nextRect.position.Y + nextRect.size.Y < Main.level.tilemap.hitboxes[i].Top())
                                {
                                    nextRect.position.Y = Main.level.tilemap.hitboxes[i].Top() - nextRect.size.Y;
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
                            //XorY = true;
                            if (nextRect.position.X > Main.level.tilemap.hitboxes[i].Right())
                            {
                                nextRect.position.X = Main.level.tilemap.hitboxes[i].Right();
                                velocity.X = 0;
                            }
                            else if (nextRect.position.X + nextRect.size.X < Main.level.tilemap.hitboxes[i].Left())
                            {
                                nextRect.position.X = Main.level.tilemap.hitboxes[i].Left() - nextRect.size.X;
                                velocity.X = 0;
                            }
                            else
                            {
                                velocity.X = 0;
                            }
                        }
                        // both must be needed
                        /*if (!XorY)
                        {
                            nextPosition.position -= velocity;
                            if (nextPosition.position.X > Main.level.tilemap.hitboxes[i].Right())
                            {
                                nextPosition.position.X = Main.level.tilemap.hitboxes[i].Right();
                                velocity.X = 0;
                            }
                            else if (nextPosition.position.X + nextPosition.size.X < Main.level.tilemap.hitboxes[i].Left())
                            {
                                nextPosition.position.X = Main.level.tilemap.hitboxes[i].Left() - nextPosition.size.X;
                                velocity.X = 0;
                            }
                            else
                            {
                                velocity.X = 0;
                            }*/
                            /*if (nextPosition.position.Y > Main.level.tilemap.hitboxes[i].Bottom())
                            {
                                nextPosition.position.Y = Main.level.tilemap.hitboxes[i].Bottom();
                                velocity.Y = 0;
                            }
                            else if (nextPosition.position.Y + nextPosition.size.Y < Main.level.tilemap.hitboxes[i].Top())
                            {
                                //nextPosition.position.Y = Main.level.tilemap.hitboxes[i].Top() - nextPosition.size.Y;
                                //velocity.Y = 0;
                            }
                            else
                            {
                                velocity.Y = 0;
                            }
                        }*/
                        /*// if he still intersects(aka velocity was 0) set him ontop of the Hitbox
                        if (nextPosition.Intersects(Main.level.tilemap.hitboxes[i]))
                        {
                            nextPosition.position = lastPosition;
                        }*/
                    }
                }
                // Set the new position
                position = nextRect.position;
            }
        }
        public void Draw()
        {
            rect.position = position;
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(rect), color);
#if DEBUG
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(new Rectangle(lastPosition.ToPoint(), rect.size.ToPoint())), Color.Green * 0.3f);
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(new Rectangle(nextPosition.ToPoint(), rect.size.ToPoint())), Color.Blue * 0.3f);
#endif
        }
        public override string ToString()
        {
            return $"pos: {position}, vel: {velocity}";
        }
    }
}
