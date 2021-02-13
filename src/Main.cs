using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.src.UI;
using Platformer.src.UI.UIStates;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer.src
{
    public class Main : Game
    {
        // Engine Stuff
        public static Main instance { get; private set; }
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Texture2D solid;
        public static SpriteFont font;
        public static Texture2D panel;
        public static Texture2D outline;
        public static Texture2D background;
        public static Rectangle Screen
        {
            get
            {
                // Update Screen variable
                var sex = System.Windows.Forms.Control.FromHandle(instance.Window.Handle).Bounds;
                return new Rectangle(sex.X, sex.Y, sex.Width, sex.Height);
            }
        }
        public static Viewport ViewPort => graphics.GraphicsDevice.Viewport;
        public static float DeltaTime { get; private set; }
        public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static string? MouseText;
        public static List<UIState> UIStates = new List<UIState>();
        public static float UIScale = 1f;
        public static Matrix UIScaleMatrix;
        public static float GameScale = 1f;
        public static Matrix GameMatrix;
        public static long globalTimer;
        public static byte gameSpeed = 1;
        public static bool freeze = false;
        public static bool UIActive = true;

        // input stuff
        /// <summary>
        /// The mouse relative to the screen
        /// </summary>
        public static MouseState mouse = Mouse.GetState();
        public static MouseState lastmouse;
        public static KeyboardState keyboard;
        public static KeyboardState lastKeyboard;
        /// <summary>
        /// How much the scroll wheel has changed since the last frame
        /// </summary>
        public static float scrollwheel;
        public static bool LeftHeld;
        public static bool RightHeld;
        public static bool LeftReleased;
        public static bool RightReleased;
        public static bool LeftClick;
        public static bool RightClick;
        /// <summary>
        /// How much the mouse has moved since the last frame
        /// </summary>
        public static bool mouseMoved;

        //Game Stuff
        public static Player player;
        public static Level level;

        public Main()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Platformer test"; // title
            IsMouseVisible = true; // mouse is visible
            Window.AllowUserResizing = true; // user can resize window

            // Maximize Window
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            var test = new TestState();
            UIStates.Add(test);
        }

        protected override void Initialize()
        {
            player = new Player();
            level = new Level(CurrentDirectory + @"\levels\level0.level");

            // initialize UIState
            for (int i = 0; i < UIStates.Count; i++)
            {
                UIStates[i].Initialize();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            solid = Content.Load<Texture2D>("solid");
            font = Content.Load<SpriteFont>("font");
            panel = Content.Load<Texture2D>("panel");
            outline = Content.Load<Texture2D>("outline");
            background = Content.Load<Texture2D>("background");
        }
        private bool frameStep;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            globalTimer++;

            // Update deltaTime
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update Mouse variables
            UpdateInput();

            GameScale -= scrollwheel;
            if (keyboard.JustPressed(Keys.F))
            {
                frameStep = true;
                freeze = false;
            }
            if (keyboard.JustPressed(Keys.G))
            {
                frameStep = false;
                freeze = false;
            }
            if(keyboard.JustPressed(Keys.U))
            {
                UIActive = !UIActive;
            }

            //Update Tilemap(Don't need that yet but maybe later) and updates Enemies(definitely need that)
            level.Update();

            if (globalTimer % gameSpeed == 0 && !freeze)
            {
                // Update Player
                player.Update();
            }
            if (frameStep)
            {
                freeze = true;
            }

            // Update UI
            if (UIActive)
            {
                for (int i = 0; i < UIStates.Count; i++)
                {
                    UIStates[i].UpdateSelf(gameTime);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, Screen, Color.White);
            spriteBatch.End();

            // Apply Zoom
            GameMatrix = Matrix.CreateScale(GameScale);
            // Apply Translation
            GameMatrix = CameraTranslate(GameMatrix);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameMatrix);

            // Draw Tiles before player to not cover him
            level.Draw();
            // Draw Player
            player.Draw();

            spriteBatch.End();

            // UI
            if (UIActive)
            {
                UIScaleMatrix = Matrix.CreateScale(UIScale);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, UIScaleMatrix);
                {
                    for (int i = 0; i < UIStates.Count; i++)
                    {
                        UIStates[i].DrawSelf(spriteBatch);
                    }

                    if (MouseText != null)
                    {
                        spriteBatch.DrawString(font, MouseText, mouse.Position.ToVector2() + new Vector2(10), Color.White);
                        MouseText = null;
                    }
                }
                spriteBatch.End();
            }



            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates input variables
        /// </summary>
        private void UpdateInput()
        {
            lastmouse = mouse;
            mouse = Mouse.GetState();
            lastKeyboard = keyboard;
            keyboard = Keyboard.GetState();
            mouseMoved = mouse.Position != lastmouse.Position;
            scrollwheel = (lastmouse.ScrollWheelValue - mouse.ScrollWheelValue) / 8000f;

            LeftHeld = mouse.LeftButton == ButtonState.Pressed;
            RightHeld = mouse.RightButton == ButtonState.Pressed;
            LeftReleased = mouse.LeftButton == ButtonState.Released;
            RightReleased = mouse.RightButton == ButtonState.Released;
            LeftClick = LeftReleased && lastmouse.LeftButton == ButtonState.Pressed;
            RightClick = RightReleased && lastmouse.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Loads texture from file
        /// </summary>
        /// <param name="path">absolute file path</param>
        /// <returns>the loaded Texture</returns>
        public static Texture2D LoadTexture(string path)
        {
            return instance.Content.Load<Texture2D>(path);
        }

        /// <summary>
        /// Loads a part of a Texture from path defined by a sourceRectangle
        /// </summary>
        /// <param name="path">absolute file path</param>
        /// <param name="srcRect">source Rectangle which defines what Part of the Texture is loaded</param>
        /// <returns>A Texture2D containing the specified part</returns>
        public static Texture2D LoadTexturePart(string path, Rectangle srcRect)
        {
            Texture2D wholeTex = instance.Content.Load<Texture2D>(path);
            Texture2D returnTex = new Texture2D(instance.GraphicsDevice, srcRect.Width, srcRect.Height);
            Color[] data = new Color[srcRect.Width * srcRect.Height];
            wholeTex.GetData(0, srcRect, data, 0, data.Length);
            returnTex.SetData(data);
            return returnTex;
        }
        public static Vector2 camOffset;
        public static Matrix CameraTranslate(Matrix matrix)
        {
            // Calculate Translation
            Vector2 viewportSize = new Vector2(ViewPort.Width, ViewPort.Height);
            camOffset = player.position - viewportSize / 2 / GameScale;

            // Prevent camera from going offscreen
            Vector2 MaxOffset = level.bounds.VectorSize() - viewportSize;
            camOffset = Vector2.Clamp(camOffset, Vector2.Zero, MaxOffset * GameScale);

            // Apply Translation
            matrix.Translation -= new Vector3(camOffset.X, camOffset.Y, 0) * GameScale;

            return matrix;
        }
        public static Vector2 InvertTranslate(Vector2 vector)
        {
            vector += (GameMatrix.Translation + new Vector3(camOffset.X, camOffset.Y, 0)).ToVector2();
            return vector;
        }
    }
}
