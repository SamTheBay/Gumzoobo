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
    class HelpScreen : PopUpScreen
    {
        Texture2D helpTexture;
        public bool isInLevel = true;

        public Texture2D HelpTexture
        {
            get { return helpTexture; }
        }

        public HelpScreen(string HelpTextureName)
            : base()
        {
            this.IsPopup = true;
            helpTexture = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/HelpWindows", HelpTextureName));

            SetPopUpAnimation(new Vector2(640 - helpTexture.Width / 2, -1 * helpTexture.Height -10), new Vector2(640 - helpTexture.Width / 2, 360 - helpTexture.Height / 2), 0, 1000, 1000);
        }


        public override void HandleInput()
        {
            if (isStable &&
                (InputManager.IsActionTriggered(InputManager.Action.Ok, BubbleGame.masterController) ||
                InputManager.IsActionTriggered(InputManager.Action.Back, BubbleGame.masterController)))
            {
                if (isInLevel)
                    Level.singletonLevel.hasShownHelp = true;
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            int boarderWidth = 12;

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2), 
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(helpTexture, windowCorner, Color.White);
            spriteBatch.End();
        }
    }
}