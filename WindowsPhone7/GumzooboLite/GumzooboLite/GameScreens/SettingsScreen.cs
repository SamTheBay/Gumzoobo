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
using Microsoft.Xna.Framework.GamerServices;

namespace BubbleGame
{
    class SettingsScreen : PopUpScreen
    {
        Texture2D buttonTexture;
        Texture2D pressTexture;
        Texture2D settingsTexture;


        public SettingsScreen()
            : base()
        {
            IsPopup = true;
            buttonTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/WideButton");
            pressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/WideButtonPress");
            settingsTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/Settings");
            windowCorner = new Vector2(-1 * 750 - 20, 480/2 - 400 / 2);


            // add buttons for the screen
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    // start the animations
                    MenuEntry entry = new MenuEntry("");
                    entry.Selected += new EventHandler<EventArgs>(entry_Selected);
                    entry.Texture = buttonTexture;
                    entry.PressTexture = pressTexture;
                    int buttonHeightStart = 195;
                    if (i == 0)
                    {
                        entry.SetStartAnimation(new Vector2(-1 * 700 - 20 + 400 - buttonTexture.Width, j * buttonTexture.Height + buttonHeightStart), new Vector2(400 - buttonTexture.Width, j * buttonTexture.Height + buttonHeightStart), 0, 1000, 1000);
                    }
                    else
                    {
                        entry.SetStartAnimation(new Vector2(-1 * 700 - 20 + 400, j * buttonTexture.Height + buttonHeightStart), new Vector2(400, j * buttonTexture.Height + buttonHeightStart), 0, 1000, 1000);
                    }
                    entry.SetAnimationType(AnimationType.Slide);
                    entry.Font = Fonts.HeaderFont;
                    MenuEntries.Add(entry);
                }

            }
            SetPopUpAnimation(new Vector2(-1 * 700 - 20, 480 / 2 - 400 / 2), new Vector2(800 / 2 - 700 / 2, 480 / 2 - 400 / 2),0, 1000, 1000);

            MenuEntries[0].Text = "Music On";
            MenuEntries[1].Text = "Sound FX On";
            MenuEntries[2].Text = "Music Off";
            MenuEntries[3].Text = "Sound FX Off";

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Rectangle helpTexture = new Rectangle(0, 0, 700, 400);
            int boarderWidth = 12;


            // set color based on settings
            if (MusicManager.musicOn)
            {
                MenuEntries[0].TextColor = Color.Red;
                MenuEntries[2].TextColor = Color.Black;
            }
            else
            {
                MenuEntries[0].TextColor = Color.Black;
                MenuEntries[2].TextColor = Color.Red;
            }
            if (AudioManager.SoundFXOn)
            {
                MenuEntries[1].TextColor = Color.Red;
                MenuEntries[3].TextColor = Color.Black;
            }
            else
            {
                MenuEntries[1].TextColor = Color.Black;
                MenuEntries[3].TextColor = Color.Red;
            }


            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2),
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(settingsTexture, new Vector2(windowCorner.X + 350 - settingsTexture.Width / 2, 60), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }




        void entry_Selected(object sender, EventArgs e)
        {
            if (selectedEntry == 0)
            {
                MusicManager.musicOn = true;
                MusicManager.SingletonMusicManager.PlayTune("intro");
            }
            else if (selectedEntry == 1)
            {
                AudioManager.SoundFXOn = true;
            }
            else if (selectedEntry == 2)
            {
                MusicManager.SingletonMusicManager.StopAll();
                MusicManager.musicOn = false;
            }
            else if (selectedEntry == 3)
            {
                AudioManager.SoundFXOn = false;
            }
        }


       

    }
}
