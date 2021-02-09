using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.src;

namespace Platformer.src.UI.UIElements
{
    class UIText : UIElement
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public UIText(string text, Color textColor)
        {
            Text = text;
            TextColor = textColor;
        }
        public UIText(int text, Color textColor)
        {
            Text = text.ToString();
            TextColor = textColor;
        }
        protected override void Draw(SpriteBatch spriteBatch)
        {
            Width.Pixels = (int)Main.font.MeasureString(Text).X;
            Height.Pixels = (int)Main.font.MeasureString(Text).Y;
            Recalculate();
            spriteBatch.DrawString(Main.font, Text, Position, TextColor);
            base.Draw(spriteBatch);
        }
    }
}
