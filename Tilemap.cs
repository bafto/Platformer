using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer
{
    public class Tilemap
    {
        public Tile[,] tiles;

        public Tilemap()
        {
            tiles = new Tile[40,22];
        }
    }
}
