
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BubbleGame
{
    public static class Util
    {
        public static readonly int Seed = Environment.TickCount;
        public static readonly Random Random = new Random(Seed);

        public static float RandomBetween(float min, float max)
        {
            return min + (float)Random.NextDouble() * (max - min);
        }

        public static double DistanceTo(this Point point, Point other)
        {
            int a = point.X - other.X;
            int b = point.Y - other.Y;
            return Math.Sqrt(a * a + b * b);
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
        
        public static String Wrap(this String text, SpriteFont font, int width)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }
    }
    
    public struct FloatRectangle
    {
        public float X, Y, Width, Height;

        FloatRectangle(float X, float Y, float Width, float Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public bool Intersects(FloatRectangle otherRect)
        {
            if ((X + Width > otherRect.X && X < otherRect.X + otherRect.Width) && 
                (Y + Height > otherRect.Y && Y < otherRect.Y + otherRect.Height))
            {
                return true;
            }
            
            return false;
        }
    }

}
