using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Platformer
{
    public class Main : Game
    {
        // Engine Stuff
        public static Main instance; //{ get; private set; } what does it do?
        public static GraphicsDeviceManager graphics { get; private set; }
        public static SpriteBatch spriteBatch { get; private set; }
        public static Texture2D solid { get; private set; }
        public static SpriteFont font { get; private set; }
        public static Rectangle screen;
        public static float deltaTime { get; private set; }
        // input stuff
        public static MouseState mouse = Mouse.GetState();
        public static MouseState lastmouse;
        public static KeyboardState keyboard;
        public static bool LeftHeld;
        public static bool RightHeld;
        public static bool LeftReleased;
        public static bool RightReleased;
        public static bool LeftClick;
        public static bool RightClick;
        public static bool mouseMoved;

        //Game Stuff
        public Player player;

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

            player = new Player();
        }

        protected override void Initialize()
        {
            player.Initialize();

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

            // Update Player
            player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // Draw Player
            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void UpdateInput()
        {
            lastmouse = mouse;
            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();
            mouseMoved = mouse.Position != lastmouse.Position;

            LeftHeld = mouse.LeftButton == ButtonState.Pressed;
            RightHeld = mouse.RightButton == ButtonState.Pressed;
            LeftReleased = mouse.LeftButton == ButtonState.Released;
            RightReleased = mouse.RightButton == ButtonState.Released;
            LeftClick = LeftReleased && lastmouse.LeftButton == ButtonState.Pressed;
            RightClick = RightReleased && lastmouse.RightButton == ButtonState.Pressed;
        }

        //Loading a Texture in another class
        public static Texture2D loadTexture(string path)
        {
            return instance.Content.Load<Texture2D>(path);
        }
    }
}
