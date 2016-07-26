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
    class RocketPacker : Enemy
    {

        public RocketPacker(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "RocketBot", new Point(60,60), new Point(30,30), 5, new Vector2(30f,30f), currentDirection)
        {
            // add this enemies animations
            AddAnimation(new Animation("EnemyRight", 4, 5, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 4, 5, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyRightUp", 4, 5, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeftUp", 4, 5, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyRightDown", 4, 5, 100, true, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically));
            AddAnimation(new Animation("EnemyLeftDown", 4, 5, 100, true, SpriteEffects.FlipVertically));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 3, 3, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 3, 3, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 3, 3, 100, false, SpriteEffects.None, Color.Blue));
            isFlyer = true;
            movementSpeed = 4f;
            if (currentDirection == Direction.RightUp)
                projectile = new Projectile(new Vector2(movementSpeed, movementSpeed), 0);
            else
                projectile = new Projectile(new Vector2(-movementSpeed, movementSpeed), 0);
            projectile.isDegrading = false;
            projectile.isGravityEffected = false;
            projectile.isVirticleColiding = true;
        }



        public override void DetermineMove(GameTime gameTime)
        {
            base.DetermineMove(gameTime);

            if (isDead)
            {
                return;
            }

            // check if we can still move in the direction that we want to
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
    }
}