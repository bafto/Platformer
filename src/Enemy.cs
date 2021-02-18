using Microsoft.Xna.Framework;

namespace Platformer.src
{
    public class Enemy : Entity
    {
        public float speed = 20f;
        public Vector2 startPosition;
        public int Damage { get; protected set; }

        public Enemy(Vector2 pos)
        {
            startPosition = pos;
            position = pos;
        }

        protected override void Initialize()
        {
            base.Initialize();
            color = Color.Red;
            Damage = 1 * Main.difficulty;
        }
        protected override void HandleCollision()
        {
            if (Helper.IsClamp(position, Vector2.Zero, Main.level.bounds.VectorSize()))
            {
                position = startPosition;
            }
            else
            {
                base.HandleCollision();
            }
        }
        protected override void AI()
        {
            velocity = Vector2.Clamp(velocity, Vector2.Zero, new Vector2(0f, Main.level.gravity));
            nextPosition = position + velocity;
        }
    }
}
