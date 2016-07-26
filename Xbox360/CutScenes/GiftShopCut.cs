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
    class GiftShopCut : CutSceneScreen
    {
        public GiftShopCut()
        {
            this.endTime = 12000;

            // setup text
            script = new ScriptLine[2];
            script[0] = new ScriptLine("Froofy - \"There he is over there!", 4000, 11000, 0, TortoisePlayer.StaticLightColor());
            script[1] = new ScriptLine("Don't let him get away! He is heading for the gift shop.\"", 4000, 11000, 1, TortoisePlayer.StaticLightColor());


            // Setup Pawns
            pawns = new Pawn[5];
            pawns[0] = new Pawn("Seal", new Vector2(-380, 450));
            pawns[1] = new Pawn("Toad", new Vector2(-180, 450));
            pawns[2] = new Pawn("Penguin", new Vector2(-280, 450));
            pawns[3] = new Pawn("Tortoise", new Vector2(-80, 450));
            pawns[4] = new Pawn("Drone", new Vector2(1320, 450));

            // setup events in the scene
            events = new CutSceneEvent[6];
            events[0] = new CutSceneEvent(pawns[4], 1, CutSceneAction.Move, new Vector2(950, 450));
            events[1] = new CutSceneEvent(pawns[0], 3000, CutSceneAction.Move, new Vector2(1400, 450));
            events[2] = new CutSceneEvent(pawns[1], 3000, CutSceneAction.Move, new Vector2(1400, 450));
            events[3] = new CutSceneEvent(pawns[2], 3000, CutSceneAction.Move, new Vector2(1400, 450));
            events[4] = new CutSceneEvent(pawns[3], 3000, CutSceneAction.Move, new Vector2(1400, 450));
            events[5] = new CutSceneEvent(pawns[4], 6000, CutSceneAction.Move, new Vector2(1400, 450));

        }
    }
}