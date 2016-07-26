using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace BubbleGame
{
    class Tune
    {
        string tuneName;
        int startCueLoopTime;
        int loopCueLoopTime;
        string startCueName;
        string loopCueName;
        bool isPlaying;
        DateTime lastTime;
        

        public Tune(string tuneName, string startCueName, string loopCueName, int startCueLoopTime, int loopCueLoopTime)
        {
            this.tuneName = tuneName;
            this.startCueLoopTime = startCueLoopTime;
            this.loopCueLoopTime = loopCueLoopTime;
            this.startCueName = startCueName;
            this.loopCueName = loopCueName;
            this.isPlaying = false;
        }


        public void PlayTune()
        {
            if (isPlaying)
                StopTune();
            AudioManager.audioManager.PlaySFX(startCueName);
            isPlaying = true;
            lastTime = DateTime.Now;
        }

        public void StopTune()
        {
            AudioManager.audioManager.StopAllSFX(startCueName);
            AudioManager.audioManager.StopAllSFX(loopCueName);
            isPlaying = false;

        }


        public void LoadTune(ContentManager content)
        {
            AudioManager.audioManager.LoadSFX(startCueName, content);
            GC.Collect();
            if (startCueName != loopCueName)
            {
                AudioManager.audioManager.LoadSFX(loopCueName, content);
                GC.Collect();
            }
        }


        public void UnloadTune()
        {
            AudioManager.audioManager.UnloadSFX(startCueName);
            GC.Collect();
            if (startCueName != loopCueName)
            {
                AudioManager.audioManager.UnloadSFX(loopCueName);
                GC.Collect();
            }
        }


        public string TuneName
        {
            get { return tuneName; }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
        }

    }


    class MusicManager
    {
        public static MusicManager SingletonMusicManager;
        private Tune[] tunes;
        public static bool musicOn = false;

        public MusicManager()
        {
            SingletonMusicManager = this;

            tunes = new Tune[9];
            tunes[0] = new Tune("desert", "Desert", "Desert", 160000, 144000);
            tunes[1] = new Tune("aquarium", "Aquarium", "Aquarium", 168000, 144000);
            tunes[2] = new Tune("intro", "Title", "Title", 72727, 69091); 
            tunes[3] = new Tune("generic", "Generic", "Generic", 137142, 129924);
            tunes[4] = new Tune("nighthouse", "NightHouse", "NightHouse", 218823, 218823);
            tunes[5] = new Tune("levelselect", "LevelSelect", "LevelSelect", 29538, 29538);
            tunes[6] = new Tune("pettingzoo", "PettingZoo", "PettingZoo", 100000, 96000);
            tunes[7] = new Tune("generic2", "Generic2", "Generic2", 101818, 101818);
            tunes[8] = new Tune("swamp", "Swamp", "Swamp", 122880, 122880);
        }

        public void PlayTune(string tuneName)
        {
            if (musicOn == true)
            {
                for (int i = 0; i < tunes.Length; i++)
                {
                    if (tunes[i].TuneName == tuneName)
                    {
                        tunes[i].PlayTune();
                    }
                }
            }
        }

        public void LoadTune(string tuneName, ContentManager content)
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                if (tunes[i].TuneName == tuneName)
                {
                    tunes[i].LoadTune(content);
                }
            }
        }

        public void UnloadTune(string tuneName)
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                if (tunes[i].TuneName == tuneName)
                {
                    tunes[i].UnloadTune();
                }
            }
        }

        public void StopTune(string tuneName)
        {
            if (musicOn == true)
            {
                for (int i = 0; i < tunes.Length; i++)
                {
                    if (tunes[i].TuneName == tuneName)
                    {
                        tunes[i].StopTune();
                    }
                }
            }
        }

        public void StopAll()
        {
            if (musicOn == true)
            {
                for (int i = 0; i < tunes.Length; i++)
                {
                    if (tunes[i].IsPlaying)
                        tunes[i].StopTune();
                }
            }
        }

    }
}