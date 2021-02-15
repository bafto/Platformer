using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.src.Enemies
{
    class CopyEnemy : Enemy
    {
        public float distanceToPlayer;
        public CopyEnemy(Vector2 pos, float dist) : base(pos)
        {
            distanceToPlayer = dist;
        }
        protected override void Initialize()
        {
            base.Initialize();
            color = Color.Blue;
        }
        protected override void AI()
        {
            if (Main.player.trail.Count >= 60)
            {
                position = Main.player.trail[(int)(Main.player.trail.Count / distanceToPlayer) - 1];
            }
            nextPosition = position + velocity;
        }
    }
}
