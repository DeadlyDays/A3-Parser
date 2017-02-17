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
        A3Variable() { }

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
