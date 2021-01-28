using Microsoft.Xna.Framework;
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
            Velocity.Y += 5f * Main.deltaTime; // gravity
            if (Velocity.X != 0)
            {
                Velocity = Vector2.Lerp(Velocity, Vector2.Zero, moveTimer / drag);
            }

            // increment timer. value represents how fast the player will reach maxSpeed
            moveTimer += Main.deltaTime * 10;

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
            if (Helper.IsClamp(position, Vector2.Zero, new Vector2(Main.screen.Width - 50, Main.screen.Height - 90)))
            {
                Velocity = Vector2.Zero;
            }

            // go though all tiles
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    // check if the tile is not air and if the player is inside a tile
                    if (Main.tilemap.tiles[x, y].TileID != 0 && rect.Intersects(Main.tilemap.tiles[x, y].rect))
                    {
                        Velocity = Vector2.Zero;
                        position = lastPosition;
                    }
                }
            }

            // set position
            lastPosition = position;
            position += Velocity;
        }
        public void Draw()
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(rect), color);
        }
        public override string ToString()
        {
            return $"pos: {position}, vel: {Velocity}, moveTimer: {moveTimer}";
        }
    }
}
