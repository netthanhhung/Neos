using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamConnaisFam")]
    public class ParamKnowledgeFam : IEntity
    {
        private string conFamilleID;
        private string genre;

        public ParamKnowledgeFam() { }
        public ParamKnowledgeFam(string conFamilleID)
        {
            this.conFamilleID = conFamilleID;            
        }
        public ParamKnowledgeFam(string conFamilleID, string genre)
        {
            this.conFamilleID = conFamilleID;
            this.genre = genre;
        }


        [EntityProperty(ColumnName = "ConFamilleID", IsIdentity = true, IsPrimaryKey = true)]
        public string ConFamilleID
        {
            get { return conFamilleID; }
            set { conFamilleID = value; }
        }


        [EntityProperty(ColumnName = "Genre")]
        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        private int numberIDUsed;
        [EntityProperty(IsPersistent = false)]
        public int NumberIDUsed
        {
            get { return numberIDUsed; }
            set
            {
                numberIDUsed = value;
            }
        }
    }
}
