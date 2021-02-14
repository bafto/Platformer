using Microsoft.Xna.Framework;

namespace Platformer.src.Enemies
{
    public class TrackEnemy : Enemy
    {
        Vector2 area;
        private bool running = false;
        public TrackEnemy(Vector2 pos, Vector2 size, float speed) : base(pos)
        {
            area = size;
            base.speed = speed;
        }

        protected override void Initialize()
        {
            base.Initialize();
            color = Color.IndianRed;
        }

        protected override void AI()
        {
            if (running)
            {
                velocity.X += Vector2.Normalize(new Vector2(Main.player.position.X - position.X, 0)).X * speed * Main.DeltaTime;
                velocity = Vector2.Clamp(velocity, new Vector2(-speed, 0f), new Vector2(speed, Main.level.gravity));
            }
            nextPosition = position + velocity;
            Vector2 min = position - area, max = position + rect.Size + area;
            if (!running && !Helper.IsClamp(Main.player.position, min, max))
                running = true;
        }
    }
}
