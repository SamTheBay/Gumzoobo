using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BubbleGame
{
    class PopUpScreen : MenuScreen
    {
        protected Vector2 windowCorner;
        protected bool isPopUpAnimated;
        protected Vector2 startPopUpPosition;
        protected Vector2 endPopUpPosition;
        protected int startPopUpTime;
        protected int popUpDuration;
        protected int endPopUpDuration;
        protected int elapsedTime;
        protected bool isPopUpExiting;
        protected bool hasPlayedWhoosh;

        public int StartPopUpTime
        {
            set { startPopUpTime = value; }
        }

        public Vector2 StartPopUpPosition
        {
            get { return startPopUpPosition; }
            set { startPopUpPosition = value; }
        }

        public Vector2 EndPopUpPosition
        {
            get { return endPopUpPosition; }
            set { endPopUpPosition = value; }
        }

        public Vector2 WindowCorner
        {
            get { return windowCorner; }
            set { windowCorner = value; }
        }

        public PopUpScreen()
        {
            IsPopup = true;
        }


        public void SetPopUpAnimation(Vector2 startPosition, Vector2 endPosition, int startTime, int duration, int endDuration)
        {
            isPopUpAnimated = true;
            this.startPopUpPosition = startPosition;
            this.endPopUpPosition = endPosition;
            this.startPopUpTime = startTime;
            this.popUpDuration = duration;
            this.endPopUpDuration = endDuration;
            this.elapsedTime = 0;
            this.isPopUpExiting = false;
            windowCorner = startPopUpPosition;
            hasPlayedWhoosh = false;
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (isPopUpAnimated && !otherScreenHasFocus && !coveredByOtherScreen)
            {
                if (isPopUpExiting == false)
                {
                    elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (hasPlayedWhoosh == false && elapsedTime > 200 + startPopUpTime)
                    {
                        AudioManager.PlayCue("Whoosh");
                        hasPlayedWhoosh = true;
                    }
                    if (elapsedTime > startPopUpTime && elapsedTime < startPopUpTime + popUpDuration)
                    {
                        float progress = ((float)elapsedTime - (float)startPopUpTime) / (float)popUpDuration;
                        progress = (float)Math.Pow((double)progress, .5f);
                        

                        windowCorner.X = startPopUpPosition.X + ((endPopUpPosition.X - startPopUpPosition.X) * progress);
                        windowCorner.Y = startPopUpPosition.Y + ((endPopUpPosition.Y - startPopUpPosition.Y) * progress);
                        isStable = false;
                    }
                    else if (elapsedTime > startPopUpTime + popUpDuration)
                    {
                        windowCorner = endPopUpPosition;
                    }
                }
                else
                {
                    elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (hasPlayedWhoosh == false && elapsedTime > 200 + startPopUpTime)
                    {
                        AudioManager.PlayCue("Whoosh");
                        hasPlayedWhoosh = true;
                    }
                    if (elapsedTime > startPopUpTime && elapsedTime < startPopUpTime + endPopUpDuration)
                    {
                        float progress = ((float)elapsedTime - (float)startPopUpTime) / (float)endPopUpDuration;
                        progress = progress * progress * progress;

                        windowCorner.X = endPopUpPosition.X + ((startPopUpPosition.X - endPopUpPosition.X) * progress);
                        windowCorner.Y = endPopUpPosition.Y + ((startPopUpPosition.Y - endPopUpPosition.Y) * progress);
                        isStable = false;
                    }
                    else if (elapsedTime > startPopUpTime + endPopUpDuration)
                    {
                        windowCorner = startPopUpPosition;
                    }
                    else
                    {
                        isStable = false;
                    }
                }
            }
        }


        public override void ExitScreen()
        {
            if (isExiting == false)
            {
                hasPlayedWhoosh = false;
                base.ExitScreen();
                isPopUpExiting = true;
                elapsedTime = 0;
            }
        }

        public override void AddNextScreen(GameScreen nextScreen)
        {
            hasPlayedWhoosh = false;
            base.AddNextScreen(nextScreen);
            isPopUpExiting = true;
            elapsedTime = 0;
        }

        public override void ResetScreen()
        {
            base.ResetScreen();
            elapsedTime = 0;
            isPopUpExiting = false;
        }


    }
}