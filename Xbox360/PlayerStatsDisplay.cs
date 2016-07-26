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
        Texture2D Seal;
        Texture2D Tortoise;
        Texture2D Toad;
        Texture2D Penguin;
        Texture2D ArrowRight;

        Texture2D[] WeaponTextures;
        Texture2D[] LifeIcon;
        Texture2D[] Background;
        Texture2D ScoreBox;
        Texture2D BlankBar;
        Texture2D FullBar;
        Texture2D BlankBarHorz;
        Texture2D FullBarHorz;
        Texture2D BlankBG;


        public PlayerStatsDisplay(PlayerSprite[] nPlayers)
        {
            players = nPlayers;
            Seal = InternalContentManager.GetTexture("Seal");
            Tortoise = InternalContentManager.GetTexture("Tortoise");
            Toad = InternalContentManager.GetTexture("Toad");
            Penguin = InternalContentManager.GetTexture("Penguin");
            ArrowRight = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\UI", "ArrowRight"));

            WeaponTextures = new Texture2D[(int)Weapon.Num];
            WeaponTextures[(int)Weapon.Bubble] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "active_bubble"));
            WeaponTextures[(int)Weapon.ABC] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "active_abc"));
            WeaponTextures[(int)Weapon.Cinnemon] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "active_cinnamon"));
            WeaponTextures[(int)Weapon.Grape] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "active_grape"));
            WeaponTextures[(int)Weapon.Mint] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "active_mint"));
            WeaponTextures[(int)Weapon.Super] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "active_super_bubble"));

            LifeIcon = new Texture2D[(int)CharacterType.Num];
            LifeIcon[(int)CharacterType.Penguin] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "cheekeze_life_icon"));
            LifeIcon[(int)CharacterType.Seal] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "bupper_life_icon"));
            LifeIcon[(int)CharacterType.Tortoise] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "froofy_life_icon"));
            LifeIcon[(int)CharacterType.Toad] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "clavis_life_icon"));

            Background = new Texture2D[(int)CharacterType.Num];
            Background[(int)CharacterType.Penguin] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "cheekeze_bg"));
            Background[(int)CharacterType.Seal] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "bupper_bg"));
            Background[(int)CharacterType.Tortoise] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "froofy_bg"));
            Background[(int)CharacterType.Toad] = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "clavis_bg"));

            ScoreBox = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "score_box"));
            BlankBar = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "emptybar"));
            FullBar = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "blankfullbar"));
            BlankBarHorz = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "emptybarhoriz"));
            FullBarHorz = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "blankfullbarhoriz"));
            BlankBG = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "blank_bg"));
        }


        public void Draw(SpriteBatch spriteBatch, GameScreen screen)
        {
            // loop for each player
            for (int i = 0; i < players.Length; i++)
            {
                PlayerSprite player = players[i];
                Vector2 baseVector;

                // set the base vector for this player
                if (i == 0)
                    baseVector = new Vector2(0,0);
                else if (i == 1)
                    baseVector = new Vector2(1072,0);
                else if (i == 2)
                    baseVector = new Vector2(0, 360);
                else
                    baseVector = new Vector2(1072, 360);


                if (player.IsActive)
                {
                    // get players type
                    CharacterType type;
                    if (player is SealPlayer)
                        type = CharacterType.Seal;
                    else if (player is ToadPlayer)
                        type = CharacterType.Toad;
                    else if (player is TortoisePlayer)
                        type = CharacterType.Tortoise;
                    else
                        type = CharacterType.Penguin;

                    // draw players background
                    spriteBatch.Draw(Background[(int)type], baseVector, Color.White);

                    // draw the characters life icons
                    for (int x = 0; x < player.Lives && x < 3; x++)
                    {
                        spriteBatch.Draw(LifeIcon[(int)type], new Vector2(baseVector.X + 40 + (x * (32 + 15)), baseVector.Y + 80), Color.White);
                    }

                    // Draw the life bar
                    Color lifeColor = Color.LightGreen;
                    if (player.Health < 30)
                    {
                        lifeColor = Color.OrangeRed;
                    }
                    else if (player.Health < 60)
                    {
                        lifeColor = Color.Yellow;
                    }
                    Vector2 lifeBarCorner = new Vector2(baseVector.X + 40, baseVector.Y + 80 + 32 + 5);
                    spriteBatch.Draw(BlankBarHorz, lifeBarCorner, Color.White);
                    spriteBatch.Draw(FullBarHorz,
                        lifeBarCorner,
                        new Rectangle(0, 0, (int)((float)FullBarHorz.Width * (float)player.Health / 100f), FullBarHorz.Height),
                        lifeColor);

                    // Draw the current weapon
                    spriteBatch.Draw(WeaponTextures[(int)player.CurrentWeapon], new Vector2(baseVector.X + 40, baseVector.Y + 80 + 32 + 5 + 28 + 5), Color.White);

                    // Draw the ammo bar
                    Vector2 ammoBarCorner = new Vector2(baseVector.X + 165 - 28 - 28 - 15, baseVector.Y + 80 + 32 + 5 + 28 + 17);
                    int ammoDeplete = (int)((float)FullBar.Height * (1f - ((float)player.GetAmmo(player.CurrentWeapon) / (float)WeaponSprite.MaxAmmo(player.CurrentWeapon))));
                    spriteBatch.Draw(BlankBar, ammoBarCorner, Color.White);
                    spriteBatch.Draw(FullBar,
                        new Vector2(ammoBarCorner.X, ammoBarCorner.Y + ammoDeplete),
                        new Rectangle(0, ammoDeplete, FullBar.Width, FullBar.Height - ammoDeplete),
                        WeaponSprite.WeaponColor(player.CurrentWeapon));

                    // Draw the chew bar
                    Vector2 chewBarCorner = new Vector2(baseVector.X + 165 - 28, baseVector.Y + 80 + 32 + 5 + 28 + 17);
                    int chewDeplete = (int)((float)FullBar.Height * (100f - (float)player.Chew) / 100f);
                    spriteBatch.Draw(BlankBar, chewBarCorner, Color.White);
                    spriteBatch.Draw(FullBar,
                        new Vector2(chewBarCorner.X, chewBarCorner.Y + chewDeplete),
                        new Rectangle(0, chewDeplete, FullBar.Width, FullBar.Height - chewDeplete),
                        WeaponSprite.WeaponColor(player.CurrentWeapon));

                    // Draw the score box
                    spriteBatch.Draw(ScoreBox, new Vector2(baseVector.X + 53, baseVector.Y + 313), Color.White);
                    
                    // Draw the score
                    Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, player.Points.ToString() , new Vector2(baseVector.X + 53 + ScoreBox.Width / 2, baseVector.Y + 313 + ScoreBox.Height/2), Color.Black);

                }
                else if (player.IsSelecting)
                {
                    // draw blank background
                    spriteBatch.Draw(BlankBG, baseVector, Color.White);
                    
                    // draw selected character
                    if (player.CurrentSelection == 0)
                    {
                        Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, SealPlayer.StaticName(), new Vector2(104, 60) + baseVector, Color.White, 1.5f);
                        spriteBatch.Draw(Seal, new Vector2(baseVector.X + 104 - 40, baseVector.Y + 150), new Rectangle(0, 0, 80, 80), Color.White);
                    }
                    else if (player.CurrentSelection == 1)
                    {
                        Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, TortoisePlayer.StaticName(), new Vector2(104, 60) + baseVector, Color.White, 1.5f);
                        spriteBatch.Draw(Tortoise, new Vector2(baseVector.X + 104 - 36, baseVector.Y + 150), new Rectangle(0, 0, 72, 62), Color.White);
                    }
                    else if (player.CurrentSelection == 2)
                    {
                        Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, ToadPlayer.StaticName(), new Vector2(104, 60) + baseVector, Color.White, 1.5f);
                        spriteBatch.Draw(Toad, new Vector2(baseVector.X + 104 - 58, baseVector.Y + 150), new Rectangle(0, 0, 86, 70), Color.White);
                    }
                    else if (player.CurrentSelection == 3)
                    {
                        Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, PenguinPlayer.StaticName(), new Vector2(104, 60) + baseVector, Color.White, 1.5f);
                        spriteBatch.Draw(Penguin, new Vector2(baseVector.X + 104 - 21, baseVector.Y + 150), new Rectangle(0, 0, 42, 74), Color.White);
                    }
                    spriteBatch.Draw(ArrowRight, new Vector2(baseVector.X + 20, baseVector.Y + 150), new Rectangle(0,0, ArrowRight.Width, ArrowRight.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.Draw(ArrowRight, new Vector2(baseVector.X + 208 - 20 - ArrowRight.Width, baseVector.Y + 150), new Rectangle(0, 0, ArrowRight.Width, ArrowRight.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else if (player.Continues > 0)
                {
                    // draw blank background
                    spriteBatch.Draw(BlankBG, baseVector, Color.White);

#if XBOX
                    Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "Press Start", new Vector2(104, 150) + baseVector, Color.White, 1.2f);
                    Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, "To Join", new Vector2(104, 180) + baseVector, Color.White, 1.2f);
#endif
                }
                else
                {
                    // draw blank background
                    spriteBatch.Draw(BlankBG, baseVector, Color.White);
                }
            }
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

    }
}