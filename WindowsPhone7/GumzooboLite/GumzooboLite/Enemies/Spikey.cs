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
    class Spikey : Enemy
    {
        int jumpTime = 3000;
        int jumpTimer = 0;

        public Spikey(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "PorkyBot", new Point(62, 75), new Point(31, 37), 12, new Vector2(31f, 37f), currentDirection)
        {
            reduceTop = baseReduceTop = 15;

            // add this enemies animations
            AddAnimation(new Animation("EnemyRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 1, 6, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 1, 1, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 1, 1, 100, false, SpriteEffects.None, Color.Blue));
            AddAnimation(new Animation("EnemyJumpRight", 7, 12, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyJumpLeft", 7, 12, 100, false, SpriteEffects.None));
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

            if (!Level.singletonLevel.IsTimeStill)
            {
                jumpTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (jumpTimer >= jumpTime)
            {
                shouldJump = true;
                jumpTimer = 0;
            }
        }

        public override void CollisionAction(GameSprite otherSprite)
        {
            if (otherSprite is WeaponSprite)
            {
                WeaponSprite weapon = (WeaponSprite)otherSprite;
                if (weapon is CinnemonWeapon && weapon.IsLethal)
                {
                    Die();
                }
                else if (weapon is MintWeapon && weapon.IsLethal && !Stuck && !isDead)
                {
                    // we are frozen
                    StuckElapsed = 0;
                    isFrozen = true;
                    PlayAnimation("EnemyFrozen", currentDirection);
                    weapon.RewardPoints(20);
                }
            }
            else if (otherSprite is PlayerSprite)
            {
                if (Stuck && !IsDead)
                {
                    // we got hit stuck, time to die
                    Die();

                    PlayerSprite player = (PlayerSprite)otherSprite;
                    player.RewardPoints(1000);
                }
            }
        }
    }
}