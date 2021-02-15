using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.src.UI.UIElements;

namespace Platformer.src.UI.UIStates
{
    public class MainMenu : UIState
    {
        private HomeScreen homescreen;
        private ChooseDifficulty chooseDifficulty;
        public override void Initialize()
        {
            homescreen = new HomeScreen();
            homescreen.Initialize();
            homescreen.Visible = true;

            chooseDifficulty = new ChooseDifficulty();
            chooseDifficulty.Initialize();
            chooseDifficulty.Visible = false;
            base.Initialize();
        }
        protected override void Draw(SpriteBatch spriteBatch)
        {
            homescreen.DrawSelf(spriteBatch);
            chooseDifficulty.DrawSelf(spriteBatch);
            base.Draw(spriteBatch);
        }
        protected override void Update(GameTime gameTime)
        {
            homescreen.UpdateSelf(gameTime);
            chooseDifficulty.UpdateSelf(gameTime);
            base.Update(gameTime);
        }
        internal class HomeScreen : UIState
        {
            public override void Initialize()
            {
                var gameName = new UIText("Test Platformer", Color.White);
                gameName.X.Percent = 50;
                gameName.Y.Percent = 15;
                Append(gameName);

                var newGameBtn = new UIButton(new UIText("New Game", Color.White), 100, 50, Color.Gray);
                newGameBtn.X.Percent = 50;
                newGameBtn.Y.Percent = 30;
                newGameBtn.OnClick += (evt, elm) => { 
                    Visible = false;
                    Main.mainMenu.chooseDifficulty.Visible = true;
                };
                Append(newGameBtn);

                var loadLevelBtn = new UIButton(new UIText("Load Level", Color.White), 100, 50, Color.Gray);
                loadLevelBtn.X.Percent = 50;
                loadLevelBtn.Y.Percent = 37;
                loadLevelBtn.OnClick += (evt, elm) =>
                {
                    using System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
                    {
                        InitialDirectory = Main.instance.Content.RootDirectory,
                        Filter = "level files (*.level)|*.level",
                        RestoreDirectory = true
                    };

                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Main.StartGame(openFileDialog.FileName);
                    }
                };
                Append(loadLevelBtn);

                var quitBtn = new UIButton(new UIText("quit", Color.Black), 100, 50, Color.Gray);
                quitBtn.X.Percent = 50;
                quitBtn.Y.Percent = 45;
                quitBtn.OnClick += (evt, elm) => Main.instance.Exit();
                Append(quitBtn);
                base.Initialize();
            }
        }
        protected class ChooseDifficulty : UIState
        {
            public override void Initialize()
            {
                var gameName = new UIText("Choose your difficulty", Color.White);
                gameName.X.Percent = 50;
                gameName.Y.Percent = 15;
                Append(gameName);

                var difficulty = new UIInput<byte>("Difficulty (1 - 10)", 150, 20, Color.White, Color.Black);
                difficulty.X.Percent = 50;
                difficulty.Y.Percent = 20;
                Append(difficulty);

                var startGameBtn = new UIButton(new UIText("Start Game", Color.White), 100, 50, Color.Gray);
                startGameBtn.X.Percent = 50;
                startGameBtn.Y.Percent = 30;
                startGameBtn.OnClick += (evt, elm) => 
                {
                    if (byte.TryParse(difficulty.Input.Text, out byte result))
                    {
                        Main.StartGame(Main.CurrentDirectory + @"\levels\level0.level");
                        Main.difficulty = result;
                    }
                };
                Append(startGameBtn);
            }
        }
    }
}
