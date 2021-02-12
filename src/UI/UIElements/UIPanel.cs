using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.src;

namespace Platformer.src.UI.UIElements
{
    class UIPanel : UIElement
    {
        public Color BackgroundColor;
        public UIPanel(int width, int height, Color backgroundcolor)
        {
            Width.Pixels = width;
            Height.Pixels = height;
            BackgroundColor = backgroundcolor;
        }
        public UIPanel(StyleDimension width, StyleDimension height, Color backgroundcolor)
        {
            Width = width;
            Height = height;
            BackgroundColor = backgroundcolor;
        }
        protected override void Draw(SpriteBatch spriteBatch)
        {
            Recalculate();
            var destinationPatches = Helper.CreatePatches(Dimensions);
            var _sourcePatches = Helper.CreatePatches(Main.panel.Bounds);
            for (var i = 0; i < _sourcePatches.Length; i++)
            {
                spriteBatch.Draw(Main.panel, destinationPatches[i], _sourcePatches[i], BackgroundColor);
            }
            base.Draw(spriteBatch);
        }
    }
}
