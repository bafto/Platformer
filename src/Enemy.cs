using Microsoft.Xna.Framework;

namespace Platformer.src
{
    public class Enemy : Entity
    {
        public float speed = 20f;
        public int damage { get; protected set; }

        public Enemy(Vector2 pos)
        {
            position = pos;
        }

        protected override void Initialize()
        {
            base.Initialize();
            color = Color.Red;
            damage = 1;
        }

        protected override void AI()
        {
            velocity = Vector2.Clamp(velocity, Vector2.Zero, new Vector2(0f, Main.level.gravity));
            nextPosition = position + velocity;
        }
    }
}
