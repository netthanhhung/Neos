using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamFonctionFam")]
    public class ParamFunctionFam : IEntity
    {
        private string fonctionFamID;
        private string genre;

        public ParamFunctionFam() { }
        public ParamFunctionFam(string fonctionFamID)
        {
            this.fonctionFamID = fonctionFamID;            
        }
        public ParamFunctionFam(string fonctionFamID, string genre)
        {
            this.fonctionFamID = fonctionFamID;
            this.genre = genre;
        }


        [EntityProperty(ColumnName = "FonctionFamID", IsIdentity = true, IsPrimaryKey = true)]
        public string FonctionFamID
        {
            get { return fonctionFamID; }
            set { fonctionFamID = value; }
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
