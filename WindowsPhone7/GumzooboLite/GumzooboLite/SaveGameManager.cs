using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace BubbleGame
{
    class SaveGameManager
    {
        SaveGameRecord[] SavedGames = new SaveGameRecord[6];
        string SaveFileLocation = "Gumzoobo.sav";
        bool savingDisabled = true;

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
            savingDisabled = false;
            SavedGames = new SaveGameRecord[6];
            questionScreen = null;
        }



        public bool ReadSaveFile()
        {
            // Open a storage container
            try
            {
                IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

                IsolatedStorageFileStream isoStream =
                    new IsolatedStorageFileStream(SaveFileLocation,
                    FileMode.OpenOrCreate,
                    FileAccess.Read,
                    isoFile);

                BinaryReader reader = new BinaryReader(isoStream);

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

                reader.Close();
                isoStream.Close();
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
                IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

                IsolatedStorageFileStream isoStream =
                    new IsolatedStorageFileStream(SaveFileLocation,
                    FileMode.Create,
                    FileAccess.Write,
                    isoFile);

                BinaryWriter writer = new BinaryWriter(isoStream);

                for (int i = 0; i < SavedGames.Length; i++)
                {
                    if (SavedGames[i] != null)
                    {
                        SavedGames[i].Serialize(writer);
                    }
                }

                writer.Close();
                isoStream.Close();
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