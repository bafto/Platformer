using Microsoft.Xna.Framework;
using Platformer.src.UI.UIElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.src.UI.UIStates
{
    class MainMenu : UIState
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
            newGameBtn.OnClick += (evt, elm) => Main.level = new Level(Main.CurrentDirectory + @"\levels\level0.level");
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
                    Main.level = new Level(openFileDialog.FileName);
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
}
