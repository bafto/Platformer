using Microsoft.Xna.Framework;
using Platformer.src.Enemies;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer.src
{
    public class Level
    {
        public Vector2 spawnPoint;
        public List<EventTrigger> EventTriggers;
        public List<Enemy> Enemies;
        public Tilemap tilemap;
        public float gravity = 25f;
        public Rectangle bounds;
        public string FilePath { get; private set; }

        private string[] fileLine;
        private uint counter;
        public Level(string file)
        {
            EventTriggers = new List<EventTrigger>();
            Enemies = new List<Enemy>();
            FilePath = file;
            counter = 0;
            Initialize(file);
        }
        public void Initialize(string file)
        {
            fileLine = File.ReadAllLines(file);

            // Parse Spawnpoint
            string[] spLine = fileLine[counter].Split(' ');
            spawnPoint = new Vector2(int.Parse(spLine[0]), int.Parse(spLine[1]));

            // reset the player
            Main.player.position = spawnPoint;
            Main.player.velocity = Vector2.Zero;
            Main.player.health = Player.maxHealth;

            InitializeEvents();
            InitializeEnemies();
            InitializeTileMap();
        }

        private void InitializeEvents()
        {
            // goes through the file and stops once it hits "enemies:"
            for (counter = 2; fileLine[counter] != "enemies:"; counter++)
            {
                string[] evtLine = fileLine[counter].Split(' ');

                //Add the Event
                EventTriggers.Add(new EventTrigger(
                    (EventTrigger.EventType)int.Parse(evtLine[0]),    // ID
                    new Rectangle(int.Parse(evtLine[1]), int.Parse(evtLine[2]), int.Parse(evtLine[3]), int.Parse(evtLine[4])))  // Bounds (x, y, w, h)
                    );

                //Add functionality to the Event
                if (EventTriggers[^1].eventType == EventTrigger.EventType.LevelLoader)
                {
                    //set nextLevel to the filename of the Level that will be loaded
                    EventTriggers[^1].nextLevel = evtLine[5];
                    EventTriggers[^1].OnPlayerInside += () => Main.level = new Level(Main.CurrentDirectory + @"\levels\" + EventTriggers[^1].nextLevel);
                }
            }
        }

        private void InitializeEnemies()
        {
            // increments the counter once, goes through the file and stops once it hits "map:" 
            for (++counter; fileLine[counter] != "map:"; counter++)
            {
                string[] enemyLine = fileLine[counter].Split(' ');
                Vector2 pos = new Vector2(int.Parse(enemyLine[1]), int.Parse(enemyLine[2]));
                //switch on the enemy ID (temporary identification to see what type of enemy it is)
                switch (int.Parse(enemyLine[0]))
                {
                    case 0:
                        {
                            Enemies.Add(new Enemy(pos));
                            break;
                        }
                    case 1:
                        {
                            int start = int.Parse(enemyLine[3]), stop = int.Parse(enemyLine[4]);
                            float speed = float.Parse(enemyLine[5]);
                            Enemies.Add(new PathEnemy(pos, start, stop, speed));
                            break;
                        }
                    case 2:
                        {
                            Vector2 area = new Vector2(int.Parse(enemyLine[3]), int.Parse(enemyLine[4]));
                            float speed = float.Parse(enemyLine[5]);
                            Enemies.Add(new TrackEnemy(pos, area, speed));
                            break;
                        }
                    case 3:
                        {
                            Enemies.Add(new CopyEnemy(pos, float.Parse(enemyLine[3])));
                            break;
                        }
                    case 4:
                        {
                            Enemies.Add(new SpinEnemy(pos, float.Parse(enemyLine[3]), float.Parse(enemyLine[4])));
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void InitializeTileMap()
        {
            //initialize the tilemap
            string[] mapLines = new string[fileLine.Length];

            for (int i = fileLine.Length - 1; i > -1; i--)
            {
                if (fileLine[i] == "map:")
                {
                    mapLines = new string[fileLine.Length - i];
                    Array.Copy(fileLine, i, mapLines, 0, fileLine.Length - i);
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
        public void Reset()
        {
            Main.player = new Player();
            Main.level = new Level(FilePath);
            Main.globalTimer = 0;
        }
    }
}
