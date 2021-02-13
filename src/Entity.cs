using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            if(!noGravity)
            {
                velocity.Y += Main.level.gravity * Main.DeltaTime;
            }
            AI();
            HandleCollision();
        }
        protected virtual void AI() //For movement
        {

        }
        protected virtual void HandleCollision() //might be altered for different collision behaviour
        {
            // Keep player in level bounds
            if (Helper.IsClamp(position, Vector2.Zero, Main.level.bounds.VectorSize()))
            {
                position = Main.level.spawnPoint;
                nextPosition = Main.level.spawnPoint;
                velocity = Vector2.Zero;
            }
            else
            {
                // Handle collision by checking X first then Y then both and resolving it accordingly
                RectangleF nextRect = new RectangleF(nextPosition, rect.Size);
                for (int i = 0; i < Main.level.tilemap.hitboxes.Count; i++)
                {
                    if (nextRect.Intersects(Main.level.tilemap.hitboxes[i]))
                    {
                        //bool XorY = false;
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
                                //XorY = true;
                                if (nextRect.Position.Y > Main.level.tilemap.hitboxes[i].Bottom())
                                {
                                    nextRect.Position.Y = Main.level.tilemap.hitboxes[i].Bottom();
                                    velocity.Y = 0;
                                }
                                else if (nextRect.Position.Y + nextRect.Size.Y < Main.level.tilemap.hitboxes[i].Top())
                                {
                                    nextRect.Position.Y = Main.level.tilemap.hitboxes[i].Top() - nextRect.Size.Y;
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
                            if (nextRect.Position.X > Main.level.tilemap.hitboxes[i].Right())
                            {
                                nextRect.Position.X = Main.level.tilemap.hitboxes[i].Right();
                                velocity.X = 0;
                            }
                            else if (nextRect.Position.X + nextRect.Size.X < Main.level.tilemap.hitboxes[i].Left())
                            {
                                nextRect.Position.X = Main.level.tilemap.hitboxes[i].Left() - nextRect.Size.X;
                                velocity.X = 0;
                            }
                            else
                            {
                                velocity.X = 0;
                            }
                        }
                    }
                }
                // Set the new position
                position = nextRect.Position;
            }
        }
        public virtual void Draw()
        {
            rect.Position = position;
            Main.spriteBatch.Draw(Main.solid, rect.toIntRect(), color);
        }
        public override string ToString()
        {
            return $"Position: {position} Velocity: {velocity}";
        }
    }
}
