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
        public List<Rectangle> hitboxes;
        private TextureMap texMap;//Holds all the Textures and handles loading them in
        public readonly int width = 40;
        public readonly int height = 22;

        public Tilemap(string file)
        {
            tiles = new Tile[width, height];
            texMap = new TextureMap();
            hitboxes = new List<Rectangle>();
            Initialize(file);
        }
        public void Initialize(string file)
        {
            string[] lines = File.ReadAllLines(file);
            //Load Textures from File (texMap handles this)
            texMap.Initialize(Main.currentDirectory + @"\\" + lines[0]);
            //Read Map from File and construct tiles
            for (int y = 0; y < 22; y++)
            {
                for (int x = 0; x < 40; x++)
                {
                    if ((int)char.GetNumericValue(lines[lines.Length - 22 + y][x]) != 0) //with texture (in textures)
                    {
                        tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[lines.Length - height + y][x]), texMap.textures[(int)char.GetNumericValue(lines[lines.Length - height + y][x])]);
                    }
                    else //without texture (0)
                    {
                        tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[lines.Length - height + y][x]));
                    }
                }
            }
        }
        private void MakeHitboxes()//merging all tile hitboxes together so we have fewer big ones
        {
            for(int y = 0; y < 22; y++)
            {
                for(int x = 0; x < 40; x++)
                {

                }
            }
        }
        public void Update() //do we even need this? Maybe, so it will stay here.
        {

        }
        public void Draw()
        {
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    tiles[x, y].Draw();
                }
            }
        }
    }
}
