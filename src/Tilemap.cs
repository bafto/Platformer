using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Platformer.src
{
    public class Tilemap
    {
        public Tile[,] tiles;
        private TextureMap texMap;//Holds all the Textures and handles loading them in
        public List<RectangleF> hitboxes;
        public int width;
        public int height;

        public Tilemap(string[] lines)
        {
            texMap = new TextureMap();
            hitboxes = new List<RectangleF>();
            Initialize(lines);
        }

        private void Initialize(string[] lines)
        {
            //Load Textures from File (texMap handles this)
            texMap.Initialize(Main.CurrentDirectory + @"\levels\" + lines[1]);

            height = lines.Length - 2;
            width = lines[2].Length;
            tiles = new Tile[width, height];
            //Read Map from File and construct tiles
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if ((int)char.GetNumericValue(lines[lines.Length - height + y][x]) != 0) //with texture (in textures)
                    {
                        tiles[x, y] = new Tile(new Vector2(x * Tile.TileSize.X, y * Tile.TileSize.Y), (int)char.GetNumericValue(lines[lines.Length - height + y][x]), texMap.textures[(int)char.GetNumericValue(lines[lines.Length - height + y][x])]);
                    }
                    else //without texture (0)
                    {
                        tiles[x, y] = new Tile(new Vector2(x * Tile.TileSize.X, y * Tile.TileSize.Y), (int)char.GetNumericValue(lines[lines.Length - height + y][x]));
                    }
                }
            }
            MakeHitboxes();
        }

        private void MakeHitboxes()//Merge the tile rects together (Don't ask how it works. Please)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y].TileID != 0 && !tiles[x, y].inHitbox)
                    {
                        RectangleF newHitbox = new RectangleF(tiles[x, y].rect);
                        tiles[x, y].inHitbox = true;
                        bool firstLoop = true, breaked = false;
                        int xEnd = 0;
                        for (int yy = y; yy < height; yy++)
                        {
                            int xx = x + 1;
                            if (firstLoop)
                            {
                                for (; xx < width && tiles[xx, yy].TileID != 0 && !tiles[xx, yy].inHitbox; xx++)
                                {
                                    newHitbox.Size.X += Tile.TileSize.X;
                                    tiles[xx, yy].inHitbox = true;
                                }
                                xEnd = xx;
                                firstLoop = false;
                            }
                            else
                            {
                                for (xx = x; xx < xEnd && tiles[xx, yy].TileID != 0 && !tiles[xx, yy].inHitbox; xx++)
                                {
                                    tiles[xx, yy].inHitbox = true;
                                }
                            }
                            if (xx != xEnd)
                            {
                                newHitbox.Size.Y -= Tile.TileSize.Y;
                                breaked = true;
                                for (int i = x; i < xx; i++)
                                {
                                    tiles[i, yy].inHitbox = false;
                                }
                                break;
                            }
                            newHitbox.Size.Y += Tile.TileSize.Y;
                        }
                        if (!breaked)
                        {
                            newHitbox.Size.Y -= Tile.TileSize.Y;
                        }
                        hitboxes.Add(newHitbox);
                    }
                }
            }
        }

        public static Tile GetTileAtPos(Vector2 pos)
        {
            return Main.level.tilemap.tiles[(int)(pos.X / Tile.TileSize.X), (int)(pos.Y / Tile.TileSize.Y)];
        }

        public bool Collides(RectangleF rect)
        {
            for (int i = 0; i < hitboxes.Count; i++)
            {
                if (rect.Intersects(hitboxes[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public void Update() //do we even need this? Maybe, so it will stay here.
        {

        }

        public void Draw()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y].Draw();
                }
            }
#if DEBUG
            for (int i = 0; i < hitboxes.Count; i++)
            {
                var destinationPatches = Helper.CreatePatches(hitboxes[i].toIntRect());
                var _sourcePatches = Helper.CreatePatches(Main.outline.Bounds);
                for (var j = 0; j < _sourcePatches.Length; j++)
                {
                    Main.spriteBatch.Draw(Main.outline, destinationPatches[j], _sourcePatches[j], Color.White);
                }
            }
#endif
        }
    }
}
