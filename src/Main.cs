﻿using Microsoft.Xna.Framework;
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
        public static Texture2D panel; // not implemented
        public static Rectangle Screen
        {
            get
            {
                // Update Screen variable
                var sex = System.Windows.Forms.Control.FromHandle(instance.Window.Handle).Bounds;
                return new Rectangle(sex.X, sex.Y, sex.Width, sex.Height);
            }
        }
        public static float DeltaTime { get; private set; }
        public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static string? MouseText;
        public static List<UIState> UIStates = new List<UIState>();
        public static float UIScale = 1f;
        public static Matrix UIScaleMatrix;

        // input stuff
        public static MouseState mouse = Mouse.GetState();
        public static MouseState lastmouse;
        public static KeyboardState keyboard;
        public static KeyboardState lastKeyboard;
        public static float scrollwheel;
        public static bool LeftHeld;
        public static bool RightHeld;
        public static bool LeftReleased;
        public static bool RightReleased;
        public static bool LeftClick;
        public static bool RightClick;
        public static bool mouseMoved;

        //Game Stuff
        public static Player player;
        public static Level level;
        public static Camera camera;

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
            camera = new Camera();

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update deltaTime
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update Mouse variables
            UpdateInput();

            //Update Tilemap...Does nothing, but maybe we will add that later
            level.Update();

            // Update Player
            player.Update();

            // Update Camera
            camera.Update();

            // Update UI
            for (int i = 0; i < UIStates.Count; i++)
            {
                UIStates[i].UpdateSelf(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Draw Tiles before player to not cover him
            level.Draw();
            // Draw Player
            player.Draw();

            spriteBatch.End();

            // UI
            UIScaleMatrix = Matrix.CreateScale(UIScale);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, UIScaleMatrix);
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
    }
}