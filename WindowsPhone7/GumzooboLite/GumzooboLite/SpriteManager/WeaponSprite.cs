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
    public enum Weapon
    {
        Bubble,
        Cinnemon,
        Grape,
        Mint,
        Super,
        ABC,
        Num
    }


    public class WeaponSprite : AnimatedSprite
    {
        protected bool isPlayerGenerated;
        protected GameSprite weaponOwner;
        protected int weaponLifespan = 20000;
        protected int elapsedLifespan = 0;
        protected Vector2 velocity = new Vector2(0f, 0f);
        protected bool isLethal = true;
        protected int lethalDuration = 400;
        protected int lethalElapsed = 0;
        protected bool isExpired = false;
        protected int expiredDuration = 0;
        protected int expiredElapsed = 0;
        protected int chewCost = 5;
        protected bool envColission = true;
        protected bool shouldPlayPop = true;
        protected int bubbleRiseSpeedTime = 500;

        public WeaponSprite(GameSprite nWeaponOwner, int nWeaponLifespan) :
            base("Bubble", new Point(120, 120), new Point(60, 60), 8, new Vector2(60f, 60f), new Vector2(50f, 50f))
        {
            weaponOwner = nWeaponOwner;
            weaponLifespan = nWeaponLifespan;
            isPawn = true;
            if (weaponOwner is PlayerSprite)
                isPlayerGenerated = true;
        }


        public WeaponSprite(GameSprite nWeaponOwner, int nWeaponLifespan, string textureName, Point frame, Point origin, int framesperrow, Vector2 offset) :
            base(textureName, frame, origin, framesperrow, offset, new Vector2(50f, 50f))
        {
            weaponOwner = nWeaponOwner;
            weaponLifespan = nWeaponLifespan;
            isPawn = true;
            if (weaponOwner is PlayerSprite)
                isPlayerGenerated = true;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isActive && ((isPlayerGenerated && isLethal) || isExpired || (this is CinnemonWeapon) || !Level.singletonLevel.IsTimeStill))
            {
                // check our lifespan
                elapsedLifespan += gameTime.ElapsedGameTime.Milliseconds;
                if (weaponLifespan != 0 && elapsedLifespan >= weaponLifespan)
                {
                    if (isExpired == false)
                        Expire();
                }

                // check our expiration
                if (isExpired == true)
                {
                    expiredElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if (expiredElapsed >= expiredDuration)
                    {
                        Deactivate(); 
                    }
                }

                // check our lethal duration
                if (isLethal)
                {
                    lethalElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if (lethalElapsed >= lethalDuration)
                    {
                        EndLethal();
                    }
                }

                // move weapon
                if (envColission == true)
                {
                    if (velocity.X > 0)
                    {
                        if (0 == CanMove(Direction.Right, (int)Math.Abs(velocity.X)))
                        {
                            velocity.X = 0;
                        }
                    }
                    else if (Velocity.X < 0)
                    {
                        if (0 == CanMove(Direction.Left, (int)Math.Abs(velocity.X)))
                        {
                            velocity.X = 0;
                        }
                    }
                }
                position += velocity;
                
            }

            
        }


        // Accessors
        public bool PlayerGenerated
        {
            get { return isPlayerGenerated; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public GameSprite Owner
        {
            get { return weaponOwner; }
        }

        public bool IsPlayerGenerated
        {
            get { return isPlayerGenerated; }
        }

        public bool IsLethal
        {
            get { return isLethal; }
        }

        public int ChewCost
        {
            get { return chewCost; }
        }

        static public float ChewSpeed (Weapon weapon)
        {
            if (weapon == Weapon.Bubble)
                return 250f;
            else if (weapon == Weapon.ABC)
                return 1f;
            else if (weapon == Weapon.Cinnemon)
                return 1000f;
            else if (weapon == Weapon.Grape)
                return 1000f;
            else if (weapon == Weapon.Mint)
                return 400f;
            else if (weapon == Weapon.Super)
                return 1000f;
            else
                return 250f;
        }


        static public int MaxAmmo(Weapon weapon)
        {
            if (weapon == Weapon.Bubble)
                return 100;
            else if (weapon == Weapon.ABC)
                return 300;
            else if (weapon == Weapon.Cinnemon)
                return 50;
            else if (weapon == Weapon.Grape)
                return 100;
            else if (weapon == Weapon.Mint)
                return 100;
            else if (weapon == Weapon.Super)
                return 50;
            else
                return 100;
        }

        // functions to control behavior
        public override void Activate()
        {
            base.Activate();

            elapsedLifespan = 0;
            isExpired = false;
            isLethal = true;
            lethalElapsed = 0;
            expiredElapsed = 0;
            shouldPlayPop = true;
        }

        public virtual void Expire()
        {
            isExpired = true;
            if (expiredDuration == 0)
                Deactivate();
        }

        public virtual void EndLethal()
        {
            isLethal = false;
        }

        public void NoPop()
        {
            shouldPlayPop = false;
        }

        public void RewardPoints(int points)
        {
            if (Owner is PlayerSprite)
            {
                PlayerSprite player = (PlayerSprite)Owner;
                player.RewardPoints(points);
            }
        }

        public override Rectangle TopBox
        {
            get { return new Rectangle((int)position.X + 37, (int)position.Y + 37, FrameDimensions.X - 74, 10); }
        }

        public static Color WeaponColor(Weapon wep)
        {
            if (wep == Weapon.Bubble)
            {
                return new Color(186, 178, 255);
            }
            else if (wep == Weapon.Cinnemon)
            {
                return new Color(255, 102, 96);
            }
            else if (wep == Weapon.Mint)
            {
                return new Color(136, 112, 255);
            }
            else if (wep == Weapon.ABC)
            {
                return new Color(165, 66, 0);
            }
            else if (wep == Weapon.Grape)
            {
                return new Color(255, 145, 242);
            }
            else if (wep == Weapon.Super)
            {
                return new Color(255, 63, 165);
            }
            else return Color.White;
        }

        public static int WeaponFrame(Weapon wep)
        {
            if (wep == Weapon.Bubble)
            {
                return 3;
            }
            else if (wep == Weapon.Cinnemon)
            {
                return 1;
            }
            else if (wep == Weapon.Mint)
            {
                return 4;
            }
            else if (wep == Weapon.ABC)
            {
                return 2;
            }
            else if (wep == Weapon.Grape)
            {
                return 5;
            }
            else if (wep == Weapon.Super)
            {
                return 3;
            }
            else return 0;
        }
    }
}