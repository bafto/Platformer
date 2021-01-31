﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Platformer
{
    public class Player
    {
        public RectangleF rect;
        public Vector2 position;
        public Vector2 lastPosition;
        public Color color;
        public float maxSpeed;
        public Vector2 velocity;
        private float moveTimer;
        public float acceleration;
        public float drag;
        public bool grounded;
        public float jumpspeed;
        public Vector2 spawnpoint;

        public Player()
        {
            Initialize();
        }
        public void Initialize()
        {
            spawnpoint = new Vector2(300, 490);
            position = spawnpoint;
            rect = new RectangleF(0, 0, 50, 50);
            color = Color.Red;
            maxSpeed = 10f;
            acceleration = 2f;
            drag = 20;
            jumpspeed = 450f;
        }

        public void Update()
        {
            lastPosition = position;

            velocity.Y += 15f * Main.deltaTime; // gravity

            if (velocity.X != 0)
            {
                velocity = Vector2.Lerp(velocity, Vector2.Zero, moveTimer / drag);
            }

            HandleInput();

            // Clamp Velocity
            velocity = Vector2.Clamp(velocity, new Vector2(-maxSpeed), new Vector2(maxSpeed));

            HandleCollision();//resolve collision and set position
        }
        private void HandleInput()
        {
            // increment timer. value represents how fast the player will reach maxSpeed
            moveTimer += Main.deltaTime * 10;

            // Handle input
            if (Main.keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= MathHelper.Lerp(velocity.X, maxSpeed, moveTimer) * acceleration * Main.deltaTime;
            }
            if (Main.keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += MathHelper.Lerp(velocity.X, maxSpeed, moveTimer) * acceleration * Main.deltaTime;
            }
            if (/*grounded && */Main.keyboard.JustPressed(Keys.W))
            {
                velocity.Y -= jumpspeed * Main.deltaTime;
            }
            if (moveTimer >= 1)
            {
                moveTimer = 0;
            }
        }
        private void HandleCollision()
        {
            // Keep player in level bounds
            if (Helper.IsClamp(position, Vector2.Zero, new Vector2(Main.tilemap.width * Tile.TileSize.X - 50, Main.tilemap.height * Tile.TileSize.Y - 90)))
            {
                position = spawnpoint;
                velocity = Vector2.Zero;
            }
            else
            {
                //Handle collision by checking X first
                //then Y then both and resolving
                //it accordingly
                RectangleF playerRect = new RectangleF((position + velocity).X, (position + velocity).Y, rect.size.X, rect.size.Y);
                for (int i = 0; i < Main.tilemap.hitboxes.Count; i++)
                {
                    if (playerRect.Intersects(Main.tilemap.hitboxes[i]))
                    {
                        bool XorY = false;
                        playerRect.position.X -= velocity.X;//try only x intersection
                        if (playerRect.Intersects(Main.tilemap.hitboxes[i]))//still intersects?
                        {
                            playerRect.position.X += velocity.X;
                            playerRect.position.Y -= velocity.Y;//try only y intersection
                            if (playerRect.Intersects(Main.tilemap.hitboxes[i]))//still intersects?
                            {
                                playerRect.position.Y += velocity.Y;//revert it caus it must be X and Y
                            }
                            else//no? set the position accordingly
                            {
                                XorY = true;
                                if (playerRect.position.Y > Main.tilemap.hitboxes[i].Bottom())
                                {
                                    playerRect.position.Y = Main.tilemap.hitboxes[i].Bottom();
                                    velocity.Y = 0;
                                }
                                else if(playerRect.position.Y + playerRect.size.Y < Main.tilemap.hitboxes[i].Top())
                                {
                                    playerRect.position.Y = Main.tilemap.hitboxes[i].Top() - playerRect.size.Y;
                                    velocity.Y = 0;
                                }
                            }
                        }
                        else//no? set the position accordingly
                        {
                            XorY = true;
                            if(playerRect.position.X > Main.tilemap.hitboxes[i].Right())
                            {
                                playerRect.position.X = Main.tilemap.hitboxes[i].Right();
                                velocity.X = 0;
                            }
                            else if(playerRect.position.X + playerRect.size.X < Main.tilemap.hitboxes[i].Left())
                            {
                                playerRect.position.X = Main.tilemap.hitboxes[i].Left() - playerRect.size.X;
                                velocity.X = 0;
                            }
                        }
                        if (!XorY)//both must be needed
                        {
                            playerRect.position -= velocity;
                            if (playerRect.position.X > Main.tilemap.hitboxes[i].Right())
                            {
                                playerRect.position.X = Main.tilemap.hitboxes[i].Right();
                                velocity.X = 0;
                            }
                            else if (playerRect.position.X + playerRect.size.X < Main.tilemap.hitboxes[i].Left())
                            {
                                playerRect.position.X = Main.tilemap.hitboxes[i].Left() - playerRect.size.X;
                                velocity.X = 0;
                            }
                            if (playerRect.position.Y > Main.tilemap.hitboxes[i].Bottom())
                            {
                                playerRect.position.Y = Main.tilemap.hitboxes[i].Bottom();
                                velocity.Y = 0;
                            }
                            else if (playerRect.position.Y + playerRect.size.Y < Main.tilemap.hitboxes[i].Top())
                            {
                                playerRect.position.Y = Main.tilemap.hitboxes[i].Top() - playerRect.size.Y;
                                velocity.Y = 0;
                            }
                        }
                    }
                }
                position = playerRect.position;//set the new position
            }
        }
        public void Draw()
        {
            rect.position = position;
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(rect), color);
        }
        public override string ToString()
        {
            return $"pos: {position}, vel: {velocity}, moveTimer: {moveTimer}";
        }
    }
}
