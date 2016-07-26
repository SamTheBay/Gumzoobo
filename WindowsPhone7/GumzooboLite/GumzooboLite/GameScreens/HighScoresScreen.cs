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
    class HighScoresScreen : PopUpScreen
    {
        Rectangle helpTexture = new Rectangle(0, 0, 500, 450);
        Texture2D highScoresTitle;
        Vector2 indexPosition = new Vector2();
        Vector2 namePosition = new Vector2();
        Vector2 scorePosition = new Vector2();
        public static int highlightIndex = 11;

        public HighScoresScreen()
            : base()
        {
            IsPopup = true;
            SetPopUpAnimation(new Vector2(810, 480 / 2 - helpTexture.Height / 2), new Vector2(800 / 2 - helpTexture.Width / 2, 480 / 2 - helpTexture.Height / 2), 0, 1000, 1000);
            highScoresTitle = GameSprite.game.Content.Load<Texture2D>("Textures/UI/HighScores");
        }

       

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // draw boarder
            int boarderWidth = 12;
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2),
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(highScoresTitle, new Vector2(windowCorner.X + helpTexture.Width / 2 - highScoresTitle.Width / 2, windowCorner.Y + 10), Color.White);
            for (int i = 0; i < 10; i++)
            {
                Color textColor;
                if (i == highlightIndex)
                    textColor = Color.Green;
                else
                    textColor = Color.Black;

                HighScoreRecord record = BubbleGame.localHighScores.GetHighScore(i);
                indexPosition.Y = windowCorner.Y + i * 35 + 85;
                indexPosition.X = windowCorner.X + 35;
                spriteBatch.DrawString(Fonts.HeaderFont, (i + 1).ToString(), indexPosition, textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                //Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, (i + 1).ToString(), indexPosition, textColor);
                if (record.name != null)
                {
                    namePosition.Y = windowCorner.Y + i * 35 + 85;
                    namePosition.X = windowCorner.X + 100;
                    spriteBatch.DrawString(Fonts.HeaderFont, record.name, namePosition, textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    scorePosition.Y = windowCorner.Y + i * 35 + 85;
                    scorePosition.X = windowCorner.X + 320;
                    spriteBatch.DrawString(Fonts.HeaderFont, record.score.ToString(), scorePosition, textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        public override void HandleInput()
        {
            if (isStable &&
                (InputManager.IsBackTriggered() || InputManager.IsLocationTapped(BubbleGame.ScreenSize)))
            {
                ExitScreen();
            }
        }


        public override void ExitScreen()
        {
            highlightIndex = 11;
            base.ExitScreen();
        }
    }
}
