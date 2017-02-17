using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3Variable
    {
        String fieldName;
        String varClassName;//the name of the class this belongs to
        int linePosition;//the line location of this variable declaration

        List<String> value;//All the contents of the A3 Variable
        List<String> originalCode;//All the code that declared the field and its value
        A3Variable() { }


    }
}
