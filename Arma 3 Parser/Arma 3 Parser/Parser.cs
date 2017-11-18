using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    

    class Parser
    {
        private List<String> fileContentList = new List<String>(); //List of Strings, each the entire content of a file.
        private DataSet DB = new DataSet();//This is a inmemory layout of the database, data is staged here then moved to online Database.

        public Parser()
        {
            //Empty Constructor
        }

        public void populateFileContentList(String path)
        //File to parse into a String, added to List<String> fileContentList
        //Overload for Directory of Files, all to be parsed into a String each and added to List<String> fileContent
        {
            //Seperate logic for System.IO.File and System.IO.Directory
            //Check if path is a Directory or File

            //Directory processing

            //File processing

        }

        public void parseFileContentList()
        //After populateFileContentList we want to parse the content of fileContentList
        //into a singular set of organized data for review/storage/queries
        {


        }

        public void prepareLocalDB()
            //This should prep Dataset DB so that it correctly mimics the online DB we will push updates to
        {

        }
        public void populateLocalDB()
            //This should populate the Dataset DB with values so that it can then by synced online later
        {

        }
        public void syncDB()
            //This should update the online DB with values in Local Dataset DB
        {

        }
        
    }
}
