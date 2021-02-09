using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private float moveTimer;
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
            maxJumpSpeed = 16f;//if the same as jumpspeed it does nothing, if lower it limits jumpspeed, if higher it enables a mechanic
            maxFallSpeed = 15f;
            acceleration = 2f;
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

            //mainly handles jumping and movement
            HandleInput();

            // Clamp Velocity
            velocity = Vector2.Clamp(velocity, new Vector2(-maxWalkSpeed, -maxJumpSpeed), new Vector2(maxWalkSpeed, maxFallSpeed));

            //resolve collision and set position
            HandleCollision();
        }
        private void HandleInput()
        {
            // increment timer. value represents how fast the player will reach maxSpeed
            moveTimer += Main.DeltaTime * 10;
            if (moveTimer > 1)
            {
                moveTimer = 0;
            }

            // Handle input
#if DEBUG
            if (Main.LeftClick)
            {
                position = Main.mouse.ToWorldCoords() - rect.size / 2;
            }
#endif
            if (Main.keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= MathHelper.Lerp(velocity.X, maxWalkSpeed, moveTimer) * acceleration * Main.DeltaTime;
            }
            if (Main.keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += MathHelper.Lerp(velocity.X, maxWalkSpeed, moveTimer) * acceleration * Main.DeltaTime;
            }
            if (grounded && Main.keyboard.JustPressed(Keys.W))
            {
                velocity.Y -= jumpspeed;
            }
            //drag so the player slows on X movement
            if (velocity.X != 0 && !(Main.keyboard.IsKeyDown(Keys.A) || Main.keyboard.IsKeyDown(Keys.D)) && grounded)
            {
                velocity.X = MathHelper.Lerp(velocity.X, 0, moveTimer / 3);
            }
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
                //Handle collision by checking X first
                //then Y then both and resolving
                //it accordingly
                RectangleF playerRect = new RectangleF(position + velocity, rect.size);
                for (int i = 0; i < Main.level.tilemap.hitboxes.Count; i++)
                {
                    if (playerRect.Intersects(Main.level.tilemap.hitboxes[i]))
                    {
                        bool XorY = false;
                        playerRect.position.X -= velocity.X;//try only x intersection
                        if (playerRect.Intersects(Main.level.tilemap.hitboxes[i]))//still intersects?
                        {
                            playerRect.position.X += velocity.X;
                            playerRect.position.Y -= velocity.Y;//try only y intersection
                            if (playerRect.Intersects(Main.level.tilemap.hitboxes[i]))//still intersects?
                            {
                                //revert it cause it must be X and Y
                                playerRect.position.Y += velocity.Y;
                            }
                            else//no? set the position accordingly
                            {
                                XorY = true;
                                if (playerRect.position.Y > Main.level.tilemap.hitboxes[i].Bottom())
                                {
                                    playerRect.position.Y = Main.level.tilemap.hitboxes[i].Bottom();
                                    velocity.Y = 0;
                                }
                                else if (playerRect.position.Y + playerRect.size.Y < Main.level.tilemap.hitboxes[i].Top())
                                {
                                    playerRect.position.Y = Main.level.tilemap.hitboxes[i].Top() - playerRect.size.Y;
                                    velocity.Y = 0;
                                }
                                else
                                {
                                    velocity.Y = 0;
                                }
                            }
                        }
                        else //no? set the position accordingly
                        {
                            XorY = true;
                            if (playerRect.position.X > Main.level.tilemap.hitboxes[i].Right())
                            {
                                playerRect.position.X = Main.level.tilemap.hitboxes[i].Right();
                                velocity.X = 0;
                            }
                            else if (playerRect.position.X + playerRect.size.X < Main.level.tilemap.hitboxes[i].Left())
                            {
                                playerRect.position.X = Main.level.tilemap.hitboxes[i].Left() - playerRect.size.X;
                                velocity.X = 0;
                            }
                            else
                            {
                                velocity.X = 0;
                            }
                        }
                        //both must be needed
                        if (!XorY)
                        {
                            playerRect.position -= velocity;
                            if (playerRect.position.X > Main.level.tilemap.hitboxes[i].Right())
                            {
                                playerRect.position.X = Main.level.tilemap.hitboxes[i].Right();
                                velocity.X = 0;
                            }
                            else if (playerRect.position.X + playerRect.size.X < Main.level.tilemap.hitboxes[i].Left())
                            {
                                playerRect.position.X = Main.level.tilemap.hitboxes[i].Left() - playerRect.size.X;
                                velocity.X = 0;
                            }
                            else
                            {
                                velocity.X = 0;
                            }
                            if (playerRect.position.Y > Main.level.tilemap.hitboxes[i].Bottom())
                            {
                                playerRect.position.Y = Main.level.tilemap.hitboxes[i].Bottom();
                                velocity.Y = 0;
                            }
                            else if (playerRect.position.Y + playerRect.size.Y < Main.level.tilemap.hitboxes[i].Top())
                            {
                                playerRect.position.Y = Main.level.tilemap.hitboxes[i].Top() - playerRect.size.Y;
                                velocity.Y = 0;
                            }
                            else
                            {
                                velocity.Y = 0;
                            }
                        }
                        if (playerRect.Intersects(Main.level.tilemap.hitboxes[i]))//if he still intersects(aka velocity was 0) set him ontop of the Hitbox
                        {
                            playerRect.position = new Vector2(playerRect.position.X, Main.level.tilemap.hitboxes[i].position.Y - rect.size.Y);
                        }
                    }
                }
                //set the new position
                position = playerRect.position;
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
