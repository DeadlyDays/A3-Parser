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

        public new List<A3Level2Class> SubClasses
        {
            get
            {
                if (subClasses == null)
                    return new List<A3Level2Class>();
                return subClasses;
            }
            set
            {
                subClasses = value;
            }
        }

        public new void recursiveParseClasses()//parses all the children classes in originalcode and tells each child to do the same
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
                foreach (A3Level2Class x in subClasses)
                {
                    x.recursiveParseClasses();//sort all the children classes in each class, recursively
                }
        }

        public new void recursiveParseVariables()
        {
            filterContentNoClasses();
            if (ContentNoClasses != null)
                for (int i = 0; i < ContentNoClasses.Count; i++)
                {

                    String cursor = ContentNoClasses[i];
                    cursor = cursor.Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("\t", "");
                    if (cursor == "")
                        continue;
                    if (cursor.Contains("[]"))//array
                    {
                        A3Variable var = new A3Variable();
                        //Begin Array processes, doesn't end until we hit a ;
                        //multiple decs in a single line will need to be special later;
                        for (int e = i; e < ContentNoClasses.Count; e++)
                        {
                            if (cursor.Contains(";"))
                            {
                                if (var.OriginalCode.Count > 0)
                                    var.OriginalCode.Add(cursor);
                                else
                                    var.OriginalCode = new List<String> { cursor };
                                i = e;
                                break;
                            }
                            else
                            {
                                if (var.OriginalCode.Count > 0)
                                    var.OriginalCode.Add(cursor);
                                else
                                    var.OriginalCode = new List<String> { cursor };
                            }
                        }
                        if (Variables.Count > 0)
                            Variables.Add(var);
                        else
                            Variables = new List<A3Variable> { var };
                    }
                    else if (cursor.Contains("="))//variable
                    {
                        if (Variables.Count > 0)
                            Variables.Add(new A3Variable(cursor));
                        else
                            Variables = new List<A3Variable> { new A3Variable(cursor) };
                    }
                    if (Variables.Count > 0)
                        Variables[Variables.Count - 1].processCode();//process the last variable added
                }
            if (SubClasses.Count > 0)
                foreach (A3Level2Class x in SubClasses)
                {
                    x.recursiveParseVariables();
                }
            foreach (A3Variable x in Variables)
            {
                x.processCode();
            }
        }

        public new void buildNestedTree()
        {
            if (SubClasses.Count > 0)
            {
                foreach (A3Level2Class x in SubClasses)
                {
                    if (x.NestedTree.Count > 0)
                        x.NestedTree.Add(A3ClassName);
                    else
                        x.NestedTree = new List<String> { A3ClassName };
                }

                foreach (A3Level2Class x in SubClasses)
                {
                    x.buildNestedTree();
                }
            }
        }

        public new void buildExtendedTree(List<A3Class> list)
        {
            Boolean workLeft = true;
            int place = 0;
            if (ExtendedTree.Count > 0)//only if there is a base class
                while (workLeft)
                {
                    String cursor = "";
                    if (ExtendedTree.Count > place)
                        cursor = ExtendedTree[place];
                    else//break out of the loop if we are no longer looking at anything
                        break;

                    for (int i = 0; i < list.Count; i++)//Find the base class
                    {
                        if (list[i].A3ClassName == cursor)//if base class name matches current looked at class in list
                        {
                            if (list[i].ExtendedTree.Count > 0)//if the looked at class has a base class
                            {
                                ExtendedTree.Add(list[i].ExtendedTree[0]);//add the looked at classes base class to current class
                                place++;//look at the next parent
                                break;
                            }
                        }
                    }
                    place++;//look at the next parent
                }
            if (SubClasses.Count > 0)
                foreach (A3Level2Class x in SubClasses)
                {
                    x.buildExtendedTree(list);
                }
        }

        public new void buildInheritanceTree()
        {
            if (NestedTree.Count > 0 && ExtendedTree.Count > 0)
                InheritanceTree = ExtendedTree.Concat(NestedTree).ToList();
            else if (NestedTree.Count > 0)
                InheritanceTree = NestedTree;
            else if (ExtendedTree.Count > 0)
                InheritanceTree = ExtendedTree;
            else
                ;
            if (SubClasses.Count > 0)
                foreach (A3Level2Class x in SubClasses)
                {
                    x.buildInheritanceTree();
                }
        }

        public new List<A3Class> grabAllClasses()
        {
            List<A3Class> list = new List<A3Class>();

            if (SubClasses.Count > 0)
                foreach (A3Level2Class x in SubClasses)
                {
                    if (list.Count > 0)
                        list.Add(x);
                    else
                        list = new List<A3Class> { x };
                    list = list.Concat(x.grabAllClasses()).ToList();//combine the list with current classes list of subclasses
                }

            return list;
        }

        
    }
}
