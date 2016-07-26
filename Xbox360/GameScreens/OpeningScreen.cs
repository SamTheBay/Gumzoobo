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
        Effect Darkener;
        Texture2D ATYG;
        Texture2D SB;

        public OpeningScreen()
        {
            Darkener = BubbleGame.sigletonGame.Content.Load<Effect>("Darkener");
            Darkener.Parameters["darkFactor"].SetValue(1f);
            ATYG = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "ATYG"));
            SB = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "SB"));
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            currentTime += gameTime.ElapsedGameTime.Milliseconds;

            if (currentTime > endTime)
            {
                BubbleGame.screenManager.AddScreen(new IntroBackgroundScreen());
                ExitScreen();
            }

            //if (InputManager.IsActionTriggered(InputManager.Action.Back, 0) ||
            //    InputManager.IsActionTriggered(InputManager.Action.Back, 1) ||
            //    InputManager.IsActionTriggered(InputManager.Action.Back, 2) ||
            //    InputManager.IsActionTriggered(InputManager.Action.Back, 3))
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
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            Darkener.Begin();
            Darkener.CurrentTechnique.Passes[0].Begin();

            float darkFactor = 1f;
            if (currentTime <= transToOpen)
            {
                darkFactor = ((float)transToOpen - (float)currentTime) / ((float)transToOpen);
            }
            else if (currentTime <= transToSwitch)
            {
                darkFactor = 0f;
            }
            else if (currentTime <= switchTime)
            {
                darkFactor = 1f - ((float)switchTime - (float)currentTime) / ((float)switchTime - (float)transToSwitch);
            }
            else if (currentTime <= transToSecond)
            {
                darkFactor = ((float)transToSecond - (float)currentTime) / ((float)transToSecond - (float)switchTime);
            }
            else if (currentTime <= transToEnd)
            {
                darkFactor = 0f;
            }
            else if (currentTime <= endTime)
            {
                darkFactor = 1f - ((float)endTime - (float)currentTime) / ((float)endTime - (float)transToEnd);
            }
            else
            {
                darkFactor = 1f;
            }

            Darkener.Parameters["darkFactor"].SetValue(darkFactor);

            if (currentTime < switchTime)
            {
                // draw atyg screen
                spriteBatch.Draw(ATYG, new Vector2(640 - ATYG.Width/2, 360 - ATYG.Height), Color.White);
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Presents", new Vector2(640, 380), Color.White, 2f); 
            }
            else
            {
                // draw sb production
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "A", new Vector2(640, 360 - SB.Height), Color.White, 2f);
                spriteBatch.Draw(SB, new Vector2(640 - SB.Width / 2, 380 - SB.Height), Color.White);
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Production", new Vector2(640, 400), Color.White,2f); 
            }

            Darkener.CurrentTechnique.Passes[0].End();
            Darkener.End();
            spriteBatch.End();
        }


    }
}