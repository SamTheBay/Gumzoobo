using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BubbleGame
{
    class IntroBackgroundScreen : GameScreen
    {
        Texture2D Background;
        static Vector2 textLocation = new Vector2(400, 260);
        ContinueQuestion PlayMusicScreen;
        bool questionAdded = false;

        public IntroBackgroundScreen()
        {
            Background = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "cover_art"));
            IsPopup = false;
            EnabledGestures = Microsoft.Xna.Framework.Input.Touch.GestureType.Tap;

            //if (MediaPlayer.GameHasControl)
            //{
            PlayMusicScreen = new ContinueQuestion(YesNoReason.Music);
            //}
            //else
            //{
            //    questionAdded = true;
            //    MusicManager.musicOn = false;
            //}
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            // draw the background
            spriteBatch.Draw(Background, Vector2.Zero, null, Color.White);

            if (!otherScreenHasFocus)
                Fonts.DrawCenteredText(spriteBatch, Fonts.HeaderFont, "Touch To Start", textLocation, Color.White, 1.3f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void HandleInput()
        {
            if (PlayMusicScreen == null)
            {
                base.HandleInput();

                if (InputManager.IsLocationTapped(BubbleGame.ScreenSize))
                {
                    ScreenManager.AddScreen(new IntroScreen());
                }
                else if (InputManager.IsBackTriggered())
                {
                    BubbleGame.sigletonGame.Exit();
                }
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (questionAdded == false && PlayMusicScreen != null)
            {
                ScreenManager.AddScreen(PlayMusicScreen);
                questionAdded = true;
            }
            if (PlayMusicScreen != null && PlayMusicScreen.IsFinished == true)
            {
                MusicManager.musicOn = PlayMusicScreen.Result;
                PlayMusicScreen = null;

                if (MusicManager.musicOn == true)
                {
                    MusicManager.SingletonMusicManager.StopAll();
                    MusicManager.SingletonMusicManager.PlayTune("intro");
                }
            }
        }

        public override void TopFullScreenAcquired()
        {
            base.TopFullScreenAcquired();

            MusicManager.SingletonMusicManager.StopAll();
            MusicManager.SingletonMusicManager.PlayTune("intro");
        }
    }
}