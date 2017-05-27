using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3TertiaryClass : A3Class
    {
        
        public A3TertiaryClass() { }

        public A3TertiaryClass(String name)
        {
            a3ClassName = name;
            fileLocation = "";
        }
        
    }
}
