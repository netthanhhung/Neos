using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using System.Web.UI.MobileControls;
using Telerik.Web.UI;
using Neos.Data;
using eWorld.UI;


public partial class Candidates : System.Web.UI.Page 
{
    public int pageSize
    {
        get
        {
            return RadCandidateGrid.PageSize;
        }
        set
        {
            RadCandidateGrid.PageSize = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["type"]) &&
            Request.QueryString["type"] == "AdvancedSearch")
        {
            lnkBackToAdvancedSearch.Visible = true;
        }
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        else if (!IsPostBack)
        {
            FillLabelLanguage();
            InitControls();
            BindGridData(null);
            RadCandidateGrid.DataBind();
            SessionManager.BackUrl = Request.Url.ToString();
        }
    }

    private void InitControls()
    {
        HttpCookie candidateGridPageSizeCookie = Request.Cookies.Get("candgrdps");
        if (candidateGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(candidateGridPageSizeCookie.Value))
                RadCandidateGrid.PageSize = Convert.ToInt32(candidateGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(candidateGridPageSizeCookie.Value) : RadCandidateGrid.PageSize;
        }
    }
    
    private void BindGridData(GridSortCommandEventArgs sortEventArgs)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
        {
            int id = int.Parse(Request.QueryString["CandidateId"]);
            Candidate candidate = NeosDAO.GetCandidateById(id);
            List<Candidate> candidateList = new List<Candidate>();
            candidateList.Add(candidate);
            RadCandidateGrid.DataSource = candidateList;
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["lastname"]) 
                || !string.IsNullOrEmpty(Request.QueryString["type"]))
        {
            //int pageSize = 20;
            int pageNumber = RadCandidateGrid.CurrentPageIndex + 1;
            string sortExpress = string.Empty;
            string sortExpressInvert = string.Empty;
            foreach (GridSortExpression item in RadCandidateGrid.MasterTableView.SortExpressions)
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

            if (!string.IsNullOrEmpty(sortExpress))
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
            if (!string.IsNullOrEmpty(Request.QueryString["lastname"]))
            {
                string name = Request.QueryString["lastname"];
                SessionManager.LastNameSearchCriteria = name;

                RadCandidateGrid.VirtualItemCount = new CandidateRepository().CountSearchCandidatesOnName(name); 
                IList<Candidate> candidateList = new CandidateRepository().SearchCandidatesOnName(name, pageSize, pageNumber, sortExpress, sortExpressInvert);
                
                candidateList = FillContactInfo(candidateList);
                RadCandidateGrid.DataSource = candidateList;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["type"])
                    && Request.QueryString["type"] == "AdvancedSearch"
                    && SessionManager.CandidateSearchCriteria != null)
            {
                RadCandidateGrid.VirtualItemCount = NeosDAO.CountAdvancedSearchCandidates(SessionManager.CandidateSearchCriteria);
                IList<Candidate> candidateList = NeosDAO.AdvancedSearchCandidatesByPage(
                    SessionManager.CandidateSearchCriteria, pageSize, pageNumber, sortExpress, sortExpressInvert);
                candidateList = FillContactInfo(candidateList);
                RadCandidateGrid.DataSource = candidateList;
            }
            //RadCandidateGrid.SortCommand += new GridSortCommandEventHandler(OnRadCandidateGridSortCommand);                 
        }
        else
        {
            IList<Candidate> candidateList = new List<Candidate>();
            RadCandidateGrid.DataSource = candidateList;
        }
        
    }

    protected void OnRadCandidateGridSortCommand(object source, GridSortCommandEventArgs e)
    {
        BindGridData(e);
    }

    protected void OnRadCandidateGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie candidateGridPageSizeCookie = new HttpCookie("candgrdps");
        candidateGridPageSizeCookie.Expires.AddDays(30);
        candidateGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(candidateGridPageSizeCookie);
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

    private void FillLabelLanguage()
    {
        lblCandidateTitle.Text = ResourceManager.GetString("candidatesPanelBarItemText");
        RadCandidateGrid.Columns[0].HeaderText = ResourceManager.GetString("columnLastNameCandidateGrid");
        RadCandidateGrid.Columns[1].HeaderText = ResourceManager.GetString("columnFirstNameCandidateGrid");
        RadCandidateGrid.Columns[2].HeaderText = ResourceManager.GetString("columnStatusCandidateGrid");
        RadCandidateGrid.Columns[3].HeaderText = ResourceManager.GetString("columnContactInfoCandidateGrid");
        RadCandidateGrid.Columns[4].HeaderText = ResourceManager.GetString("columnLastModifCandidateGrid");
        
        lnkAddNewCandidate.Text = ResourceManager.GetString("lnkAddNewCandidate");

    }

    protected void OnRadCandidateGridPageIndexChangeds(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        RadCandidateGrid.CurrentPageIndex = e.NewPageIndex;
        BindGridData(null);
        RadCandidateGrid.DataBind();
    }

    protected void OnRadCandidateGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData(null);
        
    }

    protected void OnRadCandidateGridItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Candidate cand = (Candidate)e.Item.DataItem;
            if (cand != null)
            {
                //CollapsablePanel pnlInfo = (CollapsablePanel)e.Item.FindControl("pnlInfo");
                //if (pnlInfo != null)
                //{
                //    pnlInfo.TitleText = !string.IsNullOrEmpty(cand.Address) ? string.Format("{0}: {1}", ResourceManager.GetString("lblCanAddress"), cand.Address) : string.Empty;
                //    pnlInfo.Collapsable = true;
                //}
                RadComboBox ddlContactInfo = (RadComboBox)e.Item.FindControl("ddlContactInfo");
                if (ddlContactInfo != null)
                {
                    ddlContactInfo.ItemDataBound += new RadComboBoxItemEventHandler(OnDropDownContactInfo_ItemDataBound);
                    //ddlContactInfo.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(OnDropDownContactInfo_SelectedIndexChanged);
                    List<string> contactInfoList = new List<string>();
                    if(!string.IsNullOrEmpty(cand.Address))
                        contactInfoList.Add(ResourceManager.GetString("lblCanAddress") + ": " + cand.Address);
                    if (!string.IsNullOrEmpty(cand.ContactInfoFull))
                    {

                        contactInfoList.AddRange(cand.ContactInfoFull.Split(new string[] { "<br />" }, StringSplitOptions.None));
                    }

                    ddlContactInfo.DataSource = contactInfoList;
                    ddlContactInfo.DataBind();
                }              
            }
        }
    }

    protected void OnDropDownContactInfo_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        Literal lblContactInfo = (Literal)e.Item.FindControl("lblContactInfo");
        if (lblContactInfo != null)
        {
            lblContactInfo.Text += e.Item.DataItem as string;
        }
    }

    protected void OnCandidateClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string id = lnkItem.CommandArgument;
        Response.Redirect("~/CandidateProfile.aspx?CandidateID=" + id + "&mode=edit&backurl=visible");
    }

    protected void OnAddNewCandidateClicked(object sender, EventArgs e)
    {
        SessionManager.NewCandidateTelephoneList = new List<CandidateTelephone>();
        Response.Redirect("~/CandidateProfile.aspx?&mode=edit&backurl=visible");
    }

    protected void OnBackToAdvancedSearchClicked(object sender, EventArgs e)
    {
        Response.Redirect("~/CandidateAdvancedSearch.aspx?type=AdvancedSearch");
    }

    protected void OnMyAjaxManager_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        CandidateAjaxManager.AjaxSettings.AddAjaxSetting(CandidateAjaxManager, RadCandidateGrid);
        if (e.Argument.IndexOf("DeletedSelectedCandidate") > -1)
        {
            if (RadCandidateGrid.SelectedItems.Count == 1)
            {
                CandidateRepository candRepo = new CandidateRepository();
                candRepo.Delete(new Candidate(GetSelectedCandidateID()));
                RadCandidateGrid.Rebind();
            }
        }
        else if (e.Argument.IndexOf("OpenSelectedCandidate") > -1)
        {
            if (RadCandidateGrid.SelectedItems.Count == 1)
            {
                int candidateID = GetSelectedCandidateID();
                if (candidateID > -1)
                    Response.Redirect("~/CandidateProfile.aspx?CandidateID=" + candidateID + "&mode=edit&backurl=visible", true);
            }
        }
        else if (e.Argument.IndexOf("ViewCandidateActions") > -1)
        {
            if (RadCandidateGrid.SelectedItems.Count == 1)
            {
                int candidateID = GetSelectedCandidateID();
                if (candidateID > -1)
                    Response.Redirect(string.Format("~/CandidateProfile.aspx?CandidateID={0}&tab=action&mode=edit&backurl=visible", candidateID, true));
            }
        }
    }

    private int GetSelectedCandidateID()
    {
        GridDataItem selectedItem = RadCandidateGrid.SelectedItems[0] as GridDataItem;
        TableCell candidateIDCell = selectedItem["CandidateId"];
        if (!string.IsNullOrEmpty(candidateIDCell.Text))
            return Convert.ToInt32(candidateIDCell.Text);
        return -1;
    }
}

