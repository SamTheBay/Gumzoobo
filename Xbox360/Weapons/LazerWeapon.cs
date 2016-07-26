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
    class LazerWeapon : WeaponSprite
    {
        Enemy enemyOwner;
        

        public LazerWeapon(Enemy owner) :
            base(owner, 5000, "Lazer", new Point(20,80), new Point(10,40), 9, new Vector2(10f, 40f))
        {
            enemyOwner = owner;
            AddAnimation(new Animation("LazerStart", 1, 2, 30, true, SpriteEffects.None));
            AddAnimation(new Animation("Lazer", 3, 5, 30, true, SpriteEffects.None));
            AddAnimation(new Animation("LazerEnd", 6, 9, 30, true, SpriteEffects.None));
            lethalDuration = 999999999;
            isVirticleLooping = false;
            expiredDuration = 1;
            envColission = false;
        }

        public override void Activate()
        {
            base.Activate();

            AudioManager.PlayCue("Lazer");
            PlayAnimation("Lazer");
            ResetAnimation();
            position = enemyOwner.Position;
            Velocity = new Vector2(0, 5f);
            isLethal = true;
            lethalElapsed = 0;
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
            Deactivate();
        }

    }
}