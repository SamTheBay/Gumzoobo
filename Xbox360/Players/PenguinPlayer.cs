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
    class PenguinPlayer : PlayerSprite
    {
        // variables for special move
        bool hasRevived = true;
        int specialDuration = 500;
        int specialElapsed = 0;

        static Color staticColor = new Color(0, 0, 102);
        static Color staticLightColor = new Color(91, 124, 255);


        public PenguinPlayer(int controllerIndex)
            : base(controllerIndex, "Penguin", new Point(42,74), new Point(21,37), 14,new Vector2(21f, 37f), new Vector2(250,250))
        {
            // characteristics
            name = PenguinPlayer.StaticName();
            hasSelectedCharacter = true;
            reduceTop = 14;
            reduceSides = -18;
            stripeTexture = InternalContentManager.GetTexture("PenguinStripe");
            lightColor = PenguinPlayer.StaticLightColor();
            darkColor = PenguinPlayer.StaticColor();
            Continues = 0;

            // animations
            AddAnimation(new Animation("PlayerRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerLeft", 1, 6, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerDeadRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally, .3f));
            AddAnimation(new Animation("PlayerDeadLeft", 1, 1, 100, true, SpriteEffects.None, -0.3f));
            AddAnimation(new Animation("PenguinHoverRight", 12, 13, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PenguinHoverLeft", 12, 13, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("PlayerStartJumpRight", 7, 9, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerStartJumpLeft", 7, 9, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerEndJumpRight", 10, 11, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerEndJumpLeft", 10, 11, 100, false, SpriteEffects.None));

            characterFrame = new Rectangle(0, 0, 42, 74);
        }

        public override void Update(GameTime gameTime)
        {
            // handle updates if we are doing our special
            if (inSpecial && !isDead)
            {
                if (specialElapsed == 0)
                {
                    // go into hover
                    PlayAnimation("PenguinHover", lastDirection);
                    AudioManager.PlayCue("PenguinFlapping");
                }
                specialElapsed += gameTime.ElapsedGameTime.Milliseconds;
                position.Y -= Level.singletonLevel.Gravity;

                if (specialElapsed >= specialDuration || !InputManager.IsActionPressed(InputManager.Action.Special, controllerIndex))
                {
                    // resume normal behavior
                    if (InputManager.IsActionPressed(InputManager.Action.MoveCharacterRight, controllerIndex))
                    {
                        PlayAnimation("Player", Direction.Right);
                    }
                    else if (InputManager.IsActionPressed(InputManager.Action.MoveCharacterLeft, controllerIndex))
                    {
                        PlayAnimation("Player", Direction.Left);
                    }
                    else
                    {
                        PlayAnimation("Idle", lastDirection);
                    }
                    inSpecial = false;
                }
            }
            else if (InAir() == false)
            {
                hasRevived = true;
            }

            base.Update(gameTime);
        }

        // build the seal's special ability
        protected override void Special()
        {
            base.Special();

            if (inSpecial == false && InAir() == true && IsJumping == false && hasRevived == true)
            {
                inSpecial = true;
                specialElapsed = 0;
                hasRevived = false;
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
            return "Cheekeze";
        }

    }
}