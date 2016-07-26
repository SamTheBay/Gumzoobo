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
    class PettingZooCut : CutSceneScreen
    {
        public PettingZooCut()
        {
            this.endTime = 11000;

            // setup text
            script = new ScriptLine[2];
            script[0] = new ScriptLine("Bupper - \"Come on Guys! They went this", 2000, 10000, 0, SealPlayer.StaticLightColor());
            script[1] = new ScriptLine("way. Hurry Up!\"", 2000, 10000, 1, SealPlayer.StaticLightColor());


            // Setup Pawns
            pawns = new Pawn[4];
            pawns[0] = new Pawn("Seal", new Vector2(-80, 250));
            pawns[1] = new Pawn("Toad", new Vector2(-180, 250));
            pawns[2] = new Pawn("Penguin", new Vector2(-280, 250));
            pawns[3] = new Pawn("Tortoise", new Vector2(-380, 250));

            // setup events in the scene
            events = new CutSceneEvent[4];
            events[0] = new CutSceneEvent(pawns[0], 1, CutSceneAction.Move, new Vector2(1400, 250));
            events[1] = new CutSceneEvent(pawns[1], 1, CutSceneAction.Move, new Vector2(1400, 250));
            events[2] = new CutSceneEvent(pawns[2], 1, CutSceneAction.Move, new Vector2(1400, 250));
            events[3] = new CutSceneEvent(pawns[3], 1, CutSceneAction.Move, new Vector2(1400, 250));

        }

    }
}