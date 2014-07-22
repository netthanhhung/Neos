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
using Neos.Data;
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class ChooseCandidatePopup : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        else if (!IsPostBack)
        {
            FillLabelLanguage();

            IList<Candidate> canList = new List<Candidate>();
            gridCandidate.DataSource = canList;
            gridCandidate.DataBind();
            txtCanLastName.Focus();
        }
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("chooseCandidatePopupTitle");
        lblCanLastName.Text = ResourceManager.GetString("lblCanLastName");
        lblCanFirstName.Text = ResourceManager.GetString("lblCanFirstName");        
        btnSearchCandidates.Text = ResourceManager.GetString("btnSearchCandidates");
        btnOK.Text = ResourceManager.GetString("okText");
        btnCancel.Text = ResourceManager.GetString("cancelText");

        gridCandidate.Columns[0].HeaderText = ResourceManager.GetString("columnLastNameCandidateGrid");
        gridCandidate.Columns[1].HeaderText = ResourceManager.GetString("columnFirstNameCandidateGrid");
        gridCandidate.Columns[2].HeaderText = ResourceManager.GetString("columnStatusCandidateGrid");
        gridCandidate.Columns[3].HeaderText = ResourceManager.GetString("columnContactInfoCandidateGrid");
        gridCandidate.Columns[4].HeaderText = ResourceManager.GetString("columnLastModifCandidateGrid");
    }

    protected void OnCandidateSearchClicked(object sender, EventArgs e)
    {        
        BindCandidateGrid(null);
        gridCandidate.DataBind();
        if(gridCandidate.Items.Count > 0)
            gridCandidate.Items[0].Selected = true;
    }

    private IList<Candidate> FillContactInfo(IList<Candidate> candidateList)
    {
        foreach (Candidate item in candidateList)
        {
            item.ContactInfo = string.Empty;
            item.ContactInfoFull = string.Empty;
            IList<CandidateTelephone> contactList =
                new CandidateTelephoneRepository().GetCandidateTelephonesByCandidateID(item.CandidateId);
            foreach (CandidateTelephone contact in contactList)
            {
                if (!string.IsNullOrEmpty(contact.Type))
                {
                    if (contact.Type == "T")
                    {
                        item.ContactInfoFull += ResourceManager.GetString("candidateContactPhone")
                            + ": " + contact.PhoneMail + "<br />";
                        if (item.ContactInfo == string.Empty)
                            item.ContactInfo = ResourceManager.GetString("candidateContactPhone") + ": " + contact.PhoneMail;
                    }
                    else if (contact.Type == "F")
                    {
                        item.ContactInfoFull += ResourceManager.GetString("candidateContactFax")
                            + ": " + contact.PhoneMail + "<br />";
                        if (item.ContactInfo == string.Empty)
                            item.ContactInfo = ResourceManager.GetString("candidateContactFax") + ": " + contact.PhoneMail;
                    }
                    else if (contact.Type == "G")
                    {
                        item.ContactInfoFull += ResourceManager.GetString("candidateContactMobile")
                            + ": " + contact.PhoneMail + "<br />";
                        if (item.ContactInfo == string.Empty)
                            item.ContactInfo = ResourceManager.GetString("candidateContactMobile") + ": " + contact.PhoneMail;
                    }
                    else if (contact.Type == "E")
                    {
                        item.ContactInfoFull += ResourceManager.GetString("candidateContactEmail")
                            + ": " + contact.PhoneMail + "<br />";
                        if (item.ContactInfo == string.Empty)
                            item.ContactInfo = ResourceManager.GetString("candidateContactEmail") + ": " + contact.PhoneMail;
                    }
                }
            }
        }
        return candidateList;
    }

    protected void OnBtnOkClicked(object sender, EventArgs e)
    {
        if (gridCandidate.SelectedValue != null)
        {
            int candidateID = (int)gridCandidate.SelectedValue;
            Candidate selectedCandidate = new CandidateRepository().FindOne(new Candidate(candidateID));            
            string argument = selectedCandidate.CandidateId.ToString() + "/" 
                + selectedCandidate.LastName + " " + selectedCandidate.FirstName;
            string script = "<script type=\"text/javascript\">";
            script += " OnBtnOkClientClicked(\"" + argument + "\");";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
        }
    }

    protected void OnGridCandidatePageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridCandidate.CurrentPageIndex = e.NewPageIndex;
        BindCandidateGrid(null);
        gridCandidate.DataBind();
    }

    private void BindCandidateGrid(GridSortCommandEventArgs sortEventArgs)
    {
        if (!string.IsNullOrEmpty(txtCanLastName.Text) || !string.IsNullOrEmpty(txtCanFirstName.Text))
        {
            int pageSize = 10;
            int pageNumber = gridCandidate.CurrentPageIndex + 1;
            string sortExpress = string.Empty;
            string sortExpressInvert = string.Empty;
            foreach (GridSortExpression item in gridCandidate.MasterTableView.SortExpressions)
            {
                GridSortOrder newSortOrder = item.SortOrder;
                if (sortEventArgs != null && item.FieldName == sortEventArgs.SortExpression)
                {
                    newSortOrder = sortEventArgs.NewSortOrder;
                }

                if (!string.IsNullOrEmpty(sortExpress) && newSortOrder != GridSortOrder.None)
                {
                    sortExpress += ", ";
                    sortExpressInvert += ", ";
                }
                if (newSortOrder == GridSortOrder.Ascending)
                {
                    sortExpress += item.FieldName + " ASC";
                    sortExpressInvert += item.FieldName + " DESC";
                }
                else if (newSortOrder == GridSortOrder.Descending)
                {
                    sortExpress += item.FieldName + " DESC";
                    sortExpressInvert += item.FieldName + " ASC";
                }
            }

            if (sortEventArgs != null && !sortExpress.Contains(sortEventArgs.SortExpression))
            {
                if (!string.IsNullOrEmpty(sortExpress) && sortEventArgs.NewSortOrder != GridSortOrder.None)
                {
                    sortExpress += ", ";
                    sortExpressInvert += ", ";
                }
                if (sortEventArgs.NewSortOrder == GridSortOrder.Ascending)
                {
                    sortExpress += sortEventArgs.SortExpression + " ASC";
                    sortExpressInvert += sortEventArgs.SortExpression + " DESC";
                }
                else if (sortEventArgs.NewSortOrder == GridSortOrder.Descending)
                {
                    sortExpress += sortEventArgs.SortExpression + " DESC";
                    sortExpressInvert += sortEventArgs.SortExpression + " ASC";
                }
            }

            if (sortExpress != string.Empty)
            {
                if (sortExpress.Contains("LastName"))
                {
                    sortExpress = sortExpress.Replace("LastName", "Nom");
                    sortExpressInvert = sortExpressInvert.Replace("LastName", "Nom");
                }
                if (sortExpress.Contains("FirstName"))
                {
                    sortExpress = sortExpress.Replace("FirstName", "Prenom");
                    sortExpressInvert = sortExpressInvert.Replace("FirstName", "Prenom");
                }
                if (sortExpress.Contains("Inactive"))
                {
                    sortExpress = sortExpress.Replace("Inactive", "inactif");
                    sortExpressInvert = sortExpressInvert.Replace("Inactive", "inactif");
                }
                if (sortExpress.Contains("LastModifDate"))
                {
                    sortExpress = sortExpress.Replace("LastModifDate", "DateModif");
                    sortExpressInvert = sortExpressInvert.Replace("LastModifDate", "DateModif");
                }
            }
            else
            {
                sortExpress = "Nom ASC";
                sortExpressInvert = "Nom DESC";
            }


            gridCandidate.VirtualItemCount = NeosDAO.CountSearchCandidates(txtCanLastName.Text, txtCanFirstName.Text);
            IList<Candidate> candidateList = NeosDAO.SearchCandidatesByPage(
                txtCanLastName.Text, txtCanFirstName.Text, pageSize, pageNumber, sortExpress, sortExpressInvert);
            candidateList = FillContactInfo(candidateList);
            gridCandidate.DataSource = candidateList;
        }
        else
        {
            IList<Candidate> canList = new List<Candidate>();
            gridCandidate.DataSource = canList;
        }
    }

    protected void OnGridCandidateNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindCandidateGrid(null);
    }

    protected void OnGridCandidateSortCommand(object source, GridSortCommandEventArgs e)
    {
        BindCandidateGrid(e);
    }
}
