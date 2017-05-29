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
                        if (list[i].InheritanceTree != null && list[i].InheritanceTree.Count > 0)
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
                    cursor += "," + s;
                else
                    cursor += s;
            }
            if (output != null)
            {
                output.Add(cursor + " \n");
                cursor = "";
            }
            else
            {
                output = new List<String> { cursor + " \n" };
                cursor = "";
            }

            //Create a list of strings with inputs as output
            if (select != null)
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
                    
                    if(input != null)
                        foreach (String s in input)//iterate through classes variables
                        {
                            Boolean check = false;
                            if(c.Variables != null)   
                                foreach (A3Variable v in c.Variables)//iterate through input values
                                {
                                    if (v.FieldName == s)//if fieldname matches input, add
                                    {
                                        if(v.Value[0].Contains("\""))
                                        {
                                            if (cursor != "")
                                                cursor += "," + v.Value[0];
                                            else
                                                cursor += v.Value[0];
                                        }
                                        else
                                        {
                                            if (cursor != "")
                                                cursor += ",\"" + v.Value[0] + "\"";
                                            else
                                                cursor += "\"" + v.Value[0] + "\"";
                                        }
                                        
                                        check = true;
                                    }
                                               
                                }
                            if (!check)
                                cursor += ",";
                        }
                    if (outputSource)
                    {
                        cursor += " \nClass Source:\n\"";
                        foreach (String s in c.OriginalCode)
                        {
                            cursor += s.Replace("\"", "'").Replace("\n", "").Replace("\r", "");
                        }
                        cursor += "\"";
                    }
                    if (output != null)
                    {
                        output.Add(cursor + " \n");
                        cursor = "";
                    }
                    else
                    {
                        output = new List<String> { cursor + " \n" };
                        cursor = "";
                    }

                }
            return output;
        }

        //Display All fields
        public static List<String> outputAllFields(List<A3Class> list, Boolean outputClassName, 
            Boolean outputParentClass, Boolean outputBaseClass, Boolean outputSource)
        {
            List<String> output = new List<String>();
            String cursor = "";//Current string to be added
            if (outputClassName || outputParentClass || outputBaseClass)//Column Names to be added
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
            List<String> variableList = new List<String>();//List of fields
            if(list != null)//Column Names to be added
                foreach(A3Class c in list)
                {
                    if(c.Variables != null)
                        foreach(A3Variable v in c.Variables)
                        {
                            if (variableList != null)//if variableList isn't empty
                                if (variableList.Contains(v.FieldName))//if variableList already has this field
                                    ;//do nothing
                                else//variable list isn't empty and doesn't have this field yet
                                {
                                    variableList.Add(v.FieldName);//add variable name to list

                                    if (cursor != "")//add variable name to column list
                                        cursor += "," + v.FieldName;
                                    else
                                        cursor += v.FieldName;
                                }
                            else
                                ;

                            
                        }
                }
            if (output != null)
            {
                output.Add(cursor + " \n");
                cursor = "";
            }
            else
            {
                output = new List<String> { cursor + " \n" };
                cursor = "";
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
                        if (c.ExtendedTree != null && c.ExtendedTree.Count > 0)
                        {
                            if (cursor != "")
                                cursor += "," + c.ExtendedTree[0];
                            else
                                cursor += c.ExtendedTree[0];
                        }
                        else
                            cursor += ",";
                    }
                    //cursor = "";//idk why this was here....left over?
                    if (c.Variables != null)
                    {
                        Boolean check = false;
                        foreach(String s in variableList)//iterate through the variables
                        {
                            check = false;
                            foreach (A3Variable v in c.Variables)//iterate through classes variables
                            {
                                if(v.FieldName == s)//Variable in class == variable in variableList
                                {
                                    //add variable
                                    if (cursor != "")
                                    {
                                        if (v.Value[0].Contains("\""))
                                        {
                                            cursor += "," + v.Value[0];
                                        }
                                        else
                                            cursor += ",\"" + v.Value[0] + "\"";
                                    }
                                        
                                    else
                                    {
                                        if (v.Value[0].Contains("\""))
                                        {
                                            cursor += v.Value[0];
                                        }
                                        else
                                            cursor += "\"" + v.Value[0] + "\"";
                                    }
                                    
                                    check = true;
                                    //break loop
                                    break;
                                }
                                
                            }
                            if (!check)//if we haven't added a value, we need an empty delimiter
                                cursor += ",";
                        }
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
                    { 
                        output.Add(cursor + " \n");
                        cursor = "";
                    }
                    else
                    { 
                        output = new List<String> { cursor + " \n" };
                        cursor = "";
                    }
                }

            return output;
        }

        //Remove any not selected
        public static List<A3Class> includeOnlySelected(List<A3Class> list, Boolean CfgVehicles, Boolean CfgAmmo,
            Boolean CfgWeapons, Boolean CfgMagazines, Boolean Lvl1, Boolean Lvl2,
            Boolean Tert, Boolean Other)
        {
            //we take in a list of classes, our goal is to only have the classes that are allowed at the end
            List<A3Class> newList = new List<A3Class>();
            for (int i = 0; i < list.Count; i++)//Iterate through every class
            {
                Boolean add = false;
                if (((list[i] is A3Level1Class) && Lvl1))//When Lvl 1 toggled on, if it isn't a lvl1, remove it)
                {
                    add = true;
                }
                if (((list[i] is A3Level2Class) && Lvl2))
                {
                    add = true;
                }
                if (((list[i] is A3TertiaryClass) && Tert))
                {
                    add = true;
                }
                if (add) newList.Add(list[i]);
            }
            List<A3Class> finalList = new List<A3Class>();
            for (int i = 0; i < newList.Count; i++)//Iterate through every class
            {
                Boolean add = false;
                if(newList[i].NestedTree.Count >= 1)
                { 
                if ((newList[i].NestedTree[0] == "CfgVehicles" && CfgVehicles))
                {
                    add = true;
                }
                if ((newList[i].NestedTree[0] == "CfgAmmo" && CfgAmmo))
                {
                    add = true;
                }
                if ((newList[i].NestedTree[0] == "CfgMagazines" && CfgMagazines))
                {
                    add = true;
                }
                if ((newList[i].NestedTree[0] == "CfgWeapons" && CfgWeapons))
                {
                    add = true;
                }
                if ((newList[i].NestedTree[0] != "CfgVehicles" &&
                    newList[i].NestedTree[0] != "CfgAmmo" &&
                    newList[i].NestedTree[0] != "CfgMagazines" &&
                    newList[i].NestedTree[0] != "CfgWeapons" && Other))
                {
                    add = true;
                }
                }
                if (add) finalList.Add(newList[i]);


            }

            return finalList;
        }
        
    }
}
