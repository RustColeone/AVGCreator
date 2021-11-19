using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GalPrep
{
    class GalLine
    {
        //This class stores the text of text and whether if this line is a command
        private string[] commandIndicator = GalProperties.commandIndicator;
        public string lineText; //Text of this line
        public bool isCommand; // If this line is command

        //If this line is a command, the command must have these properties
        public string command;
        public int commandEmbeddedLevel;
        public string[] parameters;

        //Initialization of this class
        //Analyze the text and decide if this is a valid command
        public GalLine(string ToSetText)
        {
            ToSetText = AnalyzeLayer(ToSetText);
            lineText = ToSetText;
            if(ToSetText == "")
            {
                isCommand = true;
                return;
            }

            for (int i = 0; i < commandIndicator.Length; i++)
            {
                if (lineText.StartsWith(commandIndicator[i]))
                {
                    isCommand = true;
                    AnalyzeCommand(ToSetText);
                    return;
                }
            }
            isCommand = false;
        }

        private void AnalyzeCommand(string rawCommand)
        {
            //If this is a command, split it into array
            string[] commandAndProperties = rawCommand.Split();
            //set the main command
            command = commandAndProperties[0];
            //set the parameters of this command
            parameters = commandAndProperties.Skip(1).ToArray();
        }

        private string AnalyzeLayer(string text)
        {
            foreach (char c in text)
            {
                if (c == '\t')
                    commandEmbeddedLevel += 1;
                else
                    break;
            }
            text = text.Remove(0, commandEmbeddedLevel);
            return text;
        }
    }
}
