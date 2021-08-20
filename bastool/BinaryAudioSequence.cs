using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Be.IO;

namespace bastool
{
    class BinaryAudioSequence
    {
#if !DEBUG
        [JsonIgnore]
#endif
        public ushort EntryCount;
        public byte Category;
        public byte Unk1;
#if !DEBUG
        [JsonIgnore]
#endif
        public uint padding1;


        public BASSoundEntry[] entries;

        public void read(BeBinaryReader reader)
        {
            EntryCount = reader.ReadUInt16();
            Category = reader.ReadByte();
            Unk1 = reader.ReadByte();
            padding1 = reader.ReadUInt32();

            entries = new BASSoundEntry[EntryCount];
            for (int e = 0; e < EntryCount; e++)
                (entries[e] = new BASSoundEntry()).read(reader);
        }

        public void write(BeBinaryWriter writer)
        {
            writer.Write(EntryCount);
            writer.Write(Category);
            writer.Write(Unk1);
            writer.Write(padding1);
            for (int i = 0; i < entries.Length; i++)
                entries[i].write(writer);
        }

    }
    class BASSoundEntry
    {
        public uint SoundID;
        public float FrameDelay;
#if !DEBUG
        [JsonIgnore]
#endif
        public uint padding1;
        public float Pitch;
#if !DEBUG
        [JsonIgnore]
#endif
        public uint Mode;

        public sbyte Panning;
        public byte unk1;
        public byte unk2;
        public byte Velocity;

#if !DEBUG
        [JsonIgnore]
#endif
        public ulong padding3;

        public void read(BeBinaryReader reader)
        {
            SoundID = reader.ReadUInt32();
            FrameDelay = reader.ReadSingle();
            padding1 = reader.ReadUInt32();
            Pitch = reader.ReadSingle();
            Mode = reader.ReadUInt32();
            Panning = reader.ReadSByte();
            unk1 = reader.ReadByte();
            unk2 = reader.ReadByte();
            Velocity = reader.ReadByte();
            padding3 = reader.ReadUInt64();
        }

        public void write(BeBinaryWriter writer)
        {
            writer.Write(SoundID);
            writer.Write(FrameDelay);
            writer.Write(padding1);
            writer.Write(Pitch);
            writer.Write(Mode);
            writer.Write(Panning);
            writer.Write(unk1);
            writer.Write(unk2);
            writer.Write(Velocity);
            writer.Write(padding3);
        }
    }
}
