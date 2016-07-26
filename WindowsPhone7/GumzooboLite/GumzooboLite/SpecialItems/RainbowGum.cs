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
    class RainbowGum : ItemSprite
    {
        public RainbowGum(Vector2 position)
            : base(position, "Gum", new Point(60,40), new Point(30, 20), 7, new Vector2(30f, 20f), 7, Color.White)
        {

        }

        public override void ItemAction(PlayerSprite actionOwner)
        {
            base.ItemAction(actionOwner);
            actionOwner.StartAmmoBonus();
        }
    }
}