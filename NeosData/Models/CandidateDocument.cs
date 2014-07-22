using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatDocument")]
    public class CandidateDocument : IEntity
    {
        private int documentID;
        [EntityProperty(ColumnName = "DocumentID",  IsIdentity = true, IsPrimaryKey = true)]
        public int DocumentID
        {
            get { return documentID; }
            set { documentID = value; }
        }
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
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

        public CandidateDocument()
        {
        }
        public CandidateDocument(int id)
        {
            this.documentID = id;
        }
    }
}
