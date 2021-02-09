using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer.src
{
    public class Level
    {
        public Vector2 spawnPoint;
        private List<EventTrigger> EventTriggers;
        public Tilemap tilemap;
        public Level(string file)
        {
            EventTriggers = new List<EventTrigger>();
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
            for (int i = 2; i < lines.Length && lines[i] != "enemies:"; i++)
            {
                string[] Lines = lines[i].Split(' ');
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
        }
        public void Update()
        {
            tilemap.Update();
            foreach (EventTrigger e in EventTriggers)
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
        }
    }
}
