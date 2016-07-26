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
    class SwampCut : CutSceneScreen
    {
        public SwampCut()
        {
            this.endTime = 12000;

            // setup text
            script = new ScriptLine[2];
            script[0] = new ScriptLine("Cheekeze - \"Get Him!\"", 3000, 6000, 0, PenguinPlayer.StaticLightColor());
            script[1] = new ScriptLine("Cheekeze - \"Run Away!\"", 7500, 11500, 1, PenguinPlayer.StaticLightColor());


            // Setup Pawns
            pawns = new Pawn[6];
            pawns[0] = new Pawn("Seal", new Vector2(-380, 250));
            pawns[1] = new Pawn("Toad", new Vector2(-180, 250));
            pawns[2] = new Pawn("Penguin", new Vector2(-80, 250));
            pawns[3] = new Pawn("Tortoise", new Vector2(-280, 250));
            pawns[4] = new Pawn("FireDrone", new Vector2(920, 250));
            pawns[5] = new Pawn("FireBall", new Vector2(700, 250));

            pawns[5].ShouldDraw = false;

            // setup events in the scene
            events = new CutSceneEvent[12];
            events[0] = new CutSceneEvent(pawns[4], 1, CutSceneAction.Move, new Vector2(700, 250));
            events[1] = new CutSceneEvent(pawns[0], 3000, CutSceneAction.Move, new Vector2(1400, 250));
            events[2] = new CutSceneEvent(pawns[1], 3000, CutSceneAction.Move, new Vector2(1400, 250));
            events[3] = new CutSceneEvent(pawns[2], 3000, CutSceneAction.Move, new Vector2(1400, 250));
            events[4] = new CutSceneEvent(pawns[3], 3000, CutSceneAction.Move, new Vector2(1400, 250));
            events[5] = new CutSceneEvent(pawns[5], 7000, CutSceneAction.StartDraw);
            events[6] = new CutSceneEvent(pawns[5], 7000, CutSceneAction.Move, new Vector2(-100, 250));
            events[7] = new CutSceneEvent(pawns[0], 7500, CutSceneAction.Move, new Vector2(-100, 250));
            events[8] = new CutSceneEvent(pawns[1], 7500, CutSceneAction.Move, new Vector2(-100, 250));
            events[9] = new CutSceneEvent(pawns[2], 7500, CutSceneAction.Move, new Vector2(-100, 250));
            events[10] = new CutSceneEvent(pawns[3], 7500, CutSceneAction.Move, new Vector2(-100, 250));
            events[11] = new CutSceneEvent(pawns[4], 10000, CutSceneAction.Move, new Vector2(1400, 250));

        }


    }
}