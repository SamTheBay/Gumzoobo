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
    class MapScreen : MenuScreen
    {
        LocationDescriptor[] locations = new LocationDescriptor[10];
        int currentLevelSetOpened;
        MenuEntry[] iconEntries = new MenuEntry[10];
        Texture2D ZooMap;
        Texture2D Lock;
        Texture2D Blank;
        List<Pawn> pawns = new List<Pawn>();
        int previouslySelectedEntry;

        // static game state members
        static public LocationDescriptor currentLocation;
        static public MapScreen singletonMapSreen;

        public MapScreen(int currentLevelSetOpened)
            : base()
        {
            if (BubbleGame.IsTrialModeCached && currentLevelSetOpened > 1)
            {
                currentLevelSetOpened = 1;
            }

            //currentLevelSetOpened = 9; // open levels hack // todo remove!
            singletonMapSreen = this;
            previouslySelectedEntry = selectedEntry;
            this.currentLevelSetOpened = currentLevelSetOpened;
            ZooMap = GameSprite.game.Content.Load<Texture2D>("Textures/UI/zoo_map");
            Lock = GameSprite.game.Content.Load<Texture2D>("Textures/UI/Lock");
            Blank = InternalContentManager.GetTexture("Blank");

            // create the location descriptors
            locations[0] = new LocationDescriptor("Petting Zoo", new Vector2(90 + 60,  5), 0, 4, "petting_zoo_bg", "../Clear_Sprite", "PettingZooTile", 1, "generic", "PettingZoo");
            locations[1] = new LocationDescriptor("Gift Shop", new Vector2(90 + 255, 0), 5, 9, "gift_shop_bg", "gift_shop_foreg", "GiftShopTile", 1, "generic2", "GiftShop");
            locations[2] = new LocationDescriptor("Jungle", new Vector2(90 + 470, 50), 10, 14, "jungle_bg", "jungle_foreg", "JungleTile", 1, "generic", "Jungle");
            locations[3] = new LocationDescriptor("Sahara", new Vector2(90 + 390, 150), 15, 19, "sahara_bg", "sahara_foreg", "SaharaTile", 1, "generic", "Sahara");
            locations[4] = new LocationDescriptor("Aviary", new Vector2(90 + 270, 300), 20, 24, "aviary_bg", "../Clear_Sprite", "AviaryTile", 1, "generic2", "Aviary");
            locations[5] = new LocationDescriptor("Swamp", new Vector2(90 + 505, 215), 25, 29, "swamp_bg", "../Clear_Sprite", "SwampTile", 1, "generic", "Swamp");
            locations[6] = new LocationDescriptor("Polar Cage", new Vector2(90 + 445, 385), 30, 34, "polar_bear_bg", "polar_bear_foreg", "PolarTile", 1, "generic2", "PolarBear");
            locations[7] = new LocationDescriptor("Monkey House", new Vector2(90 + 160, 200), 35, 39, "ape_house_bg", "ape_house_foreg", "SaharaTile", 1, "generic", "MonkeyHouse");
            locations[8] = new LocationDescriptor("Aquarium", new Vector2(90 + 90, 405), 40, 44, "aquarium_bg", "aquarium_foreg", "PolarTile", 1, "generic2", "Aquarium");
            locations[9] = new LocationDescriptor("Night House", new Vector2(90 + 75, 250), 45, 49, "night_house_bg", "../Clear_Sprite", "NightHouseTile", 1, "nighthouse", "NightHouse");


            for (int i = 0; i < locations.Length; i++)
            {
                locations[i].locationIndex = i;
                MenuEntry entry = new MenuEntry("");
                entry.Selected += new EventHandler<EventArgs>(entry_Selected);
                entry.Location = locations[i].mapIconRectangle;
                MenuEntries.Add(entry);
                iconEntries[i] = entry;
            }


            // add in pawns for the active players
            for (int i = 0; i < BubbleGame.players.Length; i++)
            {
                PlayerSprite player = BubbleGame.players[i];
                if (player.IsActive)
                {
                    AddPawn(player);
                }
            }
        }

        public void AddPawn(PlayerSprite player)
        {
            int index = pawns.Count;
            pawns.Add(new Pawn(player, new Vector2(-500, -500)));
        }


        public void RemovePawn(PlayerSprite player)
        {
            string playerString;
            if (player is SealPlayer)
                playerString = "Seal";
            else if (player is ToadPlayer)
                playerString = "Toad";
            else if (player is TortoisePlayer)
                playerString = "Tortoise";
            else
                playerString = "Penguin";

            for (int i = 0; i < pawns.Count; i++)
            {
                if (pawns[i].playerType == playerString)
                {
                    pawns.Remove(pawns[i]);
                    break;
                }
            }

        }


        void entry_Selected(object sender, EventArgs e)
        {
            MusicManager.SingletonMusicManager.StopAll();

            // discover which button we selected
            int selected = 0;
            for (selected = 0; selected < MenuEntries.Count; selected++)
            {
                if (sender.Equals(iconEntries[selected]))
                    break;
            }

            if (selected > currentLevelSetOpened)
                return;
            else
                currentLocation = locations[selected];

            // give inactive players a continue
            for (int i = 0; i < BubbleGame.players.Length; i++)
            {
                PlayerSprite player = BubbleGame.players[i];
                if (!player.IsActive && (player is TortoisePlayer || player is PenguinPlayer || player is SealPlayer || player is ToadPlayer))
                {
                    player.Activate();
                }
                else if ((player is TortoisePlayer || player is PenguinPlayer || player is SealPlayer || player is ToadPlayer))
                {
                    if (player.Lives < 3)
                    {
                        player.Lives = 3;
                    }
                }
                else
                {
                    player.Continues = 1;
                }
            }

            // load and start game play screen
            MusicManager.SingletonMusicManager.PlayTune(currentLocation.musicName);
            GameScreen screen = new GameplayScreen(BubbleGame.levelManager.GetLevel(currentLocation.startLevel), BubbleGame.players);
            BubbleGame.screenManager.AddScreen(screen);
            GameScreen cutScene = CutSceneScreen.GetCutScene(currentLocation.cutSceneName);
            if (cutScene != null)
            {
                BubbleGame.screenManager.AddScreen(cutScene);
            }
        }


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // check to make sure players are still playing
            bool stillPlaying = false;
            for (int i = 0; i < BubbleGame.players.Length; i++)
            {
                if (BubbleGame.players[i].HasSelected == true)
                    stillPlaying = true;
            }
            if (stillPlaying == false)
                return;

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            // draw the background
            Texture2D blank = InternalContentManager.GetTexture("Blank");
            spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 480), new Color(154, 189,174));
            spriteBatch.Draw(ZooMap, new Vector2(90, 0), Color.White);

            for (int i = currentLevelSetOpened + 1; i < locations.Length; i++)
            {
                spriteBatch.Draw(Lock, locations[i].mapIconRectangle, Color.White);
            }

            // draw pawns
            for (int i = 0; i < pawns.Count; i++)
            {
                pawns[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // check if our drive failed and the user wants to quit
            if (SaveGameManager.SingletonSaveManager.CheckDriveFailureExit())
                ExitScreen();

            // check to make sure players are still playing
            bool stillPlaying = false;
            for (int i = 0; i < BubbleGame.players.Length; i++)
            {
                if (BubbleGame.players[i].HasSelected == true)
                    stillPlaying = true;
            }
            if (stillPlaying == false)
                ExitScreen();

            if (otherScreenHasFocus == false && coveredByOtherScreen == false)
            {
                // update pawns
                for (int i = 0; i < pawns.Count; i++)
                {
                    pawns[i].Update(gameTime);
                }

                previouslySelectedEntry = selectedEntry;
            }
        }

        public bool OpenNextLocation()
        {
            // check for game over victory
            if (currentLocation.Equals(locations[9]))
            {
                ScreenManager.RemoveScreen(this);
                return true;
            }

            // if they played the highest level opened then open the next one
            if (currentLocation.Equals(locations[currentLevelSetOpened]))
            {
                // don't open it if we are in trial mode passed the set end leve
                if (BubbleGame.IsTrialModeCached && currentLevelSetOpened == 1)
                {
                    return false;
                }

                currentLevelSetOpened++;
                SaveGameManager.CurrentOpenedGame.ReachedLocation = currentLevelSetOpened;
                SaveGameManager.SingletonSaveManager.WriteSaveFile();
                this.selectedEntry = currentLevelSetOpened;
            }
            return false;
        }

        public override void TopFullScreenAcquired()
        {
            base.TopFullScreenAcquired();

            if (!BubbleGame.players[0].IsActive)
            {
                ScreenManager.AddScreen(new HighScoresScreen());
            }

            // setup pawn movement animation
            for (int i = 0; i < pawns.Count; i++)
            {
                if (currentLevelSetOpened != 0)
                {
                    pawns[i].SetClearPosition(locations[currentLevelSetOpened - 1].mapLocation);
                }
                else
                {
                    pawns[i].SetClearPosition(new Vector2(locations[0].mapLocation.X, locations[0].mapLocation.Y + 200));
                }

                pawns[i].SetBasicDestination(locations[currentLevelSetOpened].mapLocation);
            }

            // check if any players are active
            bool anyactive = false;
            for (int i = 0; i < BubbleGame.players.Length; i++)
            {
                if (BubbleGame.players[i].HasSelected == true)
                {
                    anyactive = true;
                    break;
                }
            }

            MusicManager.SingletonMusicManager.StopAll();

            if (anyactive)
            {
                MusicManager.SingletonMusicManager.PlayTune("levelselect");
            }
        }

    }
}