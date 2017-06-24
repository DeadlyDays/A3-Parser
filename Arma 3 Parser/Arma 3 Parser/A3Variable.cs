using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3Variable
    {
        private String fieldName;
        private String varClassName;//the name of the class this belongs to
        private int linePosition;//the line location of this variable declaration

        private List<String> value;//All the contents of the A3 Variable
        private List<String> originalCode;//All the code that declared the field and its value
        public A3Variable() { }

        public A3Variable(String codeLine)
        {
            fieldName = "";
            varClassName = "";
            linePosition = 0;
            originalCode = new List<String> { codeLine };
            
        }

        public void processCode()//sort out the name and values from originalCode
        {
            String cursor = OriginalCode[0].Replace("[]", "").Replace(";", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("{}", "");
            //Seperate the name from the value, left side of = is name, right side(and below) are values
            List<String> value = cursor.Split('=').ToList();
            //Name is first value, because left of =
            fieldName = value[0];
            
            if (Value.Count > 1)
                //check to make sure in case we don't have an = (variable not initialised, but mentioned)
                this.Value.Add(value[1].Replace("[]", "").Replace(";", "").Replace(",", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("}", "").Replace("{", ""));
            else
                //Normal, more than 1 variable so more than just name
                this.Value = new List<String> { value[1].Replace("[]", "").Replace(";", "").Replace(",", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("}", "").Replace("{", "") };

            for (int i = 1; i < originalCode.Count; i++)
                //Iterate through all the lines after 1st line(first is name, second is first value which are gathered above)
                // Now we need to grab other values;
            {
                this.Value.Add(originalCode[i].Replace("[]", "").Replace(";", "").Replace(",", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("}", "").Replace("{", ""));
            }

        }

        public String FieldName
        {
            get
            {
                if (fieldName == null)
                    return "";
                return fieldName;
            }
            set
            {
                fieldName = value;
            }
        }
        public String VarClassName
        {
            get
            {
                if (varClassName == null)
                    return "";
                return varClassName;
            }
            set
            {
                varClassName = value;
            }
        }
        public int LinePosition
        {
            get
            {
                return linePosition;
            }
            set
            {
                linePosition = value;
            }
        }
        public List<String> Value
        {
            get
            {
                if (value == null)
                    return new List<String>();
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
        public List<String> OriginalCode
        {
            get
            {
                if (originalCode == null)
                    return new List<String>();
                return originalCode;
            }
            set
            {
                originalCode = value;
            }
        }
    }
}
