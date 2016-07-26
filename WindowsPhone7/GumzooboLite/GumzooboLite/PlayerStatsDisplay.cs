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
    class PlayerStatsDisplay
    {
        enum CharacterType
        {
            Seal,
            Penguin,
            Toad,
            Tortoise,
            Num
        }

        PlayerSprite[] players;
        Texture2D ArrowUp;
        Texture2D Bubble;

        Texture2D[] WeaponTextures;
        Texture2D[] InactiveWeaponTextures;
        Texture2D[] LifeIcon;
        Texture2D ScoreBox;
        Texture2D BlankBar;
        Texture2D FullBar;
        Texture2D Blank;

        // locations
        Vector2 ammoBarCorner;
        Vector2 lifeIconCorner;
        Vector2 lifeTextCorner = new Vector2(300, 480-24);
        Rectangle ammobarFill;
        Vector2 ammobarFillStart;
        Vector2 weaponLoc = new Vector2();
        Rectangle uiSizeHeight = new Rectangle(0, 0, 56, 480);
        Rectangle uiSizeWidth = new Rectangle(0, 480 - 80, 800, 80);
        Rectangle JumpButtonRect = new Rectangle(800 - 160 / 2 - 64 / 2, 480 - 64 - ((80 - 64) / 2), 64, 64);
        Rectangle ShootButtonRect = new Rectangle(80, 480 - 48 - ((80 - 48) / 2), 48, 48);
        Rectangle ArrowUpRect;
        Rectangle BubbleRect;

        public PlayerStatsDisplay(PlayerSprite[] nPlayers)
        {
            players = nPlayers;
            ArrowUp = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\UI", "ArrowUp"));
            Blank = InternalContentManager.GetTexture("Blank");
            Bubble = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\UI", "BubbleButton"));

            WeaponTextures = new Texture2D[(int)Weapon.Num];
            WeaponTextures[(int)Weapon.Bubble] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "bubble"));
            WeaponTextures[(int)Weapon.ABC] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "abc"));
            WeaponTextures[(int)Weapon.Cinnemon] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "cinnamon"));
            WeaponTextures[(int)Weapon.Grape] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "grape"));
            WeaponTextures[(int)Weapon.Mint] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "mint"));
            WeaponTextures[(int)Weapon.Super] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "super_bubble"));

            InactiveWeaponTextures = new Texture2D[(int)Weapon.Num];
            InactiveWeaponTextures[(int)Weapon.Bubble] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "inactive_bubble"));
            InactiveWeaponTextures[(int)Weapon.ABC] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "inactive_abc"));
            InactiveWeaponTextures[(int)Weapon.Cinnemon] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "inactive_cinnamon"));
            InactiveWeaponTextures[(int)Weapon.Grape] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "inactive_grape"));
            InactiveWeaponTextures[(int)Weapon.Mint] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "inactive_mint"));
            InactiveWeaponTextures[(int)Weapon.Super] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "inactive_super_bubble"));

            LifeIcon = new Texture2D[(int)CharacterType.Num];
            LifeIcon[(int)CharacterType.Penguin] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "cheekeze_life_icon"));
            LifeIcon[(int)CharacterType.Seal] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "bupper_life_icon"));
            LifeIcon[(int)CharacterType.Tortoise] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "froofy_life_icon"));
            LifeIcon[(int)CharacterType.Toad] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "clavis_life_icon"));

            ScoreBox = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "score_box"));
            BlankBar = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "emptybar"));
            FullBar = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "blankfullbar"));


            ammoBarCorner = new Vector2(56 / 2 - FullBar.Width / 2, 340);
            ammobarFillStart = new Vector2(ammoBarCorner.X, 0);
            ammobarFill = new Rectangle(0, 0, FullBar.Width, 0);
            lifeIconCorner = new Vector2(230, 480 - 24 - LifeIcon[0].Height/2);

            ArrowUpRect = new Rectangle(0, 0, ArrowUp.Width, ArrowUp.Height);
            BubbleRect = new Rectangle(0, 0, Bubble.Width, Bubble.Height);
        }


        public void Draw(SpriteBatch spriteBatch, GameScreen screen)
        {
            // draw background
            spriteBatch.Draw(Blank, uiSizeHeight, Color.Black);
            spriteBatch.Draw(Blank, uiSizeWidth, Color.Black);

            // draw buttons
            spriteBatch.Draw(ArrowUp, JumpButtonRect, ArrowUpRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(Bubble, ShootButtonRect, BubbleRect, WeaponSprite.WeaponColor(players[0].CurrentWeapon), 0f, Vector2.Zero, SpriteEffects.None, 0);
            


            // Draw the current weapon
            for (int i = 0; i < (int)Weapon.Num; i++)
            {
                weaponLoc.X = 56 / 2 - WeaponTextures[i].Width / 2;
                weaponLoc.Y = 20 + i * (WeaponTextures[i].Height + 26);
                if (i == (int)players[0].CurrentWeapon)
                    spriteBatch.Draw(WeaponTextures[i], weaponLoc, Color.White);
                else
                    spriteBatch.Draw(InactiveWeaponTextures[i], weaponLoc, Color.White);
            }

            // Draw the ammo bar
            int ammoDeplete = (int)((float)FullBar.Height * (1f - ((float)players[0].GetAmmo(players[0].CurrentWeapon) / (float)WeaponSprite.MaxAmmo(players[0].CurrentWeapon))));
            spriteBatch.Draw(BlankBar, ammoBarCorner, Color.White);
            ammobarFillStart.Y = ammoBarCorner.Y + ammoDeplete;
            ammobarFill.Y = ammoDeplete;
            ammobarFill.Height = FullBar.Height - ammoDeplete;
            spriteBatch.Draw(FullBar,
                ammobarFillStart,
                ammobarFill,
                WeaponSprite.WeaponColor(players[0].CurrentWeapon));

            // Draw the number of lives
            //spriteBatch.Draw(LifeIcon[GetCharacterType(players[0])], lifeIconCorner, Color.White);
            //Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "X " + players[0].Lives.ToString(), lifeTextCorner, Color.White);
        }


        // Todo: move to weapon sprite?
        static public void DrawWeaponIcon(Vector2 location, SpriteBatch spriteBatch, PlayerSprite player, float scale, Weapon wep)
        {
            if (wep == Weapon.Bubble)
            {
                BubbleWeapon.DrawIcon(spriteBatch, location, scale);
            }
            else if (wep == Weapon.Cinnemon)
            {
                CinnemonWeapon.DrawIcon(spriteBatch, location, scale);
            }
            else if (wep == Weapon.Mint)
            {
                MintWeapon.DrawIcon(spriteBatch, location, scale);
            }
            else if (wep == Weapon.Grape)
            {
                GrapeWeapon.DrawIcon(spriteBatch, location, scale);
            }
            else if (wep == Weapon.ABC)
            {
                ABCWeapon.DrawIcon(spriteBatch, location, scale);
            }
            else if (wep == Weapon.Super)
            {
                SuperBubbleWeapon.DrawIcon(spriteBatch, location, scale);
            }
        }


        static int GetCharacterType(PlayerSprite player)
        {
            if (player is SealPlayer)
            {
                return (int)CharacterType.Seal;           
            }
            else if (player is PenguinPlayer)
            {
                return (int)CharacterType.Penguin;
            }
            else if (player is ToadPlayer)
            {
                return (int)CharacterType.Toad;
            }
            else
            {
                return (int)CharacterType.Tortoise;
            }
                
        }

    }
}