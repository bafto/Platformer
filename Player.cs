using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class Player
    {
        public Rectangle rect;
        public Vector2 position;
        public Color color;
        public float speed;

        public Player()
        {

        }
        public void Initialize()
        {
            position = new Vector2(0, 0);
            rect = new Rectangle(0, 0, 50, 50);
            color = Color.Red;
            speed = 200;
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw()
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            Main.spriteBatch.Draw(Main.solid, rect, color);
        }
    }
}
