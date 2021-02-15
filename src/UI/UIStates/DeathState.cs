﻿using Platformer.src.UI.UIElements;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platformer.src.UI.UIStates
{
    class DeathState : UIState
    {
        UIText timeAlive;
        public override void Initialize()
        {
            var deathPanel = new UIPanel(700, 500, Color.White);
            deathPanel.X.Percent = 50;
            deathPanel.Y.Percent = 50;
            Append(deathPanel);

            var deathText = new UIText("YOU DIED", Color.Black);
            deathText.X.Percent = 50;
            deathText.Y.Percent = 15;
            deathPanel.Append(deathText);

            timeAlive = new UIText($"You were alive for {Main.player.lastDeath} seconds!", Color.Black);
            timeAlive.X.Percent = 50;
            timeAlive.Y.Percent = 20;
            deathPanel.Append(timeAlive);

            var retyBtn = new UIButton(new UIText("Retry", Color.Black), 100, 50, Color.Gray);
            retyBtn.X.Percent = 25;
            retyBtn.Y.Percent = 50;
            retyBtn.OnClick += (evt, elm) => { Main.level = new Level(Main.CurrentDirectory + @"\levels\level0.level");  Main.level.Reset(); Visible = false; };
            deathPanel.Append(retyBtn);

            var retyLvlBtn = new UIButton(new UIText("Retry Level", Color.Black), 100, 50, Color.Gray);
            retyLvlBtn.X.Percent = 50;
            retyLvlBtn.Y.Percent = 50;
            retyLvlBtn.OnClick += (evt, elm) => { Main.level.Reset(); Visible = false; };
            deathPanel.Append(retyLvlBtn);

            var quitBtn = new UIButton(new UIText("Quit", Color.Black), 100, 50, Color.Gray);
            quitBtn.X.Percent = 75;
            quitBtn.Y.Percent = 50;
            quitBtn.OnClick += (evt, elm) => Main.instance.Exit();
            deathPanel.Append(quitBtn);
            base.Initialize();
        }
        protected override void Update(GameTime gameTime)
        {
            timeAlive.Text = $"You were alive for {Main.player.lastDeath} seconds!";
            base.Update(gameTime);
        }
    }
}