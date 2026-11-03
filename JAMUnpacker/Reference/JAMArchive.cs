using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAMUnpacker
{
    /// <summary>
    /// The structure of the JAM files used by the game. little-endian on every platform (GameCube, Wii and PS2)
    ///
    /// 0x00  char[4]   magic "FSTA" or "JAM2"
    /// 0x04  uint      build timestamp (unix time)
    /// 0x08  uint      end of the dir tree
    /// 0x0C  char[16]  comment, ive seen both "none" and "safetest"
    /// 0x1C  ushort    name count
    /// 0x1E  ushort    extension count
    /// 0x20  char[8]   names, shared by files AND directories, so this is not the file count
    ///       char[4]   extensions, first one is always empty for files without an extension
    ///       ushort    root subdir count
    ///       ushort    root file count
    ///       ...       the root directory's entries
    ///
    /// A directory's entries are all of its subdirectory entries followed by all of its file entries
    ///
    /// Subdir entry (8 bytes):
    ///       uint      bits 0-11 name, bits 12-20 subdirectory count, bits 21-31 file count (seems right?)
    ///       uint      offset of the subdirectory's own entries
    ///
    /// FSTA file entry (12 bytes):
    ///       ushort    name
    ///       ushort    extension
    ///       uint      offset aligned to 0x800
    ///       uint      size
    ///
    /// JAM2 file entry (8 bytes):
    ///       ushort    name
    ///       ushort    extension
    ///       uint      offset of a 0x20 byte header in front of the data. The header starts off with the size twice, the rest is random junk data
    ///                 Identical files are only stored once, so multiple entries can point to the same offset as a space saving measure
    /// </summary>
    class JAMArchive
    {
        const int JAM2FileHeaderSize = 0x20;

        public string Magic;
        public uint Timestamp;
        public string Comment;
        public List<FileEntry> Files = new List<FileEntry>();

        private string[] names;
        private string[] extensions;

        public class FileEntry
        {
            public string Path; // relative path inside the archive, for example DESIGN\BEHAVIOR\ACHBASE.ANL
            public uint Offset; // where the file's data starts
            public uint Size;
        }

        private class DirectoryEntry
        {
            public string Name;
            public int SubdirectoryCount;
            public int FileCount;
            public uint Offset;
        }

        public static JAMArchive Read(EndianReader reader)
        {
            JAMArchive jam = new JAMArchive();

            jam.Magic = reader.ReadAscii(4);
            if (jam.Magic != "FSTA" && jam.Magic != "JAM2")
                throw new InvalidDataException("Not a JAM file, unknown magic \"" + jam.Magic + "\".");

            jam.Timestamp = reader.ReadUInt32();
            reader.ReadUInt32();
            jam.Comment = reader.ReadAscii(0x10);

            jam.names = new string[reader.ReadUInt16()];
            jam.extensions = new string[reader.ReadUInt16()];

            for (int i = 0; i < jam.names.Length; i++)
                jam.names[i] = reader.ReadAscii(8);

            for (int i = 0; i < jam.extensions.Length; i++)
                jam.extensions[i] = reader.ReadAscii(4);

            int rootSubdirectoryCount = reader.ReadUInt16();
            int rootFileCount = reader.ReadUInt16();
            jam.ReadDirectory(reader, "", rootSubdirectoryCount, rootFileCount);

            if (jam.Magic == "JAM2")
            {
                foreach (FileEntry file in jam.Files)
                {
                    reader.SeekTo(file.Offset);
                    file.Size = reader.ReadUInt32();
                    file.Offset += JAM2FileHeaderSize;
                }
            }

            return jam;
        }

        private void ReadDirectory(EndianReader reader, string path, int subdirectoryCount, int fileCount)
        {
            DirectoryEntry[] subdirectories = new DirectoryEntry[subdirectoryCount];

            for (int i = 0; i < subdirectoryCount; i++)
            {
                uint packed = reader.ReadUInt32();
                subdirectories[i] = new DirectoryEntry
                {
                    Name = names[packed & 0xFFF],
                    SubdirectoryCount = (int)((packed >> 12) & 0x1FF),
                    FileCount = (int)(packed >> 21),
                    Offset = reader.ReadUInt32()
                };
            }

            for (int i = 0; i < fileCount; i++)
            {
                string name = names[reader.ReadUInt16()];
                string extension = extensions[reader.ReadUInt16()];
                if (extension != "")
                    name += "." + extension;

                FileEntry file = new FileEntry();
                file.Path = Path.Combine(path, name);
                file.Offset = reader.ReadUInt32();
                if (Magic == "FSTA")
                    file.Size = reader.ReadUInt32();
                Files.Add(file);
            }

            foreach (DirectoryEntry subdirectory in subdirectories)
            {
                reader.SeekTo(subdirectory.Offset);
                ReadDirectory(reader, Path.Combine(path, subdirectory.Name), subdirectory.SubdirectoryCount, subdirectory.FileCount);
            }
        }
    }
}
