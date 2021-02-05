﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Platformer.src
{
    public class Main : Game
    {
        // Engine Stuff
        public static Main instance { get; private set; }
        public static GraphicsDeviceManager graphics { get; private set; }
        public static SpriteBatch spriteBatch { get; private set; }
        public static Texture2D solid { get; private set; }
        public static SpriteFont font { get; private set; }
        public static Rectangle screen;
        public static float deltaTime { get; private set; }
        public static string currentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static bool DebugMode = true;

        // input stuff
        public static MouseState mouse = Mouse.GetState();
        public static MouseState lastmouse;
        public static KeyboardState keyboard;
        public static KeyboardState lastKeyboard;
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
        }

        protected override void Initialize()
        {
            player = new Player();
            level = new Level(currentDirectory + @"\levels\level0.level");
            camera = new Camera();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            solid = Content.Load<Texture2D>("solid");
            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update Screen variable
            var sex = System.Windows.Forms.Control.FromHandle(Window.Handle).Bounds;
            screen = new Rectangle(sex.X, sex.Y, sex.Width, sex.Height);

            // Update deltaTime
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update Mouse variables
            UpdateInput();

            //Update Tilemap...Does nothing, but maybe we will add that later
            level.Update();

            // Update Player
            player.Update();

            // Update Camera
            camera.Update();

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
