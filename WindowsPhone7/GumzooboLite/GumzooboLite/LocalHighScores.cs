using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;

namespace BubbleGame
{
    public class HighScoreRecord
    {
        public String name = null;
        public UInt32 score = 0;
        public DateTime date;

        public bool ReadRecord(BinaryReader reader)
        {
            bool result = true;
            try
            {
                score = reader.ReadUInt32();
                name = reader.ReadString();
                Int32 year = reader.ReadInt32();
                Int32 month = reader.ReadInt32();
                Int32 day = reader.ReadInt32();
                date = new DateTime(year, month, day);
            }
            catch (Exception)
            {
                result = false;
                name = null;
                score = 0;
            }
            return result;
        }

        public void WriteRecord(BinaryWriter writer)
        {
            writer.Write(score);
            writer.Write(name);
            writer.Write((Int32)date.Year);
            writer.Write((Int32)date.Month);
            writer.Write((Int32)date.Day);

        }
    }



    public class LocalHighScores
    {
        const String fileName = "LocalHighScores";
        HighScoreRecord[] highScores = new HighScoreRecord[10];


        public LocalHighScores()
        {
        }


        public void Load()
        {
            // load defaults
            for (int i = 0; i < highScores.Length; i++)
            {
                highScores[i] = new HighScoreRecord();
            }

            // load from storage
            IsolatedStorageFileStream isoStream = null;
            BinaryReader reader = null;
            try
            {
                IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

                isoStream = new IsolatedStorageFileStream(fileName,
                    FileMode.OpenOrCreate,
                    FileAccess.Read,
                    isoFile);

                reader = new BinaryReader(isoStream);

                for (int i = 0; i < highScores.Length; i++)
                {
                    bool result = highScores[i].ReadRecord(reader);
                    if (result == false)
                        throw new Exception();
                }
            }
            catch (Exception)
            {
            }
            if (reader != null)
                reader.Close();
            if (isoStream != null)
                isoStream.Close();
        }

        public void Persist()
        {
            // write records to storage
            // load from storage
            IsolatedStorageFileStream isoStream = null;
            BinaryWriter writer = null;
            try
            {
                IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

                isoStream = new IsolatedStorageFileStream(fileName,
                    FileMode.OpenOrCreate,
                    FileAccess.Write,
                    isoFile);

                writer = new BinaryWriter(isoStream);

                for (int i = 0; i < highScores.Length; i++)
                {
                    highScores[i].WriteRecord(writer);
                }
            }
            catch (Exception)
            {
            }
            if (writer != null)
                writer.Close();
            if (isoStream != null)
                isoStream.Close();
        }


        public int AddHighScore(String name, UInt32 score)
        {
            int index;

            // find the location that this record belongs at
            for (index = 0; index < highScores.Length; index++)
            {
                if (score > highScores[index].score)
                {
                    break;
                }
            }

            // insert this record
            if (index < highScores.Length)
            {
                HighScoreRecord tempRecord;
                HighScoreRecord record = new HighScoreRecord();
                record.score = score;
                record.name = name;
                record.date = DateTime.Now;
                for (int i = index; i < highScores.Length; i++)
                {
                    tempRecord = highScores[i];
                    highScores[i] = record;
                    record = tempRecord;
                }
            }

            // persist the new table
            if (index < highScores.Length)
            {
                Persist();
            }

            return index;
        }


        public HighScoreRecord GetHighScore(int index)
        {
            return highScores[index];
        }

    }
}
