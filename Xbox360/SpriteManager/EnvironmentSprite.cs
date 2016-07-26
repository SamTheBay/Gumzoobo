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
    public class EnvironmentSprite : AnimatedSprite
    {



        public EnvironmentSprite(int nFrame, Vector2 nPosition)
            : base(MapScreen.currentLocation.envTextureName, new Point(24, 24), new Point(12, 12), 14, new Vector2(12f, 12f), nPosition)
        {
            AddAnimation(new Animation("Env", nFrame, nFrame, 100, false, SpriteEffects.None));
            PlayAnimation("Env");
            isActive = true;

            topBox = new Rectangle((int)nPosition.X, (int)nPosition.Y, FrameDimensions.X, 1);
            bottomBox = new Rectangle((int)nPosition.X, (int)nPosition.Y + FrameDimensions.Y, FrameDimensions.X, 1);
            rightBox = new Rectangle((int)nPosition.X + FrameDimensions.X, (int)nPosition.Y, 1, FrameDimensions.Y);
            leftbox = new Rectangle((int)nPosition.X, (int)nPosition.Y, 1, FrameDimensions.Y);
        }





    }
}