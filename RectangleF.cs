using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class RectangleF
    {
        public Vector2 position;
        public Vector2 size;
        public RectangleF(Vector2 pos, Vector2 size)
        {
            position = pos;
            this.size = size;
        }
        public RectangleF(float x, float y, float width, float height)
        {
            position = new Vector2(x, y);
            size = new Vector2(width, height);
        }
        public bool Intersects(Vector2 vec)
        {
            return vec.X > position.X && vec.X < position.X + size.X &&
                vec.Y > position.Y && vec.Y < position.Y + size.Y;
        }
        public bool Intersects(RectangleF rect)
        {
            return rect.position.X + rect.size.X > position.X && rect.position.X < position.X + size.X &&
                rect.position.Y + rect.size.Y > position.Y && rect.position.Y < position.Y + size.Y;
        }
        public bool Intersects(Rectangle other)
        {
            RectangleF rect = new RectangleF(other.X, other.Y, other.Width, other.Height);
            return rect.position.X + rect.size.X > position.X && rect.position.X < position.X + size.X &&
                rect.position.Y + rect.size.Y > position.Y && rect.position.Y < position.Y + size.Y;
        }
        public bool Equals(RectangleF other)
        {
            return position == other.position && size == other.size;
        }
        public Rectangle toIntRect()
        {
            return new Rectangle(position.ToPoint(), size.ToPoint());
        }
    }
}
