#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.IO;
#endregion

namespace BubbleGame
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Initialization

        Level currentLevel;
        PlayerSprite[] players;
        PlayerStatsDisplay playerstats;
        Texture2D greyout;
        Rectangle screenRect = new Rectangle(0, 0, 1280, 720);


        /// <summary>
        /// Create a new GameplayScreen object.
        /// </summary>
        public GameplayScreen(Level level, PlayerSprite[] nPlayers)
            : base()
        {
            currentLevel = level;
            currentLevel.Load();
            players = nPlayers;
            foreach (PlayerSprite player in players)
            {
                if (player.HasSelected)
                    level.AddPlayer(player);
            }
            playerstats = new PlayerStatsDisplay(players);
            greyout = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures", "greyout"));
            EnabledGestures = GestureType.Tap | GestureType.Hold;
        }


        /// <summary>
        /// Handle the closing of this screen.
        /// </summary>
        public override void ExitScreen()
        {
            if (currentLevel != null)
            {
                currentLevel.Unload();
            }
            base.ExitScreen();
            BubbleGame.adControlManager.ShowAds = false;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();
        }


        #endregion


        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive && !coveredByOtherScreen)
            {
                // update the level
                currentLevel.Update(gameTime);

                // check if the level is complete
                if (currentLevel.IsComplete)
                {
                    // move onto the next level
                    currentLevel.Unload();

                    if (currentLevel.LevelNum != MapScreen.currentLocation.endLevel)
                    {
                        currentLevel = BubbleGame.levelManager.GetNextLevel();
                        currentLevel.Load();
                        for (int i = 0; i < players.Length; i++)
                        {
                            if (players[i].HasSelected)
                                currentLevel.AddPlayer(players[i]);
                        }
                    }
                    else
                    {
                        bool isGameWon = MapScreen.singletonMapSreen.OpenNextLocation();
                        if (isGameWon)
                        {
                            AddNextScreenAndExit( new VictoryScreen());
                        }
                        else
                        {
                            ExitScreen();
                        }
                    }
                }
                else if (currentLevel.IsFailed)
                {
                    ExitScreen();
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput()
        {
            if (players[0].IsActive)
            {
                if (InputManager.IsBackTriggered())
                {
                    GameScreen screen = new PauseScreen(players[0]);
                    ScreenManager.AddScreen(screen);
                    return;
                }
            }
        }


        /// <summary>
        /// Event handler for when the user selects Yes 
        /// on the "Are you sure?" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            currentLevel.Draw(gameTime, spriteBatch);
            playerstats.Draw(spriteBatch, this);

            if (PauseScreen.isPaused)
            {
                spriteBatch.Draw(greyout, screenRect, Color.White);
            }

            spriteBatch.End();
        }





        #endregion
    }
}
