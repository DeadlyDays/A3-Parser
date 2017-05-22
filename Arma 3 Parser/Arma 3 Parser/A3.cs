using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    
    class A3
    {
        private List<A3CppFile> fileList;//List of A3 Files
        private List<A3Level2Class> classList;//Lisdt of all A3 Classes

        A3()
        {

        }
        
        public List<A3CppFile> FileList
        {
            get
            {
                if (fileList == null)
                    return new List<A3CppFile>();
                return fileList;
            }
            set
            {
                fileList = value;
            }
        }
        public List<A3Level2Class> ClassList
        {
            get
            {
                if (classList == null)
                    return new List<A3Level2Class>();
                return classList;
            }
            set
            {
                classList = value;
            }
        }

        public void addFile(A3CppFile file)
        {
            if (fileList != null)
                fileList.Add(file);
            else
                fileList = new List<A3CppFile> { file };
        }
        public void addFiles(List<A3CppFile> files)
        {
            if (fileList != null)
                fileList.AddRange(files);
            else
                fileList = files;
        }
        public void addClass(A3Level2Class a3Class)
        {
            if (classList != null)
                classList.Add(a3Class);
            else
                classList = new List<A3Level2Class> { a3Class };
        }
        public void addClasses(List<A3Level2Class> a3Classes)
        {
            if (classList != null)
                classList.AddRange(a3Classes);
            else
                classList = a3Classes;
        }
        public void populateClassList()//populates classList from fileList
        {
            if (fileList != null)
            {

            }
        }
        public void actualizeInheritance()//the goal here is to ensure all classes contain all the values of their bases classes that haven't been overridden
        {
            if(classList != null)
            { 
            //Seperate all classes that share the same name AND inheritance - single threaded

            //combine fields between like classes - multithreaded

            //Iterate through every class, start at classes that have 1 extended class listed, and inherit values
            //  after entire pass through incrememt the length of the extended class we are looking for. only inherit from the closest class
            //  Single Threaded :(

            }
        }

    }
}
