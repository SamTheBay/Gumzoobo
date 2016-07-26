using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BubbleGame
{
    class SaveGameRecord
    {
        int reachedLocation;
        int saveIndex;
        string recordName;
        bool isValid;

        public SaveGameRecord()
        {
            isValid = false;
        }

        public SaveGameRecord(string recordName, int saveIndex)
        {
            reachedLocation = 0;
            this.saveIndex = saveIndex;
            this.recordName = recordName;
            isValid = true;
        }

        public bool Serialize(BinaryWriter writer)
        {
            try
            {
                writer.Write(recordName);
                writer.Write((Int32)reachedLocation);
                writer.Write((Int32)saveIndex);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public void Deserialize(BinaryReader reader)
        {
            try
            {
                recordName = reader.ReadString();
                reachedLocation = reader.ReadInt32();
                saveIndex = reader.ReadInt32();
            }
            catch
            {
                return;
            }
            isValid = true;
        }


        public string RecordName
        {
            get { return recordName; }
        }

        public int ReachedLocation
        {
            get { return reachedLocation; }
            set { reachedLocation = value; }
        }

        public int SaveIndex
        {
            get { return saveIndex; }
        }

        public bool IsValid
        {
            get { return isValid; }
        }
    }
}