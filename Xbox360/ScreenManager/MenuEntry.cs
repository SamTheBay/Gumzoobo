#region File Description
//-----------------------------------------------------------------------------
// MenuEntry.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace BubbleGame
{

    public enum AnimationType
    {
        Bounce,
        Slide
    }


    class MenuEntry
    {

        string text;
        SpriteFont spriteFont;
        float fontScale = 1f;
        Vector2 position;
        private string description;
        private Texture2D texture;
        private bool isAnimated;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private int startTime;
        private int duration;
        private int elapsedTime;
        private bool isExiting;
        private bool isStable;
        private int endDuration;
        private AnimationType animationType;


        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public SpriteFont Font
        {
            get { return spriteFont; }
            set { spriteFont = value; }
        }

        public float FontScale
        {
            get { return fontScale; }
            set { fontScale = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public bool IsStable
        {
            get { return isStable; }
        }


        public void SetStartAnimation(Vector2 startPosition, Vector2 endPosition, int startTime, int duration, int endDuration)
        {
            isAnimated = true;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.startTime = startTime;
            this.duration = duration;
            elapsedTime = 0;
            position = startPosition;
            isExiting = false;
            isStable = false;
            this.endDuration = endDuration;
            animationType = AnimationType.Bounce;
        }

        public void SetAnimationType(AnimationType type)
        {
            animationType = type;
        }

        public void StartExit()
        {
            isExiting = true;
            isStable = false;
            elapsedTime = 0;
        }

        public void Reset()
        {
            if (isAnimated)
            {
                elapsedTime = 0;
                position = startPosition;
                isExiting = false;
                isStable = false;
            }
        }


        public event EventHandler<EventArgs> Selected;


        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }


        public MenuEntry(string text)
        {
            this.text = text;
        }


        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            if (isAnimated)
            {
                if (isExiting == false)
                {
                    elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedTime > startTime && elapsedTime < startTime + duration)
                    {
                        float progress = ((float)elapsedTime - (float)startTime) / (float)duration;
                        if (animationType == AnimationType.Bounce)
                        {
                            progress = Bounce(progress);
                        }
                        else if (animationType == AnimationType.Slide)
                        {
                            progress = (float)Math.Pow((double)progress, .5f);
                        }

                        position.X = startPosition.X + ((endPosition.X - startPosition.X) * progress);
                        position.Y = startPosition.Y + ((endPosition.Y - startPosition.Y) * progress);
                    }
                    else if (elapsedTime > startTime + duration)
                    {
                        position = endPosition;
                        isStable = true;
                    }
                }
                else
                {
                    elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedTime > startTime && elapsedTime < startTime + endDuration)
                    {
                        float progress = ((float)elapsedTime - (float)startTime) / (float)endDuration;
                        progress = (float)Math.Pow((double)progress, 3f);

                        position.X = endPosition.X + ((startPosition.X - endPosition.X) * progress);
                        position.Y = endPosition.Y + ((startPosition.Y - endPosition.Y) * progress);
                    }
                    else if (elapsedTime > startTime + endDuration)
                    {
                        position = startPosition;
                        isStable = true;
                    }
                }
            }
            else
            {
                isStable = true;
            }
        }

        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Fonts.MenuSelectedColor : Fonts.TitleColor;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;

            if (texture != null)
            {
                spriteBatch.Draw(texture, position, Color.White);
                if ((spriteFont != null) && !String.IsNullOrEmpty(text))
                {
                    Vector2 textSize = spriteFont.MeasureString(text);
                    textSize.X *= fontScale;
                    textSize.Y *= fontScale;
                    Vector2 textPosition = position + new Vector2(
                        (float)Math.Floor((texture.Width - textSize.X) / 2),
                        (float)Math.Floor((texture.Height - textSize.Y) / 2));
                    spriteBatch.DrawString(spriteFont, text, textPosition, color, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                }
            }
            else if ((spriteFont != null) && !String.IsNullOrEmpty(text))
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return Font.LineSpacing;
        }


        // Penner bounce
        public static float Bounce(float pos)
        {
            if (pos < (1f / 2.75f))
            {
                return (7.5625f * pos * pos);
            }
            else if (pos < (2f / 2.75f))
            {
                return (7.5625f * (pos -= (1.5f / 2.75f)) * pos + .75f);
            }
            else if (pos < (2.5f / 2.75f))
            {
                return (7.5625f * (pos -= (2.25f / 2.75f)) * pos + .9375f);
            }
            else
            {
                return (7.5625f * (pos -= (2.625f / 2.75f)) * pos + .984375f);
            }
        }
    }
}
