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
    class WindowsTrialScreen : HelpScreen
    {
        public WindowsTrialScreen()
            : base("WindowsTrial")
        {

        }

        public override void ExitScreen()
        {
            Level.singletonLevel.hasShownTrial = true;
            base.ExitScreen();
        }
    }
}
