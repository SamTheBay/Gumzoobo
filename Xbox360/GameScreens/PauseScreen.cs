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
        Texture2D boarderTexture;
        Texture2D buttonTexture;
        Texture2D pauseTexture;
        static public bool isPaused = false;

        public PauseScreen(PlayerSprite owner)
            : base()
        {
            this.owner = owner;
            buttonTexture = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "button48593266"));
            pauseTexture = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "Paused"));
            IsPopup = true;
            controllerIndex = owner.PlayerIndex;
            isPlayerTuned = true;
            isPaused = true;

            // load boarder texture
            if (owner is ToadPlayer)
            {
                boarderTexture = InternalContentManager.GetTexture("ToadStripe");
            }
            else if (owner is SealPlayer)
            {
                boarderTexture = InternalContentManager.GetTexture("SealStripe");
            }
            else if (owner is TortoisePlayer)
            {
                boarderTexture = InternalContentManager.GetTexture("TortoiseStripe");
            }
            else
            {
                boarderTexture = InternalContentManager.GetTexture("PenguinStripe");
            }

            // add in buttons for pause menu
            MenuEntry entry = new MenuEntry("Resume Game");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(1290, 295), new Vector2((1280 - buttonTexture.Width) / 2, 295), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = buttonTexture;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Controls");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(1290, 295 + buttonTexture.Height), new Vector2((1280 - buttonTexture.Width) / 2, 295 + buttonTexture.Height), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = buttonTexture;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Instructions");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(1290, 295 + buttonTexture.Height * 2), new Vector2((1280 - buttonTexture.Width) / 2, 295 + buttonTexture.Height * 2), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = buttonTexture;
            MenuEntries.Add(entry);

            entry = new MenuEntry("Drop Out");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.SetStartAnimation(new Vector2(1290, 295 + buttonTexture.Height * 3), new Vector2((1280 - buttonTexture.Width) / 2, 295 + buttonTexture.Height * 3), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = buttonTexture;
            MenuEntries.Add(entry);

            SetPopUpAnimation(new Vector2(1290, 360 - 350 / 2), new Vector2(640 - 500 / 2, 360 - 350 / 2), 0, 1000, 1000);

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
                // controls
#if XBOX
                HelpScreen screen = new HelpScreen("Controls");
#else
                HelpScreen screen = new HelpScreen("keyboardControls");
#endif
                screen.isInLevel = false;
                screen.IsMasterControllerSensitive = true;
                BubbleGame.screenManager.AddScreen((GameScreen)screen);
            }
            else if (sender.Equals(MenuEntries[2]))
            {
                // instructions
                GameScreen screen = new InstructionsScreen();
                screen.IsMasterControllerSensitive = true;
                BubbleGame.screenManager.AddScreen(screen);

            }
            else if (sender.Equals(MenuEntries[3]))
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
            base.OnRemoval();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // draw boarder
            Rectangle helpTexture = new Rectangle(0, 0, 500, 350);
            int boarderWidth = 12;

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2),
                InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);
            spriteBatch.Draw(pauseTexture, new Vector2(windowCorner.X + helpTexture.Width/2 + boarderWidth - pauseTexture.Width / 2, windowCorner.Y + 10), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}