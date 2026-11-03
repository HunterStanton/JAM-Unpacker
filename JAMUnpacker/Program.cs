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
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("JAMUnpacker\nUnpacks JAM files used by the relatively unknown game, The Grim Adventures of Billy and Mandy.\n\nUsage: JAMUnpacker <jamfile.jam> [output directory]");
                return;
            }
            try
            {
                string outputDirectory = args.Length == 2 ? args[1] : Path.Combine(Path.GetDirectoryName(Path.GetFullPath(args[0])), Path.GetFileNameWithoutExtension(args[0]) + "_unpacked");

                using (FileStream jamStream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
                {
                    EndianReader reader = new EndianReader(jamStream, Endian.LittleEndian);
                    JAMArchive jam = JAMArchive.Read(reader);

                    Console.WriteLine("File magic: " + jam.Magic);
                    Console.WriteLine("File comment: " + jam.Comment);

                    foreach (JAMArchive.FileEntry file in jam.Files)
                    {
                        string outputPath = Path.Combine(outputDirectory, file.Path);
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                        reader.SeekTo(file.Offset);
                        File.WriteAllBytes(outputPath, reader.ReadBlock((int)file.Size));

                        Console.WriteLine(file.Path);
                    }

                    Console.WriteLine("Unpacked " + jam.Files.Count + " files to " + outputDirectory);
                }
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
            catch (InvalidDataException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
