using Microsoft.Xna.Framework;

namespace Platformer.src
{
    public struct RectangleF
    {
        public Vector2 Position;
        public Vector2 Size;

        public RectangleF(Vector2 pos, Vector2 size)
        {
            Position = pos;
            Size = size;
        }
        public RectangleF(Vector2 pos, float width, float height)
        {
            Position = pos;
            Size = new Vector2(width, height);
        }
        public RectangleF(float x, float y, float width, float height)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }
        public RectangleF(Rectangle rect)
        {
            Position = rect.Location.ToVector2();
            Size = rect.Size.ToVector2();
        }
        public bool Intersects(Vector2 vec)
        {
            return vec.X > Position.X && vec.X < Position.X + Size.X &&
                vec.Y > Position.Y && vec.Y < Position.Y + Size.Y;
        }
        public bool Intersects(RectangleF rect)
        {
            return rect.Position.X + rect.Size.X > Position.X && rect.Position.X < Position.X + Size.X &&
                rect.Position.Y + rect.Size.Y > Position.Y && rect.Position.Y < Position.Y + Size.Y;
        }
        public bool Intersects(Rectangle other)
        {
            RectangleF rect = new RectangleF(other.X, other.Y, other.Width, other.Height);
            return rect.Position.X + rect.Size.X > Position.X && rect.Position.X < Position.X + Size.X &&
                rect.Position.Y + rect.Size.Y > Position.Y && rect.Position.Y < Position.Y + Size.Y;
        }
        public Vector2 Center()
        {
            return new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
        }
        public float Top()
        {
            return Position.Y;
        }
        public float Bottom()
        {
            return Position.Y + Size.Y;
        }
        public float Left()
        {
            return Position.X;
        }
        public float Right()
        {
            return Position.X + Size.X;
        }
        public bool Equals(RectangleF other)
        {
            return Position == other.Position && Size == other.Size;
        }
        public Rectangle toIntRect()
        {
            return new Rectangle(Position.ToPoint(), Size.ToPoint());
        }
    }
}
