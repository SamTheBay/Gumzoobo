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
    class LocationDescriptor
    {
        public string name;
        public Vector2 mapLocation;
        public int startLevel;
        public int endLevel;
        public string backgroundTexture;
        public string foregroundTexture;
        public string envTextureName;
        public int envTextureFrame;
        public string musicName;
        public string cutSceneName;
        public int locationIndex;

        public LocationDescriptor(string name, Vector2 mapLocation, int startLevel, int endLevel,
            string backgroundTexture, string foregroundTexture, string envTextureName, int envTextureFrame,
            string musicName, string cutSceneName)
        {
            this.name = name;
            this.mapLocation = mapLocation;
            this.startLevel = startLevel;
            this.endLevel = endLevel;
            this.backgroundTexture = backgroundTexture;
            this.foregroundTexture = foregroundTexture;
            this.envTextureName = envTextureName;
            this.envTextureFrame = envTextureFrame;
            this.musicName = musicName;
            this.cutSceneName = cutSceneName;
        }

    }
}