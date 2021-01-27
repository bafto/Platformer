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
        public int TileID;
        public Tile(Vector2 pos, int tileID)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
            TileID = tileID;
        }

        public void Draw()
        {
            if (TileID != 0)
                Main.spriteBatch.Draw(Main.solid, rect, Color.Black);
        }
    }
}
