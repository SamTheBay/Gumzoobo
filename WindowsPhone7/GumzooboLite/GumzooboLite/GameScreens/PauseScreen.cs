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
    class PauseScreen : PopUpScreen
    {
        PlayerSprite owner;
        Texture2D buttonTexture;
        Texture2D pressTexture;
        Texture2D pauseTexture;
        static public bool isPaused = false;
        Rectangle helpTexture = new Rectangle(0, 0, 450, 400);

        public PauseScreen(PlayerSprite owner)
            : base()
        {
            this.owner = owner;
            buttonTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/WideButton");
            pressTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/WideButtonPress");
            pauseTexture = GameSprite.game.Content.Load<Texture2D>("Textures/UI/Paused");
            IsPopup = true;
            controllerIndex = owner.PlayerIndex;
            isPlayerTuned = true;
            isPaused = true;
            BubbleGame.adControlManager.ShowAds = false;

            // add in buttons for pause menu
            int buttonHeightStart = 100;
            MenuEntry entry = new MenuEntry("Resume Game");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(810 + helpTexture.Width / 2 - buttonTexture.Width / 2, buttonHeightStart), new Vector2((800 - buttonTexture.Width) / 2, buttonHeightStart), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = buttonTexture;
            entry.PressTexture = pressTexture;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Instructions");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(810 + helpTexture.Width / 2 - buttonTexture.Width / 2, buttonHeightStart + buttonTexture.Height), new Vector2((800 - buttonTexture.Width) / 2, buttonHeightStart + buttonTexture.Height), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = buttonTexture;
            entry.PressTexture = pressTexture;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Drop Out");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(810 + helpTexture.Width / 2 - buttonTexture.Width / 2, buttonHeightStart + buttonTexture.Height * 2), new Vector2((800 - buttonTexture.Width) / 2, buttonHeightStart + buttonTexture.Height * 2), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = buttonTexture;
            entry.PressTexture = pressTexture;
            MenuEntries.Add(entry);

            SetPopUpAnimation(new Vector2(810, 480/2 - 450 / 2), new Vector2(800/2 - helpTexture.Width / 2, 480/2 - 450 / 2), 0, 1000, 1000);

        }

        void entry_Selected(object sender, EventArgs e)
        {
            if (sender.Equals(MenuEntries[0]))
            {
                // resume
                ExitScreen();
            }
            else if (sender.Equals(MenuEntries[1]))
            {
                // instructions
                GameScreen screen = new InstructionsScreen();
                screen.IsMasterControllerSensitive = true;
                BubbleGame.screenManager.AddScreen(screen);

            }
            else if (sender.Equals(MenuEntries[2]))
            {
                // exit game
                owner.Lives = 0;
                owner.Continues = 0;
                owner.Deactivate();
                for (int i = 0; i < BubbleGame.players.Length; i++)
                {
                    // check if we need to promote a new master controller
                    if (BubbleGame.masterController == owner.PlayerIndex)
                    {
                        if (BubbleGame.players[i].HasSelected && !BubbleGame.players[i].Equals(owner))
                        {
                            BubbleGame.masterController = i;
                        }
                    }

                    // remove the player
                    if (BubbleGame.players[i].PlayerIndex == owner.PlayerIndex)
                    {
                        MapScreen.singletonMapSreen.RemovePawn(BubbleGame.players[i]);
                        BubbleGame.players[i] = new PlayerSprite(owner.PlayerIndex);
                        BubbleGame.players[i].Continues = 0;
                    }
                }
                ExitScreen();
            }
 
        }


        public override void OnRemoval()
        {
            isPaused = false;
            BubbleGame.adControlManager.ShowAds = true;
            base.OnRemoval();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // draw boarder
            int boarderWidth = 12;
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2),
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(pauseTexture, new Vector2(windowCorner.X + helpTexture.Width/2 - pauseTexture.Width / 2, windowCorner.Y), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}