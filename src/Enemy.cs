using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer.src
{
    public class Enemy : Entity
    {
        public float speed = 20f;
        public float start, stop;//test stuff
        public bool turn = true;//test stuff
        protected override void Initialize()
        {
            base.Initialize();
            position = Main.level.spawnPoint;
            start = Main.level.spawnPoint.X + 110f;//test stuff
            stop = Main.level.spawnPoint.X + 500f;//test stuff
        }
        public override void Update()
        {
            base.Update();
        }
        protected override void AI()
        {
            velocity.X += speed * Main.DeltaTime;
            velocity = Vector2.Clamp(velocity, new Vector2(-10f, 0f), new Vector2(10f, 15f));
            nextPosition = position + velocity;
            if (turn) // test stuff
            {
                if (nextPosition.X > stop)
                {
                    turn = false;
                    speed = -speed;
                }
            }
            else
            {
                if(nextPosition.X < start)
                {
                    turn = true;
                    speed = -speed;
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
        }
    }
}
