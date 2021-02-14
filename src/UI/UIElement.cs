using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Platformer.src.UI
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////          THIS CODE IS FROM BASELIB           //////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    public readonly struct Padding
    {
        public static readonly Padding Zero = new Padding(0);

        public readonly int Left, Top, Right, Bottom;

        public Padding(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Padding(int padding)
        {
            Left = Top = Right = Bottom = padding;
        }
    }

    public readonly struct Margin
    {
        public static readonly Margin Zero = new Margin(0);

        public readonly int Left, Top, Right, Bottom;

        public Margin(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Margin(int margin)
        {
            Left = Top = Right = Bottom = margin;
        }
    }

    public class StyleDimension
    {
        public int Pixels;
        public int Percent;

        public StyleDimension()
        {
        }

        public StyleDimension(int pixels, int percent)
        {
            Pixels = pixels;
            Percent = percent;
        }
    }

    public enum Overflow
    {
        Visible,
        Hidden
    }

    public enum Display
    {
        Visible,
        None
    }
    public class UIElement
    {
        public delegate void MouseEvent(MouseState evt, UIElement elm);
        public delegate void KeyboardEvent(KeyboardState evt, UIElement elm);

        public UIElement Parent { get; protected internal set; }

        public List<UIElement> Children = new List<UIElement>();

        public bool IsMouseHovering { get; private set; }

        public object HoverText = null;

        public Vector2 Position
        {
            get => Dimensions.Location.ToVector2();
            set
            {
                X = new StyleDimension((int)value.X, 0);
                Y = new StyleDimension((int)value.Y, 0);
                Recalculate();
            }
        }

        public Vector2 Size
        {
            get => Dimensions.Size.ToVector2();
            set
            {
                Width = new StyleDimension((int)value.X, 0);
                Height = new StyleDimension((int)value.Y, 0);
                Recalculate();
            }
        }

        public Rectangle Dimensions { get; private set; }
        public Rectangle InnerDimensions { get; private set; }
        public Rectangle OuterDimensions { get; private set; }

        public Display Display = Display.Visible;
        public Overflow Overflow = Overflow.Visible;

        public StyleDimension Width = new StyleDimension();
        public StyleDimension Height = new StyleDimension();
        public StyleDimension X = new StyleDimension();
        public StyleDimension Y = new StyleDimension();

        public Padding Padding;
        public Margin Margin;

        public int? MinWidth;
        public int? MinHeight;
        public int? MaxWidth;
        public int? MaxHeight;

        #region Events
        public event MouseEvent OnMouseMove;
        public event MouseEvent OnMouseScroll;
        public event MouseEvent OnClick;
        public event MouseEvent OnDoubleClick;
        public event MouseEvent OnTripleClick;
        public event MouseEvent OnClickAway;
        public event MouseEvent OnMouseDown;
        public event MouseEvent OnMouseUp;
        public event MouseEvent OnMouseOut;
        public event MouseEvent OnMouseOver;
        public event MouseEvent OnMouseEnter;
        public event MouseEvent OnMouseLeave;

        public event KeyboardEvent OnKeyPressed;
        public event KeyboardEvent OnKeyReleased;
        public event Action<object, TextInputEventArgs> OnKeyTyped;

        public event Action<SpriteBatch> OnPreDraw;
        #endregion

        public UIElement()
        {
            Main.instance.Window.TextInput += KeyTyped;
        }

        #region Virtual methods
        protected virtual void Update(GameTime gameTime)
        {
        }

        protected virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        protected virtual void DrawChildren(SpriteBatch spriteBatch)
        {
            if (Overflow == Overflow.Visible)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    UIElement element = this[i];
                    if (element.Display != Display.None) element.InternalDraw(spriteBatch);
                }
            }
            else if (Overflow == Overflow.Hidden)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    UIElement element = this[i];
                    if (element.Display != Display.None &&
                        Parent.Dimensions.X < element.Dimensions.X + element.Dimensions.Width
                        && Parent.Dimensions.X + Parent.Dimensions.Width > element.Dimensions.X
                        && Parent.Dimensions.Y < element.Dimensions.Y + element.Dimensions.Height
                        && Parent.Dimensions.Y + Parent.Dimensions.Height > element.Dimensions.Y)
                        element.InternalDraw(spriteBatch);
                }
            }
        }
        // Mouse Events
        protected virtual void MouseDown(MouseState args, UIElement elm)
        {
            OnMouseDown?.Invoke(args, elm);
        }

        protected virtual void MouseUp(MouseState args, UIElement elm)
        {
            OnMouseUp?.Invoke(args, elm);
        }

        protected virtual void MouseClick(MouseState args, UIElement elm)
        {
            OnClick?.Invoke(args, elm);
        }

        protected virtual void DoubleClick(MouseState args, UIElement elm)
        {
            OnDoubleClick?.Invoke(args, elm);
        }

        protected virtual void TripleClick(MouseState args, UIElement elm)
        {
            OnTripleClick?.Invoke(args, elm);
        }

        protected virtual void ClickAway(MouseState args, UIElement elm)
        {
            OnClickAway?.Invoke(args, elm);
        }

        protected virtual void MouseMove(MouseState args, UIElement elm)
        {
            OnMouseMove?.Invoke(args, elm);
        }

        protected virtual void MouseScroll(MouseState args, UIElement elm)
        {
            OnMouseScroll?.Invoke(args, elm);
        }

        protected virtual void MouseEnter(MouseState args, UIElement elm)
        {
            OnMouseEnter?.Invoke(args, elm);
        }

        protected virtual void MouseLeave(MouseState args, UIElement elm)
        {
            OnMouseLeave?.Invoke(args, elm);
        }

        protected virtual void MouseOver(MouseState args, UIElement elm)
        {
            OnMouseOver?.Invoke(args, elm);
        }

        protected virtual void MouseOut(MouseState args, UIElement elm)
        {
            OnMouseOut?.Invoke(args, elm);
        }

        // Keyboard Events
        protected virtual void KeyTyped(object sender, TextInputEventArgs args)
        {
            OnKeyTyped?.Invoke(sender, args);
        }

        protected virtual void KeyReleased(KeyboardState args, UIElement elm)
        {
            OnKeyReleased?.Invoke(args, elm);
        }

        protected virtual void KeyPressed(KeyboardState args, UIElement elm)
        {
            OnKeyPressed?.Invoke(args, elm);
        }

        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {
        }
        #endregion

        #region Internal Methods

        internal bool ContainsPoint(Vector2 point) => point.X >= Dimensions.X && point.X <= Dimensions.X + Dimensions.Width && point.Y >= Dimensions.Y && point.Y <= Dimensions.Y + Dimensions.Height;

        internal void InternalUpdate(GameTime gameTime)
        {
            Update(gameTime);

            for (int i = 0; i < Children.Count; i++)
            {
                UIElement current = this[i];
                if (current.Display != Display.None) current.InternalUpdate(gameTime);
            }

            IsMouseHovering = ContainsPoint(Main.mouse.Position.ToVector2());

            UpdateEvents();
        }

        internal void InternalDraw(SpriteBatch spriteBatch)
        {
            GraphicsDevice device = spriteBatch.GraphicsDevice;
            SamplerState sampler = SamplerState.PointClamp;
            RasterizerState rasterizer = new RasterizerState { CullMode = CullMode.None, ScissorTestEnable = true };

            Rectangle original = device.ScissorRectangle;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, DepthStencilState.None, rasterizer, null, Main.UIScaleMatrix);

            OnPreDraw?.Invoke(spriteBatch);
            Draw(spriteBatch);

            spriteBatch.End();

            if (Overflow == Overflow.Hidden)
            {
                Rectangle clippingRectangle = GetClippingRectangle(spriteBatch);
                Rectangle adjustedClippingRectangle = Rectangle.Intersect(clippingRectangle, device.ScissorRectangle);
                device.ScissorRectangle = adjustedClippingRectangle;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, DepthStencilState.None, rasterizer, null, Main.UIScaleMatrix);

            DrawChildren(spriteBatch);
            if (IsMouseHovering && HoverText != null) Main.MouseText = HoverText.ToString();

            spriteBatch.End();

            device.ScissorRectangle = original;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, DepthStencilState.None, rasterizer, null, Main.UIScaleMatrix);
            // Debug
            /*
            if (OuterDimensions != Dimensions) spriteBatch.Draw(Main.solid, OuterDimensions, Color.Goldenrod * 0.5f);
            spriteBatch.Draw(Main.solid, Dimensions, Color.LimeGreen * 0.5f);
            if (InnerDimensions != Dimensions) spriteBatch.Draw(Main.solid, InnerDimensions, Color.LightBlue * 0.5f);
            // */
        }
        #endregion
        private void UpdateEvents()
        {
            if (IsMouseHovering)
            {
                MouseOver(Main.mouse, this);
                if (Main.mouseMoved)
                {
                    MouseMove(Main.mouse, this);
                }
                if (Main.scrollwheel != 0)
                {
                    MouseScroll(Main.mouse, this);
                }
                if (Main.LeftClick)
                {
                    MouseClick(Main.mouse, this);
                }
                // literally how
                //if ()
                //{
                //    DoubleClick(Main.mouse, this);
                //}
                //if ()
                //{
                //    TripleClick(Main.mouse, this);
                //}
                if (Main.LeftHeld)
                {
                    MouseDown(Main.mouse, this);
                }
                if (Main.LeftReleased)
                {
                    MouseUp(Main.mouse, this);
                }
                if (!Dimensions.Contains(Main.lastmouse.Position))
                {
                    MouseEnter(Main.mouse, this);
                }
            }
            else
            {
                MouseOut(Main.mouse, this);
                if (Dimensions.Contains(Main.lastmouse.Position))
                {
                    MouseLeave(Main.mouse, this);
                }
                if (Main.LeftClick)
                {
                    ClickAway(Main.mouse, this);
                }
            }
            if (Main.keyboard.GetPressedKeyCount() > 0)
            {
                KeyPressed(Main.keyboard, this);
            }
            else
            {
                KeyReleased(Main.keyboard, this);
            }
        }

        public virtual void Recalculate()
        {
            Rectangle parent = Parent?.InnerDimensions ?? new Rectangle(0, 0, (int)(Main.ViewPort.Width * (1f / Main.UIScale)), (int)(Main.ViewPort.Height * (1f / Main.UIScale)));

            Rectangle dimensions = Rectangle.Empty;

            int minWidth = Math.Max(0, MinWidth ?? 0);
            int minHeight = Math.Max(0, MinHeight ?? 0);
            int maxWidth = (int)Math.Min(Main.ViewPort.Width * (1f / Main.UIScale), MaxWidth ?? Main.ViewPort.Width * (1f / Main.UIScale));
            int maxHeight = (int)Math.Min(Main.ViewPort.Height * (1f / Main.UIScale), MaxHeight ?? Main.ViewPort.Height * (1f / Main.UIScale));

            dimensions.Width = (int)(Width.Percent * parent.Width / 100f + Width.Pixels);
            if (dimensions.Width < minWidth) dimensions.Width = minWidth;
            else if (dimensions.Width > maxWidth) dimensions.Width = maxWidth;

            dimensions.Height = (int)(Height.Percent * parent.Height / 100f + Height.Pixels);
            if (dimensions.Height < minHeight) dimensions.Height = minHeight;
            else if (dimensions.Height > maxHeight) dimensions.Height = maxHeight;

            dimensions.X = (int)(parent.X + (X.Percent * parent.Width / 100f - dimensions.Width * X.Percent / 100f) + X.Pixels);
            dimensions.Y = (int)(parent.Y + (Y.Percent * parent.Height / 100f - dimensions.Height * Y.Percent / 100f) + Y.Pixels);

            Dimensions = dimensions;
            InnerDimensions = new Rectangle(dimensions.X + Padding.Left, dimensions.Y + Padding.Top, dimensions.Width - Padding.Left - Padding.Right, dimensions.Height - Padding.Top - Padding.Bottom);
            OuterDimensions = new Rectangle(dimensions.X - Margin.Left, dimensions.Y - Margin.Top, dimensions.Width + Margin.Left + Margin.Right, dimensions.Height + Margin.Top + Margin.Bottom);

            RecalculateChildren();
        }

        public virtual void RecalculateChildren()
        {
            foreach (UIElement element in Children) element.Recalculate();
        }

        protected virtual Rectangle GetClippingRectangle(SpriteBatch spriteBatch)
        {
            Vector2 topLeft = InnerDimensions.Location.ToVector2();
            Vector2 bottomRight = new Vector2(InnerDimensions.Right, InnerDimensions.Bottom);

            topLeft = Vector2.Transform(topLeft, Main.UIScaleMatrix);
            bottomRight = Vector2.Transform(bottomRight, Main.UIScaleMatrix);

            int width = spriteBatch.GraphicsDevice.Viewport.Width;
            int height = spriteBatch.GraphicsDevice.Viewport.Height;

            Rectangle result = new Rectangle
            {
                X = (int)Math.Clamp(topLeft.X, 0, width),
                Y = (int)Math.Clamp(topLeft.Y, 0, height),
                Width = (int)Math.Clamp(bottomRight.X - topLeft.X, 0, width - topLeft.X),
                Height = (int)Math.Clamp(bottomRight.Y - topLeft.Y, 0, height - topLeft.Y)
            };

            return result;
        }

        internal IEnumerable<UIElement> ElementsAt(Vector2 point)
        {
            List<UIElement> elements = new List<UIElement>();

            foreach (UIElement element in Children.Where(element => element.ContainsPoint(point) && element.Display != Display.None))
            {
                elements.Add(element);
                elements.AddRange(element.ElementsAt(point));
            }

            elements.Reverse();
            return elements;
        }

        public virtual UIElement GetElementAt(Vector2 point)
        {
            UIElement element = Children.FirstOrDefault(current => current.ContainsPoint(point) && current.Display != Display.None);

            if (element != null) return element.GetElementAt(point);

            return ContainsPoint(point) && Display != Display.None ? this : null;
        }

        public void Append(UIElement item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Children.Contains(item)) throw new Exception($"Element {item} is already added");

            Children.Add(item);
            item.Parent = this;
            item.Recalculate();
        }

        public void AppendRange(IEnumerable<UIElement> elements)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            foreach (UIElement item in elements)
            {
                Children.Add(item);
                item.Parent = this;
                item.Recalculate();
            }
        }

        public void Insert(int index, UIElement item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            Children.Insert(index, item);
            item.Parent = this;
            item.Recalculate();
        }

        public void Remove()
        {
            Parent.Children.Remove(this);
            Parent = null;
        }

        public void Clear()
        {
            Children.Clear();
        }

        public UIElement this[int index] => Children[index];
    }
}
