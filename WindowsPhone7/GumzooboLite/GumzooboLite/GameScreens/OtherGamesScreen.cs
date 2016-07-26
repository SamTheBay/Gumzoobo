using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Phone.Tasks;


namespace BubbleGame
{
    class OtherGamesScreen : PopUpScreen
    {
        Texture2D title;
        Rectangle helpTexture = new Rectangle(0, 0, 700, 400);


        public OtherGamesScreen()
        {
            title = GameSprite.game.Content.Load<Texture2D>("Textures/UI/MoreGames");


            MenuEntry entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/fightthetide");
            entry.SetStartAnimation(new Vector2(810 + helpTexture.Width / 3 - entry.Texture.Width / 2, 180), new Vector2(800 / 2 - helpTexture.Width / 2 + helpTexture.Width / 3 - (entry.Texture.Width / 2), 180), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            MenuEntries.Add(entry);


            entry = new MenuEntry("");
            entry.Selected += new EventHandler<EventArgs>(entry_Selected);
            entry.Font = Fonts.HeaderFont;
            entry.Texture = GameSprite.game.Content.Load<Texture2D>("Textures/wormhole");
            entry.SetStartAnimation(new Vector2(810 + helpTexture.Width * 2 / 3 - entry.Texture.Width / 2, 180), new Vector2(800 / 2 - helpTexture.Width / 2 + helpTexture.Width * 2 / 3 - (entry.Texture.Width / 2), 180), 0, 1000, 1000);
            entry.SetAnimationType(AnimationType.Slide);
            MenuEntries.Add(entry);


            SetPopUpAnimation(new Vector2(810, 480 / 2 - 400 / 2), new Vector2(800 / 2 - helpTexture.Width / 2, 480 / 2 - 400 / 2), 0, 1000, 1000);
        }

        void entry_Selected(object sender, EventArgs e)
        {
            if (selectorIndex == 0)
            {
                MarketplaceDetailTask task = new MarketplaceDetailTask();
                task.ContentIdentifier = "328a3556-fa18-e011-9264-00237de2db9e";
                task.ContentType = MarketplaceContentType.Applications;
                task.Show();
            }
            else if (selectorIndex == 1)
            {
                MarketplaceDetailTask task = new MarketplaceDetailTask();
                task.ContentIdentifier = "135a3b7c-3a0d-e011-9264-00237de2db9e";
                task.ContentType = MarketplaceContentType.Applications;
                task.Show();
            }
        }


        Vector2 scoreLocation = new Vector2(10, 10);
        public override void Draw(GameTime gameTime)
        {
            int boarderWidth = 12;

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            DrawBoarder(new Rectangle((int)windowCorner.X - boarderWidth, (int)windowCorner.Y - boarderWidth, helpTexture.Width + boarderWidth * 2, helpTexture.Height + boarderWidth * 2),
    InternalContentManager.GetTexture("BlueStripe"), boarderWidth, Color.White, spriteBatch);

            spriteBatch.Draw(title, new Vector2(windowCorner.X + helpTexture.Width / 2 - title.Width/2, windowCorner.Y + 20), Color.White);

            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Fight the Tide", new Vector2(windowCorner.X + helpTexture.Width / 3, 380), Color.Black);
            Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Worm Hole", new Vector2(windowCorner.X + helpTexture.Width * 2 / 3, 380), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
