using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    static class A3Presentation
    {
        //Contains filtering methods

        //Sort out any Classes where the name Contains part of input string
        public static List<A3Class> filterOutClassByPartialName(List<A3Class> list, String[] input)
        {
            if (list.Count > 0)
                for (int i = (list.Count - 1); i >= 0; i--)//iterate through list
                {
                    for (int e = 0; e < input.Length; e++)//iterate through input
                    {
                        if (list[i].A3ClassName.Contains(input[e]))//if the classname matches any of the inputs
                        {
                            list.Remove(list[i]);//remove item from list
                        }
                    }
                }
            return list;
        }

        //Sort out any Classes where the name does not Contain part of the input string
        public static List<A3Class> filterInClassByPartialName(List<A3Class> list, String[] input)
        {
            List<A3Class> output = new List<A3Class>();
            if (list.Count > 0)
                for (int i = (list.Count - 1); i >= 0; i--)//iterate through list
                {
                    for (int e = 0; e < input.Length; e++)//iterate through input
                    {
                        if (list[i].A3ClassName.Contains(input[e]))//if the classname matches any of the inputs
                        {
                            if (output != null)
                                output.Add(list[i]);
                            else
                                output = new List<A3Class> { list[i] };
                        }
                    }
                }
            output.Reverse();
            return output;
        }

        //Sort out any Classes that do not have as their direct Parent,
        //  a Class with the name matching input string
        public static List<A3Class> filterInClassByDirectParent(List<A3Class> list, String[] input)
        {
            List<A3Class> output = new List<A3Class>();
            if (list.Count > 0)
                for (int i = (list.Count - 1); i >= 0; i--)//iterate through list
                {
                    for (int e = 0; e < input.Length; e++)//iterate through input
                    {
                        if (list[i].InheritanceTree != null)
                            if (list[i].InheritanceTree[0] == input[e])//if the direct parent matches any of the inputs
                            {
                                if (output != null)
                                    output.Add(list[i]);
                                else
                                    output = new List<A3Class> { list[i] };
                                break;
                            }
                    }
                }
            output.Reverse();
            return output;
        }

        //Sort out any Classes that do not have a Parent with a name that matches input string
        public static List<A3Class> filterInClassByAnyParent(List<A3Class> list, String[] input)
        {
            List<A3Class> output = new List<A3Class>();
            if (list.Count > 0)
                for (int i = (list.Count - 1); i >= 0; i--)//iterate through list
                {
                    for (int e = 0; e < input.Length; e++)//iterate through input
                    {
                        if (list[i].InheritanceTree != null)
                            for (int z = 0; z < list[i].InheritanceTree.Count; z++)//iterate through inheritance tree
                            {
                                if (list[i].InheritanceTree[z] == input[e])//if any parent matches any of the inputs
                                {
                                    if (output != null)
                                        output.Add(list[i]);
                                    else
                                        output = new List<A3Class> { list[i] };
                                    goto leave;//break out of loop, we already know to include this item
                                }
                            }
                    }
                leave:;
                }
            output.Reverse();
            return output;
        }

        //Sort out any Classes that do not have a Variable name that matches input string.
        public static List<A3Class> filterOutClassByVariableName(List<A3Class> list, String[] input)
        {
            List<A3Class> output = new List<A3Class>();
            if (list.Count > 0)
                for (int i = (list.Count - 1); i >= 0; i--)//iterate through list
                {
                    for (int e = 0; e < input.Length; e++)//iterate through input
                    {
                        if (list[i].Variables != null)
                            for (int z = 0; z < list[i].Variables.Count; z++)//iterate through Variables
                            {
                                if (list[i].Variables[z].FieldName == input[e])//if field name matches any of the inputs
                                {
                                    if (output != null)
                                        output.Add(list[i]);
                                    else
                                        output = new List<A3Class> { list[i] };
                                    goto leave;//break out of loop, we already know to include this item
                                }
                            }
                    }
                leave:;
                }
            output.Reverse();
            return output;
        }

        //Display only these Fields
        public static List<String> outputSelectedFields(List<A3Class> list, String[] input,
            Boolean outputClassName, Boolean outputParentClass, Boolean outputBaseClass,
            Boolean outputSource)
        {
            //Create a list of only classes with selected variables
            List<A3Class> select = new List<A3Class>();
            if (list.Count > 0)
                for (int i = (list.Count - 1); i >= 0; i--)//iterate through list
                {
                    for (int e = 0; e < input.Length; e++)//iterate through input
                    {
                        if (list[i].Variables != null)
                            for (int z = 0; z < list[i].Variables.Count; z++)//iterate through Variables
                            {
                                if (list[i].Variables[z].FieldName == input[e])//if field name matches any of the inputs
                                {
                                    if (select != null)
                                        select.Add(list[i]);
                                    else
                                        select = new List<A3Class> { list[i] };
                                    goto leave;//break out of loop, we already know to include this item
                                }
                            }
                    }
                leave:;
                }
            select.Reverse();
            List<String> output = new List<String>();
            String cursor = "";
            if (outputClassName || outputParentClass || outputBaseClass)
            {
                if (outputClassName)
                {
                    if (cursor != "")
                        cursor += "," + "ClassName";
                    else
                        cursor += "ClassName";
                }
                if (outputParentClass)
                {
                    if (cursor != "")
                        cursor += "," + "ParentClass";
                    else
                        cursor += "ParentClass";
                }
                if (outputBaseClass)
                {
                    if (cursor != "")
                        cursor += "," + "BaseClass";
                    else
                        cursor += "BaseClass";
                }
            }
            foreach(String s in input)
            {
                if (cursor != "")
                    cursor += "," + input;
                else
                    cursor += input;
            }
            
            //Create a list of strings with inputs as output
            if(select != null)
                foreach (A3Class c in select)//iterate through class
                {
                    if (outputClassName)
                    {
                        if (cursor != "")
                            cursor += "," + c.A3ClassName;
                        else
                            cursor += c.A3ClassName;
                    }
                    if (outputParentClass)
                    {
                        if (c.NestedTree != null)
                        {
                            if (cursor != "")
                                cursor += "," + c.NestedTree[0];
                            else
                                cursor += c.NestedTree[0];
                        }
                        else
                            cursor += ",";
                    }
                    if (outputBaseClass)
                    {
                        if (c.ExtendedTree != null)
                        {
                            if (cursor != "")
                                cursor += "," + c.ExtendedTree[0];
                            else
                                cursor += c.ExtendedTree[0];
                        }
                        else
                            cursor += ",";
                    }
                    cursor = "";
                    if(c.Variables != null)
                        foreach (A3Variable v in c.Variables)//iterate through classes variables
                        {
                            if(input != null)   
                                foreach (String s in input)//iterate through input values
                                {
                                    if (v.FieldName.Equals(input))//if fieldname matches input, add
                                    {
                                        if (cursor != "")
                                            cursor += "," + v.Value[0];
                                        else
                                            cursor += v.Value[0];
                                    }
                                    else
                                        cursor += ",";               
                                }   
                        }
                    if (outputSource)
                    {
                        cursor += " \nClass Source:,";
                        foreach (String s in c.OriginalCode)
                        {
                            cursor += s;
                        }
                    }
                    if (output != null)
                        output.Add(cursor + " \n");
                    else
                        output = new List<String> { cursor + " \n"};
                }
            return output;
        }

        //Display All fields
        public static List<String> outputAllFields(List<A3Class> list, Boolean outputClassName, 
            Boolean outputParentClass, Boolean outputBaseClass, Boolean outputSource)
        {
            List<String> output = new List<String>();
            String cursor = "";
            if (outputClassName || outputParentClass || outputBaseClass)
            {
                if (outputClassName)
                {
                    if (cursor != "")
                        cursor += "," + "ClassName";
                    else
                        cursor += "ClassName";
                }
                if (outputParentClass)
                {
                    if (cursor != "")
                        cursor += "," + "ParentClass";
                    else
                        cursor += "ParentClass";
                }
                if (outputBaseClass)
                {
                    if (cursor != "")
                        cursor += "," + "BaseClass";
                    else
                        cursor += "BaseClass";
                }
            }
            if(list != null)
                foreach(A3Class c in list)
                {
                    if(c.Variables != null)
                        foreach(A3Variable v in c.Variables)
                        {
                            if (cursor != "")
                                cursor += "," + v.FieldName;
                            else
                                cursor += v.FieldName;
                        }
                }
            
            //Create a list of strings with inputs as output
            if (list != null)
                foreach (A3Class c in list)//iterate through class
                {
                    if(outputClassName)
                    {
                        if (cursor != "")
                            cursor += "," + c.A3ClassName;
                        else
                            cursor += c.A3ClassName;
                    }
                    if(outputParentClass)
                    {
                        if (c.NestedTree != null)
                        {
                            if (cursor != "")
                                cursor += "," + c.NestedTree[0];
                            else
                                cursor += c.NestedTree[0];
                        }
                        else
                            cursor += ",";
                    }
                    if(outputBaseClass)
                    {
                        if (c.ExtendedTree != null)
                        {
                            if (cursor != "")
                                cursor += "," + c.ExtendedTree[0];
                            else
                                cursor += c.ExtendedTree[0];
                        }
                        else
                            cursor += ",";
                    }
                    cursor = "";
                    if (c.Variables != null)
                        foreach (A3Variable v in c.Variables)//iterate through classes variables
                        {
                            if (cursor != "")
                                cursor += "," + v.Value[0];
                            else
                                cursor += v.Value[0];
                        }
                    if (outputSource)
                    {
                        cursor += " \nClass Source:,";
                        foreach(String s in c.OriginalCode)
                        {
                            cursor += s;
                        }
                    }
                    if (output != null)
                        output.Add(cursor + " \n");
                    else
                        output = new List<String> { cursor + " \n" };
                }

            return output;
        }
        
    }
}
