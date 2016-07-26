using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace BubbleGame
{
    class IntroScreen : MenuScreen
    {
        Texture2D IntroButton;
        Texture2D buttonPress;
        bool wasTrialMode;

        public IntroScreen()
            : base()
        {
            IntroButton = GameSprite.game.Content.Load<Texture2D>("Textures/UI/LargeButton");
            buttonPress = GameSprite.game.Content.Load<Texture2D>("Textures/UI/LargeButtonPress");

            IsPopup = true;
            restartOnVisible = true;
            isStable = false;

            // add in the MenuEntries
            MenuEntry entry = new MenuEntry("Start");
            entry.TextLine2 = "Game";
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            entry.PressTexture = buttonPress;
            entry.SetStartAnimation(new Vector2(-1 * IntroButton.Width - 10, -1 * IntroButton.Width - 10), new Vector2(100, 150), 0, 2000, 1000);
            MenuEntries.Add(entry);

            entry = new MenuEntry("How To");
            entry.TextLine2 = "Play";
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(-1 * IntroButton.Width - 10, 490 + IntroButton.Width), new Vector2(100, 300), 0, 2000, 1000); entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            entry.PressTexture = buttonPress;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Play");
            entry.TextLine2 = "Intro";
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(325, -1 * IntroButton.Width - 10), new Vector2(300, 150), 0, 2000, 1000); entry.Font = Fonts.HeaderFont;
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            entry.PressTexture = buttonPress;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Settings");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(810 + IntroButton.Width, -1 * IntroButton.Width - 10), new Vector2(500, 150), 0, 2000, 1000); entry.Font = Fonts.HeaderFont;
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            entry.PressTexture = buttonPress;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Exit");
            entry.TextLine2 = "Game";
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(325, 490 + IntroButton.Width), new Vector2(300, 300), 0, 2000, 1000); entry.Font = Fonts.HeaderFont;
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            entry.PressTexture = buttonPress;
            MenuEntries.Add(entry);

            entry = new MenuEntry("More");
            entry.TextLine2 = "Games";
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(810 + IntroButton.Width, 490 + IntroButton.Width), new Vector2(500, 300), 0, 2000, 1000); entry.Font = Fonts.HeaderFont;
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            entry.PressTexture = buttonPress;
            MenuEntries.Add(entry);

            wasTrialMode = BubbleGame.IsTrialModeCached;

        }

        void entry_Selected(object sender, EventArgs e)
        {
            if (sender.Equals(MenuEntries[0]))
            {
                // Start the game
                SaveGameManager.SingletonSaveManager.ResetDevice();
                BubbleGame.masterController = selectorIndex;
                GameScreen screen = new CharacterSelectScreen(selectorIndex);
                screen.IsMasterControllerSensitive = true;
                AddNextScreen(screen);
            }
            else if (sender.Equals(MenuEntries[1]))
            {
                // Instructions
                GameScreen screen = new InstructionsScreen();
                BubbleGame.masterController = selectorIndex;
                screen.IsMasterControllerSensitive = true;
                AddNextScreen(screen);
            }
            else if (sender.Equals(MenuEntries[2]))
            {
                // Play intro
                GameScreen screen = new IntroCut();
                AddNextScreen(screen);
            }
            else if (sender.Equals(MenuEntries[3]))
            {
                // open settings
                GameScreen screen = new SettingsScreen();
                AddNextScreen(screen);
            }
            else if (sender.Equals(MenuEntries[4]))
            {
                // Exit the game
                BubbleGame.sigletonGame.Exit();
            }
            else if (sender.Equals(MenuEntries[5]))
            {
                // High Scores
                AddNextScreen(new OtherGamesScreen());
            }
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (BubbleGame.IsTrialModeCached == false && wasTrialMode == true)
            {
                wasTrialMode = false;
                MenuEntries.RemoveAt(5);
            }
        }

    }
}