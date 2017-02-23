using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3CppFile
    {
        private String filePath;
        private String originalCodeString;

        private List<String> originalCodeLineList;
        private List<String> content;//This is everything between the brackets
        private List<A3Class> a3ClassList;

        public String OriginalCodeString
        {
            get
            {
                if (originalCodeString == null)
                    return "";
                return originalCodeString;
            }
            set
            {
                originalCodeString = value;
            }
        }

        public List<String> OriginalCodeLineList
        {
            get
            {
                if (originalCodeLineList == null)
                    return new List<String>();
                return originalCodeLineList;
            }
            set
            {
                originalCodeLineList = value;
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
        public List<A3Class> A3ClassList
        {
            get
            {
                if (a3ClassList == null)
                    return new List<A3Class>();
                return a3ClassList;
            }
            set
            {
                a3ClassList = value;
            }
        }

        public A3CppFile(){}
        

        public void splitOCode()//splits string into list using '\n' delimiter
        {
            originalCodeLineList = Regex.Split(originalCodeString, "\n").ToList();
        }

        public void stripClasses()//iterates through originalCodeLineList, for every Top Level class creates a new A3Class, 
                                  //then calls an A3Class.Method to recursively strip top level classes until all classes have been found
        {
            //current class object
            A3Class a3c = new A3Class(); //class gets populated and then added to a3ClassList, then this class gets cleared for new class
            int depth = 0; //this is the depth of the current line cursor, only class dec's with a depth of 0 are recorded as a new class
            for (int i = 0; i < originalCodeLineList.Count; i++)//iterates through originalCodeLintList
            {
                String cursor = originalCodeLineList[i];//value of current index of List
                
                if((depth == 0) & (cursor.Contains("class")) && (cursor.Contains(";")))//this is a class that has no contents;
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
                        if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                            a3c.OriginalCode.Add(cursor);//add originalcode line
                        else
                            a3c.OriginalCode = new List<String> { cursor };
                    }
                    if (a3ClassList != null && a3c.OriginalCode.Count > 0)
                        a3ClassList.Add(a3c);//store class
                    else
                        a3ClassList = new List<A3Class> { a3c };
                    a3c = new A3Class();//clear class
                    continue;
                }

                //First Check, has depth increased
                if (cursor.Contains("{}"))
                {
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                }
                else if(cursor.Contains("{"))
                {
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                    //Need to handle if multiple opens and closes on a single line(god forbid)
                    if (((Regex.Split(cursor, "{")).Length - 1) > 1)
                    {
                        depth += (Regex.Split(cursor, "{").Length);
                    }
                    else
                        depth++;
                }
                //check if depth has decreased (can happen on same line)
                if (cursor.Contains("{}"))
                    ;
                else if (cursor.Contains("}") && (depth > 1))
                {
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                    //Need to handle if multiple opens and closes on a single line(god forbid)
                    if (((Regex.Split(cursor, "}")).Length - 1) > 1)
                    {
                        depth -= (Regex.Split(cursor, "}").Length - 1);
                    }
                    else
                        depth--;
                }
                else if(cursor.Contains("}"))//if the depth is 1 and closing, that is the end of this class
                {
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                    depth--;//decrease depth
                    if (a3ClassList != null && a3c.OriginalCode.Count > 0)
                        a3ClassList.Add(a3c);//store class
                    else
                        a3ClassList = new List<A3Class> { a3c };
                    a3c = new A3Class();//clear class
                    continue;
                }

                //if depth is 0, check for new class dec
                if(depth == 0)
                {
                    if(cursor.Contains("class"))//found a new class dec
                    {
                        
                        String temp = GenLib.stripFormating(cursor);
                        int loc = temp.IndexOf("class");//find where in the line the class keyword starts
                        loc += 6;//find the location where the actuall classname starts
                        int end = GenLib.endOfWord(temp, loc);//the point the classname ends
                        int length = end - loc;//the length of the classname
                        a3c = new A3Class(temp.Substring(loc, length));//grab the className;
                        if(cursor.Contains(":"))//does this class extend a base class
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
                        if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                            a3c.OriginalCode.Add(cursor);//add originalcode line
                        else
                            a3c.OriginalCode = new List<String> { cursor };
                    }
                }
                else if(a3c.OriginalCode.Count < (i + 1))
                {
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                }


            }
            if(a3ClassList != null)
            foreach(A3Class x in a3ClassList)
            {
                x.recursiveParseClasses();//sort all the children classes in each class, recursively
            }
        }

        public void stripVariables()//
        {
            if(a3ClassList.Count > 0)
            foreach(A3Class x in a3ClassList)
            {
                    x.recursiveParseVariables();
            }
        }

        public void buildTrees()//populate extended and nested tree's
        {
            if(A3ClassList.Count > 0)
            foreach(A3Class x in A3ClassList)
            {
                x.buildNestedTree();
                x.buildExtendedTree(A3ClassList);
                x.buildInheritanceTree();
            }
        }

        public void actualizeInheritance()//child classes will grab inherited fields from parents
        {

        }

        public String FilePath
        {
            get
            {
                if (filePath == null)
                    return "";
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }
        
        
    }
}
