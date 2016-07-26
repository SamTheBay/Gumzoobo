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
    class CreditsScreen : GameScreen
    {
        string[] creditsLines;
        int totalLines = 60;
        int elapsedTime = 0;
        int totalTime = 40000;

        Pawn[] characters;
        int[] charaterPositions = {2,8,14,20,27};


        public CreditsScreen()
            : base()
        {
            // setup text that will be in the credits
            creditsLines = new string[totalLines];
            for (int i = 0; i < totalLines; i++)
            {
                creditsLines[i] = "";
            }

            // fill in lines
            creditsLines[0] = "Producer / Developer / Designer";
            creditsLines[1] = "Sam Bayless";
            creditsLines[6] = "Art Manager / Game Designer";
            creditsLines[7] = "Sarah Bayless";
            creditsLines[12] = "Pixel Artist";
            creditsLines[13] = "Linda Smith";
            creditsLines[18] = "Sound Effects Engineer";
            creditsLines[19] = "Matthew Webster";
            creditsLines[24] = "Musical Arrangment By";
            creditsLines[25] = "Dustin Howie and";
            creditsLines[26] = "Ben Roland";

            // load up pawns
            characters = new Pawn[5];
            characters[0] = new Pawn("Tortoise", new Vector2(640, 720));
            characters[1] = new Pawn("Seal", new Vector2(640, 720));
            characters[2] = new Pawn("Penguin", new Vector2(640, 720));
            characters[3] = new Pawn("Drone", new Vector2(640, 720));
            characters[4] = new Pawn("PorkyBot", new Vector2(640, 720));


            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].PlayAnimation("MoveRight");
            }

        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime >= totalTime)
            {
                ExitScreen();
            }

            if (InputManager.IsActionTriggered(InputManager.Action.Back, 0) ||
                InputManager.IsActionTriggered(InputManager.Action.Back, 1) ||
                InputManager.IsActionTriggered(InputManager.Action.Back, 2) ||
                InputManager.IsActionTriggered(InputManager.Action.Back, 3))
            {
                ExitScreen();
            }

            // update the pawns
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].Update(gameTime);
                characters[i].SetClearPosition(new Vector2(640 - characters[i].TextureWidth / 2, 720 - elapsedTime / 20 + charaterPositions[i] * 40));
            }


        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            BubbleGame.sigletonGame.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            // Draw background


            // Draw current text on the screen
            for (int i = 0; i < totalLines; i++)
            {
                Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, creditsLines[i], new Vector2(640, 720 - elapsedTime/20 + i*40), Color.LightBlue, 1.5f); 
            }


            // Draw characters on screen?
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}