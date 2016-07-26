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
    class IntroScreen : MenuScreen
    {
        Texture2D IntroButton;

        public IntroScreen()
            : base()
        {
            IntroButton = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "button48593266"));

            IsPopup = true;
            restartOnVisible = true;
            isStable = false;

            // add in the MenuEntries
            MenuEntry entry = new MenuEntry("Start Game");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            entry.SetStartAnimation(new Vector2(1290, 360), new Vector2((1280 - IntroButton.Width) / 2, 360), 0, 2000, 1000);
            MenuEntries.Add(entry);

            entry = new MenuEntry("Controls");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(-1 * IntroButton.Width -10, 360 + IntroButton.Height), new Vector2((1280 - IntroButton.Width) / 2, 360 + IntroButton.Height), 100, 2000, 1000);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Instructions");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(1290, 360 + IntroButton.Height * 2), new Vector2((1280 - IntroButton.Width) / 2, 360 + IntroButton.Height * 2), 200, 2000, 1000);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Play Intro");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(-1 * IntroButton.Width - 10, 360 + IntroButton.Height * 3), new Vector2((1280 - IntroButton.Width) / 2, 360 + IntroButton.Height * 3), 300, 2000, 1000);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Credits");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(1290, 360 + IntroButton.Height * 4), new Vector2((1280 - IntroButton.Width) / 2, 360 + IntroButton.Height * 4), 400, 2000, 1000);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Exit Game");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(-1 * IntroButton.Width - 10, 360 + IntroButton.Height * 5), new Vector2((1280 - IntroButton.Width) / 2, 360 + IntroButton.Height * 5), 500, 2000, 1000);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            MenuEntries.Add(entry);

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
                // Controls
#if XBOX
                HelpScreen screen = new HelpScreen("Controls");
#else
                HelpScreen screen = new HelpScreen("keyboardControls");
#endif
                BubbleGame.masterController = selectorIndex;
                screen.isInLevel = false;
                screen.IsMasterControllerSensitive = true;
                AddNextScreen((GameScreen)screen);
            }
            else if (sender.Equals(MenuEntries[2]))
            {
                // Instructions
                GameScreen screen = new InstructionsScreen();
                BubbleGame.masterController = selectorIndex;
                screen.IsMasterControllerSensitive = true;
                AddNextScreen(screen);
            }
            else if (sender.Equals(MenuEntries[3]))
            {
                // Play intro
                GameScreen screen = new IntroCut();
                AddNextScreen(screen);
            }
            else if (sender.Equals(MenuEntries[4]))
            {
                // play credits
                GameScreen screen = new CreditsScreen();
                AddNextScreen(screen);
            }
            else if (sender.Equals(MenuEntries[5]))
            {
                // Exit the game
                AddNextScreen(new PleaseRateScreen());
            }
        }

    }
}