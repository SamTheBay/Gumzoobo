using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BubbleGame
{
    class CharacterSelectScreen : PopUpScreen
    {
        Rectangle windowPlacement = new Rectangle(1280/2 - 450, 720/2 - 300, 850, 600);
        Texture2D ButtonMarker;
        Texture2D SelectedMarker;
        Texture2D Title;

        Texture2D Seal;
        Texture2D Tortoise;
        Texture2D Toad;
        Texture2D Penguin;

        ContinueQuestion shouldContinueScreen = null;

        int playerIndex;
        bool playerSignedIn = false;
        bool attemptedPlayerSignIn = false;

        bool[] isPlaying = new bool[4];
        int[] currentSelection = new int[4];
        bool[] isSelected = new bool[4];
        bool[] characterSelected = new bool[4];

        public CharacterSelectScreen(int playerIndex)
        {
            this.playerIndex = playerIndex;
            ButtonMarker = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "buttonMarker"));
            SelectedMarker = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "selectedMarker"));
            Title = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "SelectCharacter"));
            Seal = InternalContentManager.GetTexture("Seal");
            Tortoise = InternalContentManager.GetTexture("Tortoise");
            Toad = InternalContentManager.GetTexture("Toad");
            Penguin = InternalContentManager.GetTexture("Penguin");
            this.IsPopup = true;

            for (int i = 0; i < 4; i++)
            {
                isPlaying[i] = false;
                currentSelection[i] = 0;
                isSelected[i] = false;
                characterSelected[i] = false;
            }

            isPlaying[playerIndex] = true;

            SetPopUpAnimation(new Vector2(1280 / 2 - 450, 730), new Vector2(1280 / 2 - 450, 720 / 2 - 300), 0, 1000, 1000);
            windowPlacement.X = (int)windowCorner.X;
            windowPlacement.Y = (int)windowCorner.Y;
        }


        void StartGame()
        {
            for (int i = 0; i < 4; i++)
            {
                if (isPlaying[i] == true)
                {

                    if (currentSelection[i] == 0)
                    {
                        // select Seal
                        BubbleGame.players[i] = new SealPlayer(i);
                    }
                    else if (currentSelection[i] == 1)
                    {
                        // select Tortoise
                        BubbleGame.players[i] = new TortoisePlayer(i);
                    }
                    else if (currentSelection[i] == 2)
                    {
                        // select Toad
                        BubbleGame.players[i] = new ToadPlayer(i);
                    }
                    else if (currentSelection[i] == 3)
                    {
                        // select Penguin
                        BubbleGame.players[i] = new PenguinPlayer(i);

                    }
                    BubbleGame.players[i].Activate();
                }
            }
            GameScreen screen;
            if (SaveGameManager.SingletonSaveManager.SavingDisabled)
            {
#if XBOX
                screen = new MapScreen(SaveGameManager.CurrentOpenedGame.ReachedLocation);
#else
                screen = new MapScreen(0);
#endif
                screen.IsMasterControllerSensitive = true;
                AddNextScreenAndExit(screen);
            }
            else
            {
                screen = new LoadGameScreen();
                screen.IsMasterControllerSensitive = true;
            }
            AddNextScreenAndExit(screen);
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            // draw the background
            DrawBoarder(windowPlacement, InternalContentManager.GetTexture("BlueStripe"), 24, Color.White, spriteBatch);
             
            // draw title
            spriteBatch.Draw(Title, new Vector2(windowPlacement.X + windowPlacement.Width / 2 - Title.Width / 2, windowPlacement.Y + 25), Color.White);

            // draw character sprites
            spriteBatch.Draw(Seal, new Vector2(windowPlacement.X + 250 - 40, windowPlacement.Y + 100), new Rectangle(0, 0, 80, 80), Color.White);
            spriteBatch.Draw(Tortoise, new Vector2(windowPlacement.X + 400 - 36, windowPlacement.Y + 100 + 10), new Rectangle(0, 0, 72, 62), Color.White);
            spriteBatch.Draw(Toad, new Vector2(windowPlacement.X + 550 - 60, windowPlacement.Y + 100 + 5), new Rectangle(0, 0, 120, 70), Color.White);
            spriteBatch.Draw(Penguin, new Vector2(windowPlacement.X + 700 - 21, windowPlacement.Y + 100), new Rectangle(0, 0, 42, 74), Color.White);

            // draw names
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Bupper", new Vector2(250 + windowPlacement.X, 200 + windowPlacement.Y), Color.Black, 1.5f);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Froofy", new Vector2(400 + windowPlacement.X, 200 + windowPlacement.Y), Color.Black, 1.5f);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Clavis", new Vector2(550 + windowPlacement.X, 200 + windowPlacement.Y), Color.Black, 1.5f);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Cheekeze", new Vector2(700 + windowPlacement.X, 200 + windowPlacement.Y), Color.Black, 1.5f); 

            // draw players
            for (int i = 0; i < 4; i++)
            {
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Player" + (i + 1).ToString(), new Vector2(100 + windowPlacement.X, 300 + windowPlacement.Y + i * 70), Color.Black, 1.5f);

                if (isPlaying[i] == false)
                {
#if XBOX
                    Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Press Start To Join", new Vector2(475 + windowPlacement.X, 300 + windowPlacement.Y + i * 70), Color.Black, 1.5f);
#endif
                }
                else if (isSelected[i] == false)
                {
                    spriteBatch.Draw(ButtonMarker, new Vector2(250 + windowPlacement.X + currentSelection[i] * 150 - ButtonMarker.Width / 2, 300 - ButtonMarker.Height / 2 + windowPlacement.Y + i * 70), Color.White);
                }
                else
                {
                    spriteBatch.Draw(SelectedMarker, new Vector2(250 + windowPlacement.X + currentSelection[i] * 150 - SelectedMarker.Width / 2, 300 - ButtonMarker.Height / 2 + windowPlacement.Y + i * 70), Color.White);
                }
            }

            // set the correct button textures
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            windowPlacement.X = (int)windowCorner.X;
            windowPlacement.Y = (int)windowCorner.Y;

            if (isStable && attemptedPlayerSignIn == false && !SaveGameManager.SingletonSaveManager.SavingDisabled)
            {
                SaveGameManager.SingletonSaveManager.SignInMasterPlayer();
                attemptedPlayerSignIn = true;
            }
            else if (isStable == false)
            {
                return;
            }

            if (shouldContinueScreen != null)
            {
                // wait for should continue screen to finish
                if (shouldContinueScreen.IsFinished == false)
                {
                    return;
                }
                else
                {
                    // take action on the result
                    if (shouldContinueScreen.Result == true)
                    {
                        // start the game without loading 
                        SaveGameManager.SingletonSaveManager.DisableSaving();
                        playerSignedIn = true;
                        ResetScreen();
                    }
                    else
                    {
                        ExitScreenImmediate();
                    }
                    shouldContinueScreen = null;

                }
            }

#if XBOX
            if (playerSignedIn == false && SaveGameManager.SingletonSaveManager.UpdatePlayerSignIn() == true)
            {
                if (SaveGameManager.SingletonSaveManager.IsMasterPlayerSignedIn() == false)
                {
                    // check if user wants to continue without saving
                    shouldContinueScreen = new ContinueQuestion(YesNoReason.NotLoggedIn);
                    AddNextScreen(shouldContinueScreen);
                }
                else
                {
                    playerSignedIn = true;
                }
            }
#endif
        }

        public override void HandleInput()
        {
            // update players selections
            for (int i = 0; i < 4; i++)
            {
                if (InputManager.IsActionTriggered(InputManager.Action.EnterGame, i))
                {
                    if (isPlaying[i] == false)
                    {
                        isPlaying[i] = true;

                        // if this selection is used then shift
                        if (characterSelected[currentSelection[i]] == true)
                        {
                            MoveSelection(i, Direction.Right);
                        }
                    }
                }
                if (InputManager.IsActionTriggered(InputManager.Action.Ok, i))
                {
                    if (isSelected[i] == false && isPlaying[i] == true)
                    {
                        isSelected[i] = true;
                        characterSelected[currentSelection[i]] = true;

                        // move other characters off this selection
                        for (int x = 0; x < 4; x++)
                        {
                            if (x != i && isPlaying[x] == true && currentSelection[x] == currentSelection[i])
                            {
                                MoveSelection(x, Direction.Right);
                            }
                        }
                    }
                }
                if (InputManager.IsActionTriggered(InputManager.Action.Back, i))
                {
                    if (isSelected[i] == true)
                    {
                        isSelected[i] = false;
                        characterSelected[currentSelection[i]] = false;
                    }
                    else if (isPlaying[i] == true)
                    {
                        isPlaying[i] = false;
                    }
                }
                if (InputManager.IsActionTriggered(InputManager.Action.MoveCharacterRight, i))
                {
                    if (isPlaying[i] == true && isSelected[i] == false)
                    {
                        MoveSelection(i, Direction.Right);
                    }
                }
                if (InputManager.IsActionTriggered(InputManager.Action.MoveCharacterLeft, i))
                {
                    if (isPlaying[i] == true && isSelected[i] == false)
                    {
                        MoveSelection(i, Direction.Left);
                    }
                }
            }


            // check if no one is playing
            bool noOnePlaying = true;
            for (int i = 0; i < 4; i++)
            {
                if (isPlaying[i] == true)
                {
                    noOnePlaying = false;
                    break;
                }
            }
            if (noOnePlaying == true)
            {
                ExitScreen();
            }

            // check if everyone has selected
            if (noOnePlaying == false)
            {
                bool everyoneSelected = true;
                for (int i = 0; i < 4; i++)
                {
                    if (isPlaying[i] == true && isSelected[i] == false)
                    {
                        everyoneSelected = false;
                        break;
                    }
                }
                if (everyoneSelected == true)
                {
                    StartGame();
                }
            }
        }

        public void MoveSelection(int player, Direction direction)
        {
            if (direction == Direction.Right)
            {
                currentSelection[player] = (currentSelection[player] + 1) % 4;
            }
            if (direction == Direction.Left)
            {
                if (currentSelection[player] == 0)
                {
                    currentSelection[player] = 3;
                }
                else
                {
                    currentSelection[player]--;
                }
            }

            // check if this character is already selected
            while (characterSelected[currentSelection[player]] == true)
            {
                if (direction == Direction.Right)
                {
                    currentSelection[player] = (currentSelection[player] + 1) % 4;
                }
                if (direction == Direction.Left)
                {
                    if (currentSelection[player] == 0)
                    {
                        currentSelection[player] = 3;
                    }
                    else
                    {
                        currentSelection[player]--;
                    }
                }
            }
        }

    }
}