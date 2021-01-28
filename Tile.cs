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
<<<<<<< HEAD
        public Vector2 Position;
        public Tile(Vector2 pos, int tileID)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
            TileID = tileID;
            Position = pos;
=======
        private Texture2D texture;
        public Tile(Vector2 pos, int tileID, Texture2D tex = null)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
            TileID = tileID;
            texture = tex;
>>>>>>> 7d413d5d0a905bd8d8cdad7084c276fbcc7e96d6
        }

        public void Draw()
        {
            if (TileID != 0)
                Main.spriteBatch.Draw(texture, rect, Color.White);
        }
        public override string ToString()
        {
            return Position + ", ID: " + TileID;
        }
    }
}
