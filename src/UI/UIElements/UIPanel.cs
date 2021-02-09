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
            var destinationPatches = CreatePatches(Dimensions);
            var _sourcePatches = CreatePatches(Main.panel.Bounds);
            for (var i = 0; i < _sourcePatches.Length; i++)
            {
                spriteBatch.Draw(Main.panel, destinationPatches[i], _sourcePatches[i], BackgroundColor);
            }
            base.Draw(spriteBatch);
        }
        private Rectangle[] CreatePatches(Rectangle rectangle)
        {
            int padding = 10;
            var x = rectangle.X;
            var y = rectangle.Y;
            var w = rectangle.Width;
            var h = rectangle.Height;
            var middleWidth = w - padding - padding;
            var middleHeight = h - padding - padding;
            var bottomY = y + h - padding;
            var rightX = x + w - padding;
            var leftX = x + padding;
            var topY = y + padding;
            var patches = new[]
            {
                new Rectangle(x,      y,        padding,        padding),       // top left
                new Rectangle(leftX,  y,        middleWidth,    padding),       // top middle
                new Rectangle(rightX, y,        padding,        padding),       // top right
                new Rectangle(x,      topY,     padding,        middleHeight),  // left middle
                new Rectangle(leftX,  topY,     middleWidth,    middleHeight),  // middle
                new Rectangle(rightX, topY,     padding,        middleHeight),  // right middle
                new Rectangle(x,      bottomY,  padding,        padding),       // bottom left
                new Rectangle(leftX,  bottomY,  middleWidth,    padding),       // bottom middle
                new Rectangle(rightX, bottomY,  padding,        padding)        // bottom right
            };
            return patches;
        }
    }
}
