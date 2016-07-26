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
    class NightHouseCut : CutSceneScreen
    {
        public NightHouseCut()
        {
            this.endTime = 12000;

            // setup text
            script = new ScriptLine[1];
            script[0] = new ScriptLine("Cheekeze - \"Hey, who turned out the lights!\"", 5000, 10000, 0, PenguinPlayer.StaticLightColor());


            // Setup Pawns
            pawns = new Pawn[4];
            pawns[0] = new Pawn("Seal", new Vector2(-380, 450));
            pawns[1] = new Pawn("Toad", new Vector2(-180, 450));
            pawns[2] = new Pawn("Penguin", new Vector2(-80, 450));
            pawns[3] = new Pawn("Tortoise", new Vector2(-280, 450));


            // setup events in the scene
            events = new CutSceneEvent[4];
            events[0] = new CutSceneEvent(pawns[0], 1000, CutSceneAction.Move, new Vector2(1400, 450));
            events[1] = new CutSceneEvent(pawns[1], 1000, CutSceneAction.Move, new Vector2(1400, 450));
            events[2] = new CutSceneEvent(pawns[2], 1000, CutSceneAction.Move, new Vector2(1400, 450));
            events[3] = new CutSceneEvent(pawns[3], 1000, CutSceneAction.Move, new Vector2(1400, 450));

            darkenTime = 4000;
            shouldDarken = true;
        }
    }
}