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
    class Hunter : Enemy
    {
        public Hunter(Vector2 startPosition, Direction currentDirection)
            : base(startPosition, "Hunter", new Point(70,60), new Point(35, 30), 6, new Vector2(35f, 30f), currentDirection)
        {
            // add this enemies animations
            AddAnimation(new Animation("EnemyRight", 1, 3, 100, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("EnemyLeft", 1, 3, 100, true, SpriteEffects.None));
            AddAnimation(new Animation("EnemyStuck", 8, 8, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyStuckLate", 9, 9, 100, true, SpriteEffects.None, .25f));
            AddAnimation(new Animation("EnemyDead", 1, 1, 100, true, SpriteEffects.None, .7f));
            AddAnimation(new Animation("EnemyFrozenRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
            AddAnimation(new Animation("EnemyFrozenLeft", 1, 1, 100, false, SpriteEffects.None, Color.Blue));
            isFlyer = true;
            movementSpeed = 2f;
            projectile = new Projectile(new Vector2(movementSpeed, movementSpeed), 0);
            projectile.isDegrading = true;
            projectile.isGravityEffected = false;
            projectile.isVirticleColiding = true;
            projectile.isCollisionDetecting = false;
        }



        public override void DetermineMove(GameTime gameTime)
        {
            base.DetermineMove(gameTime);

            if (isDead)
            {
                return;
            }

            // check if we can still move in the direction that we want to
            PlayerSprite closestPlayer = FindClosestPlayer();
            if (closestPlayer == null)
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
            else
            {
                // create a vector that points towards the closest player
                Vector2 movement = new Vector2();
                movement.X = closestPlayer.Position.X - this.position.X;
                movement.Y = closestPlayer.Position.Y - this.position.Y;
                if (Math.Abs(movement.X) < movementSpeed)
                    movement.X = 0;
                if (Math.Abs(movement.Y) < movementSpeed)
                    movement.Y = 0;

                // normalize vector
                float distance = (float)Math.Sqrt((float)Math.Pow(movement.X, 2) + (float)Math.Pow(movement.Y, 2));
                if (distance != 0)
                {
                    movement.X /= distance;
                    movement.Y /= distance;
                }

                // apply speed
                movement.X *= movementSpeed;
                movement.Y *= movementSpeed;

                // set direction of sprite
                if (movement.X > 0)
                    nextDirection = Direction.Right;
                else
                    nextDirection = Direction.Left;

                // fix for collisions (do twice because we modify once
                for (int loop = 0; loop < 2; loop++)
                {
                    if (movement.X > 0f && 0 == CanMove(Direction.Right, (int)Math.Abs(movement.X) + 1))
                    {
                        if (movement.Y != 0f)
                        {
                            if (movement.Y > 0f)
                                movement.Y += Math.Abs(movement.X);
                            else
                                movement.Y -= Math.Abs(movement.X);
                        }
                        movement.X = 0f;
                    }
                    else if (movement.X < 0f && 0 == CanMove(Direction.Left, (int)Math.Abs(movement.X) + 1))
                    {
                        if (movement.Y != 0f)
                        {
                            if (movement.Y > 0f)
                                movement.Y += Math.Abs(movement.X);
                            else
                                movement.Y -= Math.Abs(movement.X);
                        }
                        movement.X = 0f;
                    }
                    if (movement.Y > 0f && (int)Math.Abs(movement.Y) >= CanMove(Direction.Down, (int)Math.Abs(movement.Y) + 1))
                    {
                        if (movement.X != 0f)
                        {
                            if (movement.X > 0f)
                                movement.X += Math.Abs(movement.Y);
                            else
                                movement.X -= Math.Abs(movement.Y);
                        }
                        movement.Y = 0f;
                    }
                    else if (movement.Y < 0f && 0 == CanMove(Direction.Up, (int)Math.Abs(movement.Y) + 1))
                    {
                        if (movement.X != 0f)
                        {
                            if (movement.X > 0f)
                                movement.X += Math.Abs(movement.Y);
                            else
                                movement.X -= Math.Abs(movement.Y);
                        }
                        movement.Y = 0f;
                    }
                }

                projectile.Velocity = movement;
            }

        }



        private PlayerSprite FindClosestPlayer()
        {
            int closestPlayerIndex = -1;
            double closetPlayerDistance = 99999f;
            PlayerSprite[] players = BubbleGame.players;
            // loop through active players
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].IsActive)
                {
                    // compute distance
                    double distance = Math.Sqrt(Math.Pow(((double)this.position.X - (double)players[i].Position.X), 2) + Math.Pow(((double)this.position.Y - (double)players[i].position.Y), 2));
                    if (distance < closetPlayerDistance)
                    {
                        closestPlayerIndex = i;
                        closetPlayerDistance = distance;
                    }
                }
            }

            if (closestPlayerIndex != -1)
                return players[closestPlayerIndex];
            else
                return null;
        }
    }
}