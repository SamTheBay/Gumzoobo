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
    class MonkeyHouseCut : CutSceneScreen
    {
        public MonkeyHouseCut()
        {
            this.endTime = 17000;

            // setup text
            script = new ScriptLine[3];
            script[0] = new ScriptLine("Clavis - \"Come here you!\"", 1000, 5000, 0, ToadPlayer.StaticLightColor());
            script[1] = new ScriptLine("Clavis - \"Ugh, how did that happen!?\"", 6000, 9000, 0, ToadPlayer.StaticLightColor());
            script[2] = new ScriptLine("Clavis - \"I said get back here!\"", 10000, 15000, 0, ToadPlayer.StaticLightColor());

            // Setup Pawns
            pawns = new Pawn[2];
            pawns[0] = new Pawn("Toad", new Vector2(-100, 450));
            pawns[1] = new Pawn("WarpBot", new Vector2(-100, 450));

            // setup events in the scene
            events = new CutSceneEvent[24];
            events[0] = new CutSceneEvent(pawns[1], 1, CutSceneAction.Move, new Vector2(1400, 450));
            events[1] = new CutSceneEvent(pawns[0], 1000, CutSceneAction.Move, new Vector2(1400, 450));
            events[2] = new CutSceneEvent("Invisable", 5000, CutSceneAction.PlaySound);
            events[3] = new CutSceneEvent(pawns[1], 5000, CutSceneAction.StopDraw);
            events[4] = new CutSceneEvent(pawns[1], 5200, CutSceneAction.StartDraw);
            events[5] = new CutSceneEvent(pawns[1], 5400, CutSceneAction.StopDraw);
            events[6] = new CutSceneEvent(pawns[1], 5600, CutSceneAction.StartDraw);
            events[7] = new CutSceneEvent(pawns[1], 5800, CutSceneAction.StopDraw);
            events[8] = new CutSceneEvent(pawns[1], 5850, CutSceneAction.StartDraw);
            events[9] = new CutSceneEvent(pawns[1], 5900, CutSceneAction.StopDraw);
            events[10] = new CutSceneEvent(pawns[1], 5950, CutSceneAction.StartDraw);
            events[11] = new CutSceneEvent(pawns[1], 6000, CutSceneAction.ChangeLocation, new Vector2(100, 450));
            events[12] = new CutSceneEvent(pawns[0], 6500, CutSceneAction.Move, new Vector2(100, 450));
            events[13] = new CutSceneEvent("Invisable", 9000, CutSceneAction.PlaySound);
            events[14] = new CutSceneEvent(pawns[1], 9000, CutSceneAction.StopDraw);
            events[15] = new CutSceneEvent(pawns[1], 9200, CutSceneAction.StartDraw);
            events[16] = new CutSceneEvent(pawns[1], 9400, CutSceneAction.StopDraw);
            events[17] = new CutSceneEvent(pawns[1], 9600, CutSceneAction.StartDraw);
            events[17] = new CutSceneEvent(pawns[1], 9800, CutSceneAction.StopDraw);
            events[18] = new CutSceneEvent(pawns[1], 9850, CutSceneAction.StartDraw);
            events[19] = new CutSceneEvent(pawns[1], 9900, CutSceneAction.StopDraw);
            events[20] = new CutSceneEvent(pawns[1], 9950, CutSceneAction.StartDraw);
            events[21] = new CutSceneEvent(pawns[1], 10000, CutSceneAction.ChangeLocation, new Vector2(900, 450));
            events[22] = new CutSceneEvent(pawns[0], 10500, CutSceneAction.Move, new Vector2(14000, 450));
            events[23] = new CutSceneEvent(pawns[1], 10500, CutSceneAction.Move, new Vector2(14000, 450));
        }
    }
}