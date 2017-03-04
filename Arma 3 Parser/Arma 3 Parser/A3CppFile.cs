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
        //private List<String> content;//This is everything between the brackets
        private List<A3Class> a3ClassList;
        private List<A3Class> a3EntireClassList;

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
        /*public List<String> Content
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
        }*/
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
        public List<A3Class> A3EntireClassList
        {
            get
            {
                if (a3EntireClassList == null)
                    return new List<A3Class>();
                return a3EntireClassList;
            }
            set
            {
                a3EntireClassList = value;
            }
        }

        public A3CppFile(){}
        

        public void splitOCode()//splits string into list using '\n' delimiter
        {
            originalCodeLineList = Regex.Split(originalCodeString, "\n").ToList();
        }

        public void stripClasses()//iterates through originalCodeLineList, for every Top Level class creates a new A3Class, 
                                  //then calls an A3Class.Method to recursively strip top level classes within it
                                  //until all classes have been found
        {
            //current class object
            A3Class a3c = new A3Class(); //class gets populated and then added to a3ClassList, then this class gets cleared for
                                         //new class
            int depth = 0; //this is the depth of the current line cursor, only class dec's with a depth of 0 are recorded as 
                           //a new class
            
            Boolean capped = false;


            for (int i = 0; i < originalCodeLineList.Count; i++)//iterates through originalCodeList
            {
                capped = false;
                String cursor = originalCodeLineList[i];//value of current index of List

                //BUGFIX
                //If variable dec includes a bunch of code, handle and skip line
                if (cursor.Contains("=") && cursor.Contains(";"))
                {
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                    continue;
                }

                //Check if empty class declaration at base level
                if ((depth == 0) && (cursor.Contains("class")) && (cursor.Contains(";")))//this is a class that has no contents;
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
                        capped = true;
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

                //Check, has depth increased
                //Check, is this an open/close statement, then depth as not increased or decreased
                if (cursor.Contains("{}"))
                {
                    capped = true;
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                }
                //Check, has depth actually increased
                else if (cursor.Contains("{"))
                {
                    capped = true;
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
                //Check, is this an open/close than depth has not increased or descreased
                if (cursor.Contains("{}"))
                    ;
                else if (cursor.Contains("}") && (depth > 1))
                {
                    capped = true;
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
                //Check, has depth actually descreased
                else if(cursor.Contains("}"))//if the depth is 1 and closing, that is the end of this class
                {
                    capped = true;
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                    depth--;//decrease depth
                    if (a3c.A3ClassName != null && a3c.A3ClassName != "")
                    {
                        if (a3ClassList != null && a3c.OriginalCode.Count > 0)
                            a3ClassList.Add(a3c);//store class
                        else
                            a3ClassList = new List<A3Class> { a3c };
                        a3c = new A3Class();//clear class
                    }
                    continue;
                }

                //Check, check for new nonempty class dec at base level
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
                        capped = true;
                        if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                            a3c.OriginalCode.Add(cursor);//add originalcode line
                        else
                            a3c.OriginalCode = new List<String> { cursor };
                    }
                }/**/
                 //We need to capture lines of code that are not increased or 
                //decreased to the code and within the content of the current class being captured
                else if(!capped)
                {
                    if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                    else
                        a3c.OriginalCode = new List<String> { cursor };
                }


            }
            if(A3ClassList.Count > 0)
            foreach(A3Class x in A3ClassList)
            {
                x.recursiveParseClasses();//sort all the children classes in each class, recursively
            }
            fillEntireList();
        }

        public void stripVariables()//
        {
            if(A3ClassList.Count > 0)
            foreach(A3Class x in A3ClassList)
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
                x.buildExtendedTree(A3EntireClassList);
                x.buildInheritanceTree();
            }
            actualizeInheritance(A3EntireClassList);
        }

        public void actualizeInheritance(List<A3Class> list)//child classes will grab inherited fields from parents
        {
            if(A3ClassList.Count > 0)
                foreach(A3Class x in A3ClassList)//run for each top level class
                {
                    x.actualizeInheritance(list);
                }
        }

        public void fillEntireList()//need a list of ALL classes involved including subclasses
        {
            
            if(A3ClassList.Count > 0)
                foreach(A3Class x in A3ClassList)
                {
                    if (A3EntireClassList.Count > 0)
                    {
                        A3EntireClassList.Add(x);
                        A3EntireClassList = A3EntireClassList.Concat(x.grabAllClasses()).ToList();//Combine lists
                    }
                    else
                    {
                        A3EntireClassList = new List<A3Class> { x };
                        A3EntireClassList = A3EntireClassList.Concat(x.grabAllClasses()).ToList();//Combine lists
                    }
                }
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
