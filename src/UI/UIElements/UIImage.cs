using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.src.UI.UIElements
{
    class UIImage : UIElement
    {
        public Texture2D Texture;
        public float rotation = 0f;
        public Vector2 origin = Vector2.Zero;
        public UIImage(Texture2D tex, int width, int height)
        {
            Texture = tex;
            Width.Pixels = width;
            Height.Pixels = height;
        }
        protected override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Dimensions, null, Color.White, rotation, origin, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }
    }
}
