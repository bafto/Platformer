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
            textures = new Dictionary<int, Texture2D>();
        }
        public void Initialize(string file)
        {
<<<<<<< HEAD
            //Read Map from File and construct tiles
            string[] lines = File.ReadAllLines(file);
            for(int y = 0; y < lines.Length; y++)
=======
            string[] lines = File.ReadAllLines(Main.currentDirectory + @"\level0.txt");
            //Load Textures from File
            for (int i = 0; lines[i] != "map:"; i++)
            {
                string[] line = lines[i].Split(' ');
                Debug.WriteLine(line[1]);
                textures.Add(int.Parse(line[0]), Main.loadTexture(line[1]));
            }
            //Read Map from File and construct tiles
            for(int y = 0; y < 22; y++)
>>>>>>> 7d413d5d0a905bd8d8cdad7084c276fbcc7e96d6
            {
                for(int x = 0; x < 40; x++)
                {
<<<<<<< HEAD
                    tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[y][x]));
                    Debug.WriteLine(tiles[x, y]);
=======
                    if ((int)char.GetNumericValue(lines[lines.Length - 22 + y][x]) != 0)
                    {
                        tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[lines.Length - 22 + y][x]), textures[(int)char.GetNumericValue(lines[lines.Length - 22 + y][x])]);
                        Debug.WriteLine("x: " + x + ", y: " + y + ", TileID: " + lines[y][x]);
                    }
                    else
                    {
                        tiles[x, y] = new Tile(new Vector2(x * 50, y * 50), (int)char.GetNumericValue(lines[lines.Length - 22 + y][x]));
                        Debug.WriteLine(y);
                    }
>>>>>>> 7d413d5d0a905bd8d8cdad7084c276fbcc7e96d6
                }
            }
        }
        public void Update() //do we even need this? Maybe, so it will stay here.
        {

        }
<<<<<<< HEAD
        public void Draw(SpriteBatch spriteBatch)
=======
        public void Draw()
>>>>>>> 7d413d5d0a905bd8d8cdad7084c276fbcc7e96d6
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
