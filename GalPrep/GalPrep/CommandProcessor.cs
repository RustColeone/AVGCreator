using System;
using System.Collections.Generic;
using System.Text;

namespace GalPrep
{
    class CommandProcessor
    {
        public string speakerName;
        public string SetName(string name)
        {
            speakerName = name;
            return speakerName;
        }
        public void SetBackground(string backgroundSpriteName)
        {
            return;
        }
        public void SetStyle(string targetName, string colorToSet, int fontSize)
        {
            return;
        }
    }
}
