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
    class AmmoItemSprite : ItemSprite
    {
        Weapon weaponType;

        public AmmoItemSprite(Vector2 position, Weapon weaponType)
            : base(position, WeaponSprite.WeaponFrame(weaponType), WeaponSprite.WeaponColor(weaponType), 0)
        {
            this.weaponType = weaponType;
        }

        public AmmoItemSprite(Vector2 position, Weapon weaponType, int expireTime)
            : base(position, WeaponSprite.WeaponFrame(weaponType), WeaponSprite.WeaponColor(weaponType), 0, expireTime)
        {
            this.weaponType = weaponType;
        }

        public override void ItemAction(PlayerSprite actionOwner)
        {
            int ammoAmount = 25;
            if (weaponType == Weapon.Cinnemon)
                ammoAmount = 10;
            else if (weaponType == Weapon.ABC)
                ammoAmount = 50;
            else if (weaponType == Weapon.Grape)
                ammoAmount = 15;

            actionOwner.RewardAmmo(weaponType, ammoAmount);

            base.ItemAction(actionOwner);
        }
    }
}