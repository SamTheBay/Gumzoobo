#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
#endregion

namespace BubbleGame
{
    public abstract class SoundInstance
    {
        public abstract void LoadEffect(ContentManager content);

        public abstract void UnloadEffect();

        public abstract int Play();

        public abstract void StopAll();

        public abstract String Name();
    }


    public class MusicInstance : SoundInstance
    {
        Song song;
        String name;
        String location;
        float volume;

        public MusicInstance(String EffectLocation, String Name)
        {
            Initialize(EffectLocation, Name, .5f);
        }

        public MusicInstance(String EffectLocation, String Name, float volume)
        {
            Initialize(EffectLocation, Name, volume);
        }

        public void Initialize(String Location, String Name, float volume)
        {
            this.volume = volume;
            this.location = Location;
            this.name = Name;
        }

        public override void LoadEffect(ContentManager content)
        {
            song = content.Load<Song>(location);
        }

        public override void UnloadEffect()
        {
            song.Dispose();
            song = null;
        }

        public override int Play()
        {
            if (MusicManager.musicOn)
            {
                MediaPlayer.Play(song);
                //MediaPlayer.Volume = volume;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.IsShuffled = false; 
            }
            return 0;
        }

        public override void StopAll()
        {
            if (MusicManager.musicOn)
            {
                MediaPlayer.Stop();
            }
        }

        public override String Name()
        {
            return name;
        }
    }


    public class SFXInstance : SoundInstance
    {
        SoundEffect effect;
        String name;
        String effectLocation;
        int maxIntanceCount;
        SoundEffectInstance [] instances;
        bool isMusic;
        float volume;

        public SFXInstance(String EffectLocation, String Name, int MaxInstances)
        {
            Initialize(EffectLocation, Name, MaxInstances, .5f, false);
        }

        public SFXInstance(String EffectLocation, String Name, int MaxInstances, bool isMusic)
        {
            Initialize(EffectLocation, Name, MaxInstances, .5f, isMusic);
        }

        public SFXInstance(String EffectLocation, String Name, int MaxInstances, float volume)
        {
            Initialize(EffectLocation, Name, MaxInstances, volume, false);
        }

        public SFXInstance(String EffectLocation, String Name, int MaxInstances, float volume, bool isMusic)
        {
            Initialize(EffectLocation, Name, MaxInstances, volume, isMusic);
        }

        public void Initialize(String EffectLocation, String Name, int MaxInstances, float volume, bool isMusic)
        {
            this.volume = volume;
            this.effectLocation = EffectLocation;
            this.isMusic = isMusic;
            this.name = Name;
            this.maxIntanceCount = MaxInstances;
        }

        public override void LoadEffect(ContentManager content)
        {
            if (effect == null)
                effect = content.Load<SoundEffect>(effectLocation);
            if (instances == null)
                instances = new SoundEffectInstance[maxIntanceCount];
            for (int i = 0; i < maxIntanceCount; i++)
            {
                if (instances[i] == null)
                {
                    instances[i] = effect.CreateInstance();
                    instances[i].Volume = volume;
                }
                
            }
        }

        public override void UnloadEffect()
        {
            if (instances != null)
            {
                for (int i = 0; i < maxIntanceCount; i++)
                {
                    if (instances[i] != null)
                    {
                        instances[i].Dispose();
                        instances[i] = null;
                    }
                }
                instances = null;
            }

            if (effect != null)
            {
                effect.Dispose();
                effect = null;
            }
            GC.Collect();
        }

        public override int Play()
        {
            if (AudioManager.SoundFXOn)
            {
                for (int i = 0; i < maxIntanceCount; i++)
                {
                    if (instances[i].State == SoundState.Stopped)
                    {
                        instances[i].Play();
                        return i;
                    }
                }
            }
            return int.MaxValue;
        }

        public override void StopAll()
        {
            if (AudioManager.SoundFXOn)
            {
                for (int i = 0; i < maxIntanceCount; i++)
                {
                    if (instances != null)
                        instances[i].Stop();
                }
            }
        }

        public override String Name()
        {
            return name;
        }
    }
    


    public class AudioManager : GameComponent
    {
        public static AudioManager audioManager = null;
        private List<SoundInstance> SFXList;
        public static bool SoundFXOn = true;


        private AudioManager(Game game)
            : base(game)
        {
            // load up all of the sound files
            SFXList = new List<SoundInstance>();
            SFXList.Add(new SFXInstance("Audio\\Whoosh", "Whoosh", 2));
            SFXList.Add(new SFXInstance("Audio\\bubble_jump", "BubbleJump", 2, 1f));
            SFXList.Add(new SFXInstance("Audio\\bubble_pop", "BubblePop", 2));
            SFXList.Add(new SFXInstance("Audio\\bubble_shoot", "BubbleShoot", 2, .8f));
            SFXList.Add(new SFXInstance("Audio\\cinnamon_burst", "CinnamonBurst", 2, .8f));
            SFXList.Add(new SFXInstance("Audio\\freeze", "Freeze", 2));
            SFXList.Add(new SFXInstance("Audio\\Frog Super Jump", "SuperJump", 1, 1f));
            SFXList.Add(new SFXInstance("Audio\\GAME_OVER", "GameOver", 1));
            SFXList.Add(new SFXInstance("Audio\\Gum_Grab", "GumGrab", 2, .8f));
            SFXList.Add(new SFXInstance("Audio\\Inviz wahn", "Invizable", 1));
            SFXList.Add(new SFXInstance("Audio\\Jump", "Jump", 2, 1f));
            SFXList.Add(new SFXInstance("Audio\\Lazer", "Lazer", 1));
            SFXList.Add(new SFXInstance("Audio\\menu_select", "MenuSelect", 1, .8f));
            SFXList.Add(new SFXInstance("Audio\\Multi-bubble_grape", "GrapeBubble", 2, .8f));
            SFXList.Add(new SFXInstance("Audio\\penguin_flapping", "PenguinFlapping", 1, 1f));
            SFXList.Add(new SFXInstance("Audio\\Seel_sliding", "SealSlide", 1, 1f));
            SFXList.Add(new SFXInstance("Audio\\Turtle_in_shell", "TurtleInShell", 1, 1f));
            SFXList.Add(new SFXInstance("Audio\\Turtle_out_shell", "TurtleOutShell", 1, 1f));
            SFXList.Add(new MusicInstance("Audio\\title_track", "Title"));
            //SFXList.Add(new MusicInstance("Audio\\title_loop_track", "TitleLoop"));
            SFXList.Add(new MusicInstance("Audio\\level_select", "LevelSelect"));
            ////SFXList.Add(new SFXInstance("Audio\\aquarium_loop_track", "AquariumLoop", 1, true));
            //SFXList.Add(new MusicInstance("Audio\\aquarium_track", "Aquarium"));
            //SFXList.Add(new MusicInstance("Audio\\desert_loop_track", "DesertLoop"));
            //SFXList.Add(new MusicInstance("Audio\\desert_track", "Desert"));
            //SFXList.Add(new MusicInstance("Audio\\generic_loop_track", "GenericLoop"));
            SFXList.Add(new MusicInstance("Audio\\generic_track", "Generic"));
            SFXList.Add(new MusicInstance("Audio\\generic2_track", "Generic2"));
            SFXList.Add(new MusicInstance("Audio\\night_house_track", "NightHouse"));
            ////SFXList.Add(new SFXInstance("Audio\\petting_zoo_loop_track", "PettingZooLoop", 1, true));
            ////SFXList.Add(new SFXInstance("Audio\\petting_zoo_track", "PettingZoo", 1, true));
            //SFXList.Add(new MusicInstance("Audio\\swamp_track", "Swamp"));
        }


        public static void Initialize(Game game)
        {
            audioManager = new AudioManager(game);
            if (game != null)
            {
                //game.Components.Add(audioManager);
            }
        }



        public void PlaySFX(String soundName)
        {
            for (int i = 0; i < SFXList.Count; i++)
            {
                if (soundName == SFXList[i].Name())
                {
                    SFXList[i].Play();
                }
            }
        }


        public void StopAllSFX(String soundName)
        {
            for (int i = 0; i < SFXList.Count; i++)
            {
                if (soundName == SFXList[i].Name())
                {
                    SFXList[i].StopAll();
                }
            }
        }

        public void LoadSFX(String soundName, ContentManager content)
        {
            for (int i = 0; i < SFXList.Count; i++)
            {
                if (soundName == SFXList[i].Name())
                {
                    SFXList[i].LoadEffect(content);
                    break;
                }
            }
        }

        public void LoadSFX(int index1, int index2)
        {
            for (int i = index1; i <= index2; i++)
                SFXList[i].LoadEffect(BubbleGame.sigletonGame.Content);
        }

        public void UnloadSFX(String soundName)
        {
            for (int i = 0; i < SFXList.Count; i++)
            {
                if (soundName == SFXList[i].Name())
                {
                    SFXList[i].UnloadEffect();
                    break;
                }
            }
        }

        public int SFXNum
        {
            get { return SFXList.Count; }
        }


    }
}
