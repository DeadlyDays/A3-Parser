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
        public static List<String> outputSelectedFields(List<A3Class> list, String[] input)
        {

            return new List<String>();
        }

        //Display All fields
        public static List<String> outputAllFields(List<A3Class> list)
        {

            return new List<String>();
        }

        //Display ClassName
        public static List<String> outputClassName(List<String> list)
        {

            return new List<String>();
        }

        //Display Parent(nested) Class
        public static List<String> outputParentClass(List<String> list)
        {

            return new List<String>();
        }

        //Display Base(extended) Class
        public static List<String> outputBaseClass(List<String> list)
        {

            return new List<String>();
        }

        //Display Source Code
        public static List<String> outputSource(List<String> list)
        {

            return new List<String>();
        }

    }
}
