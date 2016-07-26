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
    class PleaseRateScreen : CutSceneScreen
    {
        Texture2D Star;

        public PleaseRateScreen()
        {
            Star = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures/UI", "Star"));

            this.endTime = 8000;

            // setup text
            script = new ScriptLine[3];
            script[0] = new ScriptLine("Clavis - \"Thanks for playing!\"", 1000, 4000, 0, ToadPlayer.StaticLightColor());
            script[1] = new ScriptLine("Bupper - \"If you had fun, Don't forget\"", 4000, 8000, 0, SealPlayer.StaticLightColor());
            script[2] = new ScriptLine("to give us a rating!\" (Shameless Plug)", 4000, 8000, 1, SealPlayer.StaticLightColor());

            // Setup Pawns
            pawns = new Pawn[4];
            pawns[0] = new Pawn("Seal", new Vector2(-200, 450));
            pawns[1] = new Pawn("Toad", new Vector2(-100, 450));
            pawns[2] = new Pawn("Penguin", new Vector2(1300, 450));
            pawns[3] = new Pawn("Tortoise", new Vector2(1400, 450));

            // setup events in the scene
            events = new CutSceneEvent[4];
            events[0] = new CutSceneEvent(pawns[0], 0, CutSceneAction.Move, new Vector2(300, 450));
            events[1] = new CutSceneEvent(pawns[1], 0, CutSceneAction.Move, new Vector2(400, 450));
            events[2] = new CutSceneEvent(pawns[2], 0, CutSceneAction.Move, new Vector2(800, 450));
            events[3] = new CutSceneEvent(pawns[3], 0, CutSceneAction.Move, new Vector2(900, 450));

        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // draw stars
            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(Star, new Vector2(640 - (int)((float)Star.Width * 2.5f) + Star.Width * i, 200), Color.White);
            }

            spriteBatch.End();

        }


        public override void ExitScreen()
        {
            BubbleGame.sigletonGame.Exit();
            base.ExitScreen();
        }

    }
}