using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class Tile
    {
        public Rectangle rect;
        private Texture2D texture;

        public Tile(Vector2 pos, Texture2D tex)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
            texture = tex;
        }

        public void Draw()
        {
            Main.spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}
