using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer.src.Enemies
{
    public class PathEnemy : Enemy
    {
        private int start, stop;
        private bool turn = true;
        public PathEnemy(Vector2 pos, int Stop, float Speed)
            :
            base(pos)
        {
            Initialize();
            start = (int)pos.X;
            stop = start + Stop;
            speed = Speed;
        }
        protected override void Initialize()
        {
            base.Initialize();
            color = Color.DarkRed;
        }
        protected override void AI()
        {
            velocity.X += speed * Main.DeltaTime;
            velocity = Vector2.Clamp(velocity, new Vector2(-speed, 0f), new Vector2(speed, Main.level.gravity));
            nextPosition = position + velocity;
            if(turn)
            {
                if(nextPosition.X + rect.Size.X > stop)
                {
                    turn = false;
                    speed = -speed;
                    nextPosition.X = stop - rect.Size.X;
                }
            }
            else
            {
                if(nextPosition.X < start)
                {
                    turn = true;
                    speed = -speed;
                    nextPosition.X = start;
                }
            }
        }
    }
}
