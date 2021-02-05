using Microsoft.Xna.Framework;

namespace Platformer.src
{
    class EventTrigger
    {
        //the different event types as enum for readability if we have many
        public enum EventType
        {
            LevelLoader = 0
        }

        public delegate void PlayerEventArgs();
        public Vector2 Position;
        public int Width;
        public int Height;
        public Rectangle bounds;
        public event PlayerEventArgs OnPlayerInside;
        public event PlayerEventArgs OnPlayerEnter;
        public event PlayerEventArgs OnPlayerExit;
        public EventType eventType;
        public string nextLevel;

        public EventTrigger(Vector2 pos, int width, int height, EventType eventType)
        {
            this.eventType = eventType;
            nextLevel = string.Empty;
            Position = pos;
            Width = width;
            Height = height;
            bounds = new Rectangle(pos.ToPoint(), new Point(Width, Height));
        }
        public EventTrigger(Vector2 pos, Vector2 size, EventType eventType)
        {
            this.eventType = eventType;
            nextLevel = string.Empty;
            bounds = new Rectangle(pos.ToPoint(), size.ToPoint());
            Position = pos;
            Width = (int)size.X;
            Height = (int)size.Y;
        }
        public EventTrigger(Rectangle rect, EventType eventType)
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
        public void Draw() => Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(bounds), Color.Red * 0.5f);
    }
}
