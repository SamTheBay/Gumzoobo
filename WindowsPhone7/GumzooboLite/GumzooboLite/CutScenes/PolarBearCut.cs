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
    class PolarBearCut : CutSceneScreen
    {
        public PolarBearCut()
        {
            this.endTime = 14000;

            // setup text
            script = new ScriptLine[2];
            script[0] = new ScriptLine("Cheekeze - \"AHHHHHHHHHHHH!!!\"", 1000, 6000, 0, PenguinPlayer.StaticLightColor());
            script[1] = new ScriptLine("Froofy - \"Hold on! We're coming...\"", 7000, 11000, 0, TortoisePlayer.StaticLightColor());
            
            // Setup Pawns
            pawns = new Pawn[8];
            pawns[0] = new Pawn("Seal", new Vector2(-300, 250));
            pawns[1] = new Pawn("Toad", new Vector2(-200, 250));
            pawns[2] = new Pawn("Penguin", new Vector2(-100, 250));
            pawns[3] = new Pawn("Tortoise", new Vector2(-100, 250));
            pawns[4] = new Pawn("RocketBlaster", new Vector2(-100, 350));
            pawns[5] = new Pawn("RocketBlaster", new Vector2(-100, 250));
            pawns[6] = new Pawn("RocketBlaster", new Vector2(-100, 150));
            pawns[7] = new Pawn("RocketBlaster", new Vector2(-100, 50));

            // setup events in the scene
            events = new CutSceneEvent[8];
            events[0] = new CutSceneEvent(pawns[2], 1, CutSceneAction.Move, new Vector2(1400, 250));
            events[1] = new CutSceneEvent(pawns[4], 2000, CutSceneAction.Move, new Vector2(1400, 250));
            events[2] = new CutSceneEvent(pawns[5], 2000, CutSceneAction.Move, new Vector2(1400, 250));
            events[3] = new CutSceneEvent(pawns[6], 2000, CutSceneAction.Move, new Vector2(1400, 250));
            events[4] = new CutSceneEvent(pawns[7], 2000, CutSceneAction.Move, new Vector2(1400, 250));
            events[5] = new CutSceneEvent(pawns[0], 6000, CutSceneAction.Move, new Vector2(1400, 250));
            events[6] = new CutSceneEvent(pawns[1], 6000, CutSceneAction.Move, new Vector2(1400, 250));
            events[7] = new CutSceneEvent(pawns[3], 6000, CutSceneAction.Move, new Vector2(1400, 250));
        }

    }
}