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
    public class Enemy : AnimatedSprite
    {
        // AI variables
        protected float movementSpeed = 4f;
        protected Direction currentDirection = Direction.Right;
        protected Direction nextDirection = Direction.Right;
        protected bool shouldJump = false;
        protected bool shouldShoot = false;
        protected bool justUnstuck = false;

        // State variables
        protected int StuckElapsed = Level.singletonLevel.Details.enemyStuckTime;
        protected int StuckDuration = Level.singletonLevel.Details.enemyStuckTime;
        protected bool isDead = false;
        protected int deadDuration = 100;
        protected int deadElapsed = 0;
        protected int jumpDuration = 300;
        protected int jumpElapsed = 0;
        protected bool isJumping = false;
        protected bool wasInAir = false;
        protected float jumpSpeed = 5f;
        protected bool canAirMove = false;
        protected bool isFrozen = false;
        protected bool isFlyer = false;

        // Base Texture Info
        protected string baseTextureName;
        protected Point baseFrameDimensions;
        protected Point baseFrameOrigin;
        protected Vector2 baseSourceOffset;
        protected int baseFramesPerRow;
        protected int baseCenteredReduce;
        protected int baseReduceTop;

        // Bubble Texture Info
        protected string bubbleTextureName = "Bubble";
        protected Point bubbleDimensions = new Point(120,120);
        protected Point bubbleOrigin = new Point(60,60);
        protected Vector2 bubbleOffset =  new Vector2(60f, 60f);
        protected int bubbleFramesPerRow = 9;
        protected int bubbleCenteredReduce = 74;
        protected int bubbleReduceTop = 0;

        // Projectile for death
        protected Projectile projectile = new Projectile(new Vector2(10, -10), 4);


        public Enemy(Vector2 startPosition)
            : base("Enemies", new Point(60, 60), new Point(30, 30), 9, new Vector2(30f, 30f), startPosition)
        {

        }

        public Enemy(Vector2 startPosition, string textureName, Point frameDimensions, Point FrameCenter, int framesPerLine, Vector2 Center, Direction currentDirection)
            : base(textureName, frameDimensions, FrameCenter, framesPerLine, Center, startPosition)
        {
            baseTextureName = textureName;
            baseFrameDimensions = frameDimensions;
            baseFrameOrigin = FrameCenter;
            baseSourceOffset = Center;
            baseFramesPerRow = framesPerLine;
            baseReduceTop = reduceTop;
            baseCenteredReduce = centeredReduce;
            this.currentDirection = currentDirection;
            this.nextDirection = currentDirection;
            isPawn = true;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // check if we need to go inactive
            if (isDead)
            {
                deadElapsed++;
                if (deadElapsed >= deadDuration || projectile.IsStopped == true)
                {
                    Deactivate();
                }
                else if (projectile.IsStopped == false)
                {
                    position = projectile.UpdatePosition(position, new Point(60, 60), gameTime, Level.singletonLevel.EnvSprites);
                }
                return;
            }

            bool inAir = (0 != CanMove(Direction.Down, (int)Level.singletonLevel.Gravity));
            if (wasInAir == true && inAir == false && isFrozen == false && Stuck == false && isDead == false)
            {
                PlayAnimation("Enemy", currentDirection);
            }
            wasInAir = inAir;

            // update timers
            justUnstuck = false;
            if (Stuck && !isDead && !Level.singletonLevel.IsTimeStill)
            {
                StuckElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (Stuck)
                {
                    if (isFrozen)
                    {
                        // need to fall to the ground
                        if (inAir)
                            position.Y += CanMove(Direction.Down, (int)Level.singletonLevel.Gravity);
                    }
                    else
                    {
                        position.Y -= Level.singletonLevel.Details.bubbleRiseSpeed; // float up
                    }

                    // check if we are close to popping
                    if (StuckDuration - StuckElapsed <= 4000)
                    {
                        if (currentAnimation.Name != "EnemyStuckLate" && !isFrozen)
                            PlayAnimation("EnemyStuckLate");
                    }

                    return;     // can't move if we are stuck
                }
                else
                {
                    justUnstuck = true;
                    isFrozen = false;
                }

                
            }

            if (justUnstuck && !isDead)
            {
                SwapAnimation(baseTextureName, baseFrameDimensions, baseFrameOrigin, baseFramesPerRow, baseSourceOffset);
                centeredReduce = baseCenteredReduce;
                reduceTop = baseReduceTop;
                PlayAnimation("Enemy" + currentDirection);
            }

            // Run AI logic to determine how this enemy wants to move
            DetermineMove(gameTime);

            // update the character
            if (nextDirection != currentDirection)
            {
                // change directions
                currentDirection = nextDirection;
                PlayAnimation("Enemy", currentDirection);
            }

            // jump
            if (shouldJump && !inAir && !Level.singletonLevel.IsTimeStill)
            {
                shouldJump = false;
                isJumping = true;
                jumpElapsed = 0;
                PlayAnimation("EnemyJump", currentDirection);
            }

            // shoot
            if (shouldShoot == true && !Level.singletonLevel.IsTimeStill)
            {
                Shoot();
                shouldShoot = false;
            }

            // handle movement
            if (!isFlyer && !Level.singletonLevel.IsTimeStill)
            {
                if (inAir && !isJumping)
                {
                    position.Y += CanMove(Direction.Down, (int)Level.singletonLevel.Gravity);
                }
                else if (isJumping)
                {
                    position.Y -= jumpSpeed * (((float)jumpDuration - (float)jumpElapsed) / (float)jumpDuration);
                }
                if ((canAirMove || !inAir) && 0 != CanMove(currentDirection, (int)movementSpeed))
                {
                    if (currentDirection == Direction.Right)
                        position.X += movementSpeed;
                    else
                        position.X -= movementSpeed;
                }
            }
            else if (!Level.singletonLevel.IsTimeStill)
            {
                position = projectile.UpdatePosition(position, new Point(60, 60), gameTime, Level.singletonLevel.EnvSprites);
            }

            // adjust state
            if (jumpElapsed >= jumpDuration)
            {
                isJumping = false;
            }
            else if (!Level.singletonLevel.IsTimeStill)
            {
                jumpElapsed += gameTime.ElapsedGameTime.Milliseconds;
            }

        }



        public override void CollisionAction(GameSprite otherSprite)
        {
            base.CollisionAction(otherSprite);

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
                else if (weapon.IsPlayerGenerated && !Stuck && !isDead && weapon.IsLethal)
                {
                    // we have been hit by a player weapon
                    SwapAnimation(bubbleTextureName, bubbleDimensions, bubbleOrigin, bubbleFramesPerRow, bubbleOffset);
                    centeredReduce = bubbleCenteredReduce;
                    reduceTop = bubbleReduceTop;
                    StuckElapsed = 0;
                    PlayAnimation("EnemyStuck");
                    weapon.RewardPoints(10);
                    weapon.NoPop();
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


        public virtual void DetermineMove(GameTime gameTime)
        {
            // AI logic for this enemy goes here
        }


        public virtual void Shoot()
        {
            // What happens when this enemy shoots goes here
        }


        public virtual void Die()
        {
            if (Stuck)
            {
                SwapAnimation(baseTextureName, baseFrameDimensions, baseFrameOrigin, baseFramesPerRow, baseSourceOffset);
                centeredReduce = baseCenteredReduce;
                reduceTop = baseReduceTop;
            }

            if (Stuck && !isFrozen)
                AudioManager.audioManager.PlaySFX("BubblePop");

            isDead = true;
            isFrozen = false;
            StuckElapsed = StuckDuration;
            if (currentDirection == Direction.Right)
                projectile = new Projectile(new Vector2(5, -13), Level.singletonLevel.Gravity);
            else
                projectile = new Projectile(new Vector2(-5, -13), Level.singletonLevel.Gravity);

            // run death animation
            PlayAnimation("EnemyDead");
        }


        public override void Activate()
        {
            base.Activate();

            isDead = false;
            shouldJump = false;
            shouldShoot = false;
            justUnstuck = false;
            isFrozen = false;
            StuckElapsed = StuckDuration;
            deadElapsed = 0;
            PlayAnimation("Enemy", currentDirection);
        }

        public override void Deactivate()
        {
            base.Deactivate();

            Level.singletonLevel.AddSprite(new ItemSprite(position));
        }

        public void SetRandomPosition()
        {
            position.X = BubbleGame.rand.Next(250, 1000);
            position.Y = BubbleGame.rand.Next(25, 650);
        }

        // Accessors
        public virtual bool Stuck
        {
            get { return StuckElapsed < StuckDuration && !isDead; }
        }

        public bool IsDead
        {
            get { return isDead; }
        }
    }
}