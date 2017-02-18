using System;
using System.Collections.Generic;
using System.Linq;
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
            }
        }
    }
}
