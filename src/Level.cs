using Microsoft.Xna.Framework;
using Platformer.src.Enemies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Platformer.src
{
    public class Level
    {
        public Vector2 spawnPoint;
        private List<EventTrigger> EventTriggers;
        public List<Enemy> Enemies;
        public Tilemap tilemap;
        public float gravity = 25f;
        public Rectangle bounds;

        public Level(string file)
        {
            EventTriggers = new List<EventTrigger>();
            Enemies = new List<Enemy>();
            Initialize(file);
        }

        public void Initialize(string file)
        {
            string[] lines = File.ReadAllLines(file);
            string[] spLine = lines[0].Split(' '); //The line where the spawnPoint is written
            //reset the player
            spawnPoint = new Vector2(int.Parse(spLine[0]), int.Parse(spLine[1]));
            Main.player.position = spawnPoint;
            Main.player.velocity = Vector2.Zero;
            //initialize the Events
            int lIndex = 2;
            for (lIndex = 2; lIndex < lines.Length && lines[lIndex] != "enemies:"; lIndex++)
            {
                string[] Lines = lines[lIndex].Split(' ');
                //Add the Event with a Rectangle and a EventID
                EventTriggers.Add(new EventTrigger(new Rectangle(int.Parse(Lines[1]), int.Parse(Lines[2]), int.Parse(Lines[3]), int.Parse(Lines[4])), (EventTrigger.EventType)int.Parse(Lines[0])));
                //Add functionality to the Event
                if (EventTriggers[EventTriggers.Count - 1].eventType == EventTrigger.EventType.LevelLoader)
                {
                    EventTriggers[EventTriggers.Count - 1].nextLevel = Lines[5];//set nextLevel to the filename of the Level that will be loaded
                    EventTriggers[EventTriggers.Count - 1].OnPlayerEnter += () => Main.level = new Level(Main.CurrentDirectory + @"\levels\" + EventTriggers[EventTriggers.Count - 1].nextLevel);
                }
            }
            //initialize the Enemys
            for (++lIndex; lIndex < lines.Length && lines[lIndex] != "map:"; lIndex++)
            {
                string[] Lines = lines[lIndex].Split(' ');
                //switch on the enemy ID (temporary identification to see what type of enemy it is)
                switch (int.Parse(Lines[0]))
                {
                    case 0:
                    {
                        Vector2 pos = new Vector2(int.Parse(Lines[1]), int.Parse(Lines[2]));
                        Enemies.Add(new Enemy(pos));
                        break;
                    }
                    case 1:
                    {
                        Vector2 pos = new Vector2(int.Parse(Lines[1]), int.Parse(Lines[2]));
                        int start = int.Parse(Lines[3]), stop = int.Parse(Lines[4]);
                        Enemies.Add(new PathEnemy(pos, start, stop, float.Parse(Lines[5])));
                        break;
                    }
                    case 2:
                    {
                        Vector2 pos = new Vector2(int.Parse(Lines[1]), int.Parse(Lines[2]));
                        Vector2 area = new Vector2(int.Parse(Lines[3]), int.Parse(Lines[4]));
                            float speed = float.Parse(Lines[5]);
                        Enemies.Add(new TrackEnemy(pos, area, speed));
                        break;
                    }
                    default:
                        break;
                }
            }
            //initialize the tilemap
            string[] mapLines = new string[lines.Length];
            for (int i = lines.Length - 1; i > -1; i--)
            {
                if (lines[i] == "map:")
                {
                    mapLines = new string[lines.Length - i];
                    Array.Copy(lines, i, mapLines, 0, lines.Length - i);
                }
            }
            tilemap = new Tilemap(mapLines);
            bounds = new Rectangle(0, 0, tilemap.width * (int)Tile.TileSize.X, tilemap.height * (int)Tile.TileSize.Y);
        }

        public void Update()
        {
            tilemap.Update();
            foreach (EventTrigger e in EventTriggers)
            {
                e.Update();
            }
            foreach (Enemy e in Enemies)
            {
                e.Update();
            }
        }

        public void Draw()
        {
            tilemap.Draw();

#if DEBUG
            foreach (EventTrigger e in EventTriggers)
            {
                e.Draw();
            }
#endif
            foreach (Enemy e in Enemies)
            {
                e.Draw();
            }
        }
    }
}
