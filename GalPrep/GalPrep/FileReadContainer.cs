using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace GalPrep
{
    class FileReadContainer
    {
        public static string FileReader(string pathToFile)//If file exists, read, else, read from embedded test file
        {
            //Getting the properties of this project
            var assembly = Assembly.GetExecutingAssembly();
            //string[] name = assembly.GetManifestResourceNames();
            //Location to the embedded text file which is located in the resource
            var resourceName = "GalPrep.Resources.GalTest.txt";

            //temp storage of the result
            string result = "";

            if (File.Exists(pathToFile))//If exists
            {
                result = File.ReadAllText(pathToFile);//Read from file
            }
            else//If file not found
            {
                //Read from embedded resource text
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            //Result will be either one of them, so return
            return result;
        }
    }
}
