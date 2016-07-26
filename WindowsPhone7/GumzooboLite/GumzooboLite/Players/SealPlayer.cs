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
    class SealPlayer : PlayerSprite
    {
        // variables for special move
        bool hasWrappedUp = false;
        int specialDuration = 750;
        int specialWrapUp = 600;
        int specialElapsed = 0;

        static Color staticColor = new Color(58, 0, 91);
        static Color staticLightColor = new Color(255, 109, 255);


        public SealPlayer(int controllerIndex)
            : base(controllerIndex, "Seal", new Point(80,80), new Point(40,40), 14,new Vector2(40f, 40f), new Vector2(250,250))
        {
            // characteristics
            name = SealPlayer.StaticName();
            hasSelectedCharacter = true;
            reduceTop = 30;
            reduceSides = 20;
            lightColor = SealPlayer.StaticLightColor();
            darkColor = SealPlayer.StaticColor();
            Continues = 0;

            // animations
            AddAnimation(new Animation("PlayerRight", 1, 6, 70, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerLeft", 1, 6, 70, true, SpriteEffects.None));
            AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerDeadRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally, .3f));
            AddAnimation(new Animation("PlayerDeadLeft", 1, 1, 100, true, SpriteEffects.None, -0.3f));
            AddAnimation(new Animation("SealSlideRight", 12, 13, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("SealSlideLeft", 12, 13, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("EndSealSlideRight", 13, 14, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EndSealSlideLeft", 13, 14, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerStartJumpRight", 7, 9, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerStartJumpLeft", 7, 9, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerEndJumpRight", 10, 11, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerEndJumpLeft", 10, 11, 100, false, SpriteEffects.None));

            characterFrame = new Rectangle(0,0,80,80);
        }

        public override void Update(GameTime gameTime)
        {
            // handle updates if we are doing our special
            if (inSpecial && !isDead)
            {
                if (specialElapsed == 0)
                {
                    // go into slide
                    AudioManager.audioManager.PlaySFX("SealSlide");
                    PlayAnimation("SealSlide", lastDirection);
                    forcedMove = true;
                    canMove = false;
                    canChew = false;
                    canShoot = false;
                    movementSpeed += 4f;
                    forcedDirection = lastDirection;
                }
                specialElapsed += gameTime.ElapsedGameTime.Milliseconds;
              
                if (hasWrappedUp == false && specialElapsed >= specialWrapUp)
                {
                    // come out of slide
                    PlayAnimation("EndSealSlide", lastDirection);
                }
                if (specialElapsed >= specialDuration)
                {
                    // resume normal behavior
                    PlayAnimation("Player", lastDirection);
                    inSpecial = false;
                    forcedMove = false;
                    canMove = true;
                    canChew = true;
                    canShoot = true;
                    movementSpeed -= 4f;
                }
            }

            base.Update(gameTime);
        }

        // build the seal's special ability
        protected override void Special()
        {
            base.Special();

            if (inSpecial == false && InAir() == false)
            {
                inSpecial = true;
                specialElapsed = 0;
                hasWrappedUp = false;
            }
        }


        public static Color StaticColor()
        {
            return staticColor;
        }

        public static Color StaticLightColor()
        {
            return staticLightColor;
        }



        public static string StaticName()
        {
            return "Bupper";
        }

    }
}