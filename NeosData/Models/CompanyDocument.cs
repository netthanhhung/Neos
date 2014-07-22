using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblSocieteDocument")]
    public class CompanyDocument : IEntity
    {
        private int documentID;
        [EntityProperty(ColumnName = "DocumentID",  IsIdentity = true, IsPrimaryKey = true)]
        public int DocumentID
        {
            get { return documentID; }
            set { documentID = value; }
        }
        private int companyID;
        [EntityProperty(ColumnName = "SocieteID")]
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        private string documentName;
        [EntityProperty(ColumnName = "NomDoc")]
        public string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }
        private string documentLegend;
        [EntityProperty(ColumnName = "CheminDoc")]
        public string DocumentLegend
        {
            get { return documentLegend; }
            set { documentLegend = value; }
        }

        private DateTime? createdDate;
        [EntityProperty(ColumnName = "CreatedDate")]
        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }
        private string absoluteURL;
        [EntityProperty(ColumnName = "AbsoluteURL")]
        public string AbsoluteURL
        {
            get { return absoluteURL; }
            set { absoluteURL = value; }
        }

        private string contentType;
        [EntityProperty(ColumnName = "ContentType")]
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        private string type;
        [EntityProperty(ColumnName = "Type")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        } 

        public CompanyDocument()
        {
        }
        public CompanyDocument(int id)
        {
            this.documentID = id;
        }
    }
}
