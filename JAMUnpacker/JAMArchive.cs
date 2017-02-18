using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAMUnpacker
{
    /// <summary>
    /// The structure of the JAM file used by the game.
    /// This isn't actually used by the code really - it is simply a resource.... for now
    /// Eventually, I will make this class be used by the unpacker for completeness sake.
    /// </summary>
    class JAMArchive
    {
        public int Magic; // JAM2 or FSTA, not sure what the difference is, but the struct is the same
        public int unk; // could be a checksum of sorts?
        public int unk2; // dunno lol
        public char[] type = new char[10]; // ive seen "none" and "safetest"
        public short fileCount; // the number of files inside the JAM
        public short fileTypes; // the number of filetypes inside the file + 1

        public Name[] filenames; // null terminated filenames, max 8 chars
        public Type[] filetypes; // null terminated filetypes/extensions, max 4 chars

        public FileTableEntry[] files; // the file table that stores filename, filetype, and offset to the file

        // We don't know the structs of the files inside the archive, and that doesn't matter, because this is simply an archive structure.

        public class Name
        {
            public char[] name = new char[8];
        }

        public class Type
        {
            public char[] type = new char[4];
        }

        public class FileTableEntry
        {
            public short fileName;
            public short fileType;
            public uint offset;
        }
    }
}
