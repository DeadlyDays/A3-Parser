using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Arma_3_Parser
{
    

    class Parser
    {
        private List<String> fileContentList = new List<String>(); //List of Strings, each the entire content of a file.
        private DataSet DB = new DataSet();//This is a inmemory layout of the database, data is staged here then moved to online Database.

        private String rClassDec = @"class\s+(?<ClassName>\w+)\s*:\s*(?<ParentName>\w*)";
        private String rOpenContext = "{";
        private String rCloseContext = "}";
        private String rVariableDec = @"(?<VariableName>\w+)\s*=(?<VariableValue>.*);";
        private String rArrayDec = @"(?<ArrayName>\w+)\[]\s*=\s*{\s*(?<ArrayValue>""?\w+""?,?\s*)+\s*}\s*;";


        public Parser()
        {
            //Track Classes
            DataTable ClassTB = DB.Tables.Add("ClassTB");
            DataColumn C_pk_ClassUID = ClassTB.Columns.Add("C_pk_ClassUID", typeof(Int32));
            ClassTB.Columns.Add("C_Name", typeof(String));
            ClassTB.Columns.Add("C_ParentName", typeof(String));
            ClassTB.Columns.Add("C_OwnerName", typeof(String));
            ClassTB.Columns.Add("C_Filename", typeof(String));
            ClassTB.Columns.Add("C_LineClassDef", typeof(Int32));
            ClassTB.Columns.Add("C_LineClassStart", typeof(Int32));
            ClassTB.Columns.Add("C_LineClassEnd", typeof(Int32));

            ClassTB.PrimaryKey = new DataColumn[] { C_pk_ClassUID };

            //Track Variables
            DataTable VariableTB = DB.Tables.Add("VariableTB");
            DataColumn V_pk_VariableUID = VariableTB.Columns.Add("V_pk_VariableUID", typeof(Int32));
            VariableTB.Columns.Add("V_Name", typeof(String));
            VariableTB.Columns.Add("V_Value", typeof(String));

            VariableTB.PrimaryKey = new DataColumn[] { V_pk_VariableUID };

            //Track Arrays
            DataTable ArrayTB = DB.Tables.Add("ArrayTB");
            DataColumn A_pk_ArrayUID = ArrayTB.Columns.Add("A_pk_ArrayUID", typeof(Int32));
            ArrayTB.Columns.Add("A_Name", typeof(String));

            ArrayTB.PrimaryKey = new DataColumn[] { A_pk_ArrayUID };

            //Lookup for Variables
            DataTable VariableJoinTB = DB.Tables.Add("VariableJoinTB");
            DataColumn VJ_pk_VJ_UID = VariableJoinTB.Columns.Add("VJ_pk_VJ_UID", typeof(Int32));
            DataColumn VJ_fk_ClassUID = VariableJoinTB.Columns.Add("VJ_fk_ClassUID", typeof(Int32));
            DataColumn VJ_fk_VariableUID = VariableJoinTB.Columns.Add("VJ_fk_VariableUID", typeof(Int32));

            VariableJoinTB.PrimaryKey = new DataColumn[] { VJ_pk_VJ_UID };
            DB.Relations.Add("ClassVariableJoin",
                DB.Tables["ClassTB"].Columns["C_pk_ClassUID"],
                DB.Tables["VariableJoinTB"].Columns["VJ_fk_ClassUID"]);
            DB.Relations.Add("VariableVariableJoin",
                DB.Tables["VariableTB"].Columns["V_pk_VariableUID"],
                DB.Tables["VariableJoinTB"].Columns["VJ_fk_VariableUID"]);

            //Lookup for ArrayVariables
            DataTable ArrayJoinTB = DB.Tables.Add("ArrayJoinTB");
            DataColumn AJ_pk_UID = ArrayJoinTB.Columns.Add("AJ_pk_UID", typeof(Int32));
            DataColumn AJ_fk_ArrayUID = ArrayJoinTB.Columns.Add("AJ_fk_ArrayUID", typeof(Int32));
            DataColumn AJ_fk_ClassUID = ArrayJoinTB.Columns.Add("AJ_fk_ClassUID", typeof(Int32));
            DataColumn AJ_fk_VariableUID = ArrayJoinTB.Columns.Add("AJ_fk_VariableUID", typeof(Int32));

            ArrayJoinTB.PrimaryKey = new DataColumn[] { AJ_pk_UID };
            DB.Relations.Add("ArrayArrayJoin",
                DB.Tables["ArrayTB"].Columns["A_pk_ArrayUID"],
                DB.Tables["ArrayJoinTB"].Columns["AJ_fk_ArrayUID"]);
            DB.Relations.Add("ClassArrayJoin",
                DB.Tables["ClassTB"].Columns["C_pk_ClassUID"],
                DB.Tables["ArrayJoinTB"].Columns["AJ_fk_ClassUID"]);
            DB.Relations.Add("VariableArrayJoin",
                DB.Tables["VariableTB"].Columns["V_pk_VariableUID"],
                DB.Tables["ArrayJoinTB"].Columns["AJ_fk_VariableUID"]);


        }

        /// <summary>
        /// Read String from file, process into DB Tables
        /// </summary>
        public DataSet parseFile(String fileContent, DataSet DB)
        {
            //Read declaration by declaration
            //Identify each line

            //We need to keep in mind that we need to join some classes together (CfgVehicle)


            ///Finds Base Classes (Like CfgVehicles)
            //Find first Class
            //Find matching Open Bracket
            //Cycle through until found end bracket

            Match FirstClass = Regex.Match(fileContent, rClassDec);


            //Base Classes should now be populated, including locations of open and close brackets
            //Next, we need to recursively redo this on the contents of each class


            return new DataSet();
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
