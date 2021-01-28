using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class Tile
    {
        public Rectangle rect;
        public int TileID;
        private Texture2D texture;
        public Vector2 Position; //we don't need this, it's just nice for the toString
        public Tile(Vector2 pos, int tileID, Texture2D tex = null)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
            TileID = tileID;
            texture = tex;
            Position = pos;
        }

        public void Draw()
        {
            if (TileID != 0)
                Main.spriteBatch.Draw(texture, Main.camera.Translate(rect), Color.White);
        }
        public override string ToString()
        {
            return Position + ", ID: " + TileID;
        }
    }
}
