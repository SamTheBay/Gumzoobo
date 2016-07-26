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
    class SaharaCut : CutSceneScreen
    {
        public SaharaCut()
        {
            this.endTime = 16000;

            // setup text
            script = new ScriptLine[3];
            script[0] = new ScriptLine("Cheekeze - \"Oh no! This one is too sharp", 5000, 9000, 0, PenguinPlayer.StaticLightColor());
            script[1] = new ScriptLine("to trap in a bubble!\"", 5000, 9000, 1, PenguinPlayer.StaticLightColor());
            script[2] = new ScriptLine("Bupper - \"Looks like we will have to freeze them.\"", 10000, 15000, 0, SealPlayer.StaticLightColor());


            // Setup Pawns
            pawns = new Pawn[5];
            pawns[0] = new Pawn("Seal", new Vector2(-80, 250));
            pawns[1] = new Pawn("Penguin", new Vector2(920, 250));
            pawns[2] = new Pawn("PorkyBot", new Vector2(920, 250));
            pawns[3] = new Pawn("Bubble", new Vector2(700, 250));
            pawns[4] = new Pawn("IceBubble", new Vector2(450, 250));

            pawns[3].ShouldDraw = false;
            pawns[4].ShouldDraw = false;

            // setup events in the scene
            events = new CutSceneEvent[12];
            events[0] = new CutSceneEvent(pawns[2], 1, CutSceneAction.Move, new Vector2(600, 250));
            events[1] = new CutSceneEvent(pawns[0], 3000, CutSceneAction.Move, new Vector2(450, 250));
            events[2] = new CutSceneEvent(pawns[1], 3000, CutSceneAction.Move, new Vector2(700, 250));
            events[3] = new CutSceneEvent(pawns[3], 4500, CutSceneAction.StartDraw);
            events[4] = new CutSceneEvent(pawns[3], 4500, CutSceneAction.Move, new Vector2(600, 250));
            events[5] = new CutSceneEvent(pawns[3], 5000, CutSceneAction.StopDraw);
            events[6] = new CutSceneEvent(pawns[4], 7000, CutSceneAction.StartDraw);
            events[7] = new CutSceneEvent(pawns[4], 7000, CutSceneAction.Move, new Vector2(1400, 250));
            events[8] = new CutSceneEvent(pawns[4], 8000, CutSceneAction.StopDraw);
            events[9] = new CutSceneEvent(pawns[2], 8000, CutSceneAction.Freeze);
            events[10] = new CutSceneEvent(pawns[1], 12000, CutSceneAction.Move, new Vector2(1400, 250));
            events[11] = new CutSceneEvent(pawns[0], 12000, CutSceneAction.Move, new Vector2(1400, 250));
        }

    }
}