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
    class FireDrone : Enemy
    {

        public FireballWeapon fireball;
        int shootduration;
        int shootelapsed = 0;

        public FireDrone(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "FireDrone", new Point(35,51), new Point(17, 25), 4, new Vector2(17f, 25f), currentDirection)
        {
            // add this enemies animations
            AddAnimation(new Animation("EnemyRight", 1, 4, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 1, 4, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 1, 1, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 1, 1, 100, false, SpriteEffects.None, Color.Blue));

            fireball = new FireballWeapon(this, 3000, this.currentDirection);
            shootduration = BubbleGame.rand.Next(1000, 5000);
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

            // handle shooting
            if (!Level.singletonLevel.IsTimeStill)
            {
                shootelapsed += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (shootelapsed >= shootduration)
            {
                shouldShoot = true;
                shootelapsed = 0;
                shootduration = BubbleGame.rand.Next(1000, 5000);
            }
        }


        public override void Shoot()
        {
            base.Shoot();

            if (fireball.IsActive == false)
            {
                fireball.SetDirection(currentDirection);
                fireball.Activate();
            }
        }


    }
}