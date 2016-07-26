using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace BubbleGame
{
    class Tune
    {
        string tuneName;
        int startCueLoopTime;
        int loopCueLoopTime;
        string startCueName;
        string loopCueName;
        Cue currentCue;
        bool inLoop;
        bool isPaused;
        bool isPlaying;
        DateTime lastTime;
        

        public Tune(string tuneName, string startCueName, string loopCueName, int startCueLoopTime, int loopCueLoopTime)
        {
            this.tuneName = tuneName;
            this.startCueLoopTime = startCueLoopTime;
            this.loopCueLoopTime = loopCueLoopTime;
            this.startCueName = startCueName;
            this.loopCueName = loopCueName;
            this.inLoop = false;
            this.isPaused = false;
            this.isPlaying = false;
            this.currentCue = null;
        }


        public void PlayTune()
        {
            StopTune();
            currentCue = AudioManager.GetCue(startCueName);
            currentCue.Play();
            isPlaying = true;
            lastTime = DateTime.Now;
        }

        public void StopTune()
        {
            if (currentCue != null)
            {
                currentCue.Stop(AudioStopOptions.AsAuthored);
                currentCue.Dispose();
                currentCue = null;
            }
            inLoop = false;
            isPaused = false;
            isPlaying = false;
        }

        public void PauseTune()
        {
            if (currentCue != null && isPlaying == true && isPaused == false)
            {
                currentCue.Pause();
                isPaused = true;
            }
        }

        public void ResumeTune()
        {
            if (currentCue != null && isPlaying == true && isPaused == true)
            {
                currentCue.Resume();
                isPaused = false;
            }
        }


        public void Update(GameTime gameTime)
        {
            if (currentCue != null && isPlaying == true && isPaused == false)
            {
                TimeSpan difference = DateTime.Now - lastTime;

                if ((difference.TotalMilliseconds >= startCueLoopTime && inLoop == false) ||
                    (difference.TotalMilliseconds >= loopCueLoopTime && inLoop == true))
                {
                    currentCue = AudioManager.GetCue(loopCueName);
                    currentCue.Play();
                    inLoop = true;
                    lastTime = DateTime.Now;
                }
            }
        }


        public string TuneName
        {
            get { return tuneName; }
        }

    }


    class MusicManager
    {
        public static MusicManager SingletonMusicManager;
        private Tune[] tunes;

        public MusicManager()
        {
            SingletonMusicManager = this;

            tunes = new Tune[9];
            tunes[0] = new Tune("desert", "Desert", "DesertLoop", 160000, 144000);
            tunes[1] = new Tune("aquarium", "Aquarium", "AquariumLoop", 168000, 144000);
            tunes[2] = new Tune("intro", "Title", "TitleLoop", 72727, 69091); 
            tunes[3] = new Tune("generic", "Generic", "GenericLoop", 137142, 129924);
            tunes[4] = new Tune("nighthouse", "NightHouse", "NightHouse", 218823, 218823);
            tunes[5] = new Tune("levelselect", "LevelSelect", "LevelSelect", 29538, 29538);
            tunes[6] = new Tune("pettingzoo", "PettingZoo", "PettingZooLoop", 100000, 96000);
            tunes[7] = new Tune("generic2", "Generic2", "Generic2", 101818, 101818);
            tunes[8] = new Tune("swamp", "Swamp", "Swamp", 122880, 122880);
        }

        public void PlayTune(string tuneName)
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                if (tunes[i].TuneName == tuneName)
                {
                    tunes[i].PlayTune();
                }
            }
        }


        public void PauseTune(string tuneName)
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                if (tunes[i].TuneName == tuneName)
                {
                    tunes[i].PauseTune();
                }
            }
        }


        public void ResumeTune(string tuneName)
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                if (tunes[i].TuneName == tuneName)
                {
                    tunes[i].ResumeTune();
                }
            }
        }


        public void StopTune(string tuneName)
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                if (tunes[i].TuneName == tuneName)
                {
                    tunes[i].StopTune();
                }
            }
        }

        public void StopAll()
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                tunes[i].StopTune();
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < tunes.Length; i++)
            {
                tunes[i].Update(gameTime);
            }
        }

    }
}