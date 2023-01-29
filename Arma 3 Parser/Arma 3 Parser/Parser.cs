using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace Arma_3_Parser
{
    

    class Parser
    {
        private List<String> fileContentList = new List<String>(); //List of Strings, each the entire content of a file.
        private DataSet db = new DataSet();//This is a in-memory layout of the database, data is staged here then moved to online Database.
        public DataSet DB
        {
            get { return db; }
            set { db = value; }
        }
        private String rClassDec = @"class\s+(?<ClassName>\w+)\s*:?\s*(?<ParentName>\w*)?\s*(?<OpenContext>{)?";
        private String rEmptyClassDec = @"class\s+(?<ClassName>\w+)\s*:?\s*(?<ParentName>\w*)?\s*;";
        private String rCodeBlockOpen = "{";
        private String rCodeBlockClose = "}";
        private String rCodeBlock = @"[{}]";
        private String rVariableDec = @"(?<VariableName>\w+)\s*=(?<VariableValue>.*);";
        private String rArrayDec = @"(?<ArrayName>\w+)\[]\s*=\s*{\s*(?<ArrayValue>""?\w+""?,?\s*)+\s*}\s*;";


        public Parser()
        {
            //Track Classes
            DataTable ClassDecTB = db.Tables.Add("ClassDecTB");
            DataColumn CD_pk_ClassDecUID = ClassDecTB.Columns.Add("CD_pk_ClassDecUID", typeof(Int32));
            ClassDecTB.Columns.Add("CD_Name", typeof(String));
            ClassDecTB.Columns.Add("CD_ParentName", typeof(String));
            ClassDecTB.Columns.Add("CD_OwnerName", typeof(String));
            ClassDecTB.Columns.Add("CD_Filename", typeof(String));
            ClassDecTB.Columns.Add("CD_LineClassDef", typeof(Int32));
            ClassDecTB.Columns.Add("CD_LineClassStart", typeof(Int32));
            ClassDecTB.Columns.Add("CD_LineClassEnd", typeof(Int32));
            DataColumn CD_fk_ClassUID = ClassDecTB.Columns.Add("CD_fk_ClassUID", typeof(Int32));

            ClassDecTB.PrimaryKey = new DataColumn[] { CD_pk_ClassDecUID };
            CD_pk_ClassDecUID.AutoIncrement = true;
            CD_pk_ClassDecUID.AutoIncrementSeed = 1;
            CD_pk_ClassDecUID.AutoIncrementStep = 1;

            //Track Unique Classes
            DataTable ClassTB = db.Tables.Add("ClassTB");
            DataColumn C_pk_ClassUID = ClassDecTB.Columns.Add("C_pk_ClassUID", typeof(Int32));
            ClassTB.Columns.Add("C_Name", typeof(String));
            ClassTB.Columns.Add("C_OwnerName", typeof(String));

            ClassTB.PrimaryKey = new DataColumn[] { C_pk_ClassUID };
            C_pk_ClassUID.AutoIncrement = true;
            C_pk_ClassUID.AutoIncrementSeed = 1;
            C_pk_ClassUID.AutoIncrementStep = 1;
            db.Relations.Add("ClassClassDecJoin",
                db.Tables["ClassTB"].Columns["C_pk_ClassUID"],
                db.Tables["ClassDecTB"].Columns["CD_fk_ClassUID"]);

            //Track Variables
            DataTable VariableTB = db.Tables.Add("VariableTB");
            DataColumn V_pk_VariableUID = VariableTB.Columns.Add("V_pk_VariableUID", typeof(Int32));
            VariableTB.Columns.Add("V_Name", typeof(String));
            VariableTB.Columns.Add("V_Value", typeof(String));

            VariableTB.PrimaryKey = new DataColumn[] { V_pk_VariableUID };
            V_pk_VariableUID.AutoIncrement = true;
            V_pk_VariableUID.AutoIncrementSeed = 1;
            V_pk_VariableUID.AutoIncrementStep = 1;

            //Track Arrays
            DataTable ArrayTB = db.Tables.Add("ArrayTB");
            DataColumn A_pk_ArrayUID = ArrayTB.Columns.Add("A_pk_ArrayUID", typeof(Int32));
            ArrayTB.Columns.Add("A_Name", typeof(String));

            ArrayTB.PrimaryKey = new DataColumn[] { A_pk_ArrayUID };
            A_pk_ArrayUID.AutoIncrement = true;
            A_pk_ArrayUID.AutoIncrementSeed = 1;
            A_pk_ArrayUID.AutoIncrementStep = 1;

            //Lookup for Variables
            DataTable VariableJoinTB = db.Tables.Add("VariableJoinTB");
            DataColumn VJ_pk_VJ_UID = VariableJoinTB.Columns.Add("VJ_pk_VJ_UID", typeof(Int32));
            DataColumn VJ_fk_ClassUID = VariableJoinTB.Columns.Add("VJ_fk_ClassUID", typeof(Int32));
            DataColumn VJ_fk_VariableUID = VariableJoinTB.Columns.Add("VJ_fk_VariableUID", typeof(Int32));

            VariableJoinTB.PrimaryKey = new DataColumn[] { VJ_pk_VJ_UID };
            VJ_pk_VJ_UID.AutoIncrement = true;
            VJ_pk_VJ_UID.AutoIncrementSeed = 1;
            VJ_pk_VJ_UID.AutoIncrementStep = 1;
            db.Relations.Add("ClassVariableJoin",
                db.Tables["ClassTB"].Columns["C_pk_ClassUID"],
                db.Tables["VariableJoinTB"].Columns["VJ_fk_ClassUID"]);
            db.Relations.Add("VariableVariableJoin",
                db.Tables["VariableTB"].Columns["V_pk_VariableUID"],
                db.Tables["VariableJoinTB"].Columns["VJ_fk_VariableUID"]);

            //Lookup for Arrays
            DataTable ArrayJoinTB = db.Tables.Add("ArrayJoinTB");
            DataColumn AJ_pk_UID = ArrayJoinTB.Columns.Add("AJ_pk_UID", typeof(Int32));
            DataColumn AJ_fk_ArrayUID = ArrayJoinTB.Columns.Add("AJ_fk_ArrayUID", typeof(Int32));
            DataColumn AJ_fk_ClassUID = ArrayJoinTB.Columns.Add("AJ_fk_ClassUID", typeof(Int32));

            ArrayJoinTB.PrimaryKey = new DataColumn[] { AJ_pk_UID };
            AJ_pk_UID.AutoIncrement = true;
            AJ_pk_UID.AutoIncrementSeed = 1;
            AJ_pk_UID.AutoIncrementStep = 1;
            db.Relations.Add("ArrayArrayJoin",
                db.Tables["ArrayTB"].Columns["A_pk_ArrayUID"],
                db.Tables["ArrayJoinTB"].Columns["AJ_fk_ArrayUID"]);
            db.Relations.Add("ClassArrayJoin",
                db.Tables["ClassTB"].Columns["C_pk_ClassUID"],
                db.Tables["ArrayJoinTB"].Columns["AJ_fk_ClassUID"]);

            //Lookup for Arrays
            DataTable ArrayVariableJoinTB = db.Tables.Add("ArrayVariableJoinTB");
            DataColumn AV_pk_UID = ArrayVariableJoinTB.Columns.Add("AV_pk_UID", typeof(Int32));
            DataColumn AV_fk_ArrayUID = ArrayVariableJoinTB.Columns.Add("AV_fk_ArrayUID", typeof(Int32));
            DataColumn AV_fk_VariableUID = ArrayVariableJoinTB.Columns.Add("AV_fk_VariableUID", typeof(Int32));

            ArrayVariableJoinTB.PrimaryKey = new DataColumn[] { AJ_pk_UID };
            AV_pk_UID.AutoIncrement = true;
            AV_pk_UID.AutoIncrementSeed = 1;
            AV_pk_UID.AutoIncrementStep = 1;
            db.Relations.Add("ArrayVariableJoin",
                db.Tables["ArrayTB"].Columns["A_pk_ArrayUID"],
                db.Tables["ArrayVariableJoinTB"].Columns["AV_fk_ArrayUID"]);
            db.Relations.Add("VariableArrayJoin",
                db.Tables["VariableTB"].Columns["V_pk_VariableUID"],
                db.Tables["ArrayVariableJoinTB"].Columns["AV_fk_VariableUID"]);


        }
        public Parser(DataSet importDB)
        {
            db = importDB;
        }

        /// <summary>
        /// Read String from file, process into DB Tables
        /// </summary>
        public DataSet parseFile(String fileName, String fileContent)
        {
            //Read declaration by declaration
            //Identify each line

            //We need to keep in mind that we need to join some classes together (CfgVehicle)

            //Find all Empty Classes
            Boolean anyEmptyClasses = false;
            MatchCollection emptyClasses = Regex.Matches(fileContent, rEmptyClassDec);
            if (emptyClasses.Count == 0) anyEmptyClasses = true;
            else anyEmptyClasses = true;

            ///Finds Base Classes (Like CfgVehicles)
            //Find first Class
            //Find matching Open Bracket
            //Cycle through until found end bracket

            //Find all Classes
            MatchCollection classes = Regex.Matches(fileContent, rClassDec);
            if (classes.Count == 0)
            {
                //Empty File, end
                return new DataSet();
            }

            //Find all Open/Close Brackets
            //Open
            MatchCollection CodeBlockOpenLocations = Regex.Matches(fileContent, rCodeBlockOpen);

            //Close
            MatchCollection CodeBlockCloseLocations = Regex.Matches(fileContent, rCodeBlockClose);

            //All Open/Close
            MatchCollection CodeBlockLocations = Regex.Matches(fileContent, rCodeBlock);

            String PrevClassName = "";
            int PrevDepth = 0;
            //Iterate over all classes
            for(int i = 0; i < classes.Count; i++)
            {
                //Need to find the depth of this class
                int depth = 0;
                //Iterate over all CodeBlockOpenLocations (If index is lower, depth++)
                foreach(Match open in CodeBlockOpenLocations)
                {
                    if (open.Index < classes[i].Index) depth++;
                    else break; //Exit early, either index is less or we've gone past
                }

                //Iterate over all CodeBlockCloseLocations (If index is lower, depth--)
                foreach (Match close in CodeBlockCloseLocations)
                {
                    if (close.Index < classes[i].Index) depth--;
                    else break; //Exit early, either index is less or we've gone past
                }

                //Check if instantly ended
                if (anyEmptyClasses)
                {
                    Boolean skip = false;
                    //If there are empty classes, we need to check if this is one
                    //If this is one...skip
                    int indexOf = classes[i].Index;
                    foreach (Match m2 in emptyClasses)
                    {
                        if(m2.Index == indexOf)
                        {
                            //Match, skip
                            skip = true;
                            break;
                        }
                        else if (m2.Index > indexOf)
                        {
                            //We have gone past index of current match, early exit
                            break;
                        }
                    }
                    //This is an empty class, move to next class
                    if (skip) 
                    {
                        //TODO
                        //Should record the empty class here
                        DataRow row = db.Tables["ClassTB"].NewRow();
                        row["C_Name"] = classes[i].Groups["ClassName"].Value;
                        //Not all classes specify an inheriting parent
                        if(classes[i].Groups["ParentName"].Value != null)
                        {
                            row["C_ParentName"] = classes[i].Groups["ParentName"].Value;
                        }
                        else
                        {
                            row["C_ParentName"] = "";
                        }
                        //We need to know the depth of this class first
                        //If there is a previous class, of a lower depth, then that is the owner
                        if(i > 0) //Only go if there is a previous class
                        {
                            //Only continue if previous depth was less
                            if(PrevDepth < depth)
                            {
                                row["C_OwnerName"] = PrevClassName;
                            }
                            else { row["C_OwnerName"] = null; }
                        }
                        else { row["C_OwnerName"] = null; }

                        row["C_Filename"] = fileName;
                        row["C_LineClassDef"] = classes[i].Index;
                        //row["C_LineClassStart"] = ""; //Empty because these don't exist in empty classes
                        //row["C_LineClassEnd"] = "";
                        db.Tables["ClassTB"].Rows.Add(row);
                        PrevClassName = classes[i].Groups["ClassName"].Value;
                        PrevDepth = depth;
                        continue; 
                    }
                }

                DataRow row2 = db.Tables["ClassTB"].NewRow();
                row2["C_Name"] = classes[i].Groups["ClassName"].Value;
                //Not all classes specify an inheriting parent
                if (classes[i].Groups["ParentName"].Value != null)
                {
                    row2["C_ParentName"] = classes[i].Groups["ParentName"].Value;
                }
                else
                {
                    row2["C_ParentName"] = "";
                }
                //We need to know the depth of this class first
                //If there is a previous class, of a lower depth, then that is the owner
                if (i > 0) //Only go if there is a previous class
                {
                    //Only continue if previous depth was less
                    if (PrevDepth < depth)
                    {
                        row2["C_OwnerName"] = PrevClassName;
                    }
                    else { row2["C_OwnerName"] = null; }
                }
                else { row2["C_OwnerName"] = null; }

                row2["C_Filename"] = fileName;
                row2["C_LineClassDef"] = classes[i].Index;
                row2["C_LineClassStart"] = classes[i].Groups["OpenContext"].Index;
                //To find the matching close bracket, we need to find the next bracket that changes depth back to current.
                int depthCheck = depth; //We need to track depth changes from this point
                foreach (Match m in CodeBlockLocations)
                {
                    if(m.Index < classes[i].Groups["OpenContext"].Index)
                    {
                        continue;//We skip ahead if index is less than open, we need only check later brackets
                    }
                    if(m.Value == "{")
                    {
                        depthCheck++;
                    }
                    else if (m.Value == "}") 
                    {
                        depthCheck--;
                        if(depthCheck == depth)
                        {
                            //If we lower depth, and it is now equal again to initial depth, that means we've closed current context
                            row2["C_LineClassEnd"] = m.Index;
                            break;//We've completed task
                        }
                    }

                }
                db.Tables["ClassTB"].Rows.Add(row2);

                //Populate tracking of previous classname/depth
                PrevClassName = classes[i].Groups["ClassName"].Value;
                PrevDepth = depth;
            }

            

            return db;
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
        

        //Helper classes

    }
}
