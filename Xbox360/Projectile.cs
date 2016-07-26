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
    public class Projectile
    {
        protected Vector2 velocity;
        protected float gravity;
        protected float gravityDelta = .25f;
        protected bool isStopped = false;
        public bool isCollisionDetecting = true;
        public bool isDegrading = true;
        public bool isGravityEffected = true;
        public bool isVirticleColiding = false;

        public Projectile(Vector2 initialVelocity, float gravity)
        {
            this.velocity = initialVelocity;
            this.gravity = gravity;
        }

        public Vector2 UpdatePosition(Vector2 position, Point frameDimensions, GameTime time, List<EnvironmentSprite> env)
        {
            if (isStopped == true)
            {
                return position;
            }

            // modify for velocity
            if (isGravityEffected)
            {
                if (velocity.Y < gravity)
                {
                    velocity.Y += gravityDelta;
                    if (velocity.Y > gravity)
                    {
                        velocity.Y = gravity;
                    }
                }
            }

            // check if we have landed on something
            if (velocity.Y > 1 && 0 == CanMoveProjectile(Direction.Down, position, frameDimensions, (int)Math.Abs(velocity.Y), env))
            {
                isStopped = true;
            }

            // check if we can move in directions
            if (isCollisionDetecting)
            {
                if (velocity.X > 0)
                {
                    if (0 == CanMoveProjectile(Direction.Right, position, frameDimensions, (int)Math.Abs(velocity.X), env))
                    {
                        if (isDegrading)
                            velocity.X = 0;
                    }
                }
                else if (velocity.X < 0)
                {
                    if (0 == CanMoveProjectile(Direction.Left, position, frameDimensions, (int)Math.Abs(velocity.X), env))
                    {
                        if (isDegrading)
                            velocity.X = 0;
                    }
                }
                if (isVirticleColiding)
                {
                    if (velocity.Y > 0)
                    {
                        if (0 == CanMoveProjectile(Direction.Down, position, frameDimensions, (int)Math.Abs(velocity.X), env))
                        {
                            if (isDegrading)
                                velocity.Y = 0;
                        }
                    }
                    else if (velocity.Y < 0)
                    {
                        if (0 == CanMoveProjectile(Direction.Up, position, frameDimensions, (int)Math.Abs(velocity.X), env))
                        {
                            if (isDegrading)
                                velocity.Y = 0;
                        }
                    }
                }
            }


            // update position
            position += velocity;

            return position;
        }

        public Vector2 UpdatePosition(Vector2 position, Point frameDimesions, GameTime time)
        {
            return UpdatePosition(position, frameDimesions, time, new List<EnvironmentSprite>());
        }


        public virtual int CanMoveProjectile(Direction direction, Vector2 position, Point FrameDimensions, int ammount, List<EnvironmentSprite> env)
        {
            if (direction == Direction.Down)
            {
                Rectangle bottomRectangle = new Rectangle((int)position.X, (int)position.Y + FrameDimensions.Y + 1, FrameDimensions.X, ammount);

                // check if there is an environment sprite below us
                for (int i = 0; i < env.Count; i++)
                {
                    if (bottomRectangle.Intersects(env[i].TopBox))
                    {
                        return 0;
                    }
                }
            }

            if (direction == Direction.Up)
            {
                Rectangle topRectangle = new Rectangle((int)position.X, (int)position.Y - 1, FrameDimensions.X, ammount);

                // check if there is an environment sprite below us
                for (int i = 0; i < env.Count; i++)
                {
                    if (topRectangle.Intersects(env[i].BottomBox))
                    {
                        return 0;
                    }
                    // check for screen wrapping
                    
                }
            }

            if (direction == Direction.Left)
            {
                if (position.X <= 208f + 24f)
                {
                    return 0;
                }

                Rectangle leftRectangle = new Rectangle((int)position.X - 1, (int)position.Y, ammount, FrameDimensions.Y);

                // check if there is an environment sprite below us
                for (int i = 0; i < env.Count; i++)
                {
                    if (leftRectangle.Intersects(env[i].RightBox))
                    {
                        return 0;
                    }
                }
            }

            if (direction == Direction.Right)
            {
                if (position.X >= 1072f - FrameDimensions.X - 24)
                {
                    return 0;
                }

                Rectangle rightRectangle = new Rectangle((int)position.X + FrameDimensions.X + 1, (int)position.Y, ammount, FrameDimensions.Y);

                // check if there is an environment sprite below us
                for (int i = 0; i < env.Count; i++)
                {
                    if (rightRectangle.Intersects(env[i].LeftBox))
                    {
                        return 0;
                    }
                }
            }

            return ammount;
        }


        // Accessors
        public bool IsStopped
        {
            get { return isStopped; }
        }


        public Vector2 Velocity
        {
            get { return velocity; }
            set 
            { 
                velocity = value;
                isStopped = false;
            }
        }

        public float VelX
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }

        public float VelY
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }



    }
}