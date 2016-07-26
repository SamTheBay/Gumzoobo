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
    class IntroBackgroundScreen : GameScreen
    {
        Texture2D Background;

        public IntroBackgroundScreen()
        {
            //MusicManager.SingletonMusicManager.PlayTune("intro");
            Background = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "cover_art"));
            IsPopup = false;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            // draw the background
            spriteBatch.Draw(Background, new Vector2(0, 0), null, Color.White);

            if (!otherScreenHasFocus)
#if XBOX
                Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "Press Start", new Vector2(640, 450), Color.White, 1.5f);
#else
                Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "Press Enter", new Vector2(640, 450), Color.White, 1.5f);
#endif
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (InputManager.IsActionTriggered(InputManager.Action.EnterGame, 0) ||
                InputManager.IsActionTriggered(InputManager.Action.EnterGame, 1) ||
                InputManager.IsActionTriggered(InputManager.Action.EnterGame, 2) ||
                InputManager.IsActionTriggered(InputManager.Action.EnterGame, 3))
            {
                ScreenManager.AddScreen(new IntroScreen());
            }
        }

        public override void TopFullScreenAcquired()
        {
            base.TopFullScreenAcquired();

            MusicManager.SingletonMusicManager.StopAll();
            MusicManager.SingletonMusicManager.PlayTune("intro");

            // logic to clear out all players the could be left in the game
            BubbleGame.players[0] = new PlayerSprite(0);
            BubbleGame.players[1] = new PlayerSprite(1);
            BubbleGame.players[2] = new PlayerSprite(2);
            BubbleGame.players[3] = new PlayerSprite(3);

        }
    }
}