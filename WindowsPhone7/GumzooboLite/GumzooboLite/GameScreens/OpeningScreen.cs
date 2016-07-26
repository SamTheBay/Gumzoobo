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
    class OpeningScreen : GameScreen
    {
        int currentTime = 0;
        int transToOpen = 500;
        int transToSwitch = 2500;
        int switchTime = 3000;
        int transToSecond = 3500;
        int transToEnd = 5500;
        int endTime = 6000;
        Texture2D ATYG;
        Texture2D SB;
        Rectangle backgroundSize = new Rectangle(56, 0, 800 - 56, 480 - 48);
        Color darkColor = new Color(0, 0, 0, 255); 
        Texture2D blank;

        public OpeningScreen()
        {
            ATYG = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "ATYG"));
            SB = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "SB"));
            blank = InternalContentManager.GetTexture("Blank");
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //while (gameTime.IsRunningSlowly)
            //    return;

            currentTime += gameTime.ElapsedGameTime.Milliseconds;

            if (currentTime > endTime)
            {
                BubbleGame.screenManager.AddScreen(new IntroBackgroundScreen());
                ExitScreen();
            }

            //if (InputManager.IsBackTriggered())
            //{
            //    // TODO: remove skip
            //    BubbleGame.screenManager.AddScreen(new IntroBackgroundScreen());
            //    ExitScreen();
            //}
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            BubbleGame.screenManager.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            if (currentTime < switchTime)
            {
                // draw atyg screen
                spriteBatch.Draw(ATYG, new Vector2(400 - ATYG.Width/2, 270 - ATYG.Height), Color.White);
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Presents", new Vector2(400, 290), Color.White, 2f); 
            }
            else
            {
                // draw sb production
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "A", new Vector2(400, 260 - SB.Height), Color.White, 2f);
                spriteBatch.Draw(SB, new Vector2(400 - SB.Width / 2, 280 - SB.Height), Color.White);
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Production", new Vector2(400, 300), Color.White,2f); 
            }



            if (currentTime <= transToOpen)
            {
                darkColor.A = (Byte)(255 * (((float)transToOpen - (float)currentTime) / ((float)transToOpen)));
            }
            else if (currentTime <= transToSwitch)
            {
                darkColor.A = 0;
            }
            else if (currentTime <= switchTime)
            {
                darkColor.A = (Byte)(255 * (1f - ((float)switchTime - (float)currentTime) / ((float)switchTime - (float)transToSwitch)));
            }
            else if (currentTime <= transToSecond)
            {
                darkColor.A = (Byte)(255 * ((float)transToSecond - (float)currentTime) / ((float)transToSecond - (float)switchTime));
            }
            else if (currentTime <= transToEnd)
            {
                darkColor.A = 0;
            }
            else if (currentTime <= endTime)
            {
                darkColor.A = (Byte)(255 * (1f - ((float)endTime - (float)currentTime) / ((float)endTime - (float)transToEnd)));
            }
            else
            {
                darkColor.A = 255;
            }

            spriteBatch.Draw(blank, backgroundSize, darkColor);

            spriteBatch.End();
        }


    }
}