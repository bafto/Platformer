using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Platformer.src
{
    public class Player
    {
        public RectangleF rect;
        public Vector2 position;
        public Vector2 lastPosition;
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
                position = Main.mouse.ToWorldCoords() - rect.size / 2;
            }
#endif
            if (Main.keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= Math.Abs(velocity.X) * Main.DeltaTime + acceleration;
            }
            if (Main.keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += Math.Abs(velocity.X) * Main.DeltaTime + acceleration;
            }
            if (grounded && Main.keyboard.JustPressed(Keys.W))
            {
                velocity.Y -= jumpspeed;
            }
            // Drag so the player slows on X movement
            velocity.X -= velocity.X / 30;

            // Clamp Velocity
            velocity = Vector2.Clamp(velocity, new Vector2(-maxWalkSpeed, -maxJumpSpeed), new Vector2(maxWalkSpeed, maxFallSpeed));
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
                RectangleF nextPosition = new RectangleF(position + velocity, rect.size);
                for (int i = 0; i < Main.level.tilemap.hitboxes.Count; i++)
                {
                    if (nextPosition.Intersects(Main.level.tilemap.hitboxes[i]))
                    {
                        bool XorY = false;
                        // try only x intersection
                        nextPosition.position.X -= velocity.X;
                        // still intersects?
                        if (nextPosition.Intersects(Main.level.tilemap.hitboxes[i]))
                        {
                            nextPosition.position.X += velocity.X;
                            // try only y intersection
                            nextPosition.position.Y -= velocity.Y;
                            // still intersects?
                            if (nextPosition.Intersects(Main.level.tilemap.hitboxes[i]))
                            {
                                // revert it cause it must be X and Y
                                nextPosition.position.Y += velocity.Y;
                            }
                            else // no? set the position accordingly
                            {
                                XorY = true;
                                if (nextPosition.position.Y > Main.level.tilemap.hitboxes[i].Bottom())
                                {
                                    nextPosition.position.Y = Main.level.tilemap.hitboxes[i].Bottom();
                                    velocity.Y = 0;
                                }
                                else if (nextPosition.position.Y + nextPosition.size.Y < Main.level.tilemap.hitboxes[i].Top())
                                {
                                    nextPosition.position.Y = Main.level.tilemap.hitboxes[i].Top() - nextPosition.size.Y;
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
                            XorY = true;
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
                            }
                        }
                        // both must be needed
                        if (!XorY)
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
                            }
                            if (nextPosition.position.Y > Main.level.tilemap.hitboxes[i].Bottom())
                            {
                                nextPosition.position.Y = Main.level.tilemap.hitboxes[i].Bottom();
                                velocity.Y = 0;
                            }
                            else if (nextPosition.position.Y + nextPosition.size.Y < Main.level.tilemap.hitboxes[i].Top())
                            {
                                nextPosition.position.Y = Main.level.tilemap.hitboxes[i].Top() - nextPosition.size.Y;
                                velocity.Y = 0;
                            }
                            else
                            {
                                velocity.Y = 0;
                            }
                        }
                        // if he still intersects(aka velocity was 0) set him ontop of the Hitbox
                        if (nextPosition.Intersects(Main.level.tilemap.hitboxes[i]))
                        {
                            nextPosition.position = new Vector2(nextPosition.position.X, Main.level.tilemap.hitboxes[i].position.Y - rect.size.Y);
                        }
                    }
                }
                // Set the new position
                position = nextPosition.position;
            }
        }
        public void Draw()
        {
            rect.position = position;
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(rect), color);
        }
        public override string ToString()
        {
            return $"pos: {position}, vel: {velocity}";
        }
    }
}
