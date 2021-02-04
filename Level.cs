using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer
{
    public class Level
    {
        private List<EventTrigger> EventTriggers;
        private Tilemap tilemap;
        public Level(string file)
        {
            EventTriggers = new List<EventTrigger>();
            Initialize(file);
        }
        public void Initialize(string file)
        {
            tilemap = new Tilemap(file);
        }
        public void Update()
        {
            tilemap.Update();
            foreach(EventTrigger e in EventTriggers)
            {
                e.Update();
            }
        }
        public void Draw()
        {
            tilemap.Draw();
        }
    }
}
