using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JAMUnpacker
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("JAMUnpacker\nUnpacks JAM files used by the relatively unknown game, The Grim Adventures of Billy and Mandy.\n\nUsage: JAMUnpacker <jamfile.jam>");
                return;
            }
            try
            {
                String[] fileNames;
                String[] fileTypes;

                // Stupid temp bool needed to enable a nasty hack for one JAM file
                bool temp = false;

                FileStream savegameStream = new FileStream(args[0], FileMode.Open, FileAccess.ReadWrite);

                // Create our binary reader
                EndianReader reader = new EndianReader((Stream)savegameStream, Endian.LittleEndian);

                // ReadAscii is not technically correct here, because its expecting a null terminator when there isn't one, but it works so ¯\_(ツ)_/¯
                Console.WriteLine("File magic: " + reader.ReadAscii(4));

                // Don't know what this next one, doesn't seem important though
                reader.ReadInt32();

                int firstFileOffset = reader.ReadInt32();

                // this IS null-terminated so ReadAscii is right this time!
                Console.WriteLine("File comment: " + reader.ReadAscii(0x10));

                short fileCount = reader.ReadInt16();
                short fileTypeCount = reader.ReadInt16();

                fileNames = new String[fileCount];
                fileTypes = new String[fileTypeCount];

                for(int i = 0;i < fileCount;i++)
                {
                    fileNames[i] = reader.ReadAscii(8);
                    Console.WriteLine(fileNames[i]);
                }

                for (int i = 0; i < fileTypeCount; i++)
                {
                   fileTypes[i] = reader.ReadAscii(4);
                    Console.WriteLine(fileTypes[i]);
                }

                for(int i = 0; i < fileCount;i++)
                {
                    // Weird hack only needed for one JAM
                    if(fileCount == 128 & fileTypeCount == 3 & temp == false)
                    {
                        reader.ReadInt32();
                        temp = true;
                    }
                    string filename = fileNames[reader.ReadInt16()];
                    string filetype = fileTypes[reader.ReadInt16()];
                    reader.ReadInt32();

                    Console.WriteLine(filename + "." + filetype);
                }

                Console.WriteLine("done!");



            }

            // Bit of error handling for when the file is either not there or unaccessible
            catch (FileNotFoundException)
            {
                System.Console.WriteLine("The JAM you are trying to unpack can't be found.");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                System.Console.WriteLine("The JAM you are trying to unpack could not be unpacked because access was denied.");
            }
        }
    }
}
