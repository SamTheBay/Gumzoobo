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
    class RocketBlaster : Enemy
    {
        int blastElapsed = 0;
        int blastDuration;
        public FireballWeapon[] fireballs;
        bool isIdle = false;
        bool isIdleTrans = false;
        Vector2 prevMovement = new Vector2(0,0);

        public RocketBlaster(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "RocketBlaster", new Point(60,60), new Point(30,30), 5, new Vector2(30f,30f), currentDirection)
        {
            // add this enemies animations
            AddAnimation(new Animation("EnemyRight", 4, 5, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 4, 5, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyTransIdleRight", 3, 3, 100, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyTransIdleLeft", 3, 3, 100, false, SpriteEffects.None));
            AddAnimation(new Animation("EnemyIdleRight", 1, 2, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyIdleLeft", 1, 2, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 3, 3, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 3, 3, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 3, 3, 100, false, SpriteEffects.None, Color.Blue));
            isFlyer = true;
            movementSpeed = 4f;
            if (currentDirection == Direction.Right)
                projectile = new Projectile(new Vector2(movementSpeed, movementSpeed), 0);
            else
                projectile = new Projectile(new Vector2(-movementSpeed, movementSpeed), 0);
            projectile.isDegrading = false;
            projectile.isGravityEffected = false;
            projectile.isVirticleColiding = true;
            fireballs = new FireballWeapon[4];
            fireballs[0] = new FireballWeapon(this, 500, Direction.RightDown);
            fireballs[1] = new FireballWeapon(this, 500, Direction.LeftDown);
            fireballs[2] = new FireballWeapon(this, 500, Direction.RightUp);
            fireballs[3] = new FireballWeapon(this, 500, Direction.LeftUp);
            blastDuration = BubbleGame.rand.Next(3000, 7000);
        }



        public override void DetermineMove(GameTime gameTime)
        {
            base.DetermineMove(gameTime);

            if (isDead)
            {
                return;
            }

            // check if we can still move in the direction that we want to
            if (isIdle == false)
            {
                if (0 == CanMove(Direction.Right, (int)movementSpeed))
                {
                    projectile.VelX = -movementSpeed;
                    nextDirection = Direction.Left;
                }
                if (0 == CanMove(Direction.Left, (int)movementSpeed))
                {
                    projectile.VelX = movementSpeed;
                    nextDirection = Direction.Right;
                }
                if (0 == CanMove(Direction.Up, (int)movementSpeed))
                {
                    projectile.VelY = movementSpeed;
                }
                if ((int)movementSpeed > CanMove(Direction.Down, (int)movementSpeed))
                {
                    projectile.VelY = -movementSpeed;
                }
            }


            // handle blast
            if (shouldShoot == false)
            {
                if (!Level.singletonLevel.IsTimeStill)
                {
                    blastElapsed += gameTime.ElapsedGameTime.Milliseconds;
                }
                if (blastElapsed >= blastDuration)
                {
                    shouldShoot = true;
                    blastElapsed = 0;
                    blastDuration = BubbleGame.rand.Next(3000, 7000);
                }
            }


            // handle idleing
            if (isIdleTrans == false && isIdle == false && blastElapsed > blastDuration - 500)
            {
                prevMovement.X = projectile.VelX;
                prevMovement.Y = projectile.VelY;
                projectile.VelX = 0;
                projectile.VelY = 0;
                isIdleTrans = true;
                isIdle = true;
                PlayAnimation("EnemyTransIdle", currentDirection);
            }
            else if (isIdle == true && isIdle == true && blastElapsed > blastDuration - 400)
            {
                isIdleTrans = false;
                PlayAnimation("EnemyIdle", currentDirection);
            }
            else if (isIdle == true && isIdleTrans == false && blastElapsed > 400 && blastElapsed < blastDuration - 400)
            {
                isIdleTrans = true;
                PlayAnimation("EnemyTransIdle", currentDirection);
            }
            else if (isIdle == true && isIdleTrans == true && blastElapsed > 500 && blastElapsed < blastDuration - 400)
            {
                projectile.VelX = prevMovement.X;
                projectile.VelY = prevMovement.Y;
                isIdleTrans = false;
                isIdle = false;
                PlayAnimation("Enemy", currentDirection);
            }
        }



        public override void Shoot()
        {
            base.Shoot();
            for (int i = 0; i < fireballs.Length; i++)
            {
                fireballs[i].Activate();
            }   
        }


    }
}