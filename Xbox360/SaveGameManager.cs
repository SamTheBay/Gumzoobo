using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace BubbleGame
{
    class SaveGameManager
    {
        StorageDevice device = null;
        IAsyncResult result = null;
        SaveGameRecord[] SavedGames = new SaveGameRecord[10];
        string SaveFileLocation = "Gumzoobo.sav";
        string SaveFileContainer = "Gumzoobo";
        bool connectingDevice = false;
#if XBOX
        bool savingDisabled = false;
#else
        bool savingDisabled = true;
#endif
        string ownerGamerTag = null;

        public static SaveGameRecord CurrentOpenedGame;
        public static SaveGameManager SingletonSaveManager;

        ContinueQuestion questionScreen = null;

        public void DisableSaving()
        {
            savingDisabled = true;
            CurrentOpenedGame = new SaveGameRecord("", 0);
        }

        public bool SavingDisabled
        {
            get { return savingDisabled; }
        }


        public SaveGameManager()
        {
            SingletonSaveManager = this;
        }


        public void ResetDevice()
        {
#if XBOX
            savingDisabled = false;
#endif
            device = null;
            result = null;
            connectingDevice = false;
            SavedGames = new SaveGameRecord[10];
            questionScreen = null;
            ownerGamerTag = null;
        }

        public void StartConnectStorageDevice(PlayerIndex pindex)
        {
            if (connectingDevice == false && Guide.IsVisible == false)
            {
                try
                {
                    result = Guide.BeginShowStorageDeviceSelector(pindex,
                                                               null, null);
                }
                catch (Exception)
                {
                    return;
                }
                connectingDevice = true;
            }
        }


        public void UpdateConnectStorageDevice()
        {
            try
            {
                if ((result != null) && result.IsCompleted)
                {
                    device = Guide.EndShowStorageDeviceSelector(result);
                    result = null;
                    connectingDevice = false;
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        public bool IsStorageDeviceConnected()
        {
            return ((device != null) && device.IsConnected);
        }


        public void SignInMasterPlayer()
        {
            bool isSignedIn = false;
            for (int i = 0; i < Gamer.SignedInGamers.Count; i++)
            {
                if (Gamer.SignedInGamers[i].PlayerIndex == BubbleGame.IntToPI(BubbleGame.masterController))
                {
                    isSignedIn = true;
                    break;
                }
            }
            if (isSignedIn == false)
            {
                try
                {
                    Guide.ShowSignIn(1, false);
                }
                catch (Exception)
                {

                }
            }
            
        }

        public bool UpdatePlayerSignIn()
        {
            if (Guide.IsVisible == true)
                return false;
            else
                return true;
        }

        public bool IsMasterPlayerSignedIn()
        {
            bool isSignedIn = false;
            for (int i = 0; i < Gamer.SignedInGamers.Count; i++)
            {
                if (Gamer.SignedInGamers[i].PlayerIndex == BubbleGame.IntToPI(BubbleGame.masterController))
                {
                    isSignedIn = true;
                    break;
                }
            }
            return isSignedIn;
        }


        public bool ReadSaveFile()
        {
            // Open a storage container
            try
            {
                // save the owner of this save file
                Gamer master = SignedInGamer.SignedInGamers[BubbleGame.IntToPI(BubbleGame.masterController)];
                ownerGamerTag = master.Gamertag;

                using (StorageContainer container = device.OpenContainer(SaveFileContainer))
                {
                    // Add the container path to our filename
                    string filename = Path.Combine(container.Path, SaveFileLocation);

                    // There is no save file so exit
                    if (!File.Exists(filename))
                        return true;

                    // Open save file and read high score
                    using (FileStream saveGameFile = new FileStream(filename,
                        FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(saveGameFile))
                        {
                            SaveGameRecord record;
                            do
                            {
                                record = new SaveGameRecord();
                                record.Deserialize(reader);
                                if (record.IsValid)
                                {
                                    SavedGames[record.SaveIndex] = record;
                                }
                            } while (record.IsValid);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public bool WriteSaveFile()
        {
            if (savingDisabled)
                return false;

            // Open a storage container
            try
            {
                // check to make sure the same gamer tag is signed in
                Gamer master = SignedInGamer.SignedInGamers[BubbleGame.IntToPI(BubbleGame.masterController)];
                if (master.Gamertag != ownerGamerTag)
                {
                    throw new Exception();
                }

                using (StorageContainer container = device.OpenContainer(SaveFileContainer))
                {
                    // Add the container path to our filename
                    string filename = Path.Combine(container.Path, SaveFileLocation);

                    // Create a new file
                    using (FileStream saveGameFile = new FileStream(filename,
                        FileMode.Create))
                    {
                        using (BinaryWriter writer = new BinaryWriter(saveGameFile))
                        {
                            for (int i = 0; i < SavedGames.Length; i++)
                            {
                                if (SavedGames[i] != null)
                                {
                                    SavedGames[i].Serialize(writer);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                questionScreen = new ContinueQuestion(YesNoReason.LostDrive);
                BubbleGame.screenManager.AddScreen(questionScreen);
                return false;
            }
            return true;
        }


        public bool CheckDriveFailureExit()
        {
            if (questionScreen == null)
                return false;
            else
            {
                if (questionScreen.IsFinished == false)
                    return false;
                else
                {
                    if (questionScreen.Result == true)
                    {
                        DisableSaving();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

        }


        public SaveGameRecord GetSavedGameRecord(int index)
        {
            return SavedGames[index];
        }

        public void DeleteSaveGameRecord(int index)
        {
            SavedGames[index] = null;
        }

        public void AddSaveGameRecord(SaveGameRecord record)
        {
            SavedGames[record.SaveIndex] = record;
        }

        public void SetCurrentSaveIndex(int index)
        {
            CurrentOpenedGame = SavedGames[index];
        }

    }
}