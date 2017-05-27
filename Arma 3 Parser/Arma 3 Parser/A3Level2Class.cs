using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3Level2Class : A3Class
    {
        
        public A3Level2Class() { }

        public A3Level2Class(String name)
        {
            a3ClassName = name;
            fileLocation = "";
        }

        
    }
}
