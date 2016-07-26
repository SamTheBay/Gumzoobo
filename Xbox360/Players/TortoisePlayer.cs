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
    class TortoisePlayer : PlayerSprite
    {
        // variables for special move
        bool isLeavingSpecial = false;
        bool isSpecialActive = false;
        int inSpecialDuration = 300;
        int outSpecialDuration = 300;
        int specialElapsed = 0;

        static Color staticColor = new Color(0, 102, 102);
        static Color staticLightColor = new Color(102, 255, 94);

        public TortoisePlayer(int controllerIndex)
            : base(controllerIndex, "Tortoise", new Point(72, 62), new Point(36, 31), 24, new Vector2(36f, 31f), new Vector2(250, 250))
        {
            // characteristics
            name = TortoisePlayer.StaticName();
            hasSelectedCharacter = true;
            reduceTop = 22;
            reduceSides = 12;
            stripeTexture = InternalContentManager.GetTexture("TortoiseStripe");
            lightColor = TortoisePlayer.StaticLightColor();
            darkColor = TortoisePlayer.StaticColor();
            Continues = 0;

            // Animations
            AddAnimation(new Animation("PlayerRight", 1, 9, 60, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerLeft", 1, 9, 60, true, SpriteEffects.None));
            AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerDeadRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally, .3f));
            AddAnimation(new Animation("PlayerDeadLeft", 1, 1, 100, true, SpriteEffects.None, -0.3f));
            AddAnimation(new Animation("TortoiseHideRight", 15, 19, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("TortoiseHideLeft", 15, 19, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("EndTortoiseHideRight", 20, 23, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EndTortoiseHideLeft", 20, 23, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerStartJumpRight", 10, 12, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerStartJumpLeft", 10, 12, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("PlayerEndJumpRight", 13, 14, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("PlayerEndJumpLeft", 13, 14, 100, false, SpriteEffects.None));

            characterFrame = new Rectangle(0, 0, 72, 62);

        }

        public override void Update(GameTime gameTime)
        {
            // handle updates if we are doing our special
            if (inSpecial && !isDead)
            {
                if (specialElapsed == 0 && isLeavingSpecial == false)
                {
                    // go into hide
                    PlayAnimation("TortoiseHide", lastDirection);
                    AudioManager.PlayCue("TurtleInShell");
                    canMove = false;
                    canChew = false;
                    canShoot = false;
                }
                specialElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (!InputManager.IsActionPressed(InputManager.Action.Special, controllerIndex) && isLeavingSpecial == false)
                {
                    isLeavingSpecial = true;
                    isSpecialActive = false;
                    specialElapsed = 0;
                }

                if (isLeavingSpecial == true && specialElapsed == 0)
                {
                    // come out of hiding
                    int startFrame = 4 - currentFrameOffset;
                    if (startFrame == -1)
                        startFrame = 0;
                    outSpecialDuration = currentFrameOffset * 100;
                    PlayAnimation("EndTortoiseHide", lastDirection, startFrame);
                    if (startFrame == 0)
                        AudioManager.PlayCue("TurtleOutShell");
                }
                if (isLeavingSpecial == false && specialElapsed >= inSpecialDuration && isSpecialActive == false)
                {
                    // special is now active
                    isSpecialActive = true;
                }
                if (isLeavingSpecial == true && specialElapsed >= outSpecialDuration)
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
                    canMove = true;
                    canChew = true;
                    canShoot = true;
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
                isLeavingSpecial = false;
            }
        }

        public override void Damage(int damage)
        {
            if (isSpecialActive == false)
                base.Damage(damage);
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
            return "Froofy";
        }

    }
}