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
    class ToadPlayer : PlayerSprite
    {
        // variables for special move
        bool hasWrappedUp = false;
        int specialElapsed = 0;

        static Color staticColor = new Color(76, 81, 0);
        static Color staticLightColor = new Color(255, 187, 79);

        public ToadPlayer(int controllerIndex)
            : base(controllerIndex, "Toad", new Point(120, 70), new Point(60, 35), 14, new Vector2(60f, 35f), new Vector2(250, 250))
        {
            // characteristics
            name = ToadPlayer.StaticName();
            hasSelectedCharacter = true;
            reduceTop = 20;
            reduceSides = 60;
            lightColor = ToadPlayer.StaticLightColor();
            darkColor = ToadPlayer.StaticColor();
            Continues = 0;

            // animations
            AddAnimation(new Animation("PlayerRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerLeft", 1, 6, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerDeadRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally, .3f));
            AddAnimation(new Animation("PlayerDeadLeft", 1, 1, 100, true, SpriteEffects.None, -0.3f));
            AddAnimation(new Animation("ToadJumpRight", 7, 12, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("ToadJumpLeft", 7, 12, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("EndToadJumpRight", 13, 13, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EndToadJumpLeft", 13, 13, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerStartJumpRight", 7, 12, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerStartJumpLeft", 7, 12, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerEndJumpRight", 13, 13, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerEndJumpLeft", 13, 13, 100, false, SpriteEffects.None));

            characterFrame = new Rectangle(0, 0, 86, 70);
        }

        public override void Update(GameTime gameTime)
        {
            // handle updates if we are doing our special
            if (inSpecial && !isDead)
            {
                if (specialElapsed == 0 && hasWrappedUp == false)
                {
                    // go into slide
                    PlayAnimation("ToadJump", lastDirection);
                    canMove = false;
                    forcedDirection = Direction.Up;
                    forcedMove = true;
                    jumpDuration = 35;
                    jumpElapsed = 0;
                    jumpSpeed = 14f;

                    AudioManager.audioManager.PlaySFX("SuperJump");
                }
                specialElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (IsJumping == false)
                {
                    // come out of jump
                    PlayAnimation("EndToadJump", lastDirection);
                    canMove = true;
                    forcedDirection = Direction.Up;
                    forcedMove = false;
                    jumpDuration = 25;
                    jumpElapsed = jumpDuration;
                    inSpecial = false;
                    jumpSpeed = 10f;
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
            return "Clavis";
        }

    }
}