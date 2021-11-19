using System;
using System.Collections.Generic;
using System.Text;

namespace GalPrep
{
    class GalVariable
    {
        public string variableName;
        public string strV;
        public int intV;
        private string variableType;

        public string type
        {
            get { return variableType; }
        }

        public GalVariable(string vName, string vValue)
        {
            variableName = vName;
            strV = vValue;
            variableType = "string";
            try
            {
                intV = Convert.ToInt32(vValue);
                variableType = "int";
            }
            catch
            {
                variableType = "string";
            }
            return;
        }
    }
}
