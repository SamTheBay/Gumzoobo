using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Phone.Info;
using Microsoft.Advertising.Mobile.Xna;
using System.Threading;

namespace BubbleGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BubbleGame : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static ScreenManager screenManager;
        public static AdControlManager adControlManager;
        
        public static PlayerSprite[] players = new PlayerSprite[4];
        public static LevelManager levelManager;
        public static Game sigletonGame;
        public static int masterController = 0;
        public static bool isWindowsTrial = false;
        SaveGameManager saveGameManager;
        public static RumbleManager rumbleManager;
        public static LocalHighScores localHighScores;

        public static Rectangle ScreenSize = new Rectangle(0, 0, 800, 480);
        private static Vector2 gameTimeDrawLoc = new Vector2(100, 100);
        private static Vector2 maxGameTimeDrawLoc = new Vector2(100, 130);
        private static Vector2 memoryDrawLoc = new Vector2(100, 160);
        public static Random rand = new Random(DateTime.Now.Millisecond);
        public static bool IsTrialModeCached = false;

        public BubbleGame()
        {
            sigletonGame = this;
            graphics = new GraphicsDeviceManager(this);
            saveGameManager = new SaveGameManager();

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Pre-autoscale settings.
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = true;  

            Content.RootDirectory = "Content";

            Accelerometer.Initialize();

            //Guide.SimulateTrialMode = true;
           
            // add a gamer-services component, which is required for the storage APIs
            //Components.Add(new GamerServicesComponent(this));

            // add the screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // add the rumble manager
            rumbleManager = new RumbleManager(500, .5f, .5f);

            GameSprite.game = this;
            Level.screenManager = screenManager;

            levelManager = new LevelManager();

            localHighScores = new LocalHighScores();
            localHighScores.Load();

            IsTrialModeCached = Guide.IsTrialMode;

            adControlManager = new AdControlManager(Components, false);
            adControlManager.ShowAds = false;

            Activated += new EventHandler<EventArgs>(BubbleGameOnActivated);
            Deactivated += new EventHandler<EventArgs>(BubbleGameDeactivated);
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

            base.Initialize();

            GameScreen screen = new InitialLoadScreen();
            screenManager.AddScreen(screen);
            
        }


        void BubbleGameOnActivated(object sender, EventArgs args)
        {
            if (adControlManager != null)
            {
                adControlManager.Load();
            }

            // check if we have a game currently running
            if (screenManager.GetScreens()[screenManager.GetScreens().Length - 1] is GameplayScreen)
            {
                screenManager.AddScreen(new PauseScreen(players[0]));
            }
        }

        void BubbleGameDeactivated(object sender, EventArgs e)
        {
            if (adControlManager != null)
            {
                adControlManager.UnLoad();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            adControlManager.Load();
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

            rumbleManager.Update(gameTime);

            base.Update(gameTime);

            adControlManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            //long memory = GC.GetTotalMemory(false);
            //Debug.WriteLine(GC.GetTotalMemory(false).ToString());

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
