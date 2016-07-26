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

    public enum Direction
    {
        // regular directions
        Up,
        Down,
        Left,
        Right,

        // diagonal directions
        RightUp,
        RightDown,
        LeftUp,
        LeftDown
    }

    public class AnimatedSprite : GameSprite
    {

        Color[] textureData; // needed for collision detection

        // Animation description
        protected string textureName;
        protected Texture2D texture;
        protected int framesPerRow;
        protected Vector2 sourceOffset;
        protected List<Animation> animations = new List<Animation>();
        protected Animation currentAnimation = null;
        protected int currentFrame;
        protected float elapsedTime;
        protected float currentRotation;
        protected Rectangle sourceRectangle;

        // extra animation pieces
        protected string baseAnimationString;
        protected Direction baseAnimationDirection;
        protected int currentFrameOffset;


        public AnimatedSprite(string nTextureName, Point nFrameDimensions, Point nFrameOrigin, int nFramesPerRow, Vector2 nSourceOffset, Vector2 nPosition)
            : base(nPosition)
        {
            textureName = nTextureName;
            texture = InternalContentManager.GetTexture(nTextureName);
            frameDimensions = nFrameDimensions;
            frameOrigin = nFrameOrigin;
            sourceOffset = nSourceOffset;
            framesPerRow = nFramesPerRow;
            textureData = InternalContentManager.GetTextureData(nTextureName);
        }



        public void SwapAnimation(string nTextureName, Point nFrameDimensions, Point nFrameOrigin, int nFramesPerRow, Vector2 nSourceOffset)
        {
            // adjust position for new frame setting
            position = new Vector2(position.X + ((frameDimensions.X - nFrameDimensions.X) / 2), position.Y + ((frameDimensions.Y - nFrameDimensions.Y) / 2));

            // swap out texture data
            textureName = nTextureName;
            texture = InternalContentManager.GetTexture(nTextureName);
            frameDimensions = nFrameDimensions;
            frameOrigin = nFrameOrigin;
            sourceOffset = nSourceOffset;
            framesPerRow = nFramesPerRow;
            textureData = InternalContentManager.GetTextureData(nTextureName);

            // reset the animation
            ResetAnimation();
        }


        public string TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }


        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int FramesPerRow
        {
            get { return framesPerRow; }
            set { framesPerRow = value; }
        }

        public Vector2 SourceOffset
        {
            get { return sourceOffset; }
            set { sourceOffset = value; }
        }

        public List<Animation> Animations
        {
            get { return animations; }
            set { animations = value; }
        }

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
        }




        /// <summary>
        /// Enumerate the animations on this animated sprite.
        /// </summary>
        /// <param name="animationName">The name of the animation.</param>
        /// <returns>The animation if found; null otherwise.</returns>
        public Animation this[string animationName]
        {
            get
            {
                if (String.IsNullOrEmpty(animationName))
                {
                    return null;
                }
                foreach (Animation animation in animations)
                {
                    if (String.Compare(animation.Name, animationName) == 0)
                    {
                        return animation;
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Add the animation to the list, checking for name collisions.
        /// </summary>
        /// <returns>True if the animation was added to the list.</returns>
        public bool AddAnimation(Animation animation)
        {
            if ((animation != null) && (this[animation.Name] == null))
            {
                animations.Add(animation);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Play the given animation on the sprite.
        /// </summary>
        /// <remarks>The given animation may be null, to clear any animation.</remarks>
        public void PlayAnimation(Animation animation)
        {
            // start the new animation, ignoring redundant Plays
            if (animation != currentAnimation)
            {
                currentAnimation = animation;
                ResetAnimation();
            }
        }


        /// <summary>
        /// Play an animation given by index.
        /// </summary>
        public void PlayAnimation(int index)
        {
            // check the parameter
            if ((index < 0) || (index >= animations.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            PlayAnimation(this.animations[index]);
        }


        /// <summary>
        /// Play an animation given by name.
        /// </summary>
        public void PlayAnimation(string name)
        {
            // check the parameter
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            PlayAnimation(this[name]);
        }


        /// <summary>
        /// Play a given animation name, with the given direction suffix.
        /// </summary>
        /// <example>
        /// For example, passing "Walk" and Direction.South will play the animation
        /// named "WalkSouth".
        /// </example>
        public void PlayAnimation(string name, Direction direction)
        {
            // check the parameter
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            baseAnimationString = name;
            baseAnimationDirection = direction;
            PlayAnimation(name + direction.ToString());
        }

        public void PlayAnimation(string name, Direction direction, int StartFrameOffset)
        {
            // check the parameter
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            baseAnimationString = name;
            baseAnimationDirection = direction;
            PlayAnimation(name + direction.ToString());
            currentFrame = currentAnimation.StartingFrame + StartFrameOffset;
            currentFrameOffset = StartFrameOffset;
        }


        /// <summary>
        /// Reset the animation back to its starting position.
        /// </summary>
        public void ResetAnimation()
        {
            currentRotation = 0f;
            elapsedTime = 0f;
            if (currentAnimation != null)
            {
                currentFrame = currentAnimation.StartingFrame;
                currentFrameOffset = 0;
                // calculate the source rectangle by updating the animation
                UpdateAnimation(0f);
            }
        }


        /// <summary>
        /// Advance the current animation to the final sprite.
        /// </summary>
        public void AdvanceToEnd()
        {
            if (currentAnimation != null)
            {
                currentFrame = currentAnimation.EndingFrame;
                // calculate the source rectangle by updating the animation
                UpdateAnimation(0f);
            }
        }


        /// <summary>
        /// Stop any animation playing on the sprite.
        /// </summary>
        public void StopAnimation()
        {
            currentAnimation = null;
        }


        public void FlipAnimationDirection(Direction newDirection)
        {
            if (baseAnimationDirection != newDirection)
            {
                currentAnimation = this[baseAnimationString + newDirection.ToString()];
                baseAnimationDirection = newDirection;
                currentFrame = currentAnimation.StartingFrame + currentFrameOffset;
            }
        }


        /// <summary>
        /// Returns true if playback on the current animation is complete, or if
        /// there is no animation at all.
        /// </summary>
        public bool IsPlaybackComplete
        {
            get
            {
                return ((currentAnimation == null) ||
                    (!currentAnimation.IsLoop &&
                    (currentFrame > currentAnimation.EndingFrame)));
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateAnimation((float)gameTime.ElapsedGameTime.TotalSeconds);
        }



        /// <summary>
        /// Update the current animation.
        /// </summary>
        public void UpdateAnimation(float elapsedSeconds)
        {
            if (IsPlaybackComplete)
            {
                return;
            }

            // loop the animation if needed
            if (currentAnimation.IsLoop && (currentFrame > currentAnimation.EndingFrame))
            {
                currentFrame = currentAnimation.StartingFrame;
                currentFrameOffset = 0;
            }

            // update the source rectangle
            int column = (currentFrame - 1) / framesPerRow;
            sourceRectangle = new Rectangle(
                (currentFrame - 1 - (column * framesPerRow)) * frameDimensions.X,
                column * frameDimensions.Y,
                frameDimensions.X, frameDimensions.Y);

            // update the elapsed time
            elapsedTime += elapsedSeconds;

            currentRotation += currentAnimation.RotationSpeed;

            // advance to the next frame if ready
            while (elapsedTime * 1000f > (float)currentAnimation.Interval)
            {
                currentFrame++;
                currentFrameOffset++;
                elapsedTime -= (float)currentAnimation.Interval / 1000f;
            }
        }


        public override void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (currentAnimation != null)
                Draw(spriteBatch, position, layerDepth);
        }


        /// <summary>
        /// Draw the sprite at the given position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="position">The position of the sprite on-screen.</param>
        /// <param name="layerDepth">The depth at which the sprite is drawn.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layerDepth)
        {
               Draw(spriteBatch, position, layerDepth, currentAnimation.SpriteEffect);
        }


        /// <summary>
        /// Draw the sprite at the given position.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used to draw.</param>
        /// <param name="position">The position of the sprite on-screen.</param>
        /// <param name="layerDepth">The depth at which the sprite is drawn.</param>
        /// <param name="spriteEffect">The sprite-effect applied.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layerDepth,
            SpriteEffects spriteEffect)
        {
            if (isActive == false)
                return;

            // check the parameters
            if (spriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }

            if (texture != null)
            {
                spriteBatch.Draw(texture, position + sourceOffset, sourceRectangle, currentAnimation.Tint, currentRotation,
                    sourceOffset, 1f, spriteEffect,
                    MathHelper.Clamp(layerDepth, 0f, 1f));
            }
        }


        public override Color GetAbsolutePixel(int x, int y)
        {
            if (currentAnimation == null)
            {
                return Color.Transparent;
            }

            int myx = x - (int)position.X;
            int myy = y - (int)position.Y;
            myx %= frameDimensions.X;
            myy %= frameDimensions.Y;

            // handle sprite effects that flip the immage
            if ((currentAnimation.SpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
            {
                myx = (frameDimensions.X - 1) - myx;
            }
            if ((currentAnimation.SpriteEffect & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically)
            {
                myy = (frameDimensions.Y - 1) - myy;
            }

            myx += sourceRectangle.Left;
            myy += sourceRectangle.Top;

            return textureData[myx + myy * Texture.Width];
        }


        public virtual int CanMove(Direction direction, int velocity, List<GameSprite> env)
        {
            if (direction == Direction.Down)
            {
                Rectangle bottomRectangle = GetRectangle(Direction.Down, velocity);

                for (int i = 0; i < env.Count; i++)
                {
                    if (env[i].IsActive == false)
                        continue;
                    if (bottomRectangle.Intersects(env[i].TopBox))
                    {
                        return 0;
                    }
                }

                if (bottomRectangle.Y + bottomRectangle.Height > 442)
                {
                    Rectangle transRect = new Rectangle(bottomRectangle.X, -60, bottomRectangle.Width, bottomRectangle.Y + bottomRectangle.Height - 442 + 50);
                    for (int i = 0; i < env.Count; i++)
                    {
                        if (env[i].IsActive == false)
                            continue;
                        if (transRect.Intersects(env[i].TopBox))
                        {
                            return 0;
                        }
                    }
                }
            }

            if (direction == Direction.Left)
            {
                if (position.X <= 56f + 24f - reduceSides / 2 - centeredReduce / 2)
                {
                    return 0;
                }

                Rectangle leftRectangle = GetRectangle(Direction.Left, velocity); ;

                for (int i = 0; i < env.Count; i++)
                {
                    if (env[i].IsActive == false)
                        continue;
                    if (leftRectangle.Intersects(env[i].RightBox))
                    {
                        return 0;
                    }
                }
            }

            if (direction == Direction.Right)
            {
                if (position.X + frameDimensions.X + 24 - reduceSides / 2 - centeredReduce / 2 >= 800f)
                {
                    return 0;
                }

                Rectangle rightRectangle = GetRectangle(Direction.Right, velocity);

                for (int i = 0; i < env.Count; i++)
                {
                    if (env[i].IsActive == false)
                        continue;
                    if (rightRectangle.Intersects(env[i].LeftBox))
                    {
                        return 0;
                    }
                }
            }

            return velocity;
        }



        public virtual int CanMove(Direction direction, int velocity)
        {
            if (direction == Direction.Down)
            {
                Rectangle bottomRectangle = GetRectangle(Direction.Down, velocity);

                // check if there is an environment sprite below us
                List<EnvironmentSprite> env = Level.singletonLevel.EnvSprites;
                for (int i = 0; i < env.Count; i++)
                {
                    if (bottomRectangle.Intersects(env[i].TopBox))
                    {
                        return env[i].topBox.Top - bottomRectangle.Top;
                    }
                }
            }

            if (direction == Direction.Up)
            {
                Rectangle topRectangle = GetRectangle(Direction.Up, velocity);

                // check if there is an environment sprite below us
                List<EnvironmentSprite> env = Level.singletonLevel.EnvSprites;
                for (int i = 0; i < env.Count; i++)
                {
                    if (topRectangle.Intersects(env[i].BottomBox))
                    {
                        return 0;
                    }
                }
            }

            if (direction == Direction.Left)
            {
                if (position.X <= 56f + 24f - reduceSides / 2 - centeredReduce / 2)
                {
                    return 0;
                }

                Rectangle leftRectangle = GetRectangle(Direction.Left, velocity);

                // check if there is an environment sprite below us
                List<EnvironmentSprite> env = Level.singletonLevel.EnvSprites;
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
                if (position.X + frameDimensions.X + 24 - reduceSides / 2 - centeredReduce / 2 >= 800f)
                {
                    return 0;
                }

                Rectangle rightRectangle = GetRectangle(Direction.Right, velocity);

                // check if there is an environment sprite below us
                List<EnvironmentSprite> env = Level.singletonLevel.EnvSprites;
                for (int i = 0; i < env.Count; i++)
                {
                    if (rightRectangle.Intersects(env[i].LeftBox))
                    {
                        return 0;
                    }
                }
            }

            return velocity;
        }

        public virtual bool InAir()
        {
            return 0 != CanMove(Direction.Down, (int)Level.singletonLevel.Gravity);
        }
    }
}