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
    class InstructionsScreen : GameScreen
    {
        Texture2D arrow;
        String[] helpScreens;
        int currenthelp = 0;
        HelpScreen currentHelpScreen;
        HelpScreen previousHelpScreen;

        public InstructionsScreen()
        {
            helpScreens = new String[9];
#if XBOX
            helpScreens[0] = "KillingRobots";
#else
            helpScreens[0] = "KillingRobotsWindows";
#endif
            helpScreens[1] = "JumpingOnBubbles";
            helpScreens[2] = "SuperBubbles";
            helpScreens[3] = "JumpingTop";
#if XBOX
            helpScreens[4] = "Abilities";
#else
            helpScreens[4] = "AbilitiesWindows";
#endif
            helpScreens[5] = "Fruit";
#if XBOX
            helpScreens[6] = "Chewing";
#else
            helpScreens[6] = "ChewingWindows";
#endif
            helpScreens[7] = "Gums";
            helpScreens[8] = "PowerUps";

            arrow = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "ArrowRight"));
            IsPopup = true;
            currentHelpScreen = new HelpScreen(helpScreens[currenthelp]);
            currentHelpScreen.ScreenManager = BubbleGame.screenManager;
            Vector2 startposition = new Vector2(2000, currentHelpScreen.EndPopUpPosition.Y);
            currentHelpScreen.StartPopUpPosition = startposition;
            currentHelpScreen.WindowCorner = startposition;
            previousHelpScreen = null;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            currentHelpScreen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            isStable = currentHelpScreen.IsStable;

            if (previousHelpScreen != null)
            {
                previousHelpScreen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
                if (previousHelpScreen.IsStable)
                    previousHelpScreen = null;
            }
        }

        public override void ExitScreen()
        {
            currentHelpScreen.ExitScreen();
            isStable = false;

            base.ExitScreen();
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (InputManager.IsActionTriggered(InputManager.Action.Back, BubbleGame.masterController))
            {
                currentHelpScreen.StartPopUpTime = 0;
                ExitScreen();
            }

            if (InputManager.IsActionTriggered(InputManager.Action.MoveCharacterLeft, BubbleGame.masterController))
            {
                if (currenthelp > 0)
                {
                    currenthelp--;

                    Vector2 startposition = new Vector2(2000, currentHelpScreen.EndPopUpPosition.Y);
                    currentHelpScreen.StartPopUpPosition = startposition;
                    currentHelpScreen.StartPopUpTime = 0;
                    currentHelpScreen.ExitScreen();
                    previousHelpScreen = currentHelpScreen;
                    currentHelpScreen = new HelpScreen(helpScreens[currenthelp]);
                    currentHelpScreen.ScreenManager = BubbleGame.screenManager;
                    startposition = new Vector2(-1000, currentHelpScreen.EndPopUpPosition.Y);
                    currentHelpScreen.StartPopUpPosition = startposition;
                    currentHelpScreen.WindowCorner = startposition;
                    currentHelpScreen.StartPopUpTime = 500;
                }
            }
            else if (InputManager.IsActionTriggered(InputManager.Action.MoveCharacterRight, BubbleGame.masterController))
            {
                if (currenthelp < helpScreens.Length - 1)
                {
                    currenthelp++;
                    Vector2 startposition = new Vector2(-1000, currentHelpScreen.EndPopUpPosition.Y);
                    currentHelpScreen.StartPopUpPosition = startposition;
                    currentHelpScreen.StartPopUpTime = 0;
                    currentHelpScreen.ExitScreen();
                    previousHelpScreen = currentHelpScreen;
                    currentHelpScreen = new HelpScreen(helpScreens[currenthelp]);
                    currentHelpScreen.ScreenManager = BubbleGame.screenManager;
                    startposition = new Vector2(2000, currentHelpScreen.EndPopUpPosition.Y);
                    currentHelpScreen.StartPopUpPosition = startposition;
                    currentHelpScreen.WindowCorner = startposition;
                    currentHelpScreen.StartPopUpTime = 500;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            currentHelpScreen.Draw(gameTime);
            if (previousHelpScreen != null)
                previousHelpScreen.Draw(gameTime);

            // draw navigation arrows
            if (isStable)
            {
                SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
                spriteBatch.Begin();
                float scale;
                if (gameTime.TotalGameTime.Milliseconds < 500)
                    scale = gameTime.TotalGameTime.Milliseconds / 4000f + 1.3f; 
                else
                    scale = (500 - (gameTime.TotalGameTime.Milliseconds - 500)) / 4000f + 1.3f; 

                if (currenthelp < helpScreens.Length - 1)
                {
                    spriteBatch.Draw(arrow, new Vector2(currentHelpScreen.WindowCorner.X + currentHelpScreen.HelpTexture.Width - arrow.Width * (scale / 2) - arrow.Width, currentHelpScreen.WindowCorner.Y + currentHelpScreen.HelpTexture.Height / 2 - arrow.Height * (scale / 2)), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                if (currenthelp > 0)
                {
                    spriteBatch.Draw(arrow, new Vector2(currentHelpScreen.WindowCorner.X - arrow.Width * (scale / 2) + arrow.Width, currentHelpScreen.WindowCorner.Y + currentHelpScreen.HelpTexture.Height / 2 - arrow.Height * (scale / 2)), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0f);
                }

                spriteBatch.End();
            }
        }
    }
}