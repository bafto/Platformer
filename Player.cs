using Microsoft.Xna.Framework;
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
        private bool Collides()//check if the player collides with any tile
        {
            //go though all tiles
            for (int x = 0; x < Main.tilemap.width; x++)
            {
                for (int y = 0; y < Main.tilemap.height; y++)
                {
                    // check if the tile is not air and if the player is inside a tile
                    if (Main.tilemap.tiles[x, y].TileID != 0 && new Rectangle((int)(position + velocity).X, (int)(position + velocity).Y, 50, 50).Intersects(Main.tilemap.tiles[x, y].rect))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void HandleCollision()
        {
            // Keep player in level bounds
            if (Helper.IsClamp(position, Vector2.Zero, new Vector2(Main.tilemap.width * 50 - 50, Main.tilemap.height * 50 - 90)))
            {
                position = spawnpoint;
                velocity = Vector2.Zero;
            }
            else
            {
                RectangleF playerRect = new RectangleF((position + velocity).X, (position + velocity).Y, 50, 50);
                for (int x = 0; x < Main.tilemap.width; x++)
                {
                    for (int y = 0; y < Main.tilemap.height; y++)
                    {
                        if (Main.tilemap.tiles[x, y].TileID != 0 && playerRect.Intersects(Main.tilemap.tiles[x, y].rect))
                        {
                            bool XorY = false;
                            playerRect.position.X -= velocity.X;
                            if (playerRect.Intersects(Main.tilemap.tiles[x, y].rect))
                            {
                                playerRect.position.X += velocity.X;
                                playerRect.position.Y -= velocity.Y;
                                if (playerRect.Intersects(Main.tilemap.tiles[x, y].rect))
                                {
                                    playerRect.position.Y += velocity.Y;
                                }
                                else
                                {
                                    XorY = true;
                                }
                            }
                            else
                            {
                                XorY = true;
                            }
                            if (!XorY)
                            {
                                playerRect.position.X -= velocity.X;
                                playerRect.position.Y -= velocity.Y;
                            }
                        }
                    }
                }
                Vector2 oldVelocity = velocity;
                velocity.X = playerRect.position.X - lastPosition.X;
                velocity.Y = playerRect.position.Y - lastPosition.Y;
                position += velocity;
            }
        }
        public void Draw()
        {
            rect.position = position;
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(rect.toIntRect()), color);
        }
        public override string ToString()
        {
            return $"pos: {position}, vel: {velocity}, moveTimer: {moveTimer}";
        }
    }
}
