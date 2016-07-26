using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BubbleGame
{

    abstract class MenuScreen : GameScreen
    {
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        protected int selectedEntry = 0;
        protected bool isPlayerTuned = false;
        protected int controllerIndex = 0;
        protected int selectorIndex = 0;
        protected bool isActionable = true;


        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        protected MenuEntry SelectedMenuEntry
        {
            get
            {
                if ((selectedEntry < 0) || (selectedEntry >= menuEntries.Count))
                {
                    return null;
                }
                return menuEntries[selectedEntry];
            }
        }


        public bool IsActionable
        {
            get { return isActionable; }
            set { isActionable = value; }
        }


        public MenuScreen()
        {
            EnabledGestures = Microsoft.Xna.Framework.Input.Touch.GestureType.Tap;
        }

        public override void ResetScreen()
        {
            base.ResetScreen();

            for (int i = 0; i < menuEntries.Count; i++)
            {
                menuEntries[i].Reset();
            }
        }


        public override void HandleInput()
        {
            // check if we are stable
            if (isStable)
            {
                bool allStable = true;
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    if (menuEntries[i].IsStable == false)
                    {
                        allStable = false;
                        break;
                    }
                }
                if (allStable)
                    isStable = true;
                else
                    isStable = false;
            }

            if (!isActionable || !isStable)
                return;

            //int oldSelectedEntry = selectedEntry;

            for (int i = 0; i < menuEntries.Count; i++)
            {
                if (InputManager.IsLocationTapped(menuEntries[i].Location))
                {
                    AudioManager.audioManager.PlaySFX("MenuSelect");
                    selectedEntry = i;
                    selectorIndex = i;
                    OnSelectEntry(i);
                }
            }
            if (InputManager.IsBackTriggered())
            {
                OnCancel();
            }
        }


        protected virtual void OnSelectEntry(int entryIndex)
        {
            menuEntries[entryIndex].OnSelectEntry();
        }


        protected virtual void OnCancel()
        {
            ExitScreen();
        }


        public override void ExitScreen()
        {
            if (isExiting == false)
            {
                isStable = false;
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    menuEntries[i].StartExit();
                }
                base.ExitScreen();
            }
        }


        public override void AddNextScreen(GameScreen nextScreen)
        {
            AddNextScreen(nextScreen, true);
        }

        public override void AddNextScreen(GameScreen nextScreen, bool doExitAnimation)
        {
            if (doExitAnimation)
            {
                isStable = false;
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    menuEntries[i].StartExit();
                }
            }

            base.AddNextScreen(nextScreen, doExitAnimation);
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!otherScreenHasFocus && !coveredByOtherScreen)
            {

                // Update each nested MenuEntry object.
                bool allStable = true;
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    bool isSelected = IsActive && (i == selectedEntry);

                    menuEntries[i].Update(this, isSelected, gameTime);

                    // check if we are stable
                    if (menuEntries[i].IsStable == false)
                    {
                        allStable = false;
                    }
                }

                if (allStable)
                    isStable = true;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.End();
        }

    }
}
