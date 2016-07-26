using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Phone.Info;

namespace BubbleGame
{

    class InitialLoadScreen : GameScreen
    {
        Vector2 textLocation = new Vector2(800/2, 480/2);
        int iteration = 0;
        Texture2D BlankBarHorz;
        Texture2D FullBarHorz;
        Vector2 lifeBarCorner;
        Rectangle lifeBarFill;

        public InitialLoadScreen()
        {
            Fonts.LoadContent(BubbleGame.sigletonGame.Content);
            BlankBarHorz = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "emptybarhoriz"));
            FullBarHorz = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "blankfullbarhoriz"));
            lifeBarCorner = new Vector2(400 - FullBarHorz.Width/2, 320 - FullBarHorz.Height / 2);
            lifeBarFill = new Rectangle(0, 0, 0, FullBarHorz.Height);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            while (gameTime.IsRunningSlowly)
                return;

            //long deviceTotalMemory = (long)DeviceExtendedProperties.GetValue("DeviceTotalMemory");
            //long applicationCurrentMemoryUsage = (long)DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
            //long applicationPeakMemoryUsage = (long)DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage");
            //Debug.WriteLine("using " + ((float)applicationCurrentMemoryUsage / 1024f / 1024f).ToString() + " of " + ((float)deviceTotalMemory / 1024f / 1024f).ToString() +
            //    " with a peak of " + ((float)applicationPeakMemoryUsage /1024f /1024f).ToString() + " on iteration " + iteration.ToString());

            iteration++;
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            LoadInitial();

            if (iteration % 2 == 0)
                GC.Collect();

        }


        public void LoadInitial()
        {
            // load up content
            if (iteration == 2)
                InternalContentManager.Load();

            // load up one sound effect
            if (iteration == 4)
            {
                AudioManager.Initialize(BubbleGame.sigletonGame);
            }

            if (iteration == 6)
            {
                AudioManager.audioManager.LoadSFX(0, 4);
            }

            if (iteration == 8)
            {
                AudioManager.audioManager.LoadSFX(5, 9);
            }

            if (iteration == 10)
            {
                AudioManager.audioManager.LoadSFX(10, 14);
            }

            if (iteration == 12)
            {
                AudioManager.audioManager.LoadSFX(15, 17);
            }

            if (iteration == 14)
            {
                new MusicManager();
            }

            if (iteration == 16)
            {
                // = new ContentManager(BubbleGame.sigletonGame.Content.ServiceProvider, BubbleGame.sigletonGame.Content.RootDirectory);
                MusicManager.SingletonMusicManager.LoadTune("intro", BubbleGame.sigletonGame.Content);
                MusicManager.SingletonMusicManager.LoadTune("levelselect", BubbleGame.sigletonGame.Content);
                MusicManager.SingletonMusicManager.LoadTune("generic", BubbleGame.sigletonGame.Content);
                MusicManager.SingletonMusicManager.LoadTune("generic2", BubbleGame.sigletonGame.Content);
                MusicManager.SingletonMusicManager.LoadTune("nighthouse", BubbleGame.sigletonGame.Content);
                //MusicManager.SingletonMusicManager.LoadTune("desert", BubbleGame.sigletonGame.Content);
                //MusicManager.SingletonMusicManager.LoadTune("aquarium", BubbleGame.sigletonGame.Content);
                //MusicManager.SingletonMusicManager.LoadTune("swamp", BubbleGame.sigletonGame.Content);
                
            }




            // load the static data for the weapons
            if (iteration == 18)
            {
                BubbleWeapon.LoadStatic();
                CinnemonWeapon.LoadStatic();
                MintWeapon.LoadStatic();
                ABCWeapon.LoadStatic();
                GrapeWeapon.LoadStatic();
                SuperBubbleWeapon.LoadStatic();
            }

            // create your players (will be replaced by specific characters when added to game)
            if (iteration == 20)
            {
                BubbleGame.players[0] = new PlayerSprite(0);
                BubbleGame.players[1] = new PlayerSprite(1);
                BubbleGame.players[2] = new PlayerSprite(2);
                BubbleGame.players[3] = new PlayerSprite(3);
            }

            if (iteration == 22)
            {
                AddNextScreenAndExit(new OpeningScreen());
            }
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Loading...", textLocation, Color.White, 2.0f); 

            // draw progress bar
            //spriteBatch.Draw(BlankBarHorz, lifeBarCorner, Color.White);
            //lifeBarFill.Width = (int)((float)FullBarHorz.Width * (float)iteration / 22f);
            //spriteBatch.Draw(FullBarHorz,
            //    lifeBarCorner,
            //    lifeBarFill,
            //    Color.RoyalBlue);

            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}