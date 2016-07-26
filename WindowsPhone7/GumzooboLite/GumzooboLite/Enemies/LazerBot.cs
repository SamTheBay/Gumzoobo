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
    class LazerBot : Enemy
    {
        int ShootDuration = 3000;
        int ShootElapsed = 0;
        public LazerWeapon lazer;


        public LazerBot(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "LazerBot", new Point(45,77), new Point(22, 38), 6, new Vector2(22f, 38f), currentDirection)
        {
            // add this enemies animations
            AddAnimation(new Animation("EnemyRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 1, 6, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 1, 1, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 1, 1, 100, false, SpriteEffects.None, Color.Blue));
            lazer = new LazerWeapon(this);

            // randomize shoot time
            ShootElapsed = BubbleGame.rand.Next(0, ShootDuration);
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

            // check if we should shoot
            if (!Level.singletonLevel.IsTimeStill)
            {
                ShootElapsed += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (ShootElapsed > ShootDuration)
            {
                shouldShoot = true;
                ShootElapsed = 0;
            }

        }


        public override void Activate()
        {
            base.Activate();

            ShootElapsed = 0;
        }


        public override void Shoot()
        {
            base.Shoot();

            // activate your lazer on the map
            if (lazer.IsActive == false)
            {
                lazer.Activate();
            }
        }


    }
}