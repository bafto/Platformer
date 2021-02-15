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
        //GameMode Enum
        public enum GameMode
        {
            MainMenu = 0,
            InGame,
            DeathScreen
        }

        // Engine Stuff
        public static Main instance { get; private set; }
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Texture2D solid;
        public static SpriteFont font;
        public static Texture2D panel;
        public static Texture2D outline;
        public static Texture2D background;
        public static Vector2 WindowPos => System.Windows.Forms.Control.FromHandle(instance.Window.Handle).Location.ToVector2();
        public static Viewport ViewPort => graphics.GraphicsDevice.Viewport;
        public static float DeltaTime { get; private set; }
        public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static string? MouseText;
        public static float UIScale = 1f;
        public static Matrix UIScaleMatrix;
        public static float GameScale = 1f;
        public static Matrix InGameMatrix;
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
        public static Vector2 MouseWorld => Camera.InvertTranslate(mouse.Position);

        //Game Stuff
        public static GameMode gameMode;
        public static Player player;
        public static Level level;

        //UIStates
        public static MainMenu mainMenu;
        public static TestState testState;
        public static DeathState deathState;

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

            mainMenu = new MainMenu();
            testState = new TestState();
            deathState = new DeathState();
        }

        protected override void Initialize()
        {
            // initialize UIState
            mainMenu.Initialize();

            gameMode = Main.GameMode.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            solid = Content.Load<Texture2D>("solid");
            font = Content.Load<SpriteFont>("font");
            panel = Content.Load<Texture2D>("panel");
            outline = Content.Load<Texture2D>("outline");
            background = Content.Load<Texture2D>("brikBackground");
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

            switch (gameMode)
            {
                case GameMode.MainMenu:
                    {
                        mainMenu.UpdateSelf(gameTime);
                        break;
                    }
                case GameMode.InGame:
                    {
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
                        if (keyboard.JustPressed(Keys.U))
                        {
                            UIActive = !UIActive;
                        }
                        if (globalTimer % gameSpeed == 0 && !freeze)
                        {
                            // Update Player
                            player.Update();

                            // Update Tilemap(Don't need that yet but maybe later) and updates Enemies(definitely need that)
                            level.Update();
                        }
                        if (frameStep)
                        {
                            freeze = true;
                        }
                        if (UIActive)
                        {
                            testState.UpdateSelf(gameTime);
                        }
                        break;
                    }
                case GameMode.DeathScreen:
                    {
                        deathState.UpdateSelf(gameTime);
                        break;
                    }
                default:
                    throw new Exception("Tried to update Invalid GameMode");
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            UIScaleMatrix = Matrix.CreateScale(UIScale);

            switch (gameMode)
            {
                case GameMode.MainMenu:
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, UIScaleMatrix);
                        mainMenu.DrawSelf(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case GameMode.InGame:
                    {
                        // Apply Zoom
                        InGameMatrix = Matrix.CreateScale(GameScale);

                        // Apply Translation
                        InGameMatrix = Camera.Translate(InGameMatrix);

                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(2));
                        //spriteBatch.Draw(background, ViewPort.Bounds, Color.White);
                        spriteBatch.End();

                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, InGameMatrix);

                        // Draw Tiles before player to not cover him
                        level.Draw();
                        player.Draw();

                        spriteBatch.End();

                        //Test state for Debug Info
                        if (UIActive)
                        {
                            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, UIScaleMatrix);
                            testState.DrawSelf(spriteBatch);
                            spriteBatch.End();
                        }
                        break;
                    }
                case GameMode.DeathScreen:
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, UIScaleMatrix);
                        deathState.DrawSelf(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                default:
                    throw new Exception("Tried to update Invalid GameMode");
            }
            if (MouseText != null)
            {
                spriteBatch.DrawString(font, MouseText, mouse.Position.ToVector2() + new Vector2(10), Color.White);
                MouseText = null;
            }
            base.Draw(gameTime);
        }

        public static void StartGame(string file)
        {
            player = new Player();
            level = new Level(file);
            testState.Initialize();
            deathState.Initialize();
            gameMode = GameMode.InGame;
        }

        public static void ResetGame(string file)
        {
            player = new Player();
            level = new Level(file);
            gameMode = GameMode.InGame;
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
