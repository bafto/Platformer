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
        private Dictionary<int, Texture2D> textures; //textures which the map holds, so we dont always load all the textures

        public Tilemap()
        {
            tiles = new Tile[40,22];
            textures = new Dictionary<int, Texture2D>();
        }
        public void Initialize(string file)
        {
            string[] lines = File.ReadAllLines(file);
            //Load Textures from File
            for (int i = 0; lines[i] != "map:"; i++)
            {
                string[] line = lines[i].Split(' ');
                Debug.WriteLine(line[1]);
                textures.Add(int.Parse(line[0]), Main.LoadTexture(line[1]));
            }
            //Read Map from File and construct tiles
            for(int y = 0; y < 22; y++)
            {
                for(int x = 0; x < 40; x++)
                {
                    if ((int)char.GetNumericValue(lines[lines.Length - 22 + y][x]) != 0) //with texture (in textures)
                    {
                        tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[lines.Length - 22 + y][x]), textures[(int)char.GetNumericValue(lines[lines.Length - 22 + y][x])]);
                    }
                    else //without texture (0)
                    {
                        tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[lines.Length - 22 + y][x]));
                    }
                }
            }
        }
        public void Update() //do we even need this? Maybe, so it will stay here.
        {

        }
        public void Draw()
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
