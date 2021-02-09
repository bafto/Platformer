using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.src;
using System;

namespace Platformer.src.UI.UIElements
{
    class UIInput<T> : UIPanel
    {
        public string PreviewText { get; set; }
        public UIText Input { get; set; }
        public bool Focused { get; set; }
        public Rectangle cursor;
        private string _text = string.Empty;
        public event KeyboardEvent TextChanged;
        public UIInput(string previewText, int width, int height, Color backgroundcolor, Color textColor) : base(width, height, backgroundcolor)
        {
            PreviewText = previewText;
            Input = new UIText(previewText, textColor);
            Input.Y.Percent = 50;
            Input.X.Pixels = 5;
            Append(Input);
        }
        protected override void MouseClick(MouseState args, UIElement elm)
        {
            Focused = true;
            base.MouseClick(args, elm);
        }
        protected override void ClickAway(MouseState args, UIElement elm)
        {
            Focused = false;
            base.ClickAway(args, elm);
        }
        protected override void KeyPressed(KeyboardState args, UIElement elm)
        {
            if ((args.IsKeyDown(Keys.LeftControl) || args.IsKeyDown(Keys.RightControl)) && args.IsKeyDown(Keys.Back))
            {
                _text = "";
            }
            base.KeyPressed(args, elm);
        }
        protected override void KeyTyped(object sender, TextInputEventArgs args)
        {
            bool valid = false;
            if (Focused)
            {
                if (typeof(T) == typeof(string))
                {
                    valid = args.Character.IsValid();
                }
                else if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                {
                    valid = char.IsDigit(args.Character) || args.Character == '.' && !_text.Contains('.');
                }
                else if (typeof(T) == typeof(int))
                {
                    valid = char.IsDigit(args.Character);
                }
                else if (typeof(T) == typeof(char))
                {
                    valid = args.Character.IsValid() && _text.Length == 0;
                }

                if (valid)
                {
                    _text += args.Character;
                }
                else
                {
                    if (args.Character == (char)8 && _text.Length > 0)
                    {
                        _text = _text[0..^1];
                    }
                    if (args.Character == (char)9)
                    {
                        _text += "   ";
                    }
                }
            }
            base.KeyTyped(sender, args);
        }
        private byte blink;
        protected override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // increment blinking timer
            blink++;

            // Draw Text
            if (_text.Length > 0 || Focused)
            {
                string prevText = Input.Text;
                Input.Text = _text;
                if (prevText != Input.Text)
                {
                    TextChanged?.Invoke(Main.keyboard, this);
                }
            }
            else
            {
                Input.Text = PreviewText;
            }

            if (Focused)
            {
                // Update Cursor variable
                cursor = new Rectangle(new Vector2(Position.X + Input.Width.Pixels + 3, Input.Position.Y).ToPoint(), new Point(2, 18));

                // Draw Cursor and handle blinking
                if (blink > 25)
                {
                    spriteBatch.Draw(Main.solid, cursor, Color.Black);
                }
                if (blink > 50)
                {
                    blink = 0;
                }
            }
        }
    }
}
