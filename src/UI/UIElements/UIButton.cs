using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer.src.UI.UIElements
{
    class UIButton : UIPanel
    {
        public UIText Text;
        private Color _backgroundColor;
        public UIButton(UIText text, int width, int height, Color backgroundColor) : base(width, height, backgroundColor)
        {
            Text = text;
            Text.X.Percent = 50;
            Text.Y.Percent = 50;
            Append(text);
            _backgroundColor = BackgroundColor;
        }
        protected override void MouseEnter(MouseState args, UIElement elm)
        {
            BackgroundColor = Color.Lerp(_backgroundColor, Color.White, 0.6f);
            base.MouseEnter(args, elm);
        }
        protected override void MouseLeave(MouseState args, UIElement elm)
        {
            BackgroundColor = _backgroundColor;
            base.MouseLeave(args, elm);
        }
    }
}
