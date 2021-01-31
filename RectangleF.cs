using Microsoft.Xna.Framework;

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
        public RectangleF(Rectangle rect)
        {
            position = rect.Location.ToVector2();
            size = rect.Size.ToVector2();
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
        public Vector2 Center()
        {
            return new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
        }
        public float Top()
        {
            return position.Y;
        }
        public float Bottom()
        {
            return position.Y + size.Y;
        }
        public float Left()
        {
            return position.X;
        }
        public float Right()
        {
            return position.X + size.X;
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
