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
    public enum CutSceneAction
    {
        Move,
        Remove,
        PlaySound,
        StopDraw,
        StartDraw,
        Freeze,
        InShell,
        OutShell,
        ChangeLocation
    }


    class CutSceneEvent
    {
        public int time = 0;
        public int prevTime = -1;
        public CutSceneAction action;
        public Vector2 location = Vector2.Zero;
        public float fvalue = 0f;
        public int value = 0;
        public Pawn owner = null;
        public string stringValue = "";

        public CutSceneEvent(Pawn owner, int time, CutSceneAction action, Vector2 location)
        {
            this.time = time;
            this.action = action;
            this.location = location;
            this.owner = owner;
        }

        public CutSceneEvent(string value, int time, CutSceneAction action)
        {
            this.time = time;
            this.action = action;
            this.stringValue = value;
        }

        public CutSceneEvent(Pawn owner, int time, CutSceneAction action)
        {
            this.time = time;
            this.action = action;
            this.owner = owner;
        }

        public void RunEvent(int currTime)
        {
            if (prevTime < time && currTime >= time)
            {
                if (action == CutSceneAction.Move)
                {
                    owner.SetBasicDestination(location);
                }
                else if (action == CutSceneAction.PlaySound)
                {
                    AudioManager.PlayCue(stringValue);
                }
                else if (action == CutSceneAction.Remove)
                {
                    owner.SetClearPosition(new Vector2(-100, -100));
                }
                else if (action == CutSceneAction.StartDraw)
                {
                    owner.ShouldDraw = true;
                }
                else if (action == CutSceneAction.StopDraw)
                {
                    owner.ShouldDraw = false;
                }
                else if (action == CutSceneAction.Freeze)
                {
                    owner.PlayAnimation("Frozen", owner.CurrentDirection);
                }
                else if (action == CutSceneAction.InShell)
                {
                    owner.PlayAnimation("InShell", owner.CurrentDirection);
                }
                else if (action == CutSceneAction.OutShell)
                {
                    owner.PlayAnimation("OutShell", owner.CurrentDirection);
                }
                else if (action == CutSceneAction.ChangeLocation)
                {
                    owner.SetClearPosition(location);
                }
            }
            prevTime = currTime;
        }
    }



    class ScriptLine
    {
        public ScriptLine(string line, int startTime, int endTime, int position)
        {
            this.line = line;
            this.startTime = startTime;
            this.endTime = endTime;
            this.position = position;
            this.color = Color.White;
        }

        public ScriptLine(string line, int startTime, int endTime, int position, Color color)
        {
            this.line = line;
            this.startTime = startTime;
            this.endTime = endTime;
            this.position = position;
            this.color = color;
        }

        public string line;
        public int startTime;
        public int endTime;
        public int position;
        public Color color;
    }   



    class CutSceneScreen : GameScreen
    {
        // text that goes in the scene
        protected ScriptLine[] script;

        // pawns on the screen
        protected Pawn[] pawns;

        // events that occur in the scene
        protected CutSceneEvent[] events;

        // background for the scene
        protected Texture2D background;

        // time
        int elapsedTime = 0;
        protected int endTime = 20000;

        Effect Darkener;
        protected int darkenTime = 0;
        protected bool shouldDarken = false;

        public CutSceneScreen()
        {
            background = InternalContentManager.GetTexture("Clear");
            Darkener = BubbleGame.sigletonGame.Content.Load<Effect>("Darkener");
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            // run the events
            for (int i = 0; i < events.Length; i++)
            {
                events[i].RunEvent(elapsedTime);
            }

            // update pawns so they can move
            for (int i = 0; i < pawns.Length; i++)
            {
                pawns[i].Update(gameTime);
            }

            if (elapsedTime > endTime)
            {
                ExitScreen();
            }
        }


        public override void HandleInput()
        {
            base.HandleInput();

            // check for player exiting
            for (int i = 0; i < 4; i++)
            {
                if (InputManager.IsActionTriggered(InputManager.Action.Ok, i) ||
                    InputManager.IsActionTriggered(InputManager.Action.Back, i))
                {
                    ExitScreen();
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            BubbleGame.graphics.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // Draw the background
            spriteBatch.Draw(background, new Rectangle(208, 0, 864, 720), Color.White);

            if (shouldDarken)
            {
                Darkener.Begin();
                Darkener.CurrentTechnique.Passes[0].Begin();

                float darkFactor = 0f;
                if (elapsedTime <= darkenTime)
                {
                    darkFactor = 0f;
                }
                else if (elapsedTime <= darkenTime + 2000)
                {
                    darkFactor = ((float)elapsedTime - (float)darkenTime) / 2000f;
                }
                else
                {
                    darkFactor = 1f;
                }

                Darkener.Parameters["darkFactor"].SetValue(darkFactor);
            }

            // draw the pawns
            for (int i = 0; i < pawns.Length; i++)
            {
                pawns[i].Draw(spriteBatch);
            }

            if (shouldDarken)
            {
                Darkener.CurrentTechnique.Passes[0].End();
                Darkener.End();
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // Draw the script
            for (int i = 0; i < script.Length; i++)
            {
                if (script[i].startTime < elapsedTime && script[i].endTime > elapsedTime)
                {
                    Fonts.DrawCenteredText(spriteBatch, Fonts.DescriptionFont, script[i].line, new Vector2(640, 620 + (script[i].position * 50)), script[i].color, 1.5f); 
                }
            }

            spriteBatch.End();
        }


        static public CutSceneScreen GetCutScene(string name)
        {
            if (name == "PettingZoo")
            {
                return (CutSceneScreen)(new PettingZooCut());
            }
            else if (name == "GiftShop")
            {
                return (CutSceneScreen)(new GiftShopCut());
            }
            else if (name == "Jungle")
            {
                return (CutSceneScreen)(new JungleCut());
            }
            else if (name == "Sahara")
            {
                return (CutSceneScreen)(new SaharaCut());
            }
            else if (name == "Aviary")
            {
                return (CutSceneScreen)(new AviaryCut());
            }
            else if (name == "Swamp")
            {
                return (CutSceneScreen)(new SwampCut());
            }
            else if (name == "PolarBear")
            {
                return (CutSceneScreen)(new PolarBearCut());
            }
            else if (name == "MonkeyHouse")
            {
                return (CutSceneScreen)(new MonkeyHouseCut());
            }
            else if (name == "Aquarium")
            {
                return (CutSceneScreen)(new AquariumCut());
            }
            else if (name == "NightHouse")
            {
                return (CutSceneScreen)(new NightHouseCut());
            }
            return null;
        }
    }
}