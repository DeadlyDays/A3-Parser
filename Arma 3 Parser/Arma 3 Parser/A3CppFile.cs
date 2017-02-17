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
        private List<A3Class> a3ClassList;

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
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                        int loc = cursor.IndexOf("class");//find where in the line the class keyword starts
                        loc += 5;//find the location where the actual classname starts
                        int end = endOfWord(cursor, loc);//the point the classname ends
                        int length = end - loc;//the length of the classname
                        a3c.A3ClassName = cursor.Substring(loc, length);//grab the className;
                        if (cursor.Contains(":"))//does this class extend a base class
                        {
                            loc = cursor.IndexOf(":");
                            loc = startOfNextWord(cursor, loc);//find start of baseClass Name
                            end = endOfWord(cursor, loc);
                            length = end - loc;
                            a3c.ExtendedTree.Add(cursor.Substring(loc, length));//base class name;

                        }
                    }
                    A3ClassList.Add(a3c);//store class
                    a3c = new A3Class();//clear class
                    continue;
                }

                //First Check, has depth increased
                if(cursor.Contains("{"))
                {
                    a3c.OriginalCode.Add(cursor);
                    //Need to handle if multiple opens and closes on a single line(god forbid)
                    if (((Regex.Split(cursor, "{")).Length) > 1)
                    {
                        depth += Regex.Split(cursor, "{").Length;
                    }
                    else
                        depth++;
                }
                //check if depth has decreased (can happen on same line)
                if(cursor.Contains("}") && (depth > 1))
                {
                    a3c.OriginalCode.Add(cursor);
                    //Need to handle if multiple opens and closes on a single line(god forbid)
                    if (((Regex.Split(cursor, "}")).Length) > 1)
                    {
                        depth -= Regex.Split(cursor, "}").Length;
                    }
                    else
                        depth--;
                }
                else if(cursor.Contains("}"))//if the depth is 1 and closing, that is the end of this class
                {
                    a3c.OriginalCode.Add(cursor);
                    depth--;//decrease depth
                    A3ClassList.Add(a3c);//store class
                    a3c = new A3Class();//clear class
                    continue;
                }

                //if depth is 0, check for new class dec
                if(depth == 0)
                {
                    if(cursor.Contains("class"))//found a new class dec
                    {
                        a3c.OriginalCode.Add(cursor);//add originalcode line
                        int loc = cursor.IndexOf("class");//find where in the line the class keyword starts
                        loc += 5;//find the location where the actuall classname starts
                        int end = endOfWord(cursor, loc);//the point the classname ends
                        int length = end - loc;//the length of the classname
                        a3c.A3ClassName = cursor.Substring(loc, length);//grab the className;
                        if(cursor.Contains(":"))//does this class extend a base class
                        {
                            loc = cursor.IndexOf(":");
                            loc = startOfNextWord(cursor, loc);//find start of baseClass Name
                            end = endOfWord(cursor, loc);
                            length = end - loc;
                            a3c.ExtendedTree.Add(cursor.Substring(loc, length));//base class name;

                        }
                    }
                }
                else
                {
                    a3c.OriginalCode.Add(cursor);
                }


            }
        }

        public int endOfWord(String line, int startOfWord)
        {
            int end = line.Length;
            for(int i = startOfWord; i < line.Length; i++)
            {
                if(line[i].Equals(' ') || line[i].Equals(':') || line[i].Equals(';') || line[i].Equals('\\') || line[i].Equals('/') || line[i].Equals('{') || line[i].Equals('['))
                    return i;
            }
            return end;
        }
        public int startOfNextWord(String line, int startLoc)
        {
            int result = startLoc;
            for(int i = startLoc; i < line.Length; i++)
            {
                if (line[i].Equals(' ') || line[i].Equals(':') || line[i].Equals(';') || line[i].Equals('\\') || line[i].Equals('/') || line[i].Equals('{') || line[i].Equals('['))
                    ;
                else
                    return i;
            }

            return result;
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
    }
}
