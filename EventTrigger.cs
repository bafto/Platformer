using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer
{
    class EventTrigger
    {
        public delegate void PlayerEventArgs(Player plr);
        public Vector2 Position;
        public int Width;
        public int Height;
        public Rectangle bounds;
        public event PlayerEventArgs OnPlayerInside;
        public event PlayerEventArgs OnPlayerEnter;
        public event PlayerEventArgs OnPlayerExit;

        public EventTrigger(Vector2 pos, int width, int height, List<EventTrigger> l)
        {
            Position = pos;
            Width = width;
            Height = height;
            bounds = new Rectangle(pos.ToPoint(), new Point(Width, Height));
            l.Add(this);
        }
        public EventTrigger(Vector2 pos, Vector2 size, List<EventTrigger> l)
        {
            bounds = new Rectangle(pos.ToPoint(), size.ToPoint());
            Position = pos;
            Width = (int)size.X;
            Height = (int)size.Y;
            l.Add(this);
        }
        public EventTrigger(Rectangle rect, List<EventTrigger> l)
        {
            bounds = rect;
            Position = new Vector2(rect.X, rect.Y);
            Width = rect.Size.X;
            Height = rect.Size.Y;
            l.Add(this);
        }
        public void Update()
        {
            if (bounds.Contains(Main.player.position))
            {
                OnPlayerInside?.Invoke(Main.player);
            }
            if(bounds.Contains(Main.player.position)  && !bounds.Contains(Main.player.lastPosition))
            {
                OnPlayerEnter?.Invoke(Main.player);
            }
            if (!bounds.Contains(Main.player.position) && bounds.Contains(Main.player.lastPosition))
            {
                OnPlayerExit?.Invoke(Main.player);
            }
        }
    }
}
