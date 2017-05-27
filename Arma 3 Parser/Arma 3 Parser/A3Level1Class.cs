﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3Level1Class : A3Class
    {

        protected new List<A3Level2Class> subClasses;//all of the classes declared within context of this class

        public A3Level1Class() { }

        public A3Level1Class(String name)
        {
            a3ClassName = name;
            fileLocation = "";
        }

        public void recursiveParseClasses()//parses all the children classes in originalcode and tells each child to do the same
        {//strips only the top level and records contents
            filterContent();
            //current class object
            if (OriginalCode.Count <= 1)
                return;
            A3Level2Class a3c = new A3Level2Class(); //class gets populated and then added to a3ClassList, then this class gets cleared for new class
            int depth = 0; //this is the depth of the current line cursor, only class dec's with a depth of 0 are recorded as a new class
            Boolean capped = false;//has cursor been captured
            if (Content != null)
                for (int i = 0; i < Content.Count; i++)//iterates through originalCodeList
                {
                    String cursor = Content[i];//value of current index of List
                    capped = false;

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

                    //Is this an Empty Class
                    if ((depth == 0) && (cursor.Contains("class")) && (cursor.Contains(";")))//this is a class that has no contents;
                    {
                        if (cursor.Contains("class"))//found a new class dec
                        {
                            String temp = GenLib.stripFormating(cursor);
                            int loc = temp.IndexOf("class");//find where in the line the class keyword starts
                            loc += 6;//find the location where the actual classname starts
                            int end = GenLib.endOfWord(temp, loc);//the point the classname ends
                            int length = end - loc;//the length of the classname
                            a3c = new A3Level2Class(temp.Substring(loc, length));//grab the className;
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
                        if (a3c.A3ClassName != null && a3c.A3ClassName != "")
                        {
                            if (subClasses != null && Content.Count > 0)
                                subClasses.Add(a3c);//store class
                            else
                                subClasses = new List<A3Level2Class> { a3c };
                        }
                        a3c = new A3Level2Class();//clear class
                        continue;
                    }

                    //Has depth increased
                    if (cursor.Contains("{}"))
                    {
                        capped = true;
                        if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                            a3c.OriginalCode.Add(cursor);//add originalcode line
                        else
                            a3c.OriginalCode = new List<String> { cursor };
                    }
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
                            depth += (Regex.Split(cursor, "{").Length - 1);
                        }
                        else
                            depth++;
                    }
                    //check if depth has decreased (can happen on same line)
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
                    else if (cursor.Contains("}"))//if the depth is 1 and closing, that is the end of this class
                    {
                        capped = true;
                        if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                            a3c.OriginalCode.Add(cursor);//add originalcode line
                        else
                            a3c.OriginalCode = new List<String> { cursor };
                        depth--;//decrease depth
                        if (a3c.A3ClassName != null && a3c.A3ClassName != "")
                        {
                            if (subClasses != null && Content.Count > 0)
                                subClasses.Add(a3c);//store class
                            else
                                subClasses = new List<A3Level2Class> { a3c };
                            a3c = new A3Level2Class();//clear class
                        }
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
                            a3c = new A3Level2Class(temp.Substring(loc, length));//grab the className;
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
                    }
                    else if (!capped)
                    {
                        if (a3c.OriginalCode != null && a3c.OriginalCode.Count > 0)
                            a3c.OriginalCode.Add(cursor);//add originalcode line
                        else
                            a3c.OriginalCode = new List<String> { cursor };
                    }


                }
            if (subClasses != null)
                foreach (A3Class x in subClasses)
                {
                    x.recursiveParseClasses();//sort all the children classes in each class, recursively
                }
        }

    }
}
