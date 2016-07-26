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

            int oldSelectedEntry = selectedEntry;

            int startControllerIndex = 0;
            int endControllerIndex = 3;
            if (IsMasterControllerSensitive == true)
            {
                startControllerIndex = BubbleGame.masterController;
                endControllerIndex = BubbleGame.masterController;
            }
            else if (isPlayerTuned)
            {
                startControllerIndex = controllerIndex;
                endControllerIndex = controllerIndex;
            }

            bool hasChimed = false;
            for (int i = startControllerIndex; i <= endControllerIndex; i++)
            {
                // Move to the previous menu entry?
                if (InputManager.IsActionTriggered(InputManager.Action.CursorUp, i))
                {
                    selectedEntry--;
                    if (selectedEntry < 0)
                        selectedEntry = menuEntries.Count - 1;
                }

                // Move to the next menu entry?
                if (InputManager.IsActionTriggered(InputManager.Action.CursorDown, i))
                {
                    
                    selectedEntry++;
                    if (selectedEntry >= menuEntries.Count)
                        selectedEntry = 0;
                }

                // Accept or cancel the menu?
                if (InputManager.IsActionTriggered(InputManager.Action.Ok, i))
                {
                    if (!hasChimed)
                    {
                        AudioManager.PlayCue("MenuSelect");
                        hasChimed = true;
                    }
                    selectorIndex = i;
                    OnSelectEntry(selectedEntry);
                }
                else if (InputManager.IsActionTriggered(InputManager.Action.Back, i) ||
                    InputManager.IsActionTriggered(InputManager.Action.ExitGame, i))
                {
                    OnCancel();
                }
                else if (selectedEntry != oldSelectedEntry)
                {
                    if (!hasChimed)
                    {
                        AudioManager.PlayCue("MenuSelect");
                        hasChimed = true;
                    }
                }
            }
        }


        protected virtual void OnSelectEntry(int entryIndex)
        {
            menuEntries[selectedEntry].OnSelectEntry();
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
            isStable = false;
            for (int i = 0; i < menuEntries.Count; i++)
            {
                menuEntries[i].StartExit();
            }

            base.AddNextScreen(nextScreen);
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!otherScreenHasFocus && !coveredByOtherScreen)
            {

                // Update each nested MenuEntry object.
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    bool isSelected = IsActive && (i == selectedEntry);

                    menuEntries[i].Update(this, isSelected, gameTime);
                }

                // check if we are stable
                bool allStable = true;
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    if (menuEntries[i].IsStable == false)
                        allStable = false;
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
