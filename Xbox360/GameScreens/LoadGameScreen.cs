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
using Microsoft.Xna.Framework.Storage;

#if XBOX
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
#endif

namespace BubbleGame
{
    class LoadGameScreen : PopUpScreen
    {
        bool initialDataLoad = false;
        Texture2D buttonTexture;
        Texture2D loadGameTexture;

        // current save game info
        bool isCreatingNewGame = false;
        string saveGameName = "";
        IAsyncResult result = null;
        int saveGameIndex = 0;

        ContinueQuestion questionScreen = null;
        ContinueQuestion deleteScreen = null;
        int deleteIndex;

        public LoadGameScreen()
            : base()
        {
            IsPopup = true;
            buttonTexture = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "button48593266"));
            loadGameTexture = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "LoadGame"));
            windowCorner = new Vector2(-1 * 700 - 20, 360 - 700 / 2);

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Rectangle helpTexture = new Rectangle(0, 0, 700, 700);
            int boarderWidth = 12;

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2),
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(loadGameTexture, new Vector2(windowCorner.X + 350 - loadGameTexture.Width / 2, 35), Color.White);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "A - Select    Y - Delete", new Vector2(windowCorner.X + 350, 680), Color.Black, 1.5f);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

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

            bool connected = SaveGameManager.SingletonSaveManager.IsStorageDeviceConnected();

            if (!connected)
            {
                // connect to the storage device
                PlayerIndex index = BubbleGame.IntToPI(BubbleGame.masterController);
                SaveGameManager.SingletonSaveManager.StartConnectStorageDevice(index);
                SaveGameManager.SingletonSaveManager.UpdateConnectStorageDevice();
            }
            if (connected && initialDataLoad == false)
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
                for (int i = 0; i < 10; i++)
                {
                    // start the animations
                    MenuEntry entry;
                    SaveGameRecord record = SaveGameManager.SingletonSaveManager.GetSavedGameRecord(i);
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
                    entry.SetStartAnimation(new Vector2(-700 / 2 -20 - buttonTexture.Width / 2, i * buttonTexture.Height + 150), new Vector2(1280 / 2 - buttonTexture.Width / 2, i * buttonTexture.Height + 150), 500, 1000, 1000);
                    entry.SetAnimationType(AnimationType.Slide);
                    entry.Font = Fonts.HeaderFont;
                    MenuEntries.Add(entry);

                }
                SetPopUpAnimation(new Vector2(-1 * 700 - 20, 360 - 700 / 2), new Vector2(640 - 700 / 2, 360 - 700 / 2), 500, 1000, 1000);

                initialDataLoad = true;
            }

            if (isCreatingNewGame == true && result != null && result.IsCompleted == true)
            {
#if XBOX
                try 
                {
                    saveGameName = Guide.EndShowKeyboardInput(result);
                }
                catch (Exception)
                {

                }
#endif

                // make sure we have a valid string to avoid possible crash
                if (saveGameName == null)
                    saveGameName = "";

                result = null;
                StringBuilder adjustedName = new StringBuilder();

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


        public override void HandleInput()
        {
            base.HandleInput();

            bool connected = SaveGameManager.SingletonSaveManager.IsStorageDeviceConnected();

            if (connected && initialDataLoad == true)
            {
                // check for a delete command
                if (InputManager.IsActionTriggered(InputManager.Action.Delete, BubbleGame.masterController))
                {
                    deleteScreen = new ContinueQuestion(YesNoReason.ShouldDelete);
                    AddNextScreen(deleteScreen);
                    deleteIndex = this.selectedEntry;
                }
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
#if XBOX
                try
                {
                    result = Guide.BeginShowKeyboardInput(index, "Profile Name", "Enter the name of your new profile", "Bupper", null, null);
                }
                catch (Exception)
                {

                }
#endif
                saveGameIndex = selectedEntry;
                isCreatingNewGame = true;
            }
            else
            {
                SaveGameManager.SingletonSaveManager.SetCurrentSaveIndex(this.selectedEntry);
                StartGame();
            }

        }

        public void StartGame()
        {
            GameScreen screen = new MapScreen(SaveGameManager.CurrentOpenedGame.ReachedLocation);
            screen.IsMasterControllerSensitive = true;
            AddNextScreenAndExit(screen);
        }


        public void StartGameWithoutSaving()
        {
            SaveGameManager.SingletonSaveManager.DisableSaving();
            StartGame();
        }
    }
}