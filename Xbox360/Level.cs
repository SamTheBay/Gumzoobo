using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class Level
    {
        // Sprites
        List<GameSprite> levelSprites = new List<GameSprite>();
        List<EnvironmentSprite> envSprites = new List<EnvironmentSprite>();
        List<Enemy> enemySprites = new List<Enemy>();
        List<GameSprite> bubbleSprites = new List<GameSprite>();
        List<GameSprite> superBubbleSprites = new List<GameSprite>();
        List<ItemSprite> itemSprites = new List<ItemSprite>();
        List<PlayerSprite> playerSprites = new List<PlayerSprite>();
        List<WeaponSprite> enemyWeaponSprites = new List<WeaponSprite>();
        List<WeaponSprite> playerWeaponSprites = new List<WeaponSprite>();

        // Contents
        Texture2D background;
        Texture2D foreground;

        // static accessors
        static public ScreenManager screenManager;
        static public Level singletonLevel;

        // timers
        int timeSinceLastGeneration = 0;
        int timeSinceLastRand = 0;
        int randomGenerationMinimum = 20000;
        int randomGenerationAttemptInt = 1000;

        // initial level setup
        int levelNum;
        string[] levellayout;
        LevelDetails levelDetails;
        Vector2[] playerStartPos = new Vector2[4];
        public bool hasShownHelp = false;
        bool hasStartedHelp = false;
        public bool hasShownTrial = false;
        bool hasStartedTrial = false;
        List<Vector2> OpenSpaces;

        // state
        bool isComplete = false;
        bool isFailed = false;
        int completeDuration = 4000;
        int completeElapsed = 0;
        int failedDuration = 4000;
        int failedElapsed = 0;
        bool isStarting = true;
        int startingDuration = 2000;
        int startingElapsed = 0;
        protected bool isTimeStill = false;
        protected int timeStillDuration = 10000;
        protected int timeStillElapsed = 10000;

        // night house times
        int lightTime = 5000;
        int transToDark = 7000;
        int darkTime = 14000;
        int transToLight = 16000;
        int currentDarkTime = 0;

        Effect SeeThroughForeg;
        Effect Darkener;

        // threaded update variables
        int cpuNum = 3;
        Thread[] updateThreads;
        Int32 currentUpdateIndex = 0;
        volatile bool[] threadsRunning;
        GameTime currentGameTime;

        // dimensions
        Rectangle backgroundSize = new Rectangle(208, 0, 864, 720);


        public Level(string[] levellayout, int levelNum, LevelDetails levelDetails)
        {
            this.levellayout = levellayout;
            this.levelNum = levelNum;
            this.levelDetails = levelDetails;
            SeeThroughForeg = BubbleGame.sigletonGame.Content.Load<Effect>("ForegroundSeeThrough");
            Darkener = BubbleGame.sigletonGame.Content.Load<Effect>("Darkener");
            SeeThroughForeg.Parameters["textureW"].SetValue(864f);
            SeeThroughForeg.Parameters["textureH"].SetValue(720f);
            
#if !XBOX
            cpuNum = Environment.ProcessorCount;
#endif

            updateThreads = new Thread[cpuNum];
            threadsRunning = new bool[cpuNum];

            for (int i = 0; i < updateThreads.Length; i++)
            {
                threadsRunning[i] = false;
                updateThreads[i] = new Thread(UpdateThreadProc);
                updateThreads[i].Start();
            }
        }


        public void Init()
        {

        }


        public void Load()
        {
            singletonLevel = this;
            OpenSpaces = new List<Vector2>();
            int xoffset = 208;

            for (int y = 0; y < 30; y++)
            {
                for (int x = 0; x < 36; x++)
                {
                    // check if this location is warpable
                    bool isWarpable = true;
                    for (int x2 = x; x2 < x + 3; x2++)
                    {
                        for (int y2 = y; y2 < y + 3; y2++)
                        {
                            if (y2 >= 30 || x2 >= 36 || levellayout[y2].ToCharArray()[x2] != '-')
                            {
                                isWarpable = false;
                                break;
                            }
                        }
                        if (y+3 >= 30 || x2 >= 36 || levellayout[y+3].ToCharArray()[x2] != 'X')
                        {
                            isWarpable = false;
                            break;
                        }
                    }
                    if (isWarpable)
                    {
                        OpenSpaces.Add(new Vector2(x * 24 + xoffset, y * 24));
                    }

                    char placement = levellayout[y].ToCharArray()[x];
                    if (placement == 'X')
                    {
                        AddSprite(new EnvironmentSprite(MapScreen.currentLocation.envTextureFrame, new Vector2(x * 24 + xoffset, y * 24)));
                    }
                    else if (placement == 'D' || placement == 'd')
                    {
                        Drone drone;
                        if (placement == 'D')
                            drone = new Drone(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            drone = new Drone(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        drone.Activate();
                        AddSprite(drone);
                    }
                    else if (placement == 'L' || placement == 'l')
                    {
                        LazerBot lazerBot;
                        if (placement == 'L')
                            lazerBot = new LazerBot(new Vector2(x * 24 + xoffset, y * 24 - 20), Direction.Right);
                        else
                            lazerBot = new LazerBot(new Vector2(x * 24 + xoffset, y * 24 - 20), Direction.Left);
                        lazerBot.Activate();
                        AddSprite(lazerBot);
                    }
                    else if (placement == 'B' || placement == 'b')
                    {
                        Bouncer bouncer;
                        if (placement == 'B')
                            bouncer = new Bouncer(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            bouncer = new Bouncer(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        bouncer.Activate();
                        AddSprite(bouncer);
                    }
                    else if (placement == 'I' || placement == 'i')
                    {
                        InvisiBot invisibot;
                        if (placement == 'I')
                            invisibot = new InvisiBot(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            invisibot = new InvisiBot(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        invisibot.Activate();
                        AddSprite(invisibot);
                    }
                    else if (placement == 'S' || placement == 's')
                    {
                        Spikey spikey;
                        if (placement == 'S')
                            spikey = new Spikey(new Vector2(x * 24 + xoffset, y * 24 - 20), Direction.Right);
                        else
                            spikey = new Spikey(new Vector2(x * 24 + xoffset, y * 24 - 20), Direction.Left);
                        spikey.Activate();
                        AddSprite(spikey);
                    }
                    else if (placement == 'R' || placement == 'r')
                    {
                        RocketPacker rocketpacker;
                        if (placement == 'R')
                            rocketpacker = new RocketPacker(new Vector2(x * 24 + xoffset, y * 24), Direction.RightUp);
                        else
                            rocketpacker = new RocketPacker(new Vector2(x * 24 + xoffset, y * 24), Direction.LeftUp);
                        rocketpacker.Activate();
                        AddSprite(rocketpacker);
                    }
                    else if (placement == 'H' || placement == 'h')
                    {
                        Hunter hunter;
                        if (placement == 'H')
                            hunter = new Hunter(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            hunter = new Hunter(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        hunter.Activate();
                        AddSprite(hunter);
                    }
                    else if (placement == 'P' || placement == 'p')
                    {
                        SuperBouncer superBouncer;
                        if (placement == 'P')
                            superBouncer = new SuperBouncer(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            superBouncer = new SuperBouncer(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        superBouncer.Activate();
                        AddSprite(superBouncer);
                    }
                    else if (placement == 'T' || placement == 't')
                    {
                        RocketBlaster rocketBlaster;
                        if (placement == 'T')
                            rocketBlaster = new RocketBlaster(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            rocketBlaster = new RocketBlaster(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        rocketBlaster.Activate();
                        AddSprite(rocketBlaster);
                    }
                    else if (placement == 'W' || placement == 'w')
                    {
                        WarpBot warpBot;
                        if (placement == 'W')
                            warpBot = new WarpBot(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            warpBot = new WarpBot(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        warpBot.Activate();
                        AddSprite(warpBot);
                    }
                    else if (placement == 'O' || placement == 'o')
                    {
                        FireDrone fireDrone;
                        if (placement == 'O')
                            fireDrone = new FireDrone(new Vector2(x * 24 + xoffset, y * 24), Direction.Right);
                        else
                            fireDrone = new FireDrone(new Vector2(x * 24 + xoffset, y * 24), Direction.Left);
                        fireDrone.Activate();
                        AddSprite(fireDrone);
                    }
                    else if (placement == '1')
                    {
                        playerStartPos[0] = new Vector2(x * 24 + xoffset, y * 24 - 20);
                    }
                    else if (placement == '2')
                    {
                        playerStartPos[1] = new Vector2(x * 24 + xoffset, y * 24 - 20);
                    }
                    else if (placement == '3')
                    {
                        playerStartPos[2] = new Vector2(x * 24 + xoffset, y * 24 - 20);
                    }
                    else if (placement == '4')
                    {
                        playerStartPos[3] = new Vector2(x * 24 + xoffset, y * 24 - 20);
                    }
                    else if (placement == 'm')
                    {
                        AddSprite(new AmmoItemSprite(new Vector2(x * 24 + xoffset, y * 24), Weapon.Mint, 60000));
                    }
                    else if (placement == 'c')
                    {
                        AddSprite(new AmmoItemSprite(new Vector2(x * 24 + xoffset, y * 24), Weapon.Cinnemon, 60000));
                    }
                    else if (placement == 'g')
                    {
                        AddSprite(new AmmoItemSprite(new Vector2(x * 24 + xoffset, y * 24), Weapon.Grape, 60000));
                    }
                    else if (placement == 'u')
                    {
                        AddSprite(new AmmoItemSprite(new Vector2(x * 24 + xoffset, y * 24), Weapon.Super, 60000));
                    }
                    else if (placement == 'a')
                    {
                        AddSprite(new AmmoItemSprite(new Vector2(x * 24 + xoffset, y * 24), Weapon.ABC, 60000));
                    }
                    else if (placement == 'f')
                    {
                        AddSprite(new SpeedShoes(new Vector2(x * 24 + xoffset, y * 24)));
                    }
                    else if (placement == 'n')
                    {
                        AddSprite(new RainbowGum(new Vector2(x * 24 + xoffset, y * 24)));
                    }
                    else if (placement == 'j')
                    {
                        AddSprite(new JewelNecklace(new Vector2(x * 24 + xoffset, y * 24)));
                    }
                    else if (placement == 'k')
                    {
                        AddSprite(new Clock(new Vector2(x * 24 + xoffset, y * 24)));
                    }
                    else if (placement == 'y')
                    {
                        AddSprite(new CrystalBall(new Vector2(x * 24 + xoffset, y * 24)));
                    }

                }
            }

            background = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/Backgrounds", MapScreen.currentLocation.backgroundTexture));
            foreground = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/Backgrounds", MapScreen.currentLocation.foregroundTexture));

            GC.Collect();
        }

        public void Unload()
        {
            // release all the levels resources
            for (int i = 0; i < updateThreads.Length; i++)
            {
                updateThreads[i].Abort();
            }
            for (int i = 0; i < playerWeaponSprites.Count; i++)
            {
                playerWeaponSprites[i].Deactivate();
            }
        }

        public void Update(GameTime gameTime)
        {

            // check if this level is complete
            if (isComplete == false)
            {
                bool isAnyActive = false;
                for (int i = 0; i < enemySprites.Count; i++)
                {
                    if (enemySprites[i].IsActive == true)
                    {
                        isAnyActive = true;
                        break;
                    }
                }
                if (isAnyActive == false)
                {
                    isComplete = true;
                }

                // Check if all the players are dead
                if (isComplete == false)
                {
                    isAnyActive = false;
                    for (int i = 0; i < playerSprites.Count; i++)
                    {
                        if (playerSprites[i].IsActive)
                        {
                            isAnyActive = true;
                            break;
                        }
                    }

                    if (isAnyActive == false && !isFailed)
                    {
                        FailLevel();
                    }

                }
            }
            
            // check for windows that we should show
            if (hasStartedHelp == false)
            {
                ShowHelp();
                hasStartedHelp = true;
            }
            if (hasShownHelp == false)
            {
                return;
            }
#if XBOX
            if (Guide.IsTrialMode)
#else
            if (BubbleGame.isWindowsTrial)
#endif
            {
                if (hasStartedTrial == false)
                {
                    ShowTrial();
                    hasStartedTrial = true;
                }
                if (hasShownTrial == false)
                {
                    return;
                }
            }

            // handle start and end transition timers
            if (isStarting)
            {
                startingElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (startingElapsed > startingDuration)
                {
                    isStarting = false;
                }
                else
                {
                    return;
                }
            }
            else if (isComplete)
            {
                completeElapsed += gameTime.ElapsedGameTime.Milliseconds;

                // check if no players are in the game
                bool isAnyActive = false;
                for (int i = 0; i < playerSprites.Count; i++)
                {
                    if (playerSprites[i].IsActive)
                    {
                        isAnyActive = true;
                        break;
                    }
                }

                if (isAnyActive == false && !isFailed)
                {
                    FailLevel();
                }
            }

            if (isFailed)
            {
                failedElapsed += gameTime.ElapsedGameTime.Milliseconds;
            }

            // generate random item drops
            RunGeneration(gameTime);

            // update time still timer
            if (isTimeStill == true)
            {
                timeStillElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (timeStillElapsed >= timeStillDuration)
                {
                    EndTimeStill();
                }
            }


            // Check for player collisions
            for (int i = 0; i < playerSprites.Count; i++)
            {
                if (!playerSprites[i].IsActive || playerSprites[i].IsDead)
                {
                    continue;
                }
                // check for collisions between enemy weapons and players
                for (int j = 0; j < enemyWeaponSprites.Count; j++)
                {
                    if (!playerSprites[i].IsActive || playerSprites[i].IsDead)
                    {
                        break; // if the last collision made us inactive then stop
                    }
                    else if (enemyWeaponSprites[j].IsActive)
                    {
                        if (playerSprites[i].CollisionDetect(enemyWeaponSprites[j]))
                        {
                            enemyWeaponSprites[j].CollisionAction(playerSprites[i]);
                            playerSprites[i].CollisionAction(enemyWeaponSprites[j]);
                        }
                    }
                }
                // check for collisions between player weapons and players
                for (int j = 0; j < playerWeaponSprites.Count; j++)
                {
                    if (!playerSprites[i].IsActive || playerSprites[i].IsDead)
                    {
                        break; // if the last collision made us inactive then stop
                    }
                    else
                    {
                        if (!playerWeaponSprites[j].IsActive)
                        {
                            continue;
                        }
                        if (playerWeaponSprites[j] is CinnemonWeapon && playerWeaponSprites[j].IsLethal)
                        {
                            continue; // don't collision with exploding cinnemon gum (expensive and useless)
                        }
                        if (playerSprites[i].CollisionDetect(playerWeaponSprites[j]))
                        {
                            playerWeaponSprites[j].CollisionAction(playerSprites[i]);
                            playerSprites[i].CollisionAction(playerWeaponSprites[j]);
                        }
                    }
                }
                // check for collisions between enemies and players
                for (int j = 0; j < enemySprites.Count; j++)
                {
                    if (!playerSprites[i].IsActive || playerSprites[i].IsDead)
                    {
                        break; // if the last collision made us inactive then stop
                    }
                    else if (enemySprites[j].IsActive && !enemySprites[j].IsDead)
                    {
                        if (playerSprites[i].CollisionDetect(enemySprites[j]))
                        {
                            enemySprites[j].CollisionAction(playerSprites[i]);
                            playerSprites[i].CollisionAction(enemySprites[j]);
                        }
                    }
                }
                // check for collisions betwwen players and items
                for (int j = 0; j < itemSprites.Count; j++)
                {
                    if (!playerSprites[i].IsActive || playerSprites[i].IsDead)
                    {
                        break; // if the last collision made us inactive then stop
                    }
                    else if (null != itemSprites[j] && itemSprites[j].IsActive)
                    {
                        if (playerSprites[i].CollisionDetect(itemSprites[j]))
                        {
                            playerSprites[i].CollisionAction(itemSprites[j]);
                            itemSprites[j].CollisionAction(playerSprites[i]);
                        }
                    }
                }

            }

            // check for collisions between player weapons and enemies
            for (int i = 0; i < enemySprites.Count; i++)
            {
                if (!enemySprites[i].IsActive || enemySprites[i].IsDead)
                {
                    continue;
                }
                for (int j = 0; j < playerWeaponSprites.Count; j++)
                {
                    if (!enemySprites[i].IsActive || enemySprites[i].IsDead)
                    {
                        break; // if the last collision made us inactive then stop
                    }
                    if ((enemySprites[i].Stuck && !(playerWeaponSprites[j] is CinnemonWeapon)))
                    {
                        continue;
                    }
                    if (playerWeaponSprites[j].IsActive)
                    {
                        if (enemySprites[i].CollisionDetect(playerWeaponSprites[j]))
                        {
                            playerWeaponSprites[j].CollisionAction(enemySprites[i]);
                            enemySprites[i].CollisionAction(playerWeaponSprites[j]);
                        }
                    }
                }
            }


            // start multi threaded update
            currentUpdateIndex = 0;
            currentGameTime = gameTime;
            for (int i = 0; i < updateThreads.Length; i++)
            {
                threadsRunning[i] = true;
            }

            WaitForThreadsToFinish();

        }


        public void WaitForThreadsToFinish()
        {
            // wait for threads to finish
            bool loop = true;
            while (loop)
            {
                loop = false;
                for (int i = 0; i < updateThreads.Length; i++)
                {
                    if (threadsRunning[i] == true)
                    {
                        loop = true;
                        break;
                    }
                    if (loop == true)
                        Thread.Sleep(1);
                }
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // start darkener for night house
            if (MapScreen.currentLocation.locationIndex == 9)
            {
                Darkener.Begin();
                Darkener.CurrentTechnique.Passes[0].Begin();

                float darkFactor = 0f;
                if (!PauseScreen.isPaused)
                {
                    currentDarkTime += gameTime.ElapsedGameTime.Milliseconds;
                }
                if (currentDarkTime <= lightTime)
                {
                    darkFactor = 0f;
                }
                else if (currentDarkTime <= transToDark)
                {
                    darkFactor = ((float)currentDarkTime - (float)lightTime) / ((float)transToDark - (float)lightTime);
                }
                else if (currentDarkTime <= darkTime)
                {
                    darkFactor = 1f;
                }
                else if (currentDarkTime <= transToLight)
                {
                    darkFactor = 1f - (((float)currentDarkTime - (float)darkTime) / ((float)transToLight - (float)darkTime));
                }
                else
                {
                    darkFactor = 0f;
                    currentDarkTime = 0;
                }

                Darkener.Parameters["darkFactor"].SetValue(darkFactor);
            }

            // draw brackground
            spriteBatch.Draw(background, backgroundSize, Color.White);


            foreach (GameSprite sprite in envSprites)
            {
                if (sprite.IsActive)
                {
                    sprite.Draw(spriteBatch, 0f);
                }
            }

            // end darkener for nighthouse
            if (MapScreen.currentLocation.locationIndex == 9)
            {
                Darkener.CurrentTechnique.Passes[0].End();
                Darkener.End();
            }


            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // draw all active sprites
            foreach (GameSprite sprite in levelSprites)
            {
                if (sprite != null && sprite.IsActive)
                {
                    sprite.Draw(spriteBatch, 0f);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // Begin the shader for see through foreground
            SeeThroughForeg.Begin();
            SeeThroughForeg.CurrentTechnique.Passes[0].Begin();

            // set the location of the players for the shader
            int i = 0;
            for (i = 0; i < playerSprites.Count; i++)
            {
                if (playerSprites[i].IsActive)
                {
                    string variableName = "player" + (i + 1).ToString() + "x";
                    float value = playerSprites[i].position.X + (playerSprites[i].FrameDimensions.X / 2) - 208;
                    SeeThroughForeg.Parameters[variableName].SetValue(value);
                    variableName = "player" + (i + 1).ToString() + "y";
                    value = playerSprites[i].position.Y + (playerSprites[i].FrameDimensions.Y / 2);
                    SeeThroughForeg.Parameters[variableName].SetValue(value);
                }
                else
                {
                    SeeThroughForeg.Parameters["player" + (i + 1).ToString() + "x"].SetValue(2000);
                    SeeThroughForeg.Parameters["player" + (i + 1).ToString() + "y"].SetValue(1000);
                }
            }
            while (i < 4)
            {
                SeeThroughForeg.Parameters["player" + (i + 1).ToString() + "x"].SetValue(2000);
                SeeThroughForeg.Parameters["player" + (i + 1).ToString() + "y"].SetValue(1000);
                i++;
            }

            // draw foreground
            spriteBatch.Draw(foreground, backgroundSize, Color.White);

            // end the shader for see through foreground
            SeeThroughForeg.CurrentTechnique.Passes[0].End();
            SeeThroughForeg.End();

            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // draw foreground text
            if (isStarting && !isFailed)
            {
                Texture2D text;
                if (startingElapsed < 500)
                {
                    text = InternalContentManager.GetTexture("3");
                }
                else if (startingElapsed < 1000)
                {
                    text = InternalContentManager.GetTexture("2");
                }
                else if (startingElapsed < 1500)
                {
                    text = InternalContentManager.GetTexture("1");
                }
                else
                {
                    text = InternalContentManager.GetTexture("Go");
                }
                spriteBatch.Draw(text, new Vector2(640 - text.Width / 2, 360 - text.Height / 2), Color.White);
            }
            else if (isComplete && !isFailed)
            {
                Texture2D complete = InternalContentManager.GetTexture("Complete");
                spriteBatch.Draw(complete, new Vector2(640 - complete.Width / 2, 360 - complete.Height / 2), Color.White);
            }
            else if (isFailed)
            {
                Texture2D gameover = InternalContentManager.GetTexture("GameOver");
                spriteBatch.Draw(gameover, new Vector2(640 - gameover.Width / 2, 360 - gameover.Height / 2), Color.White);
            }
        }

        public void AddPlayer(PlayerSprite player)
        {
            player.Position = playerStartPos[player.PlayerIndex];
            player.StartPosition = playerStartPos[player.PlayerIndex];
            AddSprite(player);
        }


        public void AddSprite(GameSprite sprite)
        {
            if (!(sprite is EnvironmentSprite))
            {
                lock (levelSprites)
                {
                    levelSprites.Add(sprite);
                }
            }
            if (sprite is EnvironmentSprite)
            {
                envSprites.Add((EnvironmentSprite)sprite);
            }
            else if (sprite is Enemy)
            {
                enemySprites.Add((Enemy)sprite);

                // add enemies weapons into the level
                if (sprite is LazerBot)
                {
                    LazerBot bot = (LazerBot)sprite;
                    AddSprite(bot.lazer);
                }
                if (sprite is RocketBlaster)
                {
                    RocketBlaster bot = (RocketBlaster)sprite;
                    for (int i = 0; i < bot.fireballs.Length; i++)
                    {
                        AddSprite(bot.fireballs[i]);
                    }
                }
                if (sprite is FireDrone)
                {
                    FireDrone bot = (FireDrone)sprite;
                    AddSprite(bot.fireball);
                }
            }
            else if (sprite is ItemSprite)
            {
                lock (itemSprites)
                {
                    itemSprites.Add((ItemSprite)sprite);
                }
            }
            else if (sprite is PlayerSprite)
            {
                PlayerSprite player = (PlayerSprite)sprite;
                playerSprites.Add(player);

                // add players weapons into the level
                for (int i = 0; i < player.Bubbles.Length; i++)
                {
                    AddSprite(player.Bubbles[i]);
                }
                for (int i = 0; i < player.Cinnemon.Length; i++)
                {
                    AddSprite(player.Cinnemon[i]);
                }
                for (int i = 0; i < player.Mint.Length; i++)
                {
                    AddSprite(player.Mint[i]);
                }
                for (int i = 0; i < player.Grape.Length; i++)
                {
                    AddSprite(player.Grape[i]);
                }
                for (int i = 0; i < player.ABC.Length; i++)
                {
                    AddSprite(player.ABC[i]);
                }
                for (int i = 0; i < player.SuperBubble.Length; i++)
                {
                    AddSprite(player.SuperBubble[i]);
                }
            }
            else if (sprite is WeaponSprite)
            {
                WeaponSprite wep = (WeaponSprite)sprite;
                if (wep.IsPlayerGenerated)
                {
                    playerWeaponSprites.Add(wep);
                }
                else
                {
                    enemyWeaponSprites.Add(wep);
                }
            }

            if (sprite is BubbleWeapon || sprite is ABCWeapon || sprite is GrapeWeapon || sprite is MintWeapon || sprite is SuperBubbleWeapon)
            {
                bubbleSprites.Add(sprite);
            }
            if (sprite is SuperBubbleWeapon)
            {
                superBubbleSprites.Add(sprite);
            }
        }

        public void RemoveSprite(GameSprite sprite)
        {
            lock (levelSprites)
            {
                levelSprites.Remove(sprite);
                if (sprite is ItemSprite)
                {
                    itemSprites.Remove((ItemSprite)sprite);
                }
            }
        }

        public void StartTimeStill()
        {
            isTimeStill = true;
            timeStillElapsed = 0;
        }


        private void EndTimeStill()
        {
            isTimeStill = false;
        }

        public bool IsTimeStill
        {
            get { return isTimeStill; }
        }


        public void ShowHelp()
        {
            HelpScreen helpScreen;
            if (levelNum == 0)
            {
#if XBOX
                helpScreen = new HelpScreen("KillingRobots");
#else
                helpScreen = new HelpScreen("KillingRobotsWindows");
#endif
            }
            else if (levelNum == 1)
            {
                helpScreen = new HelpScreen("Fruit");
            }
            else if (levelNum == 2)
            {
                helpScreen = new HelpScreen("JumpingOnBubbles");
            }
            else if (levelNum == 3)
            {
                helpScreen = new HelpScreen("Gums");
            }
            else if (levelNum == 4)
            {
#if XBOX
                helpScreen = new HelpScreen("Chewing");
#else
                helpScreen = new HelpScreen("ChewingWindows");
#endif
            }
            else if (levelNum == 5)
            {
                helpScreen = new HelpScreen("SuperBubbles");
            }
            else if (levelNum == 6)
            {
                helpScreen = new HelpScreen("PowerUps");
            }
            else if (levelNum == 7)
            {
#if XBOX
                helpScreen = new HelpScreen("Abilities");
#else
                helpScreen = new HelpScreen("AbilitiesWindows");
#endif
            }
            else if (levelNum == 13)
            {
                helpScreen = new HelpScreen("JumpingTop");
            }
            else if (levelNum == 20)
            {
                helpScreen = new HelpScreen("Invisabot");
            }
            else if (levelNum == 30)
            {
                helpScreen = new HelpScreen("SpikeyBot");
            }
            else if (levelNum == 40)
            {
                helpScreen = new HelpScreen("HunterBot");
            }
            else if (levelNum == 70)
            {
                helpScreen = new HelpScreen("WarpBot");
            }
            else
            {
                hasShownHelp = true;
                return;
            }

            BubbleGame.screenManager.AddScreen(helpScreen);
        }


        public void ShowTrial()
        {
            if (levelNum % 3 == 1)
            {
                GameScreen trialModeScreen;
                if (BubbleGame.isWindowsTrial)
                    trialModeScreen = new WindowsTrialScreen();
                else
                    trialModeScreen = new TrialModeScreen();
                trialModeScreen.IsMasterControllerSensitive = true;
                BubbleGame.screenManager.AddScreen(trialModeScreen);
            }
            else
            {
                hasShownTrial = true;
            }
        }


        public void FailLevel()
        {
            isFailed = true;
            MusicManager.SingletonMusicManager.StopAll();
            AudioManager.PlayCue("GameOver");
        }


        public void RunGeneration(GameTime gameTime)
        {
            timeSinceLastGeneration += gameTime.ElapsedGameTime.Milliseconds;
            timeSinceLastRand += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastGeneration > randomGenerationMinimum && timeSinceLastRand > randomGenerationAttemptInt)
            {
                timeSinceLastRand = 0;
                int selection = BubbleGame.rand.Next(0, 1000);
                {
                    // random gum generations
                    if (selection >= 0 && selection < 50)
                    {
                        GenerateItem(new AmmoItemSprite(Vector2.Zero, Weapon.Mint));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 50 && selection < 80)
                    {
                        GenerateItem(new AmmoItemSprite(Vector2.Zero, Weapon.Cinnemon));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 80 && selection < 130)
                    {
                        GenerateItem(new AmmoItemSprite(Vector2.Zero, Weapon.ABC));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 130 && selection < 160)
                    {
                        GenerateItem(new AmmoItemSprite(Vector2.Zero, Weapon.Grape));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 160 && selection < 180)
                    {
                        GenerateItem(new AmmoItemSprite(Vector2.Zero, Weapon.Super));
                        timeSinceLastGeneration = 0;
                    }


                    // random special item generations
                    else if (selection >= 180 && selection < 200)
                    {
                        GenerateItem(new SpeedShoes(Vector2.Zero));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 200 && selection < 210)
                    {
                        GenerateItem(new Clock(Vector2.Zero));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 210 && selection < 220)
                    {
                        GenerateItem(new JewelNecklace(Vector2.Zero));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 220 && selection < 225)
                    {
                        GenerateItem(new CrystalBall(Vector2.Zero));
                        timeSinceLastGeneration = 0;
                    }
                    else if (selection >= 225 && selection < 240)
                    {
                        GenerateItem(new RainbowGum(Vector2.Zero));
                        timeSinceLastGeneration = 0;
                    }
                }
            }

        }



        public void GenerateItem(ItemSprite item)
        {
            // select a place on the screen for the item
            item.position = GetOpenLocation();
            bool isOpen = false;

            // move the position until we find a place that doesn't colide
            while (!isOpen)
            {
                isOpen = true;
                for (int i = 0; i < envSprites.Count; i++)
                {
                    if (envSprites[i].CollisionDetect(item))
                    {
                        isOpen = false;
                        item.position.X += 40;
                        item.position.Y += 40;
                        continue;
                    }
                }
            }

            // add the item to the mix
            AddSprite(item);

        }


        public void UpdateThreadProc()
        {
            try
            {
                int currentIndex = 0;
                int threadIndex = 0;

                // find our thread index
                for (int i = 0; i < updateThreads.Length; i++)
                {
                    if (Thread.CurrentThread.Equals(updateThreads[i]))
                    {
                        threadIndex = i;
                        break;
                    }
                }

#if XBOX
                // set what cpu to run on
                int cpuIndex = 0;
                if (threadIndex == 0)
                    cpuIndex = 3;
                else if (threadIndex == 1)
                    cpuIndex = 4;
                else
                    cpuIndex = 5;

                Thread.CurrentThread.SetProcessorAffinity(cpuIndex);
#endif


                while (true)
                {
                    if (threadsRunning[threadIndex] == true)
                    {
                        currentIndex = 0;
                        while (currentIndex < levelSprites.Count)
                        {
                            // get item to update
                            lock (currentGameTime)
                            {
                                currentIndex = currentUpdateIndex;
                                currentUpdateIndex++;
                            }

                            GameSprite sprite = null;
                            lock (levelSprites)
                            {
                                if (currentIndex < levelSprites.Count && levelSprites[currentIndex].IsActive)
                                    sprite = levelSprites[currentIndex];
                            }

                            if (sprite != null)
                                levelSprites[currentIndex].Update(currentGameTime);
                        }

                        // set that we are done
                        threadsRunning[threadIndex] = false;
                    }
                    else
                    {
                        // maybe sleep here
                        Thread.Sleep(1);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // abort the thread run
            }
        }


        // Accessors
        public List<EnvironmentSprite> EnvSprites
        {
            get { return envSprites; }
        }

        public List<Enemy> EnemySprites
        {
            get { return enemySprites; }
        }

        public List<GameSprite> BubbleSprites
        {
            get { return bubbleSprites; }
        }

        public List<GameSprite> SuperBubbleSprites
        {
            get { return superBubbleSprites; }
        }

        public float Gravity
        {
            get { return levelDetails.gravity; }
        }

        public int LevelNum
        {
            get { return levelNum; }
        }

        public LevelDetails Details
        {
            get { return levelDetails; }
        }

        public bool IsComplete
        {
            get { return (isComplete && (completeElapsed >= completeDuration)); }
        }

        public bool IsFailed
        {
            get { return isFailed && (failedElapsed >= failedDuration); }
        }

        public Vector2 GetOpenLocation()
        {
            if (OpenSpaces.Count > 0)
            {
                return OpenSpaces[BubbleGame.rand.Next(0, OpenSpaces.Count - 1)];
            }
            else
            {
                return new Vector2(BubbleGame.rand.Next(208, 1000), BubbleGame.rand.Next(0, 700));
            }
        }

    }
}