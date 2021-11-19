using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GalPrep
{
    class GalProcessor//
    {
        private int lineNumber = 0;//Records the current line but do not allow direct edit
        private List<GalLine> storyData = new List<GalLine>();//Each Line is processed in to a new type called GalLine
        private List<GalVariable> galVariableList = new List<GalVariable>();//Each Line is processed in to a new type called GalLine
        CommandProcessor commandProcessor;

        string speakerName = "";
        public int LineNumber//Converting lineNumber
        {
            //This is so the way user think about line number can be easilly
            //synchronized with the way the computer thinks without repeated
            //conversion, always access lineNumber from LineNumber.
            get { return lineNumber + 1; }
            set { lineNumber = value - 1; }
        }
        public void Initiate(string pathToFile)
        {
            commandProcessor = new CommandProcessor();

            //File reading is handled at GalProperties, just more customized
            string rawDataFromFile = GalProperties.FileReader(pathToFile);
            //Conversion to array
            string [] storyDataString = rawDataFromFile.Split(new string[] { "\n" }, StringSplitOptions.None);
            //Turn the array into an GalLine list, where each line was pre-processed on initialization
            for(int i = 0; i < storyDataString.Length; i++)
            {
                storyData.Add(new GalLine(storyDataString[i]));
            }
        }
        public bool ReachedEnd()
        {
            //Simple property to test when reach end in a more readable way
            //This also avoids repetedly typing the same thing again
            return (lineNumber >= storyData.Count); 
        }
        public void Next()
        {
            //When the end is not reached, line number will still be able to increase
            //This Test for end here actually does not work at all, since when on the
            //last line, this would still go over to the line that cannot be accessed

            //I guess you can say this is here as a reminder
            if (!ReachedEnd())
            {
                lineNumber += 1;
            }
        }
        public string CurrentLine()
        {
            //Getting the current line
            GalLine currentLine = storyData[lineNumber];
            //Do something is the current line is a command
            while (currentLine.isCommand)
            {
                //Execute the command
                PhraseCommand(currentLine.command, currentLine.parameters);
                Next();
                //When end reached, break
                if (ReachedEnd())
                {
                    return "";
                }
                //If there is indeed a next line, read it
                currentLine = storyData[lineNumber];
            }
            //Set the line to print
            return speakerName + "> " + storyData[lineNumber].lineText;
        }
        public void PhraseCommand(string command, string[] parameters)
        {
            if (command == "/setSpeakerName")
            {
                //speakerName = parameters[0];
                string name = "";
                foreach (string str in parameters)
                {
                    name += str + " ";
                }
                speakerName = commandProcessor.SetName(name);
            }
            if (command == "/skipToAfter")
            {
                LineNumber = Convert.ToInt32(parameters[0]);
            }
            if (command == "/set" || command == "/define")
            {
                bool isDefined = false;
                string varName = parameters[0];
                string varValue = parameters[1];
                for (int i = 0; i < galVariableList.Count; i++)
                {
                    if (galVariableList[i].variableName == varName)
                    {
                        galVariableList[i] = new GalVariable(varName, varValue);
                        isDefined = true;
                    }
                }
                if (!isDefined)
                {
                    galVariableList.Add(new GalVariable(varName, varValue));
                }
                //Define a new variable to replace the original
            }
            if (command == "/if")
            {
                string varName = parameters[0];
                string comparison = parameters[1];
                string compareValue = parameters[2];
                int compareValueInt = 0;

                bool result = false;

                GalLine currentLine = storyData[lineNumber];
                int originalCommandLevel = currentLine.commandEmbeddedLevel;

                try
                {
                    compareValueInt = Convert.ToInt32(compareValue);
                }
                catch
                {
                    if (!String.IsNullOrWhiteSpace(compareValue))
                    {
                        compareValueInt = 1;
                    }
                }

                GalVariable targetVariable = new GalVariable(null, null);

                for (
                    int i = 0; i < galVariableList.Count; i++)
                {
                    if (galVariableList[i].variableName == varName)
                    {
                        targetVariable = galVariableList[i];
                    }
                }
                if(targetVariable.variableName != null)
                {
                    if(comparison == ">")
                        if (targetVariable.intV > compareValueInt)
                            result = true;
                    if (comparison == ">=")
                        if (targetVariable.intV >= compareValueInt)
                            result = true;
                    if (comparison == "<")
                        if (targetVariable.intV < compareValueInt)
                            result = true;
                    if (comparison == "<=")
                        if (targetVariable.intV <= compareValueInt)
                            result = true;
                    if (comparison == "==")
                        if (targetVariable.strV == compareValue)
                            result = true;
                    if (comparison == "!=")
                        if (targetVariable.strV != compareValue)
                            result = true;
                }
                if(result == false)
                {
                    Next();
                    currentLine = storyData[lineNumber];
                    while (currentLine.commandEmbeddedLevel != originalCommandLevel)
                    {
                        Next();
                        //When end reached, break
                        if (ReachedEnd())
                            break;
                        //If there is indeed a next line, read it
                        currentLine = storyData[lineNumber];
                    }
                }
            }
        }
    }
}
