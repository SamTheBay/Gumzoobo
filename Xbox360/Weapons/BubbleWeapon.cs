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
    public class BubbleWeapon : WeaponSprite
    {
        PlayerSprite playerOwner;

        static Texture2D icon;
        static Color iconTint;
        static Rectangle iconLocation;

        public BubbleWeapon(PlayerSprite owner) :
            base(owner, 5000)
        {
            playerOwner = owner;
            AddAnimation(new Animation("Bubble", 1, 3, 100, false, SpriteEffects.None));
            chewCost = 5;
            centeredReduce = 74;
        }


        public override void Activate()
        {
            base.Activate();

            PlayAnimation("Bubble");
            ResetAnimation();
            position = new Vector2(playerOwner.Position.X - ((frameDimensions.X - playerOwner.FrameDimensions.X)/2),
                                   playerOwner.Position.Y - ((frameDimensions.Y - playerOwner.FrameDimensions.Y)/2));
            if (playerOwner.LastDirection == Direction.Right)
            {
                Velocity = new Vector2(5f, 0);
            }
            else
            {
                Velocity = new Vector2(-5f, 0);
            }
        }

        public override void CollisionAction(GameSprite otherSprite)
        {
            base.CollisionAction(otherSprite);

            if (otherSprite is PlayerSprite)
            {
                if (isLethal == false)
                {
                    Expire();
                }
            }
            if (otherSprite is Enemy)
            {
                Enemy enemy = (Enemy)otherSprite;
                if (enemy.IsDead == false && enemy.Stuck == false)
                {
                    if (isLethal == true)
                        shouldPlayPop = false;
                    Expire();
                }
            }
        }

        public override void EndLethal()
        {
            base.EndLethal();

            velocity.X = 0;
            velocity.Y = -1f * Level.singletonLevel.Details.bubbleRiseSpeed;
        }


        public static void LoadStatic()
        {
            // setup the icon
            icon = InternalContentManager.GetTexture("Gum");
            iconTint = Color.White;
            iconLocation = new Rectangle(300, 0, 60, 40);
        }

        public static void DrawIcon(SpriteBatch spriteBatch, Vector2 iconPosition, float scale)
        {
            spriteBatch.Draw(icon, iconPosition, iconLocation, iconTint, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }


        public override Rectangle GetRectangle(Direction direction, int velocity)
        {
            if (direction == Direction.Up)
            {
                return new Rectangle((int)position.X + 30, (int)position.Y - 1 + 30, 60, velocity);
            }
            else if (direction == Direction.Down)
            {
                return new Rectangle((int)position.X + 30, (int)position.Y + FrameDimensions.Y + 1 - 30, 60, velocity);
            }
            else if (direction == Direction.Left)
            {
                return new Rectangle((int)position.X - 1 + 30, (int)position.Y + 30, velocity, 60);
            }
            else
            {
                return new Rectangle((int)position.X + FrameDimensions.X + 1 - 30, (int)position.Y + 30, velocity, 60);
            }
        }


        public override void Expire()
        {
            base.Expire();
            if (shouldPlayPop)
            {
                AudioManager.PlayCue("BubblePop");
                shouldPlayPop = false;
            }
        }
    }
}