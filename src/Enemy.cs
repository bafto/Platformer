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
        public Enemy(Vector2 pos)
        {
            position = pos;
        }
        protected override void Initialize()
        {
            base.Initialize();
            color = Color.Red;
        }
        public override void Update()
        {
            base.Update();
        }
        protected override void AI()
        {
            velocity = Vector2.Clamp(velocity, new Vector2(0f, 0f), new Vector2(0f, 15f));
            nextPosition = position + velocity;
        }
        public override void Draw()
        {
            base.Draw();
        }
    }
}
