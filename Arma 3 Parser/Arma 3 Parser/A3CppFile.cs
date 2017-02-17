using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class A3CppFile
    {
        private String filePath;

        private List<A3Class> a3ClassList;

        public A3CppFile(){}

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
