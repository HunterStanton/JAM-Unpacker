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
                FileStream savegameStream = new FileStream(args[0], FileMode.Open, FileAccess.ReadWrite);

                // Create our binary reader
                BinaryReader reader = new BinaryReader((Stream)savegameStream);
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
