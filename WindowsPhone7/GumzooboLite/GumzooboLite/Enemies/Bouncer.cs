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
    class Bouncer : Enemy
    {

        public Bouncer(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "Bouncer", new Point(48,66), new Point(24, 33), 7, new Vector2(24f, 33f), currentDirection)
        {
            // add this enemies animations
            AddAnimation(new Animation("EnemyRight", 1, 7, 75, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 1, 7, 75, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 1, 1, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 1, 1, 100, false, SpriteEffects.None, Color.Blue));
            AddAnimation(new Animation("EnemyJumpRight", 1, 7, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyJumpLeft", 1, 7, 100, false, SpriteEffects.None));
            canAirMove = true;
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

            // check if we want to jump
            if (0 == CanMove(Direction.Down, (int)Level.singletonLevel.Gravity) && isJumping == false)
            {
                shouldJump = true;
            }
        }
    }
}