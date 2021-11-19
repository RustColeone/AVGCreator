using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace GalPrep
{
    class GalProperties//This sets some properties of the game
    {
        //Character used to determine if this line is a command
        public static string[] commandIndicator = { "/" };

        //Reading the file is handled differently
        public static string FileReader(string pathToFile)//If file exists, read, else, read from embedded test file
        {
            return FileReadContainer.FileReader(pathToFile);
        }
    }
}
