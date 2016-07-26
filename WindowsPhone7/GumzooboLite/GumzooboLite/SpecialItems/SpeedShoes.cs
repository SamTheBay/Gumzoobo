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
    class SpeedShoes : ItemSprite
    {
        public SpeedShoes(Vector2 position)
            : base(position, "Specials", new Point(56,54), new Point(28, 27), 4, new Vector2(28f, 27f), 1, Color.White)
        {

        }

        public override void ItemAction(PlayerSprite actionOwner)
        {
            base.ItemAction(actionOwner);
            actionOwner.StartSpeedBonus();
        }
    }
}