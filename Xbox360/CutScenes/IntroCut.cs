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
    class IntroCut : CutSceneScreen
    {
        public IntroCut()
        {
            // setup text
            script = new ScriptLine[9];
            script[0] = new ScriptLine("Bupper, Cheekeze, Clavis and Froofy", 0, 4000, 0);
            script[1] = new ScriptLine("were enjoying their day at the zoo", 0, 4000, 1);
            script[2] = new ScriptLine("Bupper - \"What is that!\"", 4200, 6000, 0, SealPlayer.StaticLightColor());
            script[3] = new ScriptLine("Clavis - \"I am not sure, it looks", 6200, 9000, 0, ToadPlayer.StaticLightColor());
            script[4] = new ScriptLine("like something is falling from the sky\"", 6200, 9000, 1, ToadPlayer.StaticLightColor());
            script[5] = new ScriptLine("Froofy - \"Your right, robots have come", 9200, 12000, 0, TortoisePlayer.StaticLightColor());
            script[6] = new ScriptLine("to take over the zoo!\"", 9200, 12000, 1, TortoisePlayer.StaticLightColor());
            script[7] = new ScriptLine("Cheekeze - \"Hurry, we have to stop them", 12200, 17000, 0, PenguinPlayer.StaticLightColor());
            script[8] = new ScriptLine("to save the zoo!\"", 12200, 17000, 1, PenguinPlayer.StaticLightColor());

            // Setup Pawns
            pawns = new Pawn[7];
            pawns[0] = new Pawn("Seal", new Vector2(500, 450));
            pawns[1] = new Pawn("Toad", new Vector2(600, 450));
            pawns[2] = new Pawn("Penguin", new Vector2(700, 450));
            pawns[3] = new Pawn("Tortoise", new Vector2(800, 450));
            pawns[4] = new Pawn("RocketBot", new Vector2(-80, 100));
            pawns[5] = new Pawn("RocketBot", new Vector2(-100, 50));
            pawns[6] = new Pawn("RocketBot", new Vector2(-120, 150));

            // setup events in the scene
            events = new CutSceneEvent[15];
            events[0] = new CutSceneEvent(pawns[0], 2000, CutSceneAction.Move, new Vector2(400, 450));
            events[1] = new CutSceneEvent(pawns[0], 7000, CutSceneAction.Move, new Vector2(450, 450));
            events[2] = new CutSceneEvent(pawns[0], 14000, CutSceneAction.Move, new Vector2(1300, 450));
            events[3] = new CutSceneEvent(pawns[1], 3000, CutSceneAction.Move, new Vector2(650, 450));
            events[4] = new CutSceneEvent(pawns[1], 8000, CutSceneAction.Move, new Vector2(550, 450));
            events[5] = new CutSceneEvent(pawns[1], 14000, CutSceneAction.Move, new Vector2(1300, 450));
            events[6] = new CutSceneEvent(pawns[2], 1000, CutSceneAction.Move, new Vector2(520, 450));
            events[7] = new CutSceneEvent(pawns[2], 5000, CutSceneAction.Move, new Vector2(750, 450));
            events[8] = new CutSceneEvent(pawns[2], 14000, CutSceneAction.Move, new Vector2(1300, 450));
            events[9] = new CutSceneEvent(pawns[3], 2500, CutSceneAction.Move, new Vector2(900, 450));
            events[10] = new CutSceneEvent(pawns[3], 7500, CutSceneAction.Move, new Vector2(850, 450));
            events[11] = new CutSceneEvent(pawns[3], 14000, CutSceneAction.Move, new Vector2(1300, 450));
            events[12] = new CutSceneEvent(pawns[4], 2000, CutSceneAction.Move, new Vector2(1300, 100));
            events[13] = new CutSceneEvent(pawns[5], 2000, CutSceneAction.Move, new Vector2(1300, 50));
            events[14] = new CutSceneEvent(pawns[6], 2000, CutSceneAction.Move, new Vector2(1300, 150));
        }

        public override void TopFullScreenAcquired()
        {
            base.TopFullScreenAcquired();

            MusicManager.SingletonMusicManager.StopAll();
            MusicManager.SingletonMusicManager.PlayTune("levelselect");
        }
    }
}