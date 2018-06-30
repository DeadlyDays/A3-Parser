using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma_3_Parser
{
    /// <summary>
    /// Arma Collection class is a container for a Dataset + functions for A3 config values
    /// </summary>
    class ArmaCollection
    {
        /// <summary>
        /// Contains the Internal Database of parsed config values
        /// </summary>
        DataSet A3Set;

        /// <summary>
        /// Default Constructor for an ArmaCollection Object
        /// </summary>
        public ArmaCollection()
        {

            /// Any updates to any table are logged here
            DataTable UpdatesTB = new DataTable("UpdatesTB") ;
            UpdatesTB.Columns.Add(new DataColumn("UID"));
            /// This contains the details of any updates, referencing the UpdatesTB value as a unique identifier of a set of updates.
            DataTable ArchiveTB = new DataTable("ArchiveTB");
            ArchiveTB.Columns.Add(new DataColumn("UID"));
            ArchiveTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            ArchiveTB.Columns.Add(new DataColumn("FieldEdited"));
            ArchiveTB.Columns.Add(new DataColumn("OldValue"));
            ArchiveTB.Columns.Add(new DataColumn("NewValue"));
            ArchiveTB.Columns.Add(new DataColumn("EditedBy"));
            ArchiveTB.Columns.Add(new DataColumn("EditedOnEpoch"));
            ArchiveTB.Columns.Add(new DataColumn("EditedReason"));
            ArchiveTB.Columns.Add(new DataColumn("ValueType"));
            /// This is the Name of a Mod, which PBO's/Config Files would be grouped under(and through a set of references, all classes and fields would be grouped by)
            DataTable ModTB = new DataTable("ModTB");
            ModTB.Columns.Add(new DataColumn("UID"));
            ModTB.Columns.Add(new DataColumn("Name"));
            ModTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            /// Details on the Config File are logged here, allows classes to be connected to the proper mod and file they are written in.
            DataTable ConfigFileTB = new DataTable("ConfigFileTB");
            ConfigFileTB.Columns.Add(new DataColumn("UID"));
            ConfigFileTB.Columns.Add(new DataColumn("PBOName"));
            ConfigFileTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            ConfigFileTB.Columns.Add(new DataColumn("ModTB_UID"));
            /// This is the Cfg class, connected to a PBO/Config file, and what classes it contains. If there are any fields, they are also linked.
            DataTable CfgClassesTB = new DataTable("CfgClassesTB");
            CfgClassesTB.Columns.Add(new DataColumn("UID"));
            CfgClassesTB.Columns.Add(new DataColumn("CfgName"));
            CfgClassesTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            CfgClassesTB.Columns.Add(new DataColumn("ConfigFileTB_UID"));
            CfgClassesTB.Columns.Add(new DataColumn("FieldListTB_UID"));
            /// This contains the classes that are contained in Cfg classes, which are typically units. Connected to the Cfg File, and through that, which PBO and mod this belongs to. There is also a link to which parent class it has, if it isn't a CfgClass, which links back to this tables UID
            DataTable ClassObjectsTB = new DataTable("ClassObjectsTB");
            ClassObjectsTB.Columns.Add(new DataColumn("UID"));
            ClassObjectsTB.Columns.Add(new DataColumn("Name"));
            ClassObjectsTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            ClassObjectsTB.Columns.Add(new DataColumn("CfgClassesTB_UID"));
            ClassObjectsTB.Columns.Add(new DataColumn("FieldListTB_UID"));
            ClassObjectsTB.Columns.Add(new DataColumn("ParentClass_UID"));
            /// This contains classes that are children under classObjects, which are typically collections of properties of said class. These classes also typically nest within other ClassProperties, which links are provided. There are also links to the fields contained within each.
            DataTable ClassPropertiesTB = new DataTable("ClassPropertiesTB");
            ClassPropertiesTB.Columns.Add(new DataColumn("UID"));
            ClassPropertiesTB.Columns.Add(new DataColumn("Name"));
            ClassPropertiesTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            ClassPropertiesTB.Columns.Add(new DataColumn("CfgObjectsTB_UID"));
            ClassPropertiesTB.Columns.Add(new DataColumn("FieldListTB_UID"));
            ClassPropertiesTB.Columns.Add(new DataColumn("ParentClassObject_UID"));
            ClassPropertiesTB.Columns.Add(new DataColumn("ParentClassProperty_UID"));
            /// This is just a unique identifier to connect collections of fields to a class
            DataTable FieldListTB = new DataTable("FieldListTB");
            FieldListTB.Columns.Add(new DataColumn("UID"));
            /// This contains the single fields that are within a FieldList
            DataTable FieldsTB = new DataTable("FieldsTB");
            FieldsTB.Columns.Add(new DataColumn("UID"));
            FieldsTB.Columns.Add(new DataColumn("Name"));
            FieldsTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            FieldsTB.Columns.Add(new DataColumn("FieldListTB_UID"));
            FieldsTB.Columns.Add(new DataColumn("IsArray"));
            /// This contains the value or values that belong to a fieldName
            DataTable FieldValuesTB = new DataTable("FieldValuesTB");
            FieldValuesTB.Columns.Add(new DataColumn("UID"));
            FieldValuesTB.Columns.Add(new DataColumn("FieldsTB_UID"));
            FieldValuesTB.Columns.Add(new DataColumn("Value"));
            FieldValuesTB.Columns.Add(new DataColumn("UpdatesTB_UID"));
            FieldValuesTB.Columns.Add(new DataColumn("Type"));



        }


    }
}
