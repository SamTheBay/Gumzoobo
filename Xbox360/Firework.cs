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
    class Particle
    {
        Vector2 offset = new Vector2();
        Vector2 velocity;
        Rectangle destination = new Rectangle();

        public Particle(Vector2 velocity)
        {
            Reset();
            this.velocity = velocity;
        }

        public void Reset()
        {
            offset.X = 0;
            offset.Y = 0;
        }

        public void Update(GameTime gameTime)
        {
            offset.X += velocity.X * (gameTime.ElapsedGameTime.Milliseconds / 100f);
            offset.Y += velocity.Y * (gameTime.ElapsedGameTime.Milliseconds / 100f);
        }

        public void Draw(SpriteBatch spriteBatch, Color color, Vector2 size, Vector2 center)
        {
            destination.X = (int)(center.X + offset.X);
            destination.Y = (int)(center.Y + offset.Y);
            destination.Width = (int)size.X;
            destination.Height = (int)size.Y;
            spriteBatch.Draw(InternalContentManager.GetTexture("Blank"), destination, color);
        }
    }




    class Firework
    {
        Vector2 center;
        Color color;
        int duration = 3000;
        int currentElapsed = 3000;
        Particle[] particles;
        int velocityLevels;
        int particlesPerSegment;
        Vector2 particleSize = new Vector2(3,3);

        public Firework(int particlesPerSegment, int velocityLevels, float velocityIncrements)
        {
            this.particlesPerSegment = particlesPerSegment;
            this.velocityLevels = velocityLevels;
            particles = new Particle[particlesPerSegment * 4 * velocityLevels];
            for (int v = 0; v < velocityLevels; v++)
            {
                int velocity = (int)((float)Math.Pow((double)v, 1.5f) * velocityIncrements + 1f);
                for (int p = 0; p < particlesPerSegment; p++)
                {
                    float x = (float)p / (float)particlesPerSegment-1;
                    float y = (float)Math.Sqrt(1-Math.Pow((double)x, 2));
                    x *= velocity;
                    y *= velocity;
                    particles[p * 4 + v * 4 * particlesPerSegment] = new Particle(new Vector2(x, y));
                    particles[p * 4 + 1 + v * 4 * particlesPerSegment] = new Particle(new Vector2(-1 * x, y));
                    particles[p * 4 + 2 + v * 4 * particlesPerSegment] = new Particle(new Vector2(x, -1 * y));
                    particles[p * 4 + 3 + v * 4 * particlesPerSegment] = new Particle(new Vector2(-1 * x, -1 * y));
                }
            }
        }


        public bool IsFinished
        {
            get { return currentElapsed >= duration; }
        }


        public void Update(GameTime gameTime)
        {
            currentElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (!IsFinished)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsFinished)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Draw(spriteBatch, color, particleSize, center);
                }
            }
        }


        public void Reset()
        {
            if (!IsFinished)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Reset();
                }
            }
        }

        public void SetOff(Color color, Vector2 center)
        {
            this.color = color;
            this.center = center;
            currentElapsed = 0;
            Reset();
        }

    }
}