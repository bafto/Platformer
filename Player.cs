using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Platformer
{
    public class Player
    {
        public Rectangle rect;
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
            rect = new Rectangle(0, 0, 50, 50);
            color = Color.Red;
            maxSpeed = 10f;
            acceleration = 2f;
            drag = 20;
            jumpspeed = 450f;
        }

        public void Update()
        {
            //go though all tiles
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    // check if the tile is not air and if the player is inside a tile
                    if (Main.tilemap.tiles[x, y].TileID != 0 && rect.Intersects(Main.tilemap.tiles[x, y].rect))
                    {
                        velocity.Y = 0; 
                    }
                }
            }
            grounded = Tilemap.GetTileAtPos(rect.Center.ToVector2() - new Vector2(0, -50)).TileID != 0;

            if (!grounded)
            {
                velocity.Y += 5f * Main.deltaTime; // gravity
            }

            if (velocity.X != 0)
            {
                velocity = Vector2.Lerp(velocity, Vector2.Zero, moveTimer / drag);
            }

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
            if (grounded && Main.keyboard.JustPressed(Keys.W))
            {
                velocity.Y -= jumpspeed * Main.deltaTime;
            }
            if (moveTimer >= 1)
            {
                moveTimer = 0;
            }
            // Clamp Velocity
            velocity = Vector2.Clamp(velocity, new Vector2(-maxSpeed), new Vector2(maxSpeed));

            // Keep player in level bounds
            if (Helper.IsClamp(position, Vector2.Zero, new Vector2(Main.tilemap.width * 50 - 50, Main.tilemap.height * 50 - 90)))
            {
                position = spawnpoint;
            }

            // set position
            lastPosition = position;
            position += velocity;
        }
        public void Draw()
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(rect), color);
        }
        public override string ToString()
        {
            return $"pos: {position}, vel: {velocity}, moveTimer: {moveTimer}";
        }
    }
}
