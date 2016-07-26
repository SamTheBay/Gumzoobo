using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BubbleGame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    class FireballWeapon : WeaponSprite
    {
        Enemy enemyOwner;
        Direction direction;

        public FireballWeapon(Enemy owner, int lifespan, Direction direction) :
            base(owner, lifespan, "Fireball", new Point(54, 44), new Point(27, 22), 6, new Vector2(27f, 22f))
        {
            enemyOwner = owner;
            this.direction = direction;
            AddAnimation(new Animation("FireballLeftDown", 1, 3, 70, true, SpriteEffects.None));
            AddAnimation(new Animation("FireballLeftDownEnd", 4, 6, 70, false, SpriteEffects.None));
            AddAnimation(new Animation("FireballRightDown", 1, 3, 70, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("FireballRightDownEnd", 4, 6, 70, false, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("FireballLeftUp", 1, 3, 70, true, SpriteEffects.FlipVertically));
            AddAnimation(new Animation("FireballLeftUpEnd", 4, 6, 70, false, SpriteEffects.FlipVertically));
            AddAnimation(new Animation("FireballRightUp", 1, 3, 70, true, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("FireballRightUpEnd", 4, 6, 70, false, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("FireballLeft", 7, 9, 70, true, SpriteEffects.None));
            AddAnimation(new Animation("FireballLeftEnd", 10, 12, 70, false, SpriteEffects.None));
            AddAnimation(new Animation("FireballRight", 7, 9, 70, true, SpriteEffects.FlipHorizontally));
            AddAnimation(new Animation("FireballRightEnd", 10, 12, 70, false, SpriteEffects.FlipHorizontally));
            lethalDuration = 999999999;
            isVirticleLooping = false;
            expiredDuration = 210;
            envColission = false;
            isPawn = true;
        }

        public override void Activate()
        {
            base.Activate();

            PlayAnimation("Fireball" + direction);
            ResetAnimation();
            position = enemyOwner.Position;
            isLethal = true;
            lethalElapsed = 0;

            // set the direction of this fireball
            SetDirection(direction);


        }

        public override void CollisionAction(GameSprite otherSprite)
        {
            base.CollisionAction(otherSprite);

            if (otherSprite is PlayerSprite)
            {
                PlayerSprite player = (PlayerSprite)otherSprite;
                if (isLethal == true)
                {
                    player.Damage(30);
                    Expire();
                }
            }
        }

        public override void Expire()
        {
            base.Expire();
            PlayAnimation("Fireball" + direction + "End");
        }

        public void SetDirection(Direction direction)
        {
            this.direction = direction;

            if (direction == Direction.LeftDown)
                Velocity = new Vector2(-3f, 3f);
            else if (direction == Direction.RightDown)
                Velocity = new Vector2(3f, 3f);
            else if (direction == Direction.LeftUp)
                Velocity = new Vector2(-3f, -3f);
            else if (direction == Direction.RightUp)
                Velocity = new Vector2(3f, -3f);
            else if (direction == Direction.Right)
                Velocity = new Vector2(5f, 0f);
            else if (direction == Direction.Left)
                Velocity = new Vector2(-5f, 0f);
        }
    }
}