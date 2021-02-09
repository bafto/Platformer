using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace Platformer.src.UI
{
    public class UIState
    {
        public List<UIElement> elements = new List<UIElement>();

        #region virtual methods
        public virtual void Initialize()
        {
        }

        protected virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].InternalUpdate(gameTime);
            }
        }
        protected virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].InternalDraw(spriteBatch);
            }
        }
        #endregion

        #region internal methods

        internal void UpdateSelf(GameTime gameTime)
        {
            Update(gameTime);
        }

        internal void DrawSelf(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch);
        }
        #endregion

        public void Append(UIElement item)
        {
            elements.Add(item);
            item.Recalculate();
        }

        public void Remove(UIElement item)
        {
            item.Remove();
            elements.Remove(item);
        }
    }
}
