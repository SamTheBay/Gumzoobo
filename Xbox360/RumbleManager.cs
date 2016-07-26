using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace BubbleGame
{
    public class RumbleManager
    {
        int[] rumbleElapsed;
        int rumbleDuration = 500;
        float strengthRight = .5f;
        float strengthLeft = .5f;

        public RumbleManager(int rumbleDuration, float strengthRight, float strengthLeft)
        {
            this.rumbleDuration = rumbleDuration;
            this.strengthLeft = strengthLeft;
            this.strengthRight = strengthRight;

            rumbleElapsed = new int[4];
            for (int i = 0; i < 4; i++)
            {
                rumbleElapsed[i] = rumbleDuration;
            }
        }


        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
            {
                if (rumbleElapsed[i] < rumbleDuration)
                {
                    rumbleElapsed[i] += gameTime.ElapsedGameTime.Milliseconds;
                    if (rumbleElapsed[i] >= rumbleDuration)
                    {
                        // stop rumble
#if XBOX
                        GamePad.SetVibration(BubbleGame.IntToPI(i), .0f, .0f);
#endif
                    }
                }
            }
        }

        public void StartRumbleBurst(int playerIndex)
        {
            rumbleElapsed[playerIndex] = 0;

            // start rumble
#if XBOX
            GamePad.SetVibration(BubbleGame.IntToPI(playerIndex), strengthLeft, strengthRight);
#endif
        }

    }
}