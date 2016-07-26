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
    public class ContentHolder
    {
        public string ContentName;
        public Texture2D Texture;
        public Color[] TextureData;
    }

    class InternalContentManager
    {
        static List<ContentHolder> contentList = new List<ContentHolder>();

        public InternalContentManager()
        {

        }

        static public void Load()
        {
            // load all of the sprites
            LoadContentHolder("Bubble", "BubbleSheet");
            LoadContentHolder("Blank", "Blank_Sprite");
            LoadContentHolder("Clear", "Clear_Sprite");
            LoadContentHolder("BlueStripe", "UI/textBlueStripe");
            LoadContentHolder("BlackStripe", "UI/BlackStripes");
            LoadContentHolder("TortoiseStripe", "UI/TortoiseStripes");
            LoadContentHolder("PenguinStripe", "UI/PenguinStripes");
            LoadContentHolder("ToadStripe", "UI/ToadStripes");
            LoadContentHolder("SealStripe", "UI/SealStripes");
            LoadContentHolder("PolarTile", "Backgrounds/polar_bear_tile");
            LoadContentHolder("SaharaTile", "Backgrounds/sahara_tile");
            LoadContentHolder("JungleTile", "Backgrounds/jungle_tile");
            LoadContentHolder("GiftShopTile", "Backgrounds/gift_shop_tile");
            LoadContentHolder("PettingZooTile", "Backgrounds/petting_zoo_tile");
            LoadContentHolder("SwampTile", "Backgrounds/swamp_tile");
            LoadContentHolder("AviaryTile", "Backgrounds/aviary_tile");
            LoadContentHolder("NightHouseTile", "Backgrounds/night_house_tile");
            LoadContentHolder("Fruit", "FruitSheet");
            LoadContentHolder("Specials", "SpecialSheet");
            LoadContentHolder("Gum", "GumSheet");
            LoadContentHolder("Seal", "SealSheet");
            LoadContentHolder("Tortoise", "TortoiseSheet");
            LoadContentHolder("Toad", "ToadSheet");
            LoadContentHolder("Penguin", "PenguinSheet");
            LoadContentHolder("Drone", "DroneSheet");
            LoadContentHolder("Bouncer", "BouncerSheet");
            LoadContentHolder("PorkyBot", "PorkyBot");
            LoadContentHolder("LazerBot", "LazerBotSheet");
            LoadContentHolder("RocketBot", "RocketBot");
            LoadContentHolder("RocketBlaster", "RocketBlaster");
            LoadContentHolder("Invisabot", "InvisabotSheet");
            LoadContentHolder("Warpbot", "WarpbotSheet");
            LoadContentHolder("Hunter", "HunterSheet");
            LoadContentHolder("SuperBouncer", "SuperBouncerSheet");
            LoadContentHolder("FireDrone", "FireDroneSheet");
            LoadContentHolder("Lazer", "LaserSheet");
            LoadContentHolder("Fireball", "FireballSheet");
            LoadContentHolder("100", "Points/100");
            LoadContentHolder("500", "Points/500");
            LoadContentHolder("1000", "Points/1000");
            LoadContentHolder("2000", "Points/2000");
            LoadContentHolder("5000", "Points/5000");
            LoadContentHolder("1", "UI/1");
            LoadContentHolder("2", "UI/2");
            LoadContentHolder("3", "UI/3");
            LoadContentHolder("Go", "UI/Go");
            LoadContentHolder("Complete", "UI/Complete");
            LoadContentHolder("GameOver", "UI/GameOver");
            LoadContentHolder("ThoughtBubble", "ThoughtBubble");
        }

        static private void LoadContentHolder(string name, string textureLocation)
        {
            ContentHolder holder = new ContentHolder();
            holder.ContentName = name;
            holder.Texture = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures", textureLocation));
            holder.TextureData = new Color[holder.Texture.Width * holder.Texture.Height];
            holder.Texture.GetData(holder.TextureData);
            contentList.Add(holder);
        }

        static public Texture2D GetTexture(string Name)
        {
            ContentHolder contentHolder = GetContentHolder(Name);
            return contentHolder.Texture;
        }

        static public Color[] GetTextureData(string Name)
        {
            ContentHolder contentHolder = GetContentHolder(Name);
            return contentHolder.TextureData;
        }

        static private ContentHolder GetContentHolder(string Name)
        {
            ContentHolder contentHolder = contentList[0];
            int i = 0;
            for (; i < contentList.Count; i++)
            {
                if (contentList[i].ContentName == Name)
                {
                    contentHolder = contentList[i];
                    break;
                }
            }
            if (i == contentList.Count)
            {
                throw new Exception("Unfound content");
            }
            return contentHolder;
        }

    }
}