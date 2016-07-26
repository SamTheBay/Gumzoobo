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
                // udpate the level
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
                        MapScreen.singletonMapSreen.OpenNextLocation();
                        ExitScreen();
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
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].IsActive)
                {
                    if (InputManager.IsActionTriggered(InputManager.Action.Pause, i))
                    {
                        GameScreen screen = new PauseScreen(players[i]);
                        ScreenManager.AddScreen(screen);
                        return;
                    }
                }

                else if (players[i].IsSelecting == false && players[i].HasSelected == false && players[i].Continues > 0)
                {
                    if (InputManager.IsActionTriggered(InputManager.Action.EnterGame, i))
                    {
                        // add the player in to the game if they are not active
                        players[i].IsSelecting = true;
                    }
                }

                else if (players[i].IsSelecting == true && players[i].HasSelected == false)
                {
                    // check for move action
                    if (InputManager.IsActionTriggered(InputManager.Action.MoveCharacterRight, i))
                    {
                        int selection = players[i].CurrentSelection;
                        selection++;
                        selection %= 4;
                        while (BubbleGame.IsCharacterTaken(selection))
                        {
                            selection++;
                            selection %= 4;
                        }
                        players[i].CurrentSelection = selection;
                    }
                    else if (InputManager.IsActionTriggered(InputManager.Action.MoveCharacterLeft, i))
                    {
                        int selection = players[i].CurrentSelection;
                        selection--;
                        if (selection == -1)
                            selection = 3;
                        selection %= 4;
                        while (BubbleGame.IsCharacterTaken(selection))
                        {
                            selection--;
                            if (selection == -1)
                                selection = 3;
                            selection %= 4;
                        }
                        players[i].CurrentSelection = selection;
                    }
                    else
                    {
                        // check if they are on a valid character 
                        int selection = players[i].CurrentSelection;
                        while (BubbleGame.IsCharacterTaken(selection))
                        {
                            selection++;
                            selection %= 4;
                        }
                        players[i].CurrentSelection = selection;
                    }

                    // check if we are the last player to select
                    int charactersTaken = 0;
                    for (int x = 0; x < 4; x++)
                    {
                        if (BubbleGame.IsCharacterTaken(x))
                        {
                            charactersTaken++;
                        }
                    }

                    // check for selection
                    if (InputManager.IsActionTriggered(InputManager.Action.EnterGame, i) ||
                        InputManager.IsActionTriggered(InputManager.Action.Ok, i) ||
                        charactersTaken == 3)
                    {
                        // add the player in to the game if they are not active
                        if (players[i].CurrentSelection == 0)
                        {
                            players[i] = new SealPlayer(i);
                        }
                        else if (players[i].CurrentSelection == 1)
                        {
                            players[i] = new TortoisePlayer(i);
                        }
                        else if (players[i].CurrentSelection == 2)
                        {
                            players[i] = new ToadPlayer(i);
                        }
                        else if (players[i].CurrentSelection == 3)
                        {
                            players[i] = new PenguinPlayer(i);
                        }

                        players[i].Activate();
                        players[i].Continues = 0;
                        Level.singletonLevel.AddPlayer(players[i]);
                        MapScreen.singletonMapSreen.AddPawn(players[i]);
                    }
                }

                else if (players[i].HasSelected == true && players[i].Continues > 0)
                {
                    if (InputManager.IsActionTriggered(InputManager.Action.EnterGame, i))
                    {
                        // add the player in to the game if they are not active
                        players[i].Activate();
                    }
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
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
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
