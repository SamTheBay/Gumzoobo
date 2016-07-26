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
        Rectangle windowPlacement = new Rectangle(800/2 - 350, 480/2 - 150, 700, 300);
        Texture2D Title;

        Texture2D Seal;
        Texture2D Tortoise;
        Texture2D Toad;
        Texture2D Penguin;

        ContinueQuestion shouldContinueScreen = null;

        int playerIndex;

        bool[] isPlaying = new bool[4];
        int[] currentSelection = new int[4];
        bool[] isSelected = new bool[4];
        bool[] characterSelected = new bool[4];

        public CharacterSelectScreen(int playerIndex) : base()
        {
            this.playerIndex = playerIndex;
            Title = GameSprite.game.Content.Load<Texture2D>("Textures/UI/SelectCharacter");
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

            MenuEntry entry;
            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Location = new Rectangle(windowPlacement.X + 125 - 40, windowPlacement.Y + 120, 80, 150);
            MenuEntries.Add(entry);

            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Location = new Rectangle(windowPlacement.X + 275 - 40, windowPlacement.Y + 120, 80, 150);
            MenuEntries.Add(entry);

            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Location = new Rectangle(windowPlacement.X + 425 - 40, windowPlacement.Y + 120, 80, 150);
            MenuEntries.Add(entry);

            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Location = new Rectangle(windowPlacement.X + 575 - 40, windowPlacement.Y + 120, 80, 150);
            MenuEntries.Add(entry);

            SetPopUpAnimation(new Vector2(800 / 2 - 350, 490), new Vector2(800 / 2 - 350, 480 / 2 - 150), 0, 1000, 1000);
            windowPlacement.X = (int)windowCorner.X;
            windowPlacement.Y = (int)windowCorner.Y;

        }


        void entry_Selected(object sender, EventArgs e)
        {
            if (sender.Equals(MenuEntries[0]))
            {
                // Bupper was selected
                currentSelection[0] = 0;
            }
            else if (sender.Equals(MenuEntries[1]))
            {
                // Bupper was selected
                currentSelection[0] = 1;
            }
            else if (sender.Equals(MenuEntries[2]))
            {
                // Bupper was selected
                currentSelection[0] = 2;
            }
            else if (sender.Equals(MenuEntries[3]))
            {
                // Bupper was selected
                currentSelection[0] = 3;
            }

            StartGame();
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
                screen = new MapScreen(SaveGameManager.CurrentOpenedGame.ReachedLocation);
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
            spriteBatch.Draw(Seal, new Vector2(windowPlacement.X + 125 - 40, windowPlacement.Y + 120), new Rectangle(0, 0, 80, 80), Color.White);
            spriteBatch.Draw(Tortoise, new Vector2(windowPlacement.X + 275 - 36, windowPlacement.Y + 120 + 10), new Rectangle(0, 0, 72, 62), Color.White);
            spriteBatch.Draw(Toad, new Vector2(windowPlacement.X + 425 - 60, windowPlacement.Y + 120 + 5), new Rectangle(0, 0, 120, 70), Color.White);
            spriteBatch.Draw(Penguin, new Vector2(windowPlacement.X + 575 - 21, windowPlacement.Y + 120), new Rectangle(0, 0, 42, 74), Color.White);

            // draw names
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Bupper", new Vector2(125 + windowPlacement.X, 220 + windowPlacement.Y), Color.Black, 1.5f);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Froofy", new Vector2(275 + windowPlacement.X, 220 + windowPlacement.Y), Color.Black, 1.5f);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Clavis", new Vector2(425 + windowPlacement.X, 220 + windowPlacement.Y), Color.Black, 1.5f);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Cheekeze", new Vector2(575 + windowPlacement.X, 220 + windowPlacement.Y), Color.Black, 1.5f); 

            // set the correct button textures
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            windowPlacement.X = (int)windowCorner.X;
            windowPlacement.Y = (int)windowCorner.Y;

            if (isStable == false)
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
                        //playerSignedIn = true;
                        ResetScreen();
                    }
                    else
                    {
                        ExitScreenImmediate();
                    }
                    shouldContinueScreen = null;

                }
            }
        }

    }
}