using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3Class
    {
        private String a3ClassName;
        private String fileLocation;
        private int lineLocation;

        private List<A3Class> children;//all of the child classes
        private List<String> inheritanceTree;//list of all the parent elements(things extended + classes nested within)
        private List<String> nestedTree;//just the inheritance tree of classes nested within
        private List<String> extendedTree;//just the inheritance tree of the classes extended
        private List<String> originalCode;//this is the class dec + brackets and all between
        private List<String> content;//This is everything between the brackets
        private List<A3Variable> variables;//All the variables and their values for this class

        public A3Class() { }


        public String A3ClassName
        {
            get
            {
                if (a3ClassName == null)
                    return "";
                return a3ClassName;
            }
            set
            {
                a3ClassName = value;
            }
        }
        public String FileLocation
        {
            get
            {
                if (fileLocation == null)
                    return "";
                return fileLocation;
            }
            set
            {
                fileLocation = value;
            }
        }
        public int LineLocation
        {
            get
            {
                return lineLocation;
            }
            set
            {
                lineLocation = value;
            }
        }
        public List<A3Class> Children
        {
            get
            {
                if (children == null)
                    return new List<A3Class>();
                return children;
            }
            set
            {
                children = value;
            }
        }
        public List<String> InheritanceTree
        {
            get
            {
                if (inheritanceTree == null)
                    return new List<String>();
                return inheritanceTree;
            }
            set
            {
                inheritanceTree = value;
            }
        }
        public List<String> NestedTree
        {
            get
            {
                if (nestedTree == null)
                    return new List<String>();
                return nestedTree;
            }
            set
            {
                nestedTree = value;
            }
        }
        public List<String> ExtendedTree
        {
            get
            {
                if (extendedTree == null)
                    return new List<String>();
                return extendedTree;
            }
            set
            {
                extendedTree = value;
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
        public List<String> Content
        {
            get
            {
                if (content == null)
                    return new List<String>();
                return content;
            }
            set
            {
                content = value;
            }
        }
        public List<A3Variable> Variables
        {
            get
            {
                if (variables == null)
                    return new List<A3Variable>();
                return variables;
            }
            set
            {
                variables = value;
            }
        }
    }
}
