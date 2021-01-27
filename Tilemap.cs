using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class Tilemap
    {
        public Tile[,] tiles;
        private Dictionary<int, Texture2D> textures;

        public Tilemap()
        {
            tiles = new Tile[40,22];
        }
        public void Initialize(String file)
        {
            //Load Textures from File

            //Read Map from File and construct tiles

        }
        public void Update() //do we even need this? Maybe, so it will stay here.
        {

        }
        public void Draw() //can't be used yet, tiles need to be constructed
        {
            for(int x = 0; x < 40; x++)
            {
                for(int y = 0; y < 22; y++)
                {
                    tiles[x, y].Draw();
                }
            }
        }
    }
}
