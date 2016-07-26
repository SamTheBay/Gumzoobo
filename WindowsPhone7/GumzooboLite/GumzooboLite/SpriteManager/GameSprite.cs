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
    public class GameSprite
    {
        protected bool isActive = false;
        public bool isPawn = false;
        public Vector2 position = new Vector2(0f,0f);
        protected bool isVirticleLooping = true;

        protected Rectangle topBox;
        protected Rectangle bottomBox;
        protected Rectangle rightBox;
        protected Rectangle leftbox;
        protected Rectangle boundingBox1 = new Rectangle();
        protected Rectangle boundingBox2 = new Rectangle();

        // adjustments
        protected int reduceTop = 0;
        protected int reduceSides = 0;
        protected int centeredReduce = 0;

        public static Game game;

        public GameSprite(Vector2 nPosition)
        {
            position = nPosition;

            topBox = new Rectangle((int)nPosition.X, (int)nPosition.Y, FrameDimensions.X, 1);
            bottomBox = new Rectangle((int)nPosition.X, (int)nPosition.Y + FrameDimensions.Y, FrameDimensions.X, 1);
            rightBox = new Rectangle((int)nPosition.X + FrameDimensions.X, (int)nPosition.Y, 1, FrameDimensions.Y);
            leftbox = new Rectangle((int)nPosition.X, (int)nPosition.Y, 1, FrameDimensions.Y);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            // check if the character fell below the screen and put them on top
            if (position.Y > 456f)
            {
                if (isVirticleLooping)
                {
                    position.Y = -60f;
                }
                else
                {
                    Deactivate();
                }
            }
            else if (position.Y + centeredReduce/2 + reduceTop < -61f)
            {
                if (isVirticleLooping)
                {
                    position.Y = 455;
                }
                else
                {
                    Deactivate();
                }
            }

            if (position.X < 56 + 24 - reduceSides/2 && isPawn == false)
            {
                position.X = 56 + 24 - reduceSides/2;
            }
            else if (position.X + frameDimensions.X > 800 - 24 + reduceSides/2 && isPawn == false)
            {
                position.X = 800 - 24 - frameDimensions.X + reduceSides/2;
            }
        }

        public virtual bool CollisionDetect(GameSprite otherSprite)
        {
            return PixelCollision(otherSprite);
        }


        public virtual void CollisionAction(GameSprite otherSprite)
        {
            // implemented by high level classes
        }


        public bool IsActive
        {
            get { return isActive; }
        }


        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual void Activate()
        {
            isActive = true;
        }

        public virtual void Deactivate()
        {
            isActive = false;
        }


        /// <summary>
        /// The dimensions of a single frame of animation.
        /// </summary>
        protected Point frameDimensions;

        /// <summary>
        /// The width of a single frame of animation.
        /// </summary>
        public Point FrameDimensions
        {
            get { return frameDimensions; }
            set
            {
                frameDimensions = value;
                frameOrigin.X = frameDimensions.X / 2;
                frameOrigin.Y = frameDimensions.Y / 2;
            }
        }


        /// <summary>
        /// The origin of the sprite, within a frame.
        /// </summary>
        protected Point frameOrigin;



        public virtual Rectangle GetRectangle(Direction direction, int velocity)
        {
            // TODO: pass in a rectangle instead of allocating one here
            if (direction == Direction.Up)
            {
                return new Rectangle((int)position.X + reduceSides / 2, (int)position.Y - 1 - velocity, FrameDimensions.X - reduceSides, velocity);
            }
            else if (direction == Direction.Down)
            {
                return new Rectangle((int)position.X + reduceSides / 2, (int)position.Y + FrameDimensions.Y + 1, FrameDimensions.X - reduceSides, velocity);
            }
            else if (direction == Direction.Left)
            {
                return new Rectangle((int)position.X - 1 - velocity + reduceSides / 2, (int)position.Y + reduceTop, velocity, FrameDimensions.Y - reduceTop);
            }
            else
            {
                return new Rectangle((int)position.X + FrameDimensions.X + 1 - reduceSides / 2, (int)position.Y + reduceTop, velocity, FrameDimensions.Y - reduceTop);
            }
        }



        public virtual Rectangle GetBoundingRectangle(Rectangle rect)
        {
            rect.X = (int)position.X + centeredReduce / 2;
            rect.Y = (int)position.Y + centeredReduce / 2;
            rect.Width = frameDimensions.X - centeredReduce;
            rect.Height = frameDimensions.Y - centeredReduce;
            return rect;
        }


        public virtual bool PixelCollision(GameSprite otherSprite)
        {
            // get the useable bounding rectangles
            Rectangle myRect = GetBoundingRectangle(boundingBox1);
            Rectangle theirRect = otherSprite.GetBoundingRectangle(boundingBox2);

            // Find the bounds of the rectangle intersection
            int top = Math.Max((int)myRect.Y, (int)theirRect.Y);
            int bottom = Math.Min((int)myRect.Y + (int)myRect.Height, (int)theirRect.Y + (int)theirRect.Height);
            int left = Math.Max((int)myRect.X, (int)theirRect.X);
            int right = Math.Min((int)myRect.X + (int)myRect.Width, (int)theirRect.X + (int)theirRect.Width);
		    
            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
	            for (int x = left; x < right; x++)
	            {
		            // Get the color of both pixels at this point
                    Color colorA = GetAbsolutePixel(x, y);
                    Color colorB = otherSprite.GetAbsolutePixel(x, y);

		            // If both pixels are not completely transparent,
		            if (colorA.A != 0 && colorB.A != 0)
		            {
			            // then an intersection has been found
			            return true;
		            }
	            }
            }

            // No intersection found
            return false;
        }



        public virtual Color GetAbsolutePixel(int x, int y)
        {
            return Color.Transparent;
        }


        public virtual Rectangle TopBox
        {
            get { return topBox; }
        }

        public virtual Rectangle BottomBox
        {
            get { return bottomBox; }
        }

        public virtual Rectangle RightBox
        {
            get { return rightBox; }
        }

        public virtual Rectangle LeftBox
        {
            get { return leftbox; }
        }
    }
}