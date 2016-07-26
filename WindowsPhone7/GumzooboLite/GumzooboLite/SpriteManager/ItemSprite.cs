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
    public class ItemSprite : AnimatedSprite
    {
        protected int itemFrame;
        protected Color itemTint;
        protected int pointPayout;
        protected int healthPayout;
        protected int currentTime = 0;
        protected int expireTime = 25000;


        public ItemSprite(Vector2 nPosition, string spriteSheet, Point frame, Point center, int framesperrow, 
            Vector2 offset, int itemFrame, Color itemTint)
            : base(spriteSheet, frame, center, framesperrow, offset, nPosition)
        {
            this.itemFrame = itemFrame;
            this.itemTint = itemTint;
            this.pointPayout = 0;
            this.healthPayout = 0;
            AddAnimation(new Animation("Item", itemFrame, itemFrame, 100, false, SpriteEffects.None));
            PlayAnimation("Item");
            Activate();
        }

        public ItemSprite(Vector2 nPosition, int itemFrame, Color itemTint, int pointPayout)
            : base("Gum", new Point(60, 40), new Point(30, 20), 5, new Vector2(30f, 20f), nPosition)
        {
            this.itemFrame = itemFrame;
            this.itemTint = itemTint;
            this.pointPayout = pointPayout;
            this.healthPayout = 0;
            AddAnimation(new Animation("Item", itemFrame, itemFrame, 100, false, SpriteEffects.None));
            PlayAnimation("Item");
            Activate();
        }


        public ItemSprite(Vector2 nPosition, int itemFrame, Color itemTint, int pointPayout, int expireTime)
            : base("Gum", new Point(60, 40), new Point(30, 20), 5, new Vector2(30f, 20f), nPosition)
        {
            this.expireTime = expireTime;
            this.itemFrame = itemFrame;
            this.itemTint = itemTint;
            this.pointPayout = pointPayout;
            this.healthPayout = 0;
            AddAnimation(new Animation("Item", itemFrame, itemFrame, 100, false, SpriteEffects.None));
            PlayAnimation("Item");
            Activate();
        }


        public ItemSprite(Vector2 nPosition)
            : base("Fruit", new Point(50, 54), new Point(25, 27), 6, new Vector2(25f, 27f), nPosition)
        {
            expireTime = 15000;
            itemTint = Color.White;
            this.healthPayout = 20;

            // create a random point generating item
            switch (BubbleGame.rand.Next(0, 5))
            {
                case 0:
                    itemFrame = 1;
                    pointPayout = 100;
                    break;

                case 1:
                    itemFrame = 2;
                    pointPayout = 500;
                    break;

                case 2:
                    itemFrame = 3;
                    pointPayout = 1000;
                    break;

                case 3:
                    itemFrame = 4;
                    pointPayout = 1000;
                    break;

                case 4:
                    itemFrame = 5;
                    pointPayout = 2000;
                    break;

                case 5:
                    itemFrame = 6;
                    pointPayout = 5000;
                    break;
            }

            AddAnimation(new Animation("Item", itemFrame, itemFrame, 100, false, SpriteEffects.None));
            PlayAnimation("Item");
            Activate();

        }


        public override void CollisionAction(GameSprite otherSprite)
        {
            base.CollisionAction(otherSprite);

            // if its a player then we deactivate
            if (otherSprite is PlayerSprite)
            {
                PlayerSprite player = (PlayerSprite)otherSprite;
                if (player.IsActive && player.IsDead == false)
                {
                    ItemAction(player);
                }
            }
        }

        public virtual void ItemAction(PlayerSprite actionOwner)
        {
            // default is to have no action
            AudioManager.audioManager.PlaySFX("GumGrab");
            actionOwner.RewardPoints(pointPayout);
            actionOwner.Heal(healthPayout);
            Level.singletonLevel.RemoveSprite(this);
            Deactivate();
            if (pointPayout != 0)
                Level.singletonLevel.AddSprite(new PointsSprite(pointPayout, position));
           
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            if (currentTime >= expireTime)
            {
                Level.singletonLevel.RemoveSprite(this);
                Deactivate();
            }
        }


    }
}