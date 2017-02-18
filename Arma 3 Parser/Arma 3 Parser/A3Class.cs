using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3Class
    {
        private String a3ClassName;
        private String fileLocation;
        private int lineLocation;

        private List<A3Class> subClasses;//all of the classes declared within context of this class
        private List<String> inheritanceTree;//list of all the parent elements(things extended + classes nested within)
        private List<String> nestedTree;//just the inheritance tree of classes nested within
        private List<String> extendedTree;//just the inheritance tree of the classes extended
        private List<String> originalCode;//this is the class dec + brackets and all between
        private List<String> content;//This is everything between the brackets
        private List<String> contentNoClasses;//This is everything between the brackets less the subclasses
        private List<A3Variable> variables;//All the variables and their values for this class

        public A3Class() { }

        public A3Class(String name)
        {
            a3ClassName = name;
            fileLocation = "";
        }

        public void filterContent()
        {
            if (OriginalCode.Count > 1)
            {
                content = new List<String>() { originalCode[2] };
                for (int i = 3; i < (originalCode.Count - 1); i++)
                {
                    content.Add(originalCode[i]);
                }
            }
            else
                ;
            
        }

        public void filterContentNoClasses()//strip subclasses and their content from content list
        {
            if(Content.Count > 1)
            {
                int depth = 0; int end = 0;
                for (int i = 0; i < Content.Count; i++)//iterate through content
                {
                    String cursor = Content[i];//Current value being acessed
                    if (cursor.Contains("class"))//if there is a class, find the end and skip there.
                    {
                        if (cursor.Contains(";"))
                            continue;
                        for(int e = i + 1; e < Content.Count; e++)//iterate through content starting at current location
                        {
                            String cursor2 = Content[e];
                            if (cursor2.Contains("{"))
                                depth += cursor2.Count(f => f == '{');
                            if (cursor2.Contains("}"))
                                depth -= cursor2.Count(f => f == '}');
                            if (depth == 0)
                            {
                                i = e;
                                break;
                            }
                        }
                    }

                    if (ContentNoClasses.Count > 0)
                        ContentNoClasses.Add(cursor);
                    else
                        ContentNoClasses = new List<String> { cursor };
                    
                }
            }
            
        }

        public void recursiveParseClasses()//parses all the children classes in originalcode and tells each child to do the same
        {
            filterContent();
            //current class object
            if (OriginalCode.Count <= 1)
                return;
            A3Class a3c = new A3Class(); //class gets populated and then added to a3ClassList, then this class gets cleared for new class
            int depth = 0; //this is the depth of the current line cursor, only class dec's with a depth of 0 are recorded as a new class
            if(Content != null)
            for (int i = 0; i < Content.Count; i++)//iterates through originalCodeLintList
            {
                String cursor = Content[i];//value of current index of List

                if ((depth == 0) & (cursor.Contains("class")) && (cursor.Contains(";")))//this is a class that has no contents;
                {
                    if (cursor.Contains("class"))//found a new class dec
                    {
                        String temp = GenLib.stripFormating(cursor);
                        int loc = temp.IndexOf("class");//find where in the line the class keyword starts
                        loc += 6;//find the location where the actuall classname starts
                        int end = GenLib.endOfWord(temp, loc);//the point the classname ends
                        int length = end - loc;//the length of the classname
                        a3c = new A3Class(temp.Substring(loc, length));//grab the className;
                        if (cursor.Contains(":"))//does this class extend a base class
                        {
                            loc = temp.IndexOf(":");
                            loc = GenLib.startOfNextWord(temp, loc);//find start of baseClass Name
                            end = GenLib.endOfWord(temp, loc);
                            length = end - loc;
                                if (a3c.ExtendedTree != null && a3c.ExtendedTree.Count > 0)
                                    a3c.ExtendedTree.Add(temp.Substring(loc, length));//add originalcode line
                                else
                                    a3c.ExtendedTree = new List<String> { temp.Substring(loc, length) };

                            }
                            if (a3c.Content != null && a3c.Content.Count > 0)
                                a3c.Content.Add(cursor);//add originalcode line
                            else
                                a3c.Content = new List<String> { cursor };
                        }
                    if (subClasses != null && Content.Count > 0)
                        subClasses.Add(a3c);//store class
                    else
                        subClasses = new List<A3Class> { a3c };
                    a3c = new A3Class();//clear class
                    continue;
                }

                //First Check, has depth increased
                if (cursor.Contains("{"))
                {
                        if (a3c.Content != null && a3c.Content.Count > 0)
                            a3c.Content.Add(cursor);//add originalcode line
                        else
                            a3c.Content = new List<String> { cursor };
                        //Need to handle if multiple opens and closes on a single line(god forbid)
                        if (((Regex.Split(cursor, "{")).Length - 1) > 1)
                    {
                        depth += (Regex.Split(cursor, "{").Length - 1);
                    }
                    else
                        depth++;
                }
                //check if depth has decreased (can happen on same line)
                if (cursor.Contains("}") && (depth > 1))
                {
                        if (a3c.Content != null && a3c.Content.Count > 0)
                            a3c.Content.Add(cursor);//add originalcode line
                        else
                            a3c.Content = new List<String> { cursor };
                        //Need to handle if multiple opens and closes on a single line(god forbid)
                        if (((Regex.Split(cursor, "}")).Length - 1) > 1)
                    {
                        depth -= (Regex.Split(cursor, "}").Length - 1);
                    }
                    else
                        depth--;
                }
                else if (cursor.Contains("}"))//if the depth is 1 and closing, that is the end of this class
                {
                        if (a3c.Content != null && a3c.Content.Count > 0)
                            a3c.Content.Add(cursor);//add originalcode line
                        else
                            a3c.Content = new List<String> { cursor };
                        depth--;//decrease depth
                    if (subClasses != null && Content.Count > 0)
                        subClasses.Add(a3c);//store class
                    else
                        subClasses = new List<A3Class> { a3c };
                    a3c = new A3Class();//clear class
                    continue;
                }

                //if depth is 0, check for new class dec
                if (depth == 0)
                {
                    if (cursor.Contains("class"))//found a new class dec
                    {

                        String temp = GenLib.stripFormating(cursor);
                        int loc = temp.IndexOf("class");//find where in the line the class keyword starts
                        loc += 6;//find the location where the actuall classname starts
                        int end = GenLib.endOfWord(temp, loc);//the point the classname ends
                        int length = end - loc;//the length of the classname
                        a3c = new A3Class(temp.Substring(loc, length));//grab the className;
                        if (cursor.Contains(":"))//does this class extend a base class
                        {
                            loc = temp.IndexOf(":");
                            loc = GenLib.startOfNextWord(temp, loc);//find start of baseClass Name
                            end = GenLib.endOfWord(temp, loc);
                            length = end - loc;
                                if (a3c.ExtendedTree != null && a3c.ExtendedTree.Count > 0)
                                    a3c.ExtendedTree.Add(temp.Substring(loc, length));//add originalcode line
                                else
                                    a3c.ExtendedTree = new List<String> { temp.Substring(loc, length) };

                            }
                            if (a3c.Content != null && a3c.Content.Count > 0)
                                a3c.Content.Add(cursor);//add originalcode line
                            else
                                a3c.Content = new List<String> { cursor };
                        }
                }
                else
                {
                        if (a3c.Content != null && a3c.Content.Count > 0)
                            a3c.Content.Add(cursor);//add originalcode line
                        else
                            a3c.Content = new List<String> { cursor };
                    }


            }
            if(subClasses != null)
            foreach (A3Class x in subClasses)
            {
                x.recursiveParseClasses();//sort all the children classes in each class, recursively
            }
        }

        public void recursiveParseVariables()
        {

        }



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
        public List<A3Class> SubClasses
        {
            get
            {
                if (subClasses == null)
                    return new List<A3Class>();
                return subClasses;
            }
            set
            {
                subClasses = value;
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
        public List<String> ContentNoClasses
        {
            get
            {
                if (contentNoClasses == null)
                    return new List<String>();
                return contentNoClasses;
            }
            set
            {
                contentNoClasses = value;
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
