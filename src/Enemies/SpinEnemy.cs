using Microsoft.Xna.Framework;

namespace Platformer.src.Enemies
{
    class SpinEnemy : Enemy
    {
        public float distanceToAnchor;
        public float timer = 0;
        public SpinEnemy(Vector2 pos, float distToAnchor, float rotationSpeed) : base(pos)
        {
            noGravity = true;
            distanceToAnchor = distToAnchor;
            color = Color.Yellow;
            speed = rotationSpeed;
        }
        protected override void AI()
        {
            position = startPosition + Vector2.One.RotatedBy(timer) * distanceToAnchor;
            nextPosition = position + velocity;
            timer += speed / 100;
        }
        protected override void HandleCollision()
        {
            // No Collision
        }
    }
}
