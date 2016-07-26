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
    class CrystalBall : ItemSprite
    {
        public CrystalBall(Vector2 position)
            : base(position, "Specials", new Point(56,54), new Point(28, 27), 4, new Vector2(28f, 27f), 3, Color.White)
        {

        }

        public override void ItemAction(PlayerSprite actionOwner)
        {
            base.ItemAction(actionOwner);

            List<Enemy> enemySprites = Level.singletonLevel.EnemySprites;
            for (int i = 0; i < enemySprites.Count; i++)
            {
                Enemy enemy = enemySprites[i];
                if (enemy.IsDead == false && enemy.IsActive == true)
                {
                    enemy.Die();
                }
            }
        }
    }
}