using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;


namespace BubbleGame
{
    class LoadGameScreen : PopUpScreen
    {
        bool initialDataLoad = false;
        Texture2D buttonTexture;
        Texture2D pressTexture;
        Texture2D loadGameTexture;

        // current save game info
        bool isCreatingNewGame = false;
        string saveGameName = "";
        IAsyncResult result = null;
        int saveGameIndex = 0;

        ContinueQuestion questionScreen = null;
        ContinueQuestion deleteScreen = null;
        ContinueQuestion playScreen = null;
        int deleteIndex;

        public LoadGameScreen()
            : base()
        {
            IsPopup = true;
            buttonTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/WideButton");
            pressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/WideButtonPress");
            loadGameTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/LoadGame");
            windowCorner = new Vector2(-1 * 750 - 20, 480/2 - 450 / 2);

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Rectangle helpTexture = new Rectangle(0, 0, 700, 450);
            int boarderWidth = 12;

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2),
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(loadGameTexture, new Vector2(windowCorner.X + 350 - loadGameTexture.Width / 2, 35), Color.White);
            //Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "A - Select    Y - Delete", new Vector2(windowCorner.X + 350, 680), Color.Black, 1.5f);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (otherScreenHasFocus)
                return;

            if (isExiting)
                return;

            // check if our drive failed and the user wants to quit
            if (SaveGameManager.SingletonSaveManager.CheckDriveFailureExit())
            {
                ExitScreen();
                return;
            }
            else if (SaveGameManager.SingletonSaveManager.SavingDisabled)
            {
                StartGame();
                return;
            }

            if (playScreen != null)
            {
                if (playScreen.IsFinished == false)
                    return;
                else
                {
                    if (playScreen.Cancelled == true)
                    {
                        ResetScreen();
                    }
                    else if (playScreen.Result == true)
                    {
                        SaveGameManager.SingletonSaveManager.SetCurrentSaveIndex(this.selectedEntry);
                        StartGame(true);
                    }
                    else
                    {
                        deleteScreen = new ContinueQuestion(YesNoReason.ShouldDelete);
                        AddNextScreen(deleteScreen, false);
                        deleteIndex = this.selectedEntry;
                    }
                    playScreen = null;
                }
            }

            // check the question screen to determine what to do next
            if (questionScreen != null)
            {
                if (questionScreen.IsFinished == false)
                    return;
                else
                {
                    if (questionScreen.Result == true)
                    {
                        StartGameWithoutSaving();
                    }
                    else
                    {
                        ExitScreen();
                    }
                    questionScreen = null;
                }
            }

            // check for delete screen results
            if (deleteScreen != null)
            {
                if (deleteScreen.IsFinished == false)
                    return;
                else
                {
                    if (deleteScreen.Result == true)
                    {
                        SaveGameManager.SingletonSaveManager.DeleteSaveGameRecord(deleteIndex);
                        MenuEntries[selectedEntry].Text = "New Game";
                        SaveGameManager.SingletonSaveManager.WriteSaveFile();
                    }

                    ResetScreen();
                    deleteScreen = null;
                }
            }

            if (initialDataLoad == false)
            {
                // load the save games
                bool saveresult = SaveGameManager.SingletonSaveManager.ReadSaveFile();
                if (saveresult == false)
                {
                    questionScreen = new ContinueQuestion(YesNoReason.CannotConnectDrive);
                    AddNextScreen(questionScreen);
                    return;
                }

                // add buttons for the screen
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // start the animations
                        MenuEntry entry;
                        SaveGameRecord record = SaveGameManager.SingletonSaveManager.GetSavedGameRecord(i * 3 + j);
                        if (record == null)
                        {
                            entry = new MenuEntry("New Game");
                        }
                        else
                        {
                            entry = new MenuEntry(record.RecordName);
                        }
                        entry.Selected += new EventHandler<EventArgs>(entry_Selected);
                        entry.Texture = buttonTexture;
                        entry.PressTexture = pressTexture;
                        if (i == 0)
                        {
                            entry.SetStartAnimation(new Vector2(-1 * 700 - 20 + 400 - buttonTexture.Width, j * buttonTexture.Height + 150), new Vector2(400 - buttonTexture.Width, j * buttonTexture.Height + 150), 500, 1000, 1000);
                        }
                        else
                        {
                            entry.SetStartAnimation(new Vector2(-1 * 700 - 20 + 400, j * buttonTexture.Height + 150), new Vector2(400, j * buttonTexture.Height + 150), 500, 1000, 1000);
                        }
                        entry.SetAnimationType(AnimationType.Slide);
                        entry.Font = Fonts.HeaderFont;
                        MenuEntries.Add(entry);
                    }

                }
                SetPopUpAnimation(new Vector2(-1 * 700 - 20, 480/2 - 450 / 2), new Vector2(800/2 - 700 / 2, 480/2 - 450 / 2), 500, 1000, 1000);

                initialDataLoad = true;
            }

            if (isCreatingNewGame == true && result != null && result.IsCompleted == true)
            {
                saveGameName = Guide.EndShowKeyboardInput(result);
                result = null;
                StringBuilder adjustedName = new StringBuilder();

                if (saveGameName == null)
                {
                    // abort and reset the screne
                    //saveGameName = "";
                    isCreatingNewGame = false;
                    result = null;
                    return;
                }

                // crunch the save game name to remove any invalid characters
                for (int i = 0; i < saveGameName.Length && i < 15; i++)
                {
                    if (Fonts.HeaderFont.Characters.Contains(saveGameName.ToCharArray()[i]))
                    {
                        adjustedName.Append(saveGameName.ToCharArray()[i]);
                    }
                }

                SaveGameRecord record = new SaveGameRecord(adjustedName.ToString(), saveGameIndex);
                SaveGameManager.SingletonSaveManager.AddSaveGameRecord(record);
                SaveGameManager.SingletonSaveManager.SetCurrentSaveIndex(saveGameIndex);
                bool saved = SaveGameManager.SingletonSaveManager.WriteSaveFile();
                if (saved)
                    StartGame();
            }
        }



        void entry_Selected(object sender, EventArgs e)
        {
            SaveGameRecord record = SaveGameManager.SingletonSaveManager.GetSavedGameRecord(this.selectedEntry);
            if (record == null)
            {
                PlayerIndex index = PlayerIndex.One;
                switch (BubbleGame.masterController)
                {
                    case 0:
                        index = PlayerIndex.One;
                        break;
                    case 1:
                        index = PlayerIndex.Two;
                        break;
                    case 2:
                        index = PlayerIndex.Three;
                        break;
                    case 3:
                        index = PlayerIndex.Four;
                        break;

                }
                result = Guide.BeginShowKeyboardInput(index, "Profile Name", "Enter the name of your new profile", "Bupper", null, null);
                saveGameIndex = selectedEntry;
                isCreatingNewGame = true;
            }
            else
            {
                playScreen = new ContinueQuestion(YesNoReason.PlayOrDelete);
                AddNextScreen(playScreen);
            }

        }


        public void StartGame()
        {
            StartGame(false);
        }

        public void StartGame(bool exitImmediate)
        {
            GameScreen screen = new MapScreen(SaveGameManager.CurrentOpenedGame.ReachedLocation);
            screen.IsMasterControllerSensitive = true;
            if (exitImmediate)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(screen);
            }
            else
            {
                AddNextScreenAndExit(screen);
            }
        }

        public void StartGameWithoutSaving()
        {
            SaveGameManager.SingletonSaveManager.DisableSaving();
            StartGame();
        }
    }
}