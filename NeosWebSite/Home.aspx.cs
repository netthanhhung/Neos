using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using Neos.Data;
using System.Text;
using System.Xml;

public partial class Home : System.Web.UI.Page
{
    PageStatePersister _pers;
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
            if (string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                Response.Redirect("~/Login.aspx?CandidateId=" + Request.QueryString["CandidateId"], true);
            }
        }

        InitControls();
        if (!IsPostBack)
        {
            FillLabelLanguage();
            CheckPermission();
            BindData();
            if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            {
                radPaneContent.ContentUrl = "~/CandidateProfile.aspx?CandidateId=" + Request.QueryString["CandidateId"] + "&mode=edit";
            }

            string script1 = "<script type='text/javascript'>";
            script1 += "SetSessionKeepAliveTimer();";
            script1 += "</script>";

            if (!ClientScript.IsClientScriptBlockRegistered("setSessionKeepAliveTimer"))
                ClientScript.RegisterStartupScript(this.GetType(), "setSessionKeepAliveTimer", script1);

        }

        string script = "<script type='text/javascript'>";
        script += "updateMapSize();";
        script += "</script>";

        if (!ClientScript.IsClientScriptBlockRegistered("resizeMap"))
            ClientScript.RegisterStartupScript(this.GetType(), "resizeMap", script);
    }



    #region Common
    private void InitControls()
    {
        int count = 0;
        if (SessionManager.CurrentUser != null)
        {
            count = new UserMessageRepository().CountUnreadJobRemindMessagesToday(SessionManager.CurrentUser.UserID);
            lnkUnreadMessage.Text = string.Format(ResourceManager.GetString("lblUnreadMessage"), count);
            if (count > 0)
            {
                lnkUnreadMessage.Font.Bold = true;
            }
            else
            {
                lnkUnreadMessage.Font.Bold = false;
                lnkUnreadMessage.Text = string.Format(ResourceManager.GetString("lblUnreadMessage"), 0);
            }
        }
    }

    private void CheckPermission()
    {
        bool viewInvoicingPermission = false;
        bool viewAdministration = false;
        if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.Permissions != null)
        {
            bool haveAllActionPermission = false;            
            foreach (ParamUserPermission item in SessionManager.CurrentUser.Permissions)
            {
                if (item.PermissionCode == "VIEWALLACTIONS")
                {
                    haveAllActionPermission = true;                    
                }
                if (item.PermissionCode == "INVOICING")
                {
                    viewInvoicingPermission = true;                    
                }
                if (item.PermissionCode == "ADMINISTRATION")
                {
                    viewAdministration = true;
                }
            }

            divAllAction.Visible = haveAllActionPermission;
            ddlResponsibleAction.Enabled = haveAllActionPermission;
        }
        else
        {
            divAllAction.Visible = false;
            ddlResponsibleAction.Enabled = false;
        }

        SectionPanel.FindItemByValue("invoicing").Enabled = viewInvoicingPermission;
        SectionPanel.FindItemByValue("administration").Enabled = viewAdministration;
    }

    private void BindData()
    {
        BindLast5ViewedCandidate();
        BindLast5ViewedCompany();

        //fill Job sections
        BindProfileData();
        BindFunctionData();
        BindLocationData();
        BindResponsible();
        //Action
        ddlTypeAction.DataValueField = "ParamActionID";
        ddlTypeAction.DataTextField = "Label";
        IList<ParamTypeAction> list = new List<ParamTypeAction>();
        list = new ParamTypeActionRepository().FindAll();
        list.Insert(0, new ParamTypeAction(-1, string.Empty));
        ddlTypeAction.DataSource = list;
        ddlTypeAction.DataBind();

        ParamUserRepository paramUserRepo = new ParamUserRepository();
        ddlResponsibleAction.DataValueField = "UserID";
        ddlResponsibleAction.DataTextField = "LastName";
        ddlResponsibleAction.DataSource = paramUserRepo.GetAllUser(true);
        ddlResponsibleAction.DataBind();

        if (SessionManager.CurrentUser != null)
        {
            ddlResponsibleAction.SelectedValue = SessionManager.CurrentUser.UserID;
        }

        //Invoice
        ddlInvoiceType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("allText"), ""));
        ddlInvoiceType.Items.Add(new RadComboBoxItem("Invoice", "I"));
        ddlInvoiceType.Items.Add(new RadComboBoxItem("Credite note", "C"));
        ddlInvoiceType.SelectedValue = "I";

        ddlFiscalYear.Items.Add(new RadComboBoxItem(ResourceManager.GetString("allText"), ""));
        for (int year = DateTime.Today.Year; year >= 2000; year--)
        {
            ddlFiscalYear.Items.Add(new RadComboBoxItem(year.ToString(), year.ToString()));
        }
        ddlFiscalYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    private void BindLast5ViewedCandidate()
    {
        //Last 5 viewed candidates.
        XmlDocument doc = new XmlDocument();
        IList<Candidate> last5Candidates = new List<Candidate>();
        try
        {
            doc.Load(Server.MapPath("~/App_Data/LastViewedCandidates.xml"));

            XmlElement rootNode = doc.DocumentElement;
            if (rootNode != null)
            {
                XmlNode userNode = rootNode.SelectSingleNode("User[@id='" + SessionManager.CurrentUser.UserID + "']");
                if (userNode != null) //user node existed
                {
                    if (userNode.Attributes["viewed-candidates"] != null && !string.IsNullOrEmpty(userNode.Attributes["viewed-candidates"].Value))
                    {
                        List<string> list = new List<string>(userNode.Attributes["viewed-candidates"].Value.Split('&'));
                        foreach (string candidateID in list)
                        {
                            if (!string.IsNullOrEmpty(candidateID))
                            {
                                Candidate cand = new CandidateRepository().FindOne(new Candidate(Convert.ToInt32(candidateID)));
                                if (cand != null)
                                {
                                    last5Candidates.Add(cand);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        #region commented
        /*
        if (lastCanCookie != null)
        {
            CandidateRepository canRepo = new CandidateRepository();
            string[] values = lastCanCookie.Values.ToString().Split('&');
            if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
            {
                string idList = string.Empty;
                for (int i = 0; i < values.Length; i++)
                {
                    string[] userAndCanIds = values[i].Split('=');
                    if (userAndCanIds[0] == SessionManager.CurrentUser.UserID.Trim())
                    {
                        idList = userAndCanIds[1];
                        break;
                    }
                }
                if (idList != string.Empty)
                {
                    string[] idArray = idList.Split('A');
                    for (int j = idArray.Length - 1; j >= 0; j--)
                    {
                        int id = int.Parse(idArray[j]);
                        Candidate can = canRepo.FindOne(new Candidate(id));
                        if (can != null)
                        {
                            last5Candidates.Add(can);
                        }
                    }
                }
            }
        }*/
        #endregion

        lastFiveCandidateList.DataSource = last5Candidates;
        lastFiveCandidateList.DataBind();
    }

    private void BindLast5ViewedCompany()
    {
        //Company
        //Last 5 viewed company.        
        XmlDocument doc = new XmlDocument();
        IList<Company> last5Companies = new List<Company>();
        try
        {
            doc.Load(Server.MapPath("~/App_Data/LastViewedCompanies.xml"));

            XmlElement rootNode = doc.DocumentElement;
            if (rootNode != null)
            {
                XmlNode userNode = rootNode.SelectSingleNode("User[@id='" + SessionManager.CurrentUser.UserID + "']");
                if (userNode != null) //user node existed
                {
                    if (userNode.Attributes["viewed-companies"] != null && !string.IsNullOrEmpty(userNode.Attributes["viewed-companies"].Value))
                    {
                        List<string> list = new List<string>(userNode.Attributes["viewed-companies"].Value.Split('&'));
                        foreach (string companyID in list)
                        {
                            if (!string.IsNullOrEmpty(companyID))
                            {
                                Company com = new CompanyRepository().FindOne(Convert.ToInt32(companyID));
                                if (com != null)
                                {
                                    last5Companies.Add(com);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        /*
        if (lastComCookie != null)
        {
            CompanyRepository canRepo = new CompanyRepository();
            string[] values = lastComCookie.Values.ToString().Split('&');
            if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
            {
                string idList = string.Empty;
                for (int i = 0; i < values.Length; i++)
                {
                    string[] userAndComIds = values[i].Split('=');
                    if (userAndComIds[0] == SessionManager.CurrentUser.UserID.Trim())
                    {
                        idList = userAndComIds[1];
                        break;
                    }
                }
                if (idList != string.Empty)
                {
                    string[] idArray = idList.Split('A');
                    for (int j = idArray.Length - 1; j >= 0; j--)
                    {
                        int id = int.Parse(idArray[j]);
                        Company com = canRepo.FindOne(new Company(id));
                        if (com != null)
                        {
                            last5Companies.Add(com);
                        }
                    }
                }
            }
        }
        */
        rptLastViewedCompanies.DataSource = last5Companies;
        rptLastViewedCompanies.DataBind();
    }

    protected void OnPanelBarItemClicked(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
    {
        RadPanelItem itemClicked = e.Item;
        if (itemClicked.Selected)
        {
            if (itemClicked.Level == 0)
            {
                switch (itemClicked.Value)
                {
                    case "candidates":
                        IList<Candidate> last5Candidates = NeosDAO.GetLastModifCandidates(5);
                        lastFiveCandidateList.DataSource = last5Candidates;
                        lastFiveCandidateList.DataBind();
                        radPaneContent.ContentUrl = "~/Candidates.aspx";
                        break;
                    case "companies":
                        radPaneContent.ContentUrl = "~/Companies.aspx";
                        break;
                    case "actions":

                        break;
                }
            }
            else
            {
            }
        }

    }

    private void FillLabelLanguage()
    {
        //Home item        

        lbnSignOut.Text = ResourceManager.GetString("lblLogout");
        //current user section        
        RadDockLogout.Title = ResourceManager.GetString("lblWelcome");
        Literal lblCurrentUser = RadDockLogout.ContentContainer.FindControl("lblCurrentUser") as Literal;
        if (lblCurrentUser != null)
            lblCurrentUser.Text = SessionManager.CurrentUser != null ? SessionManager.CurrentUser.LastName : "";
        LinkButton lbnLogout = RadDockLogout.ContentContainer.FindControl("lbnLogout") as LinkButton;
        if (lbnLogout != null)
        {
            lbnLogout.Text = ResourceManager.GetString("lblLogout");
        }
        SectionPanel.Items[0].Text = ResourceManager.GetString("candidatesPanelBarItemText");
        SectionPanel.Items[1].Text = ResourceManager.GetString("companiesPanelBarItemText");
        SectionPanel.Items[2].Text = ResourceManager.GetString("actionsPanelBarItemText");
        SectionPanel.Items[3].Text = ResourceManager.GetString("jobsPanelBarItemText");
        SectionPanel.Items[4].Text = ResourceManager.GetString("statisticsPanelBarItemText");
        SectionPanel.Items[5].Text = ResourceManager.GetString("administrationPanelBarItemText");
        SectionPanel.Items[6].Text = ResourceManager.GetString("invoicingPanelBarItemText");
        SectionPanel.Items[7].Text = ResourceManager.GetString("notificationPanelBarItemText");

        lblCandidatesHeaderPageView.Text = ResourceManager.GetString("candidatesPanelBarItemText");
        lblCompaniesHeaderPageView.Text = ResourceManager.GetString("companiesPanelBarItemText");
        lblActionsHeaderPageView.Text = ResourceManager.GetString("actionsPanelBarItemText");
        lblJobsHeaderPageView.Text = ResourceManager.GetString("jobsPanelBarItemText");
        lblstatisticsHeaderPageView.Text = ResourceManager.GetString("statisticsPanelBarItemText");
        lblAdministrationsHeaderPageView.Text = ResourceManager.GetString("administrationPanelBarItemText");
        lblInvoicingHeaderPageView.Text = ResourceManager.GetString("invoicingPanelBarItemText");

        //candidate panel bar
        btnSearchCandidates.Text = ResourceManager.GetString("btnSearchCandidates");
        lnkCandidateAdvancedSearch.Text = ResourceManager.GetString("lnkCandidateAdvancedSearch");
        litLastFiveCandidate.Text = ResourceManager.GetString("litLastFiveCandidate");
        lblSearchCV.Text = ResourceManager.GetString("lblSearchCV");
        btnSearchCV.Text = ResourceManager.GetString("btnSearch");
        //company panel bar 
        btnCompanySearch.Text = ResourceManager.GetString("btnCompanySearch");

        rdoListCompanyType.Items[0].Text = ResourceManager.GetString("lnkAllCompanies");
        rdoListCompanyType.Items[1].Text = ResourceManager.GetString("lnkCustomerCompany");
        rdoListCompanyType.Items[2].Text = ResourceManager.GetString("lnkProspectCompany");
        rdoListCompanyType.Items[3].Text = ResourceManager.GetString("lnkDisabledCompany");
        
        lblLastViewedCompanies.Text = ResourceManager.GetString("lblLastViewedCompanies");

        //Action panel bar
        lblActiveActionBoth.Text = ResourceManager.GetString("bothText");
        lblActiveActionNo.Text = ResourceManager.GetString("noText");
        lblActiveActionYes.Text = ResourceManager.GetString("yesText");
        lblActiveActionSearch.Text = ResourceManager.GetString("columnActiveActionCan");
        lblDateBetweenAction.Text = ResourceManager.GetString("dateText") + " "
            + ResourceManager.GetString("betweenText");
        lblCandidateActionSearch.Text = ResourceManager.GetString("columnCandidateActionCan");
        lblCompanyActionSearch.Text = ResourceManager.GetString("columnCompanyActionCan");
        lblTypeActionSearch.Text = ResourceManager.GetString("columnTypeActionCan");
        lblDescriptionActionSearch.Text = ResourceManager.GetString("columnDescriptionActionCan");
        lblResponsibleActionSearch.Text = ResourceManager.GetString("columnResponsibleActionCan");
        btnActionSearch.Text = ResourceManager.GetString("btnSearchCandidates");
        lblMyActions.Text = ResourceManager.GetString("lblMyActions");
        lnkMyActiveThisWeek.Text = ResourceManager.GetString("lnkMyActiveThisWeek");
        lnkMyActive.Text = ResourceManager.GetString("lnkMyActive");
        lnkMyInactive.Text = ResourceManager.GetString("lnkMyInactive");
        lnkMyActions.Text = ResourceManager.GetString("lnkMyActions");
        lblAllAction.Text = ResourceManager.GetString("lblAllAction");
        lnkActiveAction.Text = ResourceManager.GetString("lnkActiveAction");
        lnkInactiveAction.Text = ResourceManager.GetString("lnkInactiveAction");
        lnkAllActions.Text = ResourceManager.GetString("lnkAllActions");

        //Job section
        lblTitle.Text = ResourceManager.GetString("lblJobPanelTitle");
        lblCreatedDate.Text = ResourceManager.GetString("lblJobPanelCreatedDate");
        lblActivation.Text = ResourceManager.GetString("lblJobPanelActivatedDate");
        lblExpiredDate.Text = ResourceManager.GetString("lblJobPanelExpiredDate");
        lblProfile.Text = ResourceManager.GetString("lblJobPanelProfile");
        lblFunctionFam.Text = ResourceManager.GetString("lblJobPanelFunctionFam");
        lblLocation.Text = ResourceManager.GetString("lblJobPanelLocation");
        lblResponsible.Text = ResourceManager.GetString("lblJobPanelResponsible");
        lblCompany.Text = ResourceManager.GetString("lblJobPanelCompanyName");
        lblActiveJob.Text = ResourceManager.GetString("lblJobPanelIsActive");
        lblActiveJobYes.Text = ResourceManager.GetString("yesText");
        lblActiveJobNo.Text = ResourceManager.GetString("noText");
        lblActiveJobBoth.Text = ResourceManager.GetString("bothText");
        btnJobSearch.Text = ResourceManager.GetString("lblJobPanelSearch");
        lnkActiveJobs.Text = ResourceManager.GetString("lblJobPanelActive");
        lnkInactiveJobs.Text = ResourceManager.GetString("lblJobPanelInActive");
        lnkAllJobs.Text = ResourceManager.GetString("lblJobPanelAll");        
        //Statistics section
        hypGeneralStatistics.Text = ResourceManager.GetString("hypGeneralStatistics");
        hypStudyLevelStatistics.Text = ResourceManager.GetString("hypStudyLevelStatistics");
        hypCandidateInscription.Text = ResourceManager.GetString("hypCandidateInscription");
        hypCandidateLocation.Text = ResourceManager.GetString("hypCandidateLocation");

        //Invoicing section
        lblInvoicingHeaderPageView.Text = ResourceManager.GetString("invoicingPanelBarItemText");
        lblInvoiceNumber.Text = ResourceManager.GetString("lblInvoiceNumber");
        lblInvoiceNumberTo.Text = ResourceManager.GetString("toText");
        lblFiscalYear.Text = ResourceManager.GetString("lblFiscalYear");
        lblInvoiceDate.Text = ResourceManager.GetString("lblInvoiceDate");
        lblInvoiceDateTo.Text = ResourceManager.GetString("toText");
        lblInvoiceType.Text = ResourceManager.GetString("lblInvoiceType");
        lblInvoiceCustomer.Text = ResourceManager.GetString("lblInvoiceCustomer");
        btnInvoiceSearch.Text = ResourceManager.GetString("btnSearchCandidates");
        hypUnpaidInvoice.Text = ResourceManager.GetString("hypUnpaidInvoice");
        hypFutureInvoice.Text = ResourceManager.GetString("hypFutureInvoice");
        hypTurnover.Text = ResourceManager.GetString("hypTurnover");
        
  

        //tool bar
        ConfirmDeleteCandidate.Value = ResourceManager.GetString("confirmDeleteCandidate");
        ConfirmDeleteCompany.Value = ResourceManager.GetString("confirmDeleteCompany");
        ConfirmDeleteAction.Value = ResourceManager.GetString("confirmDeleteAction");
        ConfirmDeleteJob.Value = ResourceManager.GetString("confirmDeleteJob");
        ConfirmDeleteInvoice.Value = ResourceManager.GetString("confirmDeleteInvoice");
        //ToolBar - CandidateToolBar
        CandidateToolBar.FindItemByValue("newcandidate").Text = ResourceManager.GetString("toolBarNewCandidate");
        CandidateToolBar.FindItemByValue("newcandidateDefault").Text = ResourceManager.GetString("toolBarNew");
        CandidateToolBar.FindItemByValue("newcompany").Text = ResourceManager.GetString("toolBarNewCompany");
        CandidateToolBar.FindItemByValue("newaction").Text = ResourceManager.GetString("toolBarNewAction");
        CandidateToolBar.FindItemByValue("newjob").Text = ResourceManager.GetString("toolBarNewJob");
        CandidateToolBar.FindItemByValue("newinvoicing").Text = ResourceManager.GetString("toolBarNewInvoice");

        CandidateToolBar.FindItemByValue("opencandidate").ToolTip = ResourceManager.GetString("toolBarOpen");
        CandidateToolBar.FindItemByValue("deletecandidate").ToolTip = ResourceManager.GetString("toolBarDelete");

        CandidateToolBar.FindItemByValue("vieweditcandidate").Text = ResourceManager.GetString("toolBarViewEdit");
        CandidateToolBar.FindItemByValue("vieweditcandidate").ToolTip = ResourceManager.GetString("toolBarViewEditChangeMode");

        CandidateToolBar.FindItemByValue("savecandidate").ToolTip = ResourceManager.GetString("toolBarSave");
        CandidateToolBar.FindItemByValue("advancesearch").ToolTip = ResourceManager.GetString("toolBarAdvanceSearch");
        CandidateToolBar.FindItemByValue("viewcandidateactions").ToolTip = ResourceManager.GetString("toolBarActions");
        CandidateToolBar.FindItemByValue("addcandidateactions").ToolTip = ResourceManager.GetString("toolBarAddAction");
        //ToolBar - CompanyToolBar
        CompanyToolBar.FindItemByValue("newcandidate").Text = ResourceManager.GetString("toolBarNewCandidate");
        CompanyToolBar.FindItemByValue("newcompanyDefault").Text = ResourceManager.GetString("toolBarNew");
        CompanyToolBar.FindItemByValue("newcompany").Text = ResourceManager.GetString("toolBarNewCompany");
        CompanyToolBar.FindItemByValue("newaction").Text = ResourceManager.GetString("toolBarNewAction");
        CompanyToolBar.FindItemByValue("newjob").Text = ResourceManager.GetString("toolBarNewJob");
        CompanyToolBar.FindItemByValue("newinvoicing").Text = ResourceManager.GetString("toolBarNewInvoice");

        CompanyToolBar.FindItemByValue("opencompany").ToolTip = ResourceManager.GetString("toolBarOpen");
        CompanyToolBar.FindItemByValue("deletecompany").ToolTip = ResourceManager.GetString("toolBarDelete");

        CompanyToolBar.FindItemByValue("vieweditcompany").Text = ResourceManager.GetString("toolBarViewEdit");
        CompanyToolBar.FindItemByValue("vieweditcompany").ToolTip = ResourceManager.GetString("toolBarViewEditChangeMode");
        CompanyToolBar.FindItemByValue("savecompany").ToolTip = ResourceManager.GetString("toolBarSave");

        CompanyToolBar.FindItemByValue("viewcompanyactions").Text = ResourceManager.GetString("toolBarActions");
        CompanyToolBar.FindItemByValue("addcompanyaction").Text = ResourceManager.GetString("toolBarAddAction");

        CompanyToolBar.FindItemByValue("viewcompanyjobs").Text = ResourceManager.GetString("toolBarJobs");
        CompanyToolBar.FindItemByValue("addcompanyjobs").Text = ResourceManager.GetString("toolBarAddJob");

        CompanyToolBar.FindItemByValue("viewcompanyinvoices").Text = ResourceManager.GetString("toolBarInvoices");
        CompanyToolBar.FindItemByValue("addcompanyinvoice").Text = ResourceManager.GetString("toolBarAddInvoice");

        //ToolBar - ActionToolBar
        ActionToolBar.FindItemByValue("newcandidate").Text = ResourceManager.GetString("toolBarNewCandidate");
        ActionToolBar.FindItemByValue("newactionDefault").Text = ResourceManager.GetString("toolBarNew");
        ActionToolBar.FindItemByValue("newcompany").Text = ResourceManager.GetString("toolBarNewCompany");
        ActionToolBar.FindItemByValue("newaction").Text = ResourceManager.GetString("toolBarNewAction");
        ActionToolBar.FindItemByValue("newjob").Text = ResourceManager.GetString("toolBarNewJob");
        ActionToolBar.FindItemByValue("newinvoicing").Text = ResourceManager.GetString("toolBarNewInvoice");

        ActionToolBar.FindItemByValue("openaction").ToolTip = ResourceManager.GetString("toolBarOpen");
        ActionToolBar.FindItemByValue("deleteaction").ToolTip = ResourceManager.GetString("toolBarDelete");

        ActionToolBar.FindItemByValue("vieweditaction").Text = ResourceManager.GetString("toolBarViewEdit");
        ActionToolBar.FindItemByValue("vieweditaction").ToolTip = ResourceManager.GetString("toolBarViewEditChangeMode");
        ActionToolBar.FindItemByValue("saveaction").ToolTip = ResourceManager.GetString("toolBarSave");

        //ToolBar - JobToolBar
        JobToolBar.FindItemByValue("newcandidate").Text = ResourceManager.GetString("toolBarNewCandidate");
        JobToolBar.FindItemByValue("newjobDefault").Text = ResourceManager.GetString("toolBarNew");
        JobToolBar.FindItemByValue("newcompany").Text = ResourceManager.GetString("toolBarNewCompany");
        JobToolBar.FindItemByValue("newaction").Text = ResourceManager.GetString("toolBarNewAction");
        JobToolBar.FindItemByValue("newjob").Text = ResourceManager.GetString("toolBarNewJob");
        JobToolBar.FindItemByValue("newinvoicing").Text = ResourceManager.GetString("toolBarNewInvoice");

        JobToolBar.FindItemByValue("openjob").ToolTip = ResourceManager.GetString("toolBarOpen");
        JobToolBar.FindItemByValue("deletejob").ToolTip = ResourceManager.GetString("toolBarDelete");

        JobToolBar.FindItemByValue("vieweditjob").Text = ResourceManager.GetString("toolBarViewEdit");
        JobToolBar.FindItemByValue("vieweditjob").ToolTip = ResourceManager.GetString("toolBarViewEditChangeMode");
        JobToolBar.FindItemByValue("savejob").ToolTip = ResourceManager.GetString("toolBarSave");
        JobToolBar.FindItemByValue("previewjob").ToolTip = ResourceManager.GetString("toolBarPreview");

        //ToolBar - InvoicingToolBar
        InvoicingToolBar.FindItemByValue("newcandidate").Text = ResourceManager.GetString("toolBarNewCandidate");
        InvoicingToolBar.FindItemByValue("newinvoicingDefault").Text = ResourceManager.GetString("toolBarNew");
        InvoicingToolBar.FindItemByValue("newcompany").Text = ResourceManager.GetString("toolBarNewCompany");
        InvoicingToolBar.FindItemByValue("newaction").Text = ResourceManager.GetString("toolBarNewAction");
        InvoicingToolBar.FindItemByValue("newjob").Text = ResourceManager.GetString("toolBarNewJob");
        InvoicingToolBar.FindItemByValue("newinvoicing").Text = ResourceManager.GetString("toolBarNewInvoice");

        InvoicingToolBar.FindItemByValue("openinvoice").ToolTip = ResourceManager.GetString("toolBarOpen");
        InvoicingToolBar.FindItemByValue("deleteinvoice").ToolTip = ResourceManager.GetString("toolBarDelete");

        InvoicingToolBar.FindItemByValue("vieweditinvoice").Text = ResourceManager.GetString("toolBarViewEdit");
        InvoicingToolBar.FindItemByValue("vieweditinvoice").ToolTip = ResourceManager.GetString("toolBarViewEditChangeMode");
        InvoicingToolBar.FindItemByValue("saveinvoice").ToolTip = ResourceManager.GetString("toolBarSave");

        InvoicingToolBar.FindItemByValue("printinvoice").ToolTip = ResourceManager.GetString("toolBarPrint");
        InvoicingToolBar.FindItemByValue("emailinvoice").ToolTip = ResourceManager.GetString("toolBarEmail");
        InvoicingToolBar.FindItemByValue("copyinvoice").ToolTip = ResourceManager.GetString("toolBarCopy");

        //ToolBar - InvoicingToolBar
        DefaultToolBar.FindItemByValue("new").Text = ResourceManager.GetString("toolBarNew");
        DefaultToolBar.FindItemByValue("newcandidate").Text = ResourceManager.GetString("toolBarNewCandidate");
        DefaultToolBar.FindItemByValue("newcompany").Text = ResourceManager.GetString("toolBarNewCompany");
        DefaultToolBar.FindItemByValue("newaction").Text = ResourceManager.GetString("toolBarNewAction");
        DefaultToolBar.FindItemByValue("newjob").Text = ResourceManager.GetString("toolBarNewJob");
        DefaultToolBar.FindItemByValue("newinvoicing").Text = ResourceManager.GetString("toolBarNewInvoice");
    }

    protected void OnButtonLogout_Click(object sender, EventArgs e)
    {
        SessionManager.CurrentUser = null;
        Response.Redirect("~/Login.aspx", true);
        //FormsAuthentication.SignOut();
        //FormsAuthentication.RedirectToLoginPage();

    }


    #endregion Common

    #region Candidate
    protected void OnCandidateSearchClicked(object sender, EventArgs e)
    {
        string lastname = txtLastNameSearch.Text;
        Session["LastNameSearchCriteria"] = lastname;
        radPaneContent.ContentUrl = "~/Candidates.aspx?lastname=" + lastname;
    }

    //public void OnLast5CandidateItemClicked(object sender, EventArgs e)
    //{
    //    LinkButton lnkItem = (LinkButton)sender;
    //    string id = lnkItem.CommandArgument;
    //    Session["LastNameSearchCriteria"] = null;
    //    radPaneContent.ContentUrl = "~/CandidateProfile.aspx?CandidateID=" + id;
    //}

    protected void OnLastFiveListItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {            
            Candidate dataItem = e.Item.DataItem as Candidate;
            if (dataItem != null)
            {
                HyperLink lnkItem = (HyperLink)e.Item.FindControl("lnkLastFiveItem");
                if (lnkItem != null)
                {
                    lnkItem.Attributes.Add("onclick", string.Format("return OnLast5CandidateItemClicked('{0}');", dataItem.CandidateId));
                    lnkItem.Text = dataItem.FirstName + " " + dataItem.LastName;
                    //lnkItem.NavigateUrl = "~/CandidateProfile.aspx?CandidateID=" + dataItem.CandidateId.ToString();
                }

            }
        }

    }
    #endregion Candidate

    #region Company
    public void rptLastViewedCompanies_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Company c = e.Item.DataItem as Company;
            if (c != null)
            {
                HyperLink lnkLast5Company = (HyperLink)e.Item.FindControl("lnkLast5Company");
                if (lnkLast5Company != null)
                {
                    lnkLast5Company.Text = c.CompanyName;
                    //lnkCompany.NavigateUrl = "#";
                    lnkLast5Company.Attributes.Add("onclick", string.Format("return OnLast5CompanyItemClicked('{0}');", c.CompanyID));
                }
            }
        }
    }

    //public void OnLast5CompanyItemClicked(object sender, EventArgs e)
    //{
    //    LinkButton lnkItem = (LinkButton)sender;
    //    string id = lnkItem.CommandArgument;
    //    //Session["LastNameSearchCriteria"] = null;
    //    radPaneContent.ContentUrl = "~/CompanyProfile.aspx?CompanyId=" + id;
    //}
    

    #endregion Company

    #region Job

    protected void OnButonJobSearch_Click(object sender, EventArgs e)
    {
        string locations = "";
        foreach (ListItem item in lbxLocation.Items)
        {
            if (item.Selected)
                locations += item.Value + ";";
        }
        //locations = locations.TrimEnd(';');
        StringBuilder query = new StringBuilder();
        if (!string.IsNullOrEmpty(txtJobTitle.Text))
            query.AppendFormat("&title={0}", txtJobTitle.Text);
        if (radActiveJobYes.Checked)
            query.Append("&active=Yes");
        else if (radActiveJobNo.Checked)
            query.Append("&active=No");

        if (calCreatedDate1.SelectedDate.HasValue)
            query.AppendFormat("&createdMin={0}", calCreatedDate1.SelectedDate.Value.ToString("dd/MM/yyyy"));
        if (calCreatedDate2.SelectedDate.HasValue)
            query.AppendFormat("&createdMax={0}", calCreatedDate2.SelectedDate.Value.ToString("dd/MM/yyyy"));
        if (calActivationDate1.SelectedDate.HasValue)
            query.AppendFormat("&activatedMin={0}", calActivationDate1.SelectedDate.Value.ToString("dd/MM/yyyy"));
        if (calActivationDate2.SelectedDate.HasValue)
            query.AppendFormat("&activatedMax={0}", calActivationDate2.SelectedDate.Value.ToString("dd/MM/yyyy"));
        if (calExpired1.SelectedDate.HasValue)
            query.AppendFormat("&expiredMin={0}", calExpired1.SelectedDate.Value.ToString("dd/MM/yyyy"));
        if (calExpired2.SelectedDate.HasValue)
            query.AppendFormat("&expiredMax={0}", calExpired2.SelectedDate.Value.ToString("dd/MM/yyyy"));
        if (!string.IsNullOrEmpty(ddlProfile.SelectedValue))
            query.AppendFormat("&profile={0}", ddlProfile.SelectedValue);
        if (!string.IsNullOrEmpty(ddlFunctionFam.SelectedValue))
            query.AppendFormat("&functionFam={0}", ddlFunctionFam.SelectedValue);
        if (!string.IsNullOrEmpty(locations))
            query.AppendFormat("&location={0}", locations.TrimEnd(';'));
        if (!string.IsNullOrEmpty(ddlResponsible.SelectedValue))
            query.AppendFormat("&responsible={0}", ddlResponsible.SelectedValue);
        if (!string.IsNullOrEmpty(txtCompany.Text))
            query.AppendFormat("&company={0}", txtCompany.Text);

        radPaneContent.ContentUrl = string.Format("Jobs.aspx?mode=search{0}", query.ToString());
    }

    private void BindProfileData()
    {
        ddlProfile.DataTextField = "Profile";
        ddlProfile.DataValueField = "ProfileID";
        ddlProfile.DataSource = new ParamProfileRepository().FindAll();
        ddlProfile.DataBind();

        ddlProfile.Items.Insert(0, new RadComboBoxItem("- select -", ""));
    }

    private void BindFunctionData()
    {
        ddlFunctionFam.DataTextField = "FonctionFamID";
        ddlFunctionFam.DataValueField = "FonctionFamID";
        ddlFunctionFam.DataSource = new ParamFunctionFamRepository().FindAll();
        ddlFunctionFam.DataBind();
        ddlFunctionFam.Items.Insert(0, new RadComboBoxItem("-select-", ""));
    }

    private void BindLocationData()
    {
        lbxLocation.DataValueField = "Location";
        //check current language
        lbxLocation.DataTextField = "Location";
        lbxLocation.DataSource = new ParamLocationsRepository().GetAllLocations();
        lbxLocation.DataBind();
    }

    private void BindResponsible()
    {
        ddlResponsible.DataValueField = "UserID";
        ddlResponsible.DataTextField = "LastName";
        ddlResponsible.DataSource = new ParamUserRepository().GetAllUser(true);
        ddlResponsible.DataBind();

        ddlResponsible.Items.Insert(0, new RadComboBoxItem("- select -", ""));

    }

    #endregion

    #region Action
    protected void OnBtnActionSearchClicked(object sender, EventArgs e)
    {
        StringBuilder paramSearch = new StringBuilder();
        paramSearch.Append("type=search");
        if (radActiveActionYes.Checked)
            paramSearch.Append("&active=Yes");
        else if (radActiveActionNo.Checked)
            paramSearch.Append("&active=No");

        if (datDateBetweenAction.SelectedDate.HasValue)
        {
            string date = datDateBetweenAction.SelectedDate.Value.ToString("dd/MM/yyyy");
            paramSearch.Append("&dateFrom=" + date);
        }
        if (datDateAndAction.SelectedDate.HasValue)
        {
            string date = datDateAndAction.SelectedDate.Value.ToString("dd/MM/yyyy");
            paramSearch.Append("&dateTo=" + date);
        }
        if (!string.IsNullOrEmpty(txtCandidateAction.Text))
            paramSearch.Append("&candidate=" + txtCandidateAction.Text.Trim());
        if (!string.IsNullOrEmpty(txtCompanyAction.Text))
            paramSearch.Append("&company=" + txtCompanyAction.Text.Trim());
        if (!string.IsNullOrEmpty(ddlTypeAction.SelectedValue) && int.Parse(ddlTypeAction.SelectedValue) > 0)
            paramSearch.Append("&typeAction=" + ddlTypeAction.SelectedValue.Trim());
        if (!string.IsNullOrEmpty(txtDescriptionAction.Text))
            paramSearch.Append("&description=" + txtDescriptionAction.Text.Trim());
        if (!string.IsNullOrEmpty(ddlResponsibleAction.SelectedValue))
            paramSearch.Append("&responsible=" + ddlResponsibleAction.SelectedValue.Trim());

        radPaneContent.ContentUrl = "Actions.aspx?" + paramSearch.ToString();
    }
    #endregion

     #region Invoices
    protected void OnBtnInvoiceSearchClicked(object sender, EventArgs e)
    {
        StringBuilder paramSearch = new StringBuilder();
        paramSearch.Append("type=search");
        if(!string.IsNullOrEmpty(txtInvoiceNumberFrom.Text)) 
            paramSearch.Append("&invoiceNumberFrom=" + txtInvoiceNumberFrom.Text);
        if(!string.IsNullOrEmpty(txtInvoiceNumberTo.Text)) 
            paramSearch.Append("&invoiceNumberTo=" + txtInvoiceNumberTo.Text);

        if (!string.IsNullOrEmpty(ddlFiscalYear.SelectedValue))
            paramSearch.Append("&fiscalYear=" + ddlFiscalYear.SelectedValue.Trim());

        if (datInvoiceDateFrom.SelectedDate.HasValue)
        {
            string date = datInvoiceDateFrom.SelectedDate.Value.ToString("dd/MM/yyyy");
            paramSearch.Append("&dateFrom=" + date);
        }
        if (datInvoiceDateTo.SelectedDate.HasValue)
        {
            string date = datInvoiceDateTo.SelectedDate.Value.ToString("dd/MM/yyyy");
            paramSearch.Append("&dateTo=" + date);
        }

        if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
            paramSearch.Append("&invoiceType=" + ddlInvoiceType.SelectedValue.Trim());


        if (hiddenCompanyId.Value != "-1" && !string.IsNullOrEmpty(hiddenCompanyId.Value))
        {
            string[] arguments = hiddenCompanyId.Value.Split('/');
            paramSearch.Append("&customer=" + arguments[0]);
            if (arguments.Length == 2)
            {
                txtInvoiceCompanyName.Text = arguments[1];
            }
        }
        else
        {
            txtInvoiceCompanyName.Text = string.Empty;
        }
        radPaneContent.ContentUrl = "InvoicesPage.aspx?" + paramSearch.ToString();

        
    }

    protected void OnDropdownCompany_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        string companyName = e.Text;
        if (!string.IsNullOrEmpty(companyName))
        {
            List<Company> list = new CompanyRepository().FindByName(companyName);

            ddlCompany.DataSource = list;
            ddlCompany.DataBind();
        }
    }
    #endregion

    #region ajax events
    protected void OnHomeAjaxManager_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("BindLast5ViewedCandidate") != -1)
        {
            homeAjaxManager.AjaxSettings.AddAjaxSetting(homeAjaxManager, divLast5Candidate);
            BindLast5ViewedCandidate();
        }
        else if (e.Argument.IndexOf("BindLast5ViewedCompany") != -1)
        {
            homeAjaxManager.AjaxSettings.AddAjaxSetting(homeAjaxManager, divLast5Company);
            BindLast5ViewedCompany();
        }
    }
    #endregion
}

