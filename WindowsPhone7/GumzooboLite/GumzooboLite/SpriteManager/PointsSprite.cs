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
    class PointsSprite : GameSprite
    {
        Texture2D texture;
        float floatSpeed = 1f;
        int lifeSpan = 1000;
        int lifeElapsed = 0;

        public PointsSprite(int points, Vector2 position)
            : base(position)
        {
            texture = InternalContentManager.GetTexture(points.ToString());

            // add yourself into the level
            Level.singletonLevel.AddSprite(this);
            isActive = true;
        }


        public override void Update(GameTime gameTime)
        {
            position.Y -= floatSpeed;

            lifeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (lifeElapsed >= lifeSpan)
            {
                Level.singletonLevel.RemoveSprite(this);
                isActive = false;
            }

            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            base.Draw(spriteBatch, layerDepth);

            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
        }
    }
}