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
    class VictoryScreen : CutSceneScreen
    {
        Texture2D congrats;

        //Firework firework;

        public VictoryScreen()
        {
            this.endTime = 20000;

            // setup text
            script = new ScriptLine[5];
            script[0] = new ScriptLine("You have saved the zoo", 0, 4000, 0, Color.White);
            script[1] = new ScriptLine("from the evil robot invaders.", 4000, 8000, 0, Color.White);
            script[2] = new ScriptLine("Now Bupper, Cheekeze, Clavis and Froofy", 8000, 12000, 0, Color.White);
            script[3] = new ScriptLine("can enjoy life with their friends again.", 12000, 16000, 0, Color.White);
            script[4] = new ScriptLine("THANK YOU!", 16000, 20000, 0, Color.White);

            // Setup Pawns
            pawns = new Pawn[4];
            pawns[0] = new Pawn("Seal", new Vector2(900, 250));
            pawns[1] = new Pawn("Toad", new Vector2(1000, 250));
            pawns[2] = new Pawn("Penguin", new Vector2(-200, 250));
            pawns[3] = new Pawn("Tortoise", new Vector2(-100, 250));

            // setup events in the scene
            events = new CutSceneEvent[8];
            events[0] = new CutSceneEvent(pawns[0], 1, CutSceneAction.Move, new Vector2(-200, 250));
            events[1] = new CutSceneEvent(pawns[1], 1, CutSceneAction.Move, new Vector2(-100, 250));
            events[2] = new CutSceneEvent(pawns[2], 1, CutSceneAction.Move, new Vector2(900, 250));
            events[3] = new CutSceneEvent(pawns[3], 1, CutSceneAction.Move, new Vector2(1000, 250));
            events[4] = new CutSceneEvent(pawns[0], 10000, CutSceneAction.Move, new Vector2(200, 250));
            events[5] = new CutSceneEvent(pawns[1], 10000, CutSceneAction.Move, new Vector2(300, 250));
            events[6] = new CutSceneEvent(pawns[2], 10000, CutSceneAction.Move, new Vector2(500, 250));
            events[7] = new CutSceneEvent(pawns[3], 10000, CutSceneAction.Move, new Vector2(600, 250));

            congrats = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures", "UI/Congratulations"));
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = Level.screenManager.SpriteBatch;
            spriteBatch.Begin();

            // Draw congrats
            spriteBatch.Draw(congrats, new Vector2(400 - congrats.Width * .75f / 2, 100),null, Color.White, 0f, Vector2.Zero, .75f, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        public override void TopFullScreenAcquired()
        {
            base.TopFullScreenAcquired();

            MusicManager.SingletonMusicManager.StopAll();
            MusicManager.SingletonMusicManager.PlayTune("levelselect");
        }

        public override void OnRemoval()
        {
            base.OnRemoval();

            CreditsScreen screen = new CreditsScreen();
            screen.ShouldResetMusic = false;
            ScreenManager.AddScreen(screen);

        }
    }
}