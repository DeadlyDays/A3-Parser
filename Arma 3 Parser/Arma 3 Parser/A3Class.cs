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
        String a3ClassName;
        String fileLocation;
        String lineLocation;
        
        List<A3Class> children;//all of the child classes
        List<String> inheritanceTree;//list of all the parent elements(things extended + classes nested within)
        List<String> nestedTree;//just the inheritance tree of classes nested within
        List<String> extendedTree;//just the inheritance tree of the classes extended
        List<String> originalCode;//this is the class dec + brackets and all between
        List<String> content;//This is everything between the brackets
        List<A3Variable> variables;//All the variables and their values for this class

        A3Class() { }

    }
}
