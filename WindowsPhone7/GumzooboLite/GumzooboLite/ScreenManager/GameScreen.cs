#region File Description
//-----------------------------------------------------------------------------
// GameScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace BubbleGame
{
    public enum ScreenState
    {
        Active,
        Hidden
    }


    public abstract class GameScreen
    {
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;


        public bool IsMasterControllerSensitive
        {
            get { return isMasterControllerSensitive; }
            set { isMasterControllerSensitive = value; }
        }

        bool isMasterControllerSensitive = false;


        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.Active;


        protected bool isExiting = false;
        public bool IsExiting
        {
            get { return isExiting; }
            set { isExiting = value; }
        }

        protected bool isStable = true;
        public bool IsStable
        {
            get { return isStable; }
        }


        protected bool restartOnVisible = false;
        public bool RestartOnVisible
        {
            get { return restartOnVisible; }
            set { restartOnVisible = value; }
        }

        bool isLoadingNext = false;
        GameScreen nextScreen;
        public bool IsLoadingNext
        {
            get { return isLoadingNext; }
        }


        public virtual void AddNextScreen(GameScreen nextScreen)
        {
            AddNextScreen(nextScreen, true);
        }

        public virtual void AddNextScreen(GameScreen nextScreen, bool doExitAnimation)
        {
            isLoadingNext = true;
            this.nextScreen = nextScreen;
        }

        public virtual void AddNextScreenAndExit(GameScreen nextScreen)
        {
            AddNextScreen(nextScreen);
            ExitScreen();
        }


        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus;
            }
        }

        protected bool otherScreenHasFocus;

        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;


        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void ResetScreen()
        { 
            isLoadingNext = false;
            isExiting = false;
            nextScreen = null;
        }

        public virtual void TopFullScreenAcquired() { }

        public virtual void OnRemoval() { }


        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            // set state of whether we are hidden or not
            if (coveredByOtherScreen)
                screenState = ScreenState.Hidden;
            else
                screenState = ScreenState.Active;

            if (IsExiting)
            {
                if (isStable)
                {
                    // When the transition finishes, remove the screen.
                    OnRemoval();
                    ScreenManager.RemoveScreen(this);
                }
            }
            if (isLoadingNext)
            {
                if (isStable)
                {
                    screenManager.AddScreen(nextScreen);
                    isLoadingNext = false;
                }
            }
        }

        public GestureType EnabledGestures
        {
            get { return enabledGestures; }
            protected set
            {
                enabledGestures = value;

                // the screen manager handles this during screen changes, but
                // if this screen is active and the gesture types are changing,
                // we have to update the TouchPanel ourself.
                if (ScreenState == ScreenState.Active)
                {
                    TouchPanel.EnabledGestures = value;
                }
            }
        }

        GestureType enabledGestures = GestureType.None;


        public virtual void HandleInput() { }

        public virtual void Draw(GameTime gameTime) { }


        public virtual void ExitScreen()
        {
            // flag that it should transition off and then exit.
            IsExiting = true;
            // If the screen has a zero transition time, remove it immediately.
            if (isStable)
            {
                OnRemoval();
                ScreenManager.RemoveScreen(this);
                if (nextScreen != null)
                    screenManager.AddScreen(nextScreen);
            }
        }

        public virtual void ExitScreenImmediate()
        {
            OnRemoval();
            ScreenManager.RemoveScreen(this);
        }


        public void DrawBoarder(Rectangle windowLocation, Texture2D texture, int boarderwidth, Color centerTint, SpriteBatch spriteBatch)
        {
            // fill will boarder texture
            for (int x = windowLocation.X; x < windowLocation.X + windowLocation.Width; x += texture.Width)
            {
                for (int y = windowLocation.Y; y < windowLocation.Y + windowLocation.Height; y += texture.Height)
                {
                    int WidthToUse = texture.Width, HeightToUse = texture.Height;
                    if (x + texture.Width > windowLocation.X + windowLocation.Width)
                        WidthToUse = windowLocation.X + windowLocation.Width - x;
                    if (y + texture.Height > windowLocation.Y + windowLocation.Height)
                        HeightToUse = windowLocation.Y + windowLocation.Height - y;
                    spriteBatch.Draw(texture, new Rectangle(x, y, WidthToUse, HeightToUse), new Rectangle(0, 0, WidthToUse, HeightToUse), Color.White);
                }
            }

            // cover center with blank

            Texture2D blank = InternalContentManager.GetTexture("Blank");
            for (int x = windowLocation.X + boarderwidth; x < windowLocation.X + windowLocation.Width - boarderwidth; x += texture.Width)
            {
                for (int y = windowLocation.Y + boarderwidth; y < windowLocation.Y + windowLocation.Height - boarderwidth; y += texture.Height)
                {
                    int WidthToUse = texture.Width, HeightToUse = texture.Height;
                    if (x + texture.Width > windowLocation.X + windowLocation.Width - boarderwidth)
                        WidthToUse = windowLocation.X + windowLocation.Width - boarderwidth - x;
                    if (y + texture.Height > windowLocation.Y + windowLocation.Height - boarderwidth)
                        HeightToUse = windowLocation.Y + windowLocation.Height - boarderwidth - y;
                    spriteBatch.Draw(blank, new Rectangle(x, y, WidthToUse, HeightToUse), new Rectangle(0, 0, WidthToUse, HeightToUse), centerTint);
                }
            }


        }

        static public PlayerIndex IntToPlayerIndex(int index)
        {
            if (index == 0)
                return PlayerIndex.One;
            else if (index == 1)
                return PlayerIndex.Two;
            else if (index == 2)
                return PlayerIndex.Three;
            else if (index == 3)
                return PlayerIndex.Four;
            return PlayerIndex.One;
        }


    }
}
