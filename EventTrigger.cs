﻿using Microsoft.Xna.Framework;
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
        public static List<EventTrigger> triggers = new List<EventTrigger>();

        public EventTrigger(Vector2 pos, int width, int height)
        {
            Position = pos;
            Width = width;
            Height = height;
            bounds = new Rectangle(pos.ToPoint(), new Point(Width, Height));
            triggers.Add(this);
        }
        public EventTrigger(Vector2 pos, Vector2 size)
        {
            bounds = new Rectangle(pos.ToPoint(), size.ToPoint());
            Position = pos;
            Width = (int)size.X;
            Height = (int)size.Y;
            triggers.Add(this);
        }
        public EventTrigger(Rectangle rect)
        {
            bounds = rect;
            Position = new Vector2(rect.X, rect.Y);
            Width = rect.Size.X;
            Height = rect.Size.Y;
            triggers.Add(this);
        }
        public void Update()
        {
            var lastplayerrect = new Rectangle(Main.player.lastPosition.ToPoint(), new Point(50, 50));
            if (Main.player.rect.Intersects(bounds))
            {
                OnPlayerInside?.Invoke(Main.player);
            }
            if (Main.player.rect.Intersects(bounds) && !lastplayerrect.Intersects(bounds))
            {
                OnPlayerEnter?.Invoke(Main.player);
            }
            if (!Main.player.rect.Intersects(bounds) && lastplayerrect.Intersects(bounds))
            {
                OnPlayerExit?.Invoke(Main.player);
            }
        }
        public void Draw() => Main.spriteBatch.Draw(Main.solid, Main.camera.Translate(bounds), Color.Red * 0.5f);
    }
}
