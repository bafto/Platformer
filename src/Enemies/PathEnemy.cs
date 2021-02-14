using Microsoft.Xna.Framework;

namespace Platformer.src.Enemies
{
    public class PathEnemy : Enemy
    {
        private int _start, _stop;
        private bool turn = true;

        public PathEnemy(Vector2 pos, int start, int stop, float speed) : base(pos)
        {
            _start = start; // set the start point (should be higher than the start position)
            _stop = stop; // set the stop point (should be higher than start)
            base.speed = speed;
        }

        protected override void Initialize()
        {
            base.Initialize();
            color = Color.DarkRed;
        }

        protected override void AI()
        {
            if (turn)
            {
                velocity.X += speed * Main.DeltaTime;
            }
            else
            {
                velocity.X -= speed * Main.DeltaTime;
            }

            // clamp velocity  to speed. Maybe one wants to set a different value then <speed>
            velocity = Vector2.Clamp(velocity, new Vector2(-speed, 0f), new Vector2(speed, Main.level.gravity));

            nextPosition = position + velocity;

            if (turn)
            {
                if (position.X + rect.Size.X > _stop)
                {
                    turn = false;
                    // With this the enemy is in a invisible box, and bumps off the walls. Without it he first slows down.
                    // If this is not enabled, the speed must be taken in account, or the enemie might fall of edges, because he cannot slow down quick enough.
                    // Might set this in the constructor later
                    velocity.X = 0;
                }
            }
            else
            {
                if (position.X < _start)
                {
                    turn = true;
                    velocity.X = 0;
                }
            }
        }
    }
}
