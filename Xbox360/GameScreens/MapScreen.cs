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
using Microsoft.Xna.Framework.Storage;

#if XBOX
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
#endif

namespace BubbleGame
{
    class MapScreen : MenuScreen
    {
        LocationDescriptor[] locations = new LocationDescriptor[10];
        int currentLevelSetOpened;
        MenuEntry[] dormantEntries = new MenuEntry[10];
        Texture2D MapButton;
        Texture2D ZooMap;
        List<Pawn> pawns = new List<Pawn>();
        int previouslySelectedEntry;
        PointMap pointMap = new PointMap();
        List<MPoint> points = new List<MPoint>();
        List<Connection> connections = new List<Connection>();
        bool pathSet = true;

        // static game state members
        static public LocationDescriptor currentLocation;
        static public MapScreen singletonMapSreen;

        public MapScreen(int currentLevelSetOpened)
            : base()
        {
#if XBOX
            if (Guide.IsTrialMode && currentLevelSetOpened > 1)
            {
                currentLevelSetOpened = 1;
            }
#else
            if (BubbleGame.isWindowsTrial && currentLevelSetOpened > 1)
            {
                currentLevelSetOpened = 1;
            }
#endif

            //currentLevelSetOpened = 9; // open levels hack // todo remove!
            singletonMapSreen = this;
            previouslySelectedEntry = selectedEntry;
            this.currentLevelSetOpened = currentLevelSetOpened;
            MapButton = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "buttonMap"));
            ZooMap = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "zoo_map"));

            // create the location descriptors
            locations[0] = new LocationDescriptor("Petting Zoo", new Vector2(350 + 150, 80), 0, 9, "petting_zoo_bg", "../Clear_Sprite", "PettingZooTile", 1, "pettingzoo", "PettingZoo");
            locations[1] = new LocationDescriptor("Gift Shop", new Vector2(350 + 440, 60), 10, 19, "gift_shop_bg", "gift_shop_foreg", "GiftShopTile", 1, "generic", "GiftShop");
            locations[2] = new LocationDescriptor("Jungle", new Vector2(350 + 760, 130), 20, 29, "jungle_bg", "jungle_foreg", "JungleTile", 1, "generic2", "Jungle");
            locations[3] = new LocationDescriptor("Sahara", new Vector2(350 + 640, 280), 30, 39, "sahara_bg", "sahara_foreg", "SaharaTile", 1, "desert", "Sahara");
            locations[4] = new LocationDescriptor("Aviary", new Vector2(350 + 460, 510), 40, 49, "aviary_bg", "../Clear_Sprite", "AviaryTile", 1, "generic", "Aviary");
            locations[5] = new LocationDescriptor("Swamp", new Vector2(350 + 815, 380), 50, 59, "swamp_bg", "../Clear_Sprite", "SwampTile", 1, "swamp", "Swamp");
            locations[6] = new LocationDescriptor("Polar Bear Cage", new Vector2(350 + 720, 630), 60, 69, "polar_bear_bg", "polar_bear_foreg", "PolarTile", 1, "generic2", "PolarBear");
            locations[7] = new LocationDescriptor("Monkey House", new Vector2(350 + 290, 350), 70, 79, "ape_house_bg", "ape_house_foreg", "SaharaTile", 1, "generic", "MonkeyHouse");
            locations[8] = new LocationDescriptor("Aquarium", new Vector2(350 + 185, 660), 80, 89, "aquarium_bg", "aquarium_foreg", "PolarTile", 1, "aquarium", "Aquarium");
            locations[9] = new LocationDescriptor("Night House", new Vector2(350 + 165, 430), 90, 99, "night_house_bg", "../Clear_Sprite", "NightHouseTile", 1, "nighthouse", "NightHouse");

            // setup point map for path finding with pawns
            points.Add(new MPoint(new Vector2(350 + 0, 0))); // 0
            points.Add(new MPoint(new Vector2(350 + 150, 275))); // 1
            points.Add(new MPoint(new Vector2(350 + 365, 275))); // 2
            points.Add(new MPoint(new Vector2(350 + 365, 130))); // 3
            points.Add(new MPoint(new Vector2(350 + 550, 130))); // 4
            points.Add(new MPoint(new Vector2(350 + 550, 280))); // 5
            points.Add(new MPoint(new Vector2(350 + 555, 355))); // 6
            points.Add(new MPoint(new Vector2(350 + 590, 385))); // 7
            points.Add(new MPoint(new Vector2(350 + 700, 490))); // 8
            points.Add(new MPoint(new Vector2(350 + 720, 515))); // 9
            points.Add(new MPoint(new Vector2(350 + 670, 570))); // 10
            points.Add(new MPoint(new Vector2(350 + 585, 640))); // 11
            points.Add(new MPoint(new Vector2(350 + 345, 640))); // 12
            points.Add(new MPoint(new Vector2(350 + 345, 545))); // 13
            points.Add(new MPoint(new Vector2(350 + 295, 545))); // 14
            points.Add(new MPoint(new Vector2(350 + 180, 545))); // 15
            points.Add(new MPoint(new Vector2(350 + 90, 545 ))); // 16
            points.Add(new MPoint(new Vector2(350 + 85, 385))); // 17
            points.Add(new MPoint(new Vector2(350 + 90, 280))); // 18
            points.Add(new MPoint(new Vector2(350 + 150, 80), "Petting Zoo")); // 19
            points.Add(new MPoint(new Vector2(350 + 440, 60), "Gift Shop"));
            points.Add(new MPoint(new Vector2(350 + 760, 130), "Jungle"));
            points.Add(new MPoint(new Vector2(350 + 640, 280), "Sahara"));
            points.Add(new MPoint(new Vector2(350 + 460, 510), "Aviary"));
            points.Add(new MPoint(new Vector2(350 + 815, 380), "Swamp"));
            points.Add(new MPoint(new Vector2(350 + 720, 630), "Polar Bear Cage"));
            points.Add(new MPoint(new Vector2(350 + 290, 350), "Monkey House"));
            points.Add(new MPoint(new Vector2(350 + 185, 660), "Aquarium"));
            points.Add(new MPoint(new Vector2(350 + 165, 430), "Night House"));
            pointMap.SetPoints(points);

            pointMap.AddConnection(new Connection(points[19], points[1]));
            pointMap.AddConnection(new Connection(points[1], points[2]));
            pointMap.AddConnection(new Connection(points[2], points[3]));
            pointMap.AddConnection(new Connection(points[3], points[4]));
            pointMap.AddConnection(new Connection(points[4], points[5]));
            pointMap.AddConnection(new Connection(points[5], points[6]));
            pointMap.AddConnection(new Connection(points[6], points[7]));
            pointMap.AddConnection(new Connection(points[7], points[8]));
            pointMap.AddConnection(new Connection(points[8], points[9]));
            pointMap.AddConnection(new Connection(points[9], points[10]));
            pointMap.AddConnection(new Connection(points[10], points[11]));
            pointMap.AddConnection(new Connection(points[11], points[12]));
            pointMap.AddConnection(new Connection(points[12], points[13]));
            pointMap.AddConnection(new Connection(points[13], points[14]));
            pointMap.AddConnection(new Connection(points[14], points[15]));
            pointMap.AddConnection(new Connection(points[15], points[16]));
            pointMap.AddConnection(new Connection(points[16], points[17]));
            pointMap.AddConnection(new Connection(points[17], points[18]));
            pointMap.AddConnection(new Connection(points[18], points[1]));
            pointMap.AddConnection(new Connection(points[3], points[20]));
            pointMap.AddConnection(new Connection(points[21], points[4]));
            pointMap.AddConnection(new Connection(points[22], points[5]));
            pointMap.AddConnection(new Connection(points[7], points[23]));
            pointMap.AddConnection(new Connection(points[8], points[24]));
            pointMap.AddConnection(new Connection(points[10], points[25]));
            pointMap.AddConnection(new Connection(points[26], points[14]));
            pointMap.AddConnection(new Connection(points[15], points[27]));
            pointMap.AddConnection(new Connection(points[17], points[28]));


            // add in the MenuEntries
            int leftside = 0;
            int height = 0;
            for (int i = 0; i < locations.Length; i++)
            {
                locations[i].locationIndex = i;
                MenuEntry entry = new MenuEntry(locations[i].name);
                entry.Selected += new EventHandler<EventArgs>(entry_Selected);
                entry.Position = new Vector2(leftside, height);
                entry.Font = Fonts.HeaderFont;
                entry.Texture = MapButton;
                dormantEntries[i] = entry;
                height += MapButton.Height;
            }

            for (int i = 0; i <= currentLevelSetOpened; i++)
            {
                MenuEntries.Add(dormantEntries[i]);
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
            pawns.Add(new Pawn(player, locations[selectedEntry].mapLocation));
            pawns[index].SetDestination(locations[selectedEntry].mapLocation, (selectedEntry + index) % 4);
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
                if (sender.Equals(MenuEntries[selected]))
                    break;
            }
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

            // start game play screen
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
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            // draw the background
            Texture2D blank = InternalContentManager.GetTexture("Blank");
            spriteBatch.Draw(blank, new Rectangle(0, 0, 350, 720), new Color(154, 189,174));
            spriteBatch.Draw(ZooMap, new Vector2(350, 0), Color.White);

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
                // kick off new path find
                if (previouslySelectedEntry != selectedEntry)
                {
                    pathSet = false;
                    pointMap.RunPathFindingThread(pawns[0].CurrentPoint, pawns[0].CurrentConnection, pawns[0].IsAtPoint, locations[selectedEntry].name);
                }

                // update pawns path if it is ready
                if (pointMap.IsResultReady && pathSet == false)
                {
                    for (int i = 0; i < pawns.Count; i++)
                    {
                        pawns[i].SetPath(pointMap.Result, i);
                    }
                    pathSet = true;
                }

                // update pawns
                for (int i = 0; i < pawns.Count; i++)
                {
                    pawns[i].Update(gameTime, (selectedEntry + i) % 4);
                }

                previouslySelectedEntry = selectedEntry;
            }
        }

        public void OpenNextLocation()
        {
            // check for game over victory
            if (currentLocation.Equals(locations[9]))
            {
                ScreenManager.AddScreen(new VictoryScreen());
                ExitScreen();
                return;
            }

            // if they played the highest level opened then open the next one
            if (currentLocation.Equals(locations[currentLevelSetOpened]))
            {
                // don't open it if we are in trial mode passed the set end leve
#if XBOX
                if (Guide.IsTrialMode && currentLevelSetOpened == 1)
#else
                if (BubbleGame.isWindowsTrial && currentLevelSetOpened == 1)
#endif
                {
                    return;
                }

                currentLevelSetOpened++;
#if XBOX
                SaveGameManager.CurrentOpenedGame.ReachedLocation = currentLevelSetOpened;
                SaveGameManager.SingletonSaveManager.WriteSaveFile();
#endif
                MenuEntries.Add(dormantEntries[currentLevelSetOpened]);
                this.selectedEntry = currentLevelSetOpened;
            }
        }

        public override void TopFullScreenAcquired()
        {
            base.TopFullScreenAcquired();

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