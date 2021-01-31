using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Platformer
{
    public class Tilemap
    {
        public Tile[,] tiles;
        private TextureMap texMap;//Holds all the Textures and handles loading them in
        public int width;
        public int height;

        public Tilemap(string file)
        {
            texMap = new TextureMap();
            Initialize(file);
        }
        public void Initialize(string file)
        {
            string[] lines = File.ReadAllLines(file);
            //Load Textures from File (texMap handles this)
            texMap.Initialize(Main.currentDirectory + @"\\" + lines[0]);

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
        }
        public static Tile GetTileAtPos(Vector2 pos)
        {
            return Main.tilemap.tiles[(int)(pos.X / Tile.TileSize.X), (int)(pos.Y / Tile.TileSize.Y)];
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
        }
    }
}
