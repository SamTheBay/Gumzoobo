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
    class AviaryCut : CutSceneScreen
    {
        public AviaryCut()
        {
            this.endTime = 14000;

            // setup text
            script = new ScriptLine[3];
            script[0] = new ScriptLine("Froofy - \"Help! It's after me!\"", 500, 4000, 0, TortoisePlayer.StaticLightColor());
            script[1] = new ScriptLine("Clavis - \"Come and get me robo-face!\"", 5500, 9000, 0, ToadPlayer.StaticLightColor());
            script[2] = new ScriptLine("Froofy - \"Phew\"", 11000, 14000, 0, TortoisePlayer.StaticLightColor());

            // Setup Pawns
            pawns = new Pawn[3];
            pawns[0] = new Pawn("Toad", new Vector2(-100, 250));
            pawns[1] = new Pawn("Tortoise", new Vector2(-100, 250));
            pawns[2] = new Pawn("Hunter", new Vector2(-100, 100));

            // setup events in the scene
            events = new CutSceneEvent[8];
            events[0] = new CutSceneEvent(pawns[2], 1000, CutSceneAction.Move, new Vector2(700, 250));
            events[1] = new CutSceneEvent(pawns[1], 1, CutSceneAction.Move, new Vector2(700, 250));
            events[2] = new CutSceneEvent(pawns[1], 5000, CutSceneAction.InShell);
            events[3] = new CutSceneEvent(pawns[0], 5000, CutSceneAction.Move, new Vector2(200, 250));
            events[4] = new CutSceneEvent(pawns[0], 7000, CutSceneAction.Move, new Vector2(-100, 250));
            events[5] = new CutSceneEvent(pawns[2], 7000, CutSceneAction.Move, new Vector2(-100, 250));
            events[6] = new CutSceneEvent(pawns[1], 10000, CutSceneAction.OutShell);
            events[7] = new CutSceneEvent(pawns[1], 11000, CutSceneAction.Move, new Vector2(1400, 250));

        }
    }
}