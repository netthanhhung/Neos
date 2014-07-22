using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Neos.Data;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;
using Lucene.Net.Search;

public partial class CandidateCVSearchResult : System.Web.UI.Page
{
    PageStatePersister _pers;

    #region Common
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            if (_pers == null)
            {
                _pers = new SessionPageStatePersister(this);
            }
            return _pers;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        if (!IsPostBack)
        {
            FillLabelText();
            BindData();
        }
    }

    private void FillLabelText()
    {
        rfvKeyWord.ErrorMessage = ResourceManager.GetString("lblEnterAKeyword");
        lblKeyword.Text = ResourceManager.GetString("lblKeyword");
        btnSearchCandidateCV.Text = ResourceManager.GetString("btnSearch");
        lblCandidateCV.Text = ResourceManager.GetString("lblCandidateCV");
        lblResultTitle.Text = ResourceManager.GetString("lblCandidateCVResultTitle");

        txtKeyword.Text = Request.QueryString["keyword"];
        

    }



    private void BindData()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["keyword"]))
        {
            List<Lucene.Net.Documents.Document> result = SearchCandidateCV(Request.QueryString["keyword"]);
            GridCandidateDocument.DataSource = result;
            GridCandidateDocument.DataBind();

            if(result.Count >0)
            {
                if(result.Count > GridCandidateDocument.PageSize)
                    lblResultTitle.Text = string.Format(ResourceManager.GetString("lblCandidateCVResultTitle"), "1 - " + GridCandidateDocument.PageSize, result.Count, Request.QueryString["keyword"]);
                else
                    lblResultTitle.Text = string.Format(ResourceManager.GetString("lblCandidateCVResultTitle"), "1 - " + result.Count, result.Count, Request.QueryString["keyword"]);
            }
            else//lblNoCandidateCVFound 
                lblResultTitle.Text = string.Format(ResourceManager.GetString("lblNoCandidateCVFound"), Request.QueryString["keyword"]);

        }
    }   

    private List<Lucene.Net.Documents.Document> SearchCandidateCV(string keyword)
    {
        List<Lucene.Net.Documents.Document> list = new List<Lucene.Net.Documents.Document>();

        string indexFileLocation = WebConfig.DocumentIndexPhysicalPath;
        if (!Common.IsIndexExists(indexFileLocation))
            return new List<Lucene.Net.Documents.Document>();
        Lucene.Net.Store.Directory dir = Lucene.Net.Store.FSDirectory.GetDirectory(indexFileLocation, false);

        //create an index searcher that will perform the search
        Lucene.Net.Search.IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(dir);
        try
        {
            //build a query object
            //Lucene.Net.Index.Term searchTerm = new Lucene.Net.Index.Term("content", keyword.ToLower());
            //Lucene.Net.Search.Query query = new Lucene.Net.Search.TermQuery(searchTerm);
            //NGA :
            //Lucene.Net.QueryParsers.QueryParser parse = new Lucene.Net.QueryParsers.QueryParser(keyword, new Lucene.Net.Analysis.WhitespaceAnalyzer());
            //Lucene.Net.Search.Query query = parse.Query("content");
            //Lucene.Net.Search.PhraseQuery query = new Lucene.Net.QueryParsers.QueryParser() .Parse("content:" + keyword.ToLower());

            //HUNG :             
            QueryParser queryParser = new QueryParser("content", new WhitespaceAnalyzer());
            Query query = queryParser.Parse(keyword); 

            //execute the query
            Lucene.Net.Search.Hits hits = searcher.Search(query);

            for (int i = 0; i < hits.Length(); i++)
            {
                Lucene.Net.Documents.Document doc = hits.Doc(i);
                list.Add(doc);
            }
        }        
        finally
        {
            searcher.Close();
            dir.Close();
        }
        return list;
    }

    
    #endregion

    #region Grid's Events
    protected void OnGridCandidateDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridCandidateDocument.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void OnGridCandidateDocument_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Lucene.Net.Documents.Document doc = e.Row.DataItem as Lucene.Net.Documents.Document;
            if(doc != null)
            {
                HyperLink lnkCandidateID = (HyperLink)e.Row.FindControl("lnkCandidateID");
                if(lnkCandidateID != null)
                {
                    Candidate candidate = new CandidateRepository().FindOne(new Candidate(Convert.ToInt32(doc.Get("candidateID"))));
                    if (candidate != null)
                    {
                        lnkCandidateID.Text = string.Format("{0} {1}", candidate.FirstName, candidate.LastName);
                        lnkCandidateID.NavigateUrl = string.Format("~/CandidateProfile.aspx?CandidateID={0}&mode=edit", candidate.CandidateId);
                    }
                }
                Literal lblCandidateContent = (Literal)e.Row.FindControl("lblCandidateContent");
                if (lblCandidateContent != null)
                {
                    lblCandidateContent.Text = Common.FirstWords(doc.Get("content"), 50);
                }

                HyperLink lnkCandidateCV = (HyperLink)e.Row.FindControl("lnkCandidateCV");
                if (lnkCandidateCV != null)
                {
                    lnkCandidateCV.Text = doc.Get("path");
                    lnkCandidateCV.NavigateUrl = doc.Get("path");
                }
            }
        }
    }
    #endregion

    protected void OnButtonSearchCandidateCV_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtKeyword.Text.Trim()))
        {
            Response.Redirect("~/CandidateCVSearchResult.aspx?keyword=" + txtKeyword.Text.Trim(), true);
        }
    }
}
