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
    class AquariumCut : CutSceneScreen
    {
        public AquariumCut()
        {
            this.endTime = 18000;

            // setup text
            script = new ScriptLine[1];
            script[0] = new ScriptLine("Clavis - \"We've got them on the run!\"", 6000, 11000, 0, ToadPlayer.StaticLightColor());


            // Setup Pawns
            pawns = new Pawn[13];
            pawns[0] = new Pawn("Seal", new Vector2(-300, 450));
            pawns[1] = new Pawn("Toad", new Vector2(-200, 450));
            pawns[2] = new Pawn("Penguin", new Vector2(-400, 450));
            pawns[3] = new Pawn("Tortoise", new Vector2(-100, 450));
            pawns[4] = new Pawn("PorkyBot", new Vector2(-100, 450));
            pawns[5] = new Pawn("RocketBot", new Vector2(-200, 150));
            pawns[6] = new Pawn("RocketBlaster", new Vector2(-300, 150));
            pawns[7] = new Pawn("InvisiBot", new Vector2(-200, 450));
            pawns[8] = new Pawn("LazerBot", new Vector2(-300, 450));
            pawns[9] = new Pawn("WarpBot", new Vector2(-400, 450));
            pawns[10] = new Pawn("Drone", new Vector2(-500, 460));
            pawns[11] = new Pawn("FireDrone", new Vector2(-600, 460));
            pawns[12] = new Pawn("Hunter", new Vector2(-100, 350));


            // setup events in the scene
            events = new CutSceneEvent[13];
            events[0] = new CutSceneEvent(pawns[4], 1, CutSceneAction.Move, new Vector2(1400, 450));
            events[1] = new CutSceneEvent(pawns[5], 1, CutSceneAction.Move, new Vector2(1400, 150));
            events[2] = new CutSceneEvent(pawns[6], 1, CutSceneAction.Move, new Vector2(1400, 150));
            events[3] = new CutSceneEvent(pawns[7], 1, CutSceneAction.Move, new Vector2(1400, 450));
            events[4] = new CutSceneEvent(pawns[8], 1, CutSceneAction.Move, new Vector2(1400, 450));
            events[5] = new CutSceneEvent(pawns[9], 1, CutSceneAction.Move, new Vector2(1400, 450));
            events[6] = new CutSceneEvent(pawns[10], 1, CutSceneAction.Move, new Vector2(1400, 460));
            events[7] = new CutSceneEvent(pawns[11], 1, CutSceneAction.Move, new Vector2(1400, 460));
            events[8] = new CutSceneEvent(pawns[0], 6000, CutSceneAction.Move, new Vector2(1400, 450));
            events[9] = new CutSceneEvent(pawns[1], 6000, CutSceneAction.Move, new Vector2(1400, 450));
            events[10] = new CutSceneEvent(pawns[2], 6000, CutSceneAction.Move, new Vector2(1400, 450));
            events[11] = new CutSceneEvent(pawns[3], 6000, CutSceneAction.Move, new Vector2(1400, 450));
            events[12] = new CutSceneEvent(pawns[12], 10000, CutSceneAction.Move, new Vector2(1400, 350));
        }
    }
}