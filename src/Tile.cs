using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.src
{
    public class Tile
    {
        public Rectangle rect;
        public readonly int TileID;
        public Texture2D texture;
        public Vector2 Position; //we don't need this, it's just nice for the toString
        public static readonly Vector2 TileSize = new Vector2(50, 50);//for less magic numbers
        public bool inHitbox;

        public Tile(Vector2 pos, int tileID, Texture2D tex = null)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, (int)TileSize.X, (int)TileSize.Y);
            TileID = tileID;
            texture = tex;
            Position = pos;
            inHitbox = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (TileID != 0)
            {
                spriteBatch.Draw(texture, rect, Color.White);
            }
        }

        public override string ToString()
        {
            return Position + ", ID: " + TileID + ", index: " + (int)(Position.X / TileSize.X) + ", " + (int)(Position.Y / TileSize.Y);
        }
    }
}
