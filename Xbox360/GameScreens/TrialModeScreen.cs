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
    class TrialModeScreen : PopUpScreen
    {
        Texture2D screenTexture;
        Texture2D IntroButton;

        public TrialModeScreen()
            : base()
        {
            this.IsPopup = true;
            screenTexture = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/HelpWindows", "Trial"));
            IntroButton = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "button48593266"));

            // add in the MenuEntries
            MenuEntry entry = new MenuEntry("Purchase Full Version");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2((1280 - IntroButton.Width) / 2, 730 + screenTexture.Height - 120), new Vector2((1280 - IntroButton.Width) / 2, 360 + screenTexture.Height / 2 - 120), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Continue Trial Version");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2((1280 - IntroButton.Width) / 2, 730 + screenTexture.Height - 70), new Vector2((1280 - IntroButton.Width) / 2, 360 + screenTexture.Height / 2 - 70), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = IntroButton;
            MenuEntries.Add(entry);

            //Vector2 WindowCorner = new Vector2(640 - screenTexture.Width / 2, 360 - screenTexture.Height / 2);
            SetPopUpAnimation(new Vector2(640 - screenTexture.Width / 2, 730), new Vector2(640 - screenTexture.Width / 2, 360 - screenTexture.Height / 2), 0, 1000, 1000);
        }



        void entry_Selected(object sender, EventArgs e)
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

            if (sender.Equals(MenuEntries[0]))
            {
                // go to marketplace
                try
                {
#if XBOX
                    Guide.ShowMarketplace(index);
#endif
                }
                catch(Exception)
                {
                    ExitScreen();
                }
            }
            else if (sender.Equals(MenuEntries[1]))
            {
                // Continue
                ExitScreen();
            }
        }


        public override void ExitScreen()
        {
            Level.singletonLevel.hasShownTrial = true;
            base.ExitScreen();
        }


        public override void Draw(GameTime gameTime)
        {
            int boarderWidth = 12;

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, screenTexture.Width + boarderWidth * 2, screenTexture.Height + boarderWidth * 2),
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(screenTexture, windowCorner, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
#if XBOX
            if (Guide.IsTrialMode == false)
            {
                Level.singletonLevel.hasShownTrial = true;
                ExitScreen();
            }
#endif
        }

    }
}