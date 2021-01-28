using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Platformer
{
    public class Tilemap
    {
        public Tile[,] tiles;

        public Tilemap()
        {
            tiles = new Tile[40,22];
        }
        public void Initialize(string file)
        {
            //Read Map from File and construct tiles
            string[] lines = File.ReadAllLines(file);
            for(int y = 0; y < lines.Length; y++)
            {
                for(int x = 0; x < lines[y].Length; x++)
                {
                    tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[y][x]));
                    Debug.WriteLine(tiles[x, y]);
                }
            }
        }
        public void Update() //do we even need this? Maybe, so it will stay here.
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int x = 0; x < 40; x++)
            {
                for(int y = 0; y < 22; y++)
                {
                    tiles[x, y].Draw(spriteBatch);
                }
            }
        }
    }
}
