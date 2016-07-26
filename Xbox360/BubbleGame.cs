using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BubbleGame : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static ScreenManager screenManager;
        public static PlayerSprite[] players = new PlayerSprite[4];
        public static LevelManager levelManager;
        public static Game sigletonGame;
        public static int masterController = 0;
        public static bool isWindowsTrial = false;
        public static Random rand = new Random(DateTime.Now.Millisecond);
        SaveGameManager saveGameManager;
        public static RumbleManager rumbleManager;

        public BubbleGame()
        {
            sigletonGame = this;
            graphics = new GraphicsDeviceManager(this);
            saveGameManager = new SaveGameManager();
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

#if !XBOX
            graphics.IsFullScreen = true;
            isWindowsTrial = true;

#endif

            Content.RootDirectory = "Content";

            //Guide.SimulateTrialMode = true;
           
#if XBOX
            // add a gamer-services component, which is required for the storage APIs
            Components.Add(new GamerServicesComponent(this));
#endif
            
            // add the audio manager
            AudioManager.Initialize(this, @"Content\Audio\GumzooboAudio.xgs",
                @"Content\Audio\GumzooboWB.xwb", @"Content\Audio\GumzooboSB.xsb");

            new MusicManager();

            // add the screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // add the rumble manager
            rumbleManager = new RumbleManager(500, .5f, .5f);

            GameSprite.game = this;
            Level.screenManager = screenManager;

            levelManager = new LevelManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InputManager.Initialize();
            InternalContentManager.Load();

            base.Initialize();

            // create your players (will be replaced by specific characters when added to game)
            players[0] = new PlayerSprite(0);
            players[1] = new PlayerSprite(1);
            players[2] = new PlayerSprite(2);
            players[3] = new PlayerSprite(3);

            GameScreen screen = new OpeningScreen();
            screenManager.AddScreen(screen);
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Fonts.LoadContent(Content);

            // load the static data for the weapons
            BubbleWeapon.LoadStatic();
            CinnemonWeapon.LoadStatic();
            MintWeapon.LoadStatic();
            ABCWeapon.LoadStatic();
            GrapeWeapon.LoadStatic();
            SuperBubbleWeapon.LoadStatic();


            base.LoadContent();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Fonts.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            MusicManager.SingletonMusicManager.Update(gameTime);

            rumbleManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        static public bool IsCharacterTaken(int index)
        {
            if (index == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (players[i] is SealPlayer)
                        return true;
                }
            }
            if (index == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (players[i] is TortoisePlayer)
                        return true;
                }
            }
            if (index == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (players[i] is ToadPlayer)
                        return true;
                }
            }
            if (index == 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (players[i] is PenguinPlayer)
                        return true;
                }
            }

            return false;
        }


        public static PlayerIndex IntToPI(int controller)
        {
            if (controller == 0)
                return PlayerIndex.One;
            else if (controller == 1)
                return PlayerIndex.Two;
            else if (controller == 2)
                return PlayerIndex.Three;
            else
                return PlayerIndex.Four;

        }

    }
}
