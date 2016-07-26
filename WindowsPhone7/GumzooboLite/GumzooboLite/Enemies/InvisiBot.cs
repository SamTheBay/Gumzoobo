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
    class InvisiBot : Enemy
    {
        bool isVisible = true;
        bool startAudio = true;
        int actionTimer = 0;
        int visibleTime = 3000;
        int flashTime = 3500;
        int fastFlashTime = 4000;
        int invisibleTime = 7000;

        public InvisiBot(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "Invisabot", new Point(40, 68), new Point(20, 34), 5, new Vector2(20f, 34f), currentDirection)
        {
            // add this enemies animations
            AddAnimation(new Animation("EnemyRight",1, 5, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 1, 5, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 1, 1, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 1, 1, 100, false, SpriteEffects.None, Color.Blue));

            // randomize when the character is invisible
            actionTimer = BubbleGame.rand.Next(0, 7000);
        }


        public override void DetermineMove(GameTime gameTime)
        {
            base.DetermineMove(gameTime);

            if (isDead)
            {
                return;
            }

            // check if we can still move in the direction that we want to
            if (0 == CanMove(currentDirection, (int)movementSpeed))
            {
                if (currentDirection == Direction.Right)
                {
                    nextDirection = Direction.Left;
                }
                else
                {
                    nextDirection = Direction.Right;
                }
            }
            else if (actionTimer > fastFlashTime)
            {
                // add random movements when invisible

            }
        }

        public override void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (isVisible)
                base.Draw(spriteBatch, layerDepth);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Stuck || isDead)
            {
                isVisible = true;
                return;
            }

            // count for action
            if (!Level.singletonLevel.IsTimeStill)
            {
                actionTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (actionTimer >= invisibleTime)
            {
                // loop the action
                actionTimer = 0;
                isVisible = true;
            }
            else if (actionTimer >= fastFlashTime)
            {
                // invisible period
                isVisible = false;
            }
            else if (actionTimer >= flashTime)
            {
                // do fast flash
                int flashtime = 50;
                if ((actionTimer / flashtime) % 2 == 0)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            else if (actionTimer >= visibleTime)
            {
                // do slow flash
                if (startAudio == true)
                {
                    AudioManager.audioManager.PlaySFX("Invisable");
                    startAudio = false;
                }
                int flashtime = 100;
                if ((actionTimer / flashtime) % 2 == 0)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            else
            {
                // visible period
                startAudio = true;
                isVisible = true;
            }
        }
    }
}