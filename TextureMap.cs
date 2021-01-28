﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Diagnostics;

namespace Platformer
{
    //The whole class is essentially a wrapper
    //for a <int, Texture2D> Dictionary and handles
    //the texture loading from a .texmap file
    public class TextureMap
    {
        public Dictionary<int, Texture2D> textures;

        public TextureMap()
        {
            textures = new Dictionary<int, Texture2D>();
        }
        public void Initialize(string file)
        {
            //Load File
            string[] lines = File.ReadAllLines(file);
            //Load Textures from File
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');
                if (line.Length == 2)
                {
                    textures.Add(int.Parse(line[0]), Main.LoadTexture(line[1]));
                }
                else if (line.Length == 6)
                {
                    Rectangle srcRect = new Rectangle(int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]), int.Parse(line[5]));
                    textures.Add(int.Parse(line[0]), Main.LoadTexturePart(line[1], srcRect));
                }
                else
                {
                    Debug.WriteLine($"File \"{file}\" is written in a wrong way");
                }
            }
        }
    }
}