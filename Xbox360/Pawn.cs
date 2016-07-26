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

    public enum PawnType
    {
        Seal,
        Toad,
        Penguin,
        Tortoise,
        Drone,
        RocketBot,
        InvisiBot,
        Hunter,
        PorkyBot,
        Bubble,
        IceBubble,
        FireDrone,
        FireBall,
        WarpBot,
        LazerBot,
        Lazer,
        RocketBlaster,
        Size
    }



    class Pawn
    {
        AnimatedSprite displaySprite;
        Direction currentDirection = Direction.Right;
        bool isMoving = false;
        Vector2 destination = new Vector2(640, 360);
        float movementSpeed = 3.0f;
        public string playerType;
        bool shouldDraw = true;
        MPath path;
        bool isPathFollowing = false;
        bool isAtPoint;
        Connection currentConnection;
        MPoint currentPoint;
        int currentPathPoint = 0;
        int delayMove = 0;
        int delayMoveElapsed = 0;


        public Pawn(PlayerSprite player, Vector2 position)
        {
            if (player is SealPlayer)
            {
                Setup(PawnType.Seal, position);
            }
            else if (player is ToadPlayer)
            {
                Setup(PawnType.Toad, position);
            }
            else if (player is PenguinPlayer)
            {
                Setup(PawnType.Penguin, position);
            }
            else
            {
                Setup(PawnType.Tortoise, position);
            }
        }



        public Pawn(string character, Vector2 position)
        {
            for (int i = 0; i < (int)PawnType.Size; i++)
            {
                if (character == ((PawnType)i).ToString())
                {
                    Setup((PawnType)i, position);
                }
            }
        }


        public void Setup(PawnType characterindex, Vector2 position)
        {
            if (characterindex == PawnType.Seal)
            {
                playerType = "Seal";
                displaySprite = new AnimatedSprite("Seal", new Point(80, 80), new Point(40, 40), 14, new Vector2(40f, 40f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 6, 70, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 6, 70, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            }
            else if (characterindex == PawnType.Toad)
            {
                playerType = "Toad";
                displaySprite = new AnimatedSprite("Toad", new Point(120, 70), new Point(60, 35), 14, new Vector2(60f, 35f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 6, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            }
            else if (characterindex == PawnType.Penguin)
            {
                playerType = "Penguin";
                displaySprite = new AnimatedSprite("Penguin", new Point(42, 74), new Point(21, 37), 14, new Vector2(21f, 37f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 6, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            }
            else if (characterindex == PawnType.Tortoise)
            {
                playerType = "Tortoise";
                displaySprite = new AnimatedSprite("Tortoise", new Point(72, 62), new Point(36, 31), 24, new Vector2(36f, 31f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 9, 60, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 9, 60, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("InShellRight", 15, 19, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("InShellLeft", 15, 19, 100, false, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("OutShellRight", 20, 23, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("OutShellLeft", 20, 23, 100, false, SpriteEffects.None));

            }
            else if (characterindex == PawnType.Drone)
            {
                playerType = "Drone";
                displaySprite = new AnimatedSprite("Drone",  new Point(35,51), new Point(17, 25), 4, new Vector2(17f, 25f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 4, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 4, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            }
            else if (characterindex == PawnType.RocketBot)
            {
                playerType = "RocketBot";
                displaySprite = new AnimatedSprite("RocketBot", new Point(60, 60), new Point(30, 30), 5, new Vector2(30f, 30f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 4, 5, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 4, 5, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 2, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 2, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.InvisiBot)
            {
                playerType = "InvisiBot";
                displaySprite = new AnimatedSprite("Invisabot", new Point(40, 68), new Point(20, 34), 5, new Vector2(20f, 34f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 5, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 5, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.Hunter)
            {
                playerType = "Hunter";
                displaySprite = new AnimatedSprite("Hunter", new Point(70, 60), new Point(35, 30), 6, new Vector2(35f, 30f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 3, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 3, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 3, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 3, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.PorkyBot)
            {
                playerType = "PorkyBot";
                displaySprite = new AnimatedSprite("PorkyBot", new Point(62, 75), new Point(31, 37), 12, new Vector2(31f, 37f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 6, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("FrozenRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally, Color.Blue));
                displaySprite.AddAnimation(new Animation("FrozenLeft", 1, 1, 100, true, SpriteEffects.None, Color.Blue));
            }
            else if (characterindex == PawnType.Bubble)
            {
                playerType = "Bubble";
                displaySprite = new AnimatedSprite("Bubble", new Point(120, 120), new Point(60, 60), 8, new Vector2(60f, 60f), new Vector2(50f, 50f));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 3, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 3, 100, false, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.IceBubble)
            {
                playerType = "IceBubble";
                displaySprite = new AnimatedSprite("Bubble", new Point(120, 120), new Point(60, 60), 8, new Vector2(60f, 60f), new Vector2(50f, 50f));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 3, 100, false, SpriteEffects.FlipHorizontally, Color.Blue));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 3, 100, false, SpriteEffects.None, Color.Blue));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally, Color.Blue));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, true, SpriteEffects.None, Color.Blue));
            }
            else if (characterindex == PawnType.FireDrone)
            {
                playerType = "FireDrone";
                displaySprite = new AnimatedSprite("FireDrone", new Point(35, 51), new Point(17, 25), 4, new Vector2(17f, 25f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 4, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 4, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, false, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, false, SpriteEffects.None));
            }
            else if (characterindex == PawnType.FireBall)
            {
                playerType = "FireBall";
                displaySprite = new AnimatedSprite("Fireball", new Point(54, 44), new Point(27, 22), 6, new Vector2(27f, 22f), new Vector2(50f, 50f));
                displaySprite.AddAnimation(new Animation("MoveRight", 7, 9, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 7, 9, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 7, 9, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 7, 9, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.WarpBot)
            {
                playerType = "WarpBot";
                displaySprite = new AnimatedSprite("Warpbot", new Point(40, 68), new Point(20, 34), 5, new Vector2(20f, 34f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 5, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 5, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.LazerBot)
            {
                playerType = "LazerBot";
                displaySprite = new AnimatedSprite("LazerBot", new Point(45, 77), new Point(22, 38), 6, new Vector2(22f, 38f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 1, 6, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 1, 6, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 1, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 1, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.Lazer)
            {
                playerType = "Lazer";
                displaySprite = new AnimatedSprite("Lazer", new Point(20, 80), new Point(10, 40), 9, new Vector2(10f, 40f), new Vector2(50f, 50f));
                displaySprite.AddAnimation(new Animation("MoveRight", 3, 5, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 3, 5, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 3, 5, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 3, 5, 100, true, SpriteEffects.None));
            }
            else if (characterindex == PawnType.RocketBlaster)
            {
                playerType = "RocketBlaster";
                displaySprite = new AnimatedSprite("RocketBlaster", new Point(60, 60), new Point(30, 30), 5, new Vector2(30f, 30f), new Vector2(250, 250));
                displaySprite.AddAnimation(new Animation("MoveRight", 4, 5, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("MoveLeft", 4, 5, 100, true, SpriteEffects.None));
                displaySprite.AddAnimation(new Animation("IdleRight", 1, 2, 100, true, SpriteEffects.FlipHorizontally));
                displaySprite.AddAnimation(new Animation("IdleLeft", 1, 2, 100, true, SpriteEffects.None));
            }


            displaySprite.PlayAnimation("Idle", currentDirection);
            displaySprite.Position = position;
            destination = position;
            isAtPoint = true;
            currentPoint = new MPoint(position);
            displaySprite.isPawn = true;
            displaySprite.Activate();
        }


        public void SetDestination(Vector2 destination, int index)
        {
            isPathFollowing = false;
            this.destination = destination;
            this.destination.X -= displaySprite.FrameDimensions.X / 2;
            this.destination.Y -= displaySprite.FrameDimensions.Y / 2;

            // adjust for index
            if (index == 0)
            {
                this.destination.X -= 30;
                this.destination.Y -= 30;
            }
            else if (index == 1)
            {
                this.destination.X += 30;
                this.destination.Y -= 30;
            }
            else if (index == 2)
            {
                this.destination.X -= 30;
                this.destination.Y += 30;
            }
            else if (index == 3)
            {
                this.destination.X += 30;
                this.destination.Y += 30;
            }
        }


        public void SetBasicDestination(Vector2 destination)
        {
            this.destination = destination;
        }


        public void SetPath(MPath path, int index)
        {
            this.path = path;
            currentPathPoint = 0;

            if (isPathFollowing == false)
            {
                delayMove = 250 * index;
                delayMoveElapsed = 0;
            }

            isPathFollowing = true;
        }


        public void Update(GameTime gameTime)
        {
            Update(gameTime, 0);
        }

        public void Update(GameTime gameTime, int index)
        {
            if (delayMove > delayMoveElapsed)
            {
                delayMoveElapsed += gameTime.ElapsedGameTime.Milliseconds;
                return;
            }

            displaySprite.Update(gameTime);
            Vector2 currentDestination;

            if (isPathFollowing == false)
            {
                currentDestination = destination;
            }
            else
            {
                currentDestination = path.GetPoint(currentPathPoint).Location;
                currentDestination.X -= displaySprite.FrameDimensions.X / 2;
                currentDestination.Y -= displaySprite.FrameDimensions.Y / 2;
                if (currentDestination == displaySprite.position && currentPathPoint != path.Length-1)
                {
                    currentPathPoint++;
                    currentDestination = path.GetPoint(currentPathPoint).Location;

                    // set our connection
                    currentConnection = new Connection(path.GetPoint(currentPathPoint), path.GetPoint(currentPathPoint - 1));
                    isAtPoint = false;
                }
                else if (currentDestination == displaySprite.position && currentPathPoint == path.Length - 1)
                {
                    currentPoint = path.LastPoint;
                    isAtPoint = true;
                    isPathFollowing = false;
                    SetDestination(currentPoint.Location, index);
                }
            }

            // move the pawn
            if (displaySprite.Position != currentDestination)
            {
                Direction newDirection = currentDirection;

                // determine how to move
                Vector2 movement = new Vector2();
                movement.X = currentDestination.X - displaySprite.Position.X;
                movement.Y = currentDestination.Y - displaySprite.Position.Y;

                // normalize vector
                float distance = (float)Math.Sqrt((float)Math.Pow(movement.X, 2) + (float)Math.Pow(movement.Y, 2));
                if (distance > movementSpeed)
                {
                    movement.X /= distance;
                    movement.Y /= distance;

                    movement.X *= movementSpeed;
                    movement.Y *= movementSpeed;
                }

                // update position
                displaySprite.position.X += movement.X;
                displaySprite.position.Y += movement.Y;

                // update direction
                if (movement.X > 0)
                    newDirection = Direction.Right;
                else
                    newDirection = Direction.Left;

                // update animation
                if (isMoving == false || newDirection != currentDirection)
                {
                    currentDirection = newDirection;
                    displaySprite.PlayAnimation("Move", currentDirection);
                    isMoving = true;
                }
            }
            else if (isMoving == true)
            {
                displaySprite.PlayAnimation("Idle", currentDirection);
                isMoving = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (shouldDraw)
            {
                displaySprite.Draw(spriteBatch, 0);
            }
        }

        public void SetClearPosition(Vector2 position)
        {
            displaySprite.position = position;
            destination = position;
        }

        public void PlayAnimation(string animation)
        {
            displaySprite.PlayAnimation(animation);
        }

        public void PlayAnimation(string animation, Direction direction)
        {
            displaySprite.PlayAnimation(animation, direction);
        }


        public int TextureWidth
        {
            get { return displaySprite.FrameDimensions.X; }
        }

        public int TextureHeight
        {
            get { return displaySprite.FrameDimensions.Y; }
        }

        public bool ShouldDraw
        {
            get { return shouldDraw; }
            set { shouldDraw = value; }
        }

        public Direction CurrentDirection
        {
            get { return currentDirection; }
        }

        public Connection CurrentConnection
        {
            get { return currentConnection; }
        }

        public MPoint CurrentPoint
        {
            get { return currentPoint; }
        }

        public bool IsAtPoint
        {
            get { return isAtPoint; }
        }
    }
}