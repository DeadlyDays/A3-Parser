using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    [Serializable]
    class Config
    {

        private String pboPath, binPath, cppPath, serialPath, outputPath, unpackPath, convertPath, notContainPartClass, containPartClass, hasDirectParent,
            hasOneOfParent, anyField, dispField;
        public Config(){}
        
        public String PBOPath
        {
            get
            {
                if (pboPath == null)
                    return "";
                return pboPath;
            }
            set
            {
                pboPath = value;
            }
        }
        public String BinPath
        {
            get
            {
                if (binPath == null)
                    return "";
                return binPath;
            }
            set
            {
                binPath = value;
            }
        }
        public String CPPPath
        {
            get
            {
                if (cppPath == null)
                    return "";
                return cppPath;
            }
            set
            {
                cppPath = value;
            }
        }
        public String SerialPath
        {
            get
            {
                if (serialPath == null)
                    return "";
                return serialPath;
            }
            set
            {
                serialPath = value;
            }
        }
        public String OutputPath
        {
            get
            {
                if (outputPath == null)
                    return "";
                return outputPath;
            }
            set
            {
                outputPath = value;
            }
        }
        public String UnpackPath
        {
            get
            {
                if (unpackPath == null)
                    return "";
                return unpackPath;
            }
            set
            {
                unpackPath = value;
            }
        }
        public String ConvertPath
        {
            get
            {
                if (convertPath == null)
                    return "";
                return convertPath;
            }
            set
            {
                convertPath = value;
            }
        }

        public String NotContainPartClass
        {
            get
            {
                if (notContainPartClass == null)
                    return "";
                return notContainPartClass;
            }
            set
            {
                notContainPartClass = value;
            }
        }

        public String ContainPartClass
        {
            get
            {
                if (containPartClass == null)
                    return "";
                return containPartClass;
            }
            set
            {
                containPartClass = value;
            }
        }
        public String HasDirectParent
        {
            get
            {
                if (hasDirectParent == null)
                    return "";
                return hasDirectParent;
            }
            set
            {
                hasDirectParent = value;
            }
        }
        public String HasOneOfParent
        {
            get
            {
                if (hasOneOfParent == null)
                    return "";
                return hasOneOfParent;
            }
            set
            {
                hasOneOfParent = value;
            }
        }
        public String AnyField
        {
            get
            {
                if (anyField == null)
                    return "";
                return anyField;
            }
            set
            {
                anyField = value;
            }
        }
        public String DispField
        {
            get
            {
                if (dispField == null)
                    return "";
                return dispField;
            }
            set
            {
                dispField = value;
            }
        }
    }
}
