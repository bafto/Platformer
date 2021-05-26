using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.src.ID;

namespace Platformer.src
{
    public class EventTrigger
    {
        public delegate void PlayerEventArgs();
        public Vector2 Position;
        public int Width;
        public int Height;
        public Rectangle bounds;
        public event PlayerEventArgs OnPlayerInside;
        public event PlayerEventArgs OnPlayerEnter;
        public event PlayerEventArgs OnPlayerExit;
        public EventID eventType;
        public string nextLevel;

        public EventTrigger(EventID eventType, Vector2 pos, int width, int height)
        {
            this.eventType = eventType;
            nextLevel = string.Empty;
            Position = pos;
            Width = width;
            Height = height;
            bounds = new Rectangle(pos.ToPoint(), new Point(Width, Height));
        }

        public EventTrigger(EventID eventType, Vector2 pos, Vector2 size)
        {
            this.eventType = eventType;
            nextLevel = string.Empty;
            bounds = new Rectangle(pos.ToPoint(), size.ToPoint());
            Position = pos;
            Width = (int)size.X;
            Height = (int)size.Y;
        }

        public EventTrigger(EventID eventType, Rectangle rect)
        {
            this.eventType = eventType;
            nextLevel = string.Empty;
            bounds = rect;
            Position = new Vector2(rect.X, rect.Y);
            Width = rect.Size.X;
            Height = rect.Size.Y;
        }

        public void Update()
        {
            var lastplayerrect = new Rectangle(Main.player.lastPosition.ToPoint(), new Point(50, 50));
            if (Main.player.rect.Intersects(bounds))
            {
                OnPlayerInside?.Invoke();
            }
            if (Main.player.rect.Intersects(bounds) && !lastplayerrect.Intersects(bounds))
            {
                OnPlayerEnter?.Invoke();
            }
            if (!Main.player.rect.Intersects(bounds) && lastplayerrect.Intersects(bounds))
            {
                OnPlayerExit?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Main.solid, bounds, Color.Red * 0.5f);
        }
    }
}
