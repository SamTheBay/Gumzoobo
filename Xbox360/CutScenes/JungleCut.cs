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
    class JungleCut : CutSceneScreen
    {
        public JungleCut()
        {
            this.endTime = 18000;

            // setup text
            script = new ScriptLine[3];
            script[0] = new ScriptLine("Clavis - \"I see another one over there!\"", 5000, 8000, 0, ToadPlayer.StaticLightColor());
            script[1] = new ScriptLine("Bupper - \"Where did he go?\"", 9000, 12000, 0, SealPlayer.StaticLightColor());
            script[2] = new ScriptLine("Clavis - \"It's like he just disappeared!?\"", 12000, 15000, 0, ToadPlayer.StaticLightColor());

            // Setup Pawns
            pawns = new Pawn[5];
            pawns[0] = new Pawn("Seal", new Vector2(-380, 450));
            pawns[1] = new Pawn("Toad", new Vector2(-280, 450));
            pawns[2] = new Pawn("Penguin", new Vector2(-180, 450));
            pawns[3] = new Pawn("Tortoise", new Vector2(-80, 450));
            pawns[4] = new Pawn("InvisiBot", new Vector2(-80, 450));

            // setup events in the scene
            events = new CutSceneEvent[19];
            events[0] = new CutSceneEvent(pawns[4], 1, CutSceneAction.Move, new Vector2(600, 450));
            events[1] = new CutSceneEvent(pawns[3], 5000, CutSceneAction.Move, new Vector2(650, 450));
            events[2] = new CutSceneEvent(pawns[2], 5000, CutSceneAction.Move, new Vector2(550, 450));
            events[3] = new CutSceneEvent(pawns[1], 5000, CutSceneAction.Move, new Vector2(450, 450));
            events[4] = new CutSceneEvent(pawns[0], 5000, CutSceneAction.Move, new Vector2(300, 450));
            events[5] = new CutSceneEvent("Invisable", 7000, CutSceneAction.PlaySound);
            events[6] = new CutSceneEvent(pawns[4], 7000, CutSceneAction.StopDraw);
            events[7] = new CutSceneEvent(pawns[4], 7200, CutSceneAction.StartDraw);
            events[8] = new CutSceneEvent(pawns[4], 7400, CutSceneAction.StopDraw);
            events[9] = new CutSceneEvent(pawns[4], 7600, CutSceneAction.StartDraw);
            events[10] = new CutSceneEvent(pawns[4], 7800, CutSceneAction.StopDraw);
            events[11] = new CutSceneEvent(pawns[4], 7850, CutSceneAction.StartDraw);
            events[12] = new CutSceneEvent(pawns[4], 7900, CutSceneAction.StopDraw);
            events[13] = new CutSceneEvent(pawns[4], 7950, CutSceneAction.StartDraw);
            events[14] = new CutSceneEvent(pawns[4], 8000, CutSceneAction.StopDraw);
            events[15] = new CutSceneEvent(pawns[0], 12000, CutSceneAction.Move, new Vector2(1400, 450));
            events[16] = new CutSceneEvent(pawns[1], 12000, CutSceneAction.Move, new Vector2(1400, 450));
            events[17] = new CutSceneEvent(pawns[2], 12000, CutSceneAction.Move, new Vector2(1400, 450));
            events[18] = new CutSceneEvent(pawns[3], 12000, CutSceneAction.Move, new Vector2(1400, 450));

        }

    }
}