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
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web.Services;
using System.IO;
using System.Collections.Specialized;
using System.Xml;

public partial class CandidateProfile : System.Web.UI.Page
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
        //Common.KeepSessionAlive(this);
        if (!IsPostBack)
        {
            FillLabelLanguage();
            InitControls();

            if (!string.IsNullOrEmpty(SessionManager.BackUrl)
                && SessionManager.BackUrl.Contains("Candidates.aspx")
                && !string.IsNullOrEmpty(Request.QueryString["backurl"])
                && Request.QueryString["backurl"] == "visible")
            {
                lnkBack.Visible = true;
            }
            else
            {
                SessionManager.BackUrl = null;
                lnkBack.Visible = false;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            {
                int candidateID = int.Parse(Request.QueryString["CandidateId"]);
                Candidate currentCandidate = NeosDAO.GetCandidateById(candidateID);
                SessionManager.CurrentCandidate = currentCandidate;
                SaveLastViewCandidatesToCookie(currentCandidate);

                string script1 = "<script type='text/javascript'>";
                script1 += "onSaveOrLoadCandidateProfilePage();";
                script1 += "</script>";
                if (!ClientScript.IsClientScriptBlockRegistered("onSaveOrLoadCandidateProfilePage"))
                    ClientScript.RegisterStartupScript(this.GetType(), "onSaveOrLoadCandidateProfilePage", script1);

                LoadCandidateData(currentCandidate);

                if (Request.QueryString["originalPage"] == "Action")
                {
                    lnkBackToAction.Visible = true;
                    lnkBack.Visible = false;
                    hidActionUrl.Value = Request.UrlReferrer.PathAndQuery;
                }
                else
                {                    
                    lnkBackToAction.Visible = false;
                }
                if (Request.QueryString["mode"] == "view")
                    EnableCandidateControls(false);
                else
                    EnableCandidateControls(true);

                //show the title
                lblCandidateProfileTitle.Text = string.Format(ResourceManager.GetString("lblRightPaneCandidateProfileTitle"), currentCandidate.FirstName + " " + currentCandidate.LastName);
            }
            else
            {
                SessionManager.CurrentCandidate = null;
                SessionManager.CandidateEvaluation = null;
                SessionManager.CandidateExpectation = null;
                SessionManager.NewCandidateTelephoneList = new List<CandidateTelephone>();

                LoadGeneralTabData(null);
                radTabStripCandidateProfile.FindTabByValue("GeneralView").PageViewID = "GeneralView";
                radTabStripCandidateProfile.FindTabByValue("GeneralView").PageView.Selected = true;
                DrawGeneralTab();

                btnEditSave.Text = ResourceManager.GetString("saveText");
                EnableCandidateControls(true);
                //lnkCanContactAdd.Visible = false;
                SessionManager.NewCandidateTelephoneList = null;
                lnkAddNewAction.Visible = false;
                lnkAddNewStudy.Visible = false;
                lnkAddNewExperience.Visible = false;
                lnkAddNewDocument.Visible = false;

                //show the title
                lblCandidateProfileTitle.Text = ResourceManager.GetString("lblRightPaneAddNewCandidateTitle");
            }
        }
        string script = "<script type='text/javascript'>";
        script += "onLoadCandidateProfilePage();";        
        script += "</script>";
        if (!ClientScript.IsClientScriptBlockRegistered("LoadCandidateProfile"))
            ClientScript.RegisterStartupScript(this.GetType(), "LoadCandidateProfile", script);
    }

    private void SaveLastViewCandidatesToCookie(Candidate candidate)
    {
        XmlDocument doc = new XmlDocument();
        try
        {
            doc.Load(Server.MapPath("~/App_Data/LastViewedCandidates.xml"));

            XmlElement rootNode = doc.DocumentElement;
            if (rootNode != null)
            {
                XmlNode userNode = rootNode.SelectSingleNode("User[@id='" + SessionManager.CurrentUser.UserID + "']");
                if (userNode != null) //user node existed
                {
                    string viewedCandidates = userNode.Attributes["viewed-candidates"].Value;
                    if (!string.IsNullOrEmpty(viewedCandidates))
                    {
                        List<string> viewedCandidateList = new List<string>(viewedCandidates.Split('&'));

                        viewedCandidateList.Remove(candidate.CandidateId.ToString());
                        viewedCandidateList.Insert(0, candidate.CandidateId.ToString());
                        if (viewedCandidateList.Count > WebConfig.NumberOfRecentCandidate)
                            viewedCandidateList.RemoveAt(viewedCandidateList.Count - 1);
                        
                        viewedCandidates = "";
                        foreach (string candidateID in viewedCandidateList)
                        {
                            viewedCandidates += candidateID + "&";
                        }
                        viewedCandidates = viewedCandidates.TrimEnd('&');

                        userNode.Attributes["viewed-candidates"].Value = viewedCandidates;
                    }
                    else
                    {
                        userNode.Attributes["viewed-candidates"].Value = candidate.CandidateId.ToString();
                    }
                }
                else //create new user node
                {
                    userNode = doc.CreateElement("User");


                    XmlAttribute id = doc.CreateAttribute("id");
                    id.Value = SessionManager.CurrentUser.UserID;
                    

                    XmlAttribute viewedCandidateNode = doc.CreateAttribute("viewed-candidates");
                    viewedCandidateNode.Value = candidate.CandidateId.ToString();

                    userNode.Attributes.Append(id);
                    userNode.Attributes.Append(viewedCandidateNode);

                    rootNode.AppendChild(userNode);
                }
            }

            doc.Save(Server.MapPath("~/App_Data/LastViewedCandidates.xml"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        /*
        HttpCookie lastCanCookie = Request.Cookies.Get("lastCandidatesCookie");

        if (lastCanCookie != null)
        {
            string[] values = lastCanCookie.Values.ToString().Split('&');
            IDictionary<string, string> userIDsDic = new Dictionary<string, string>();
            if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    string[] userAndCanIds = values[i].Split('=');
                    if (userAndCanIds[0] == SessionManager.CurrentUser.UserID.Trim())
                    {                        
                        string idsOfUser = userAndCanIds[1];
                        IList<int> idList = new List<int>();
                        string[] idArray = idsOfUser.Split('A');
                        for (int t = 0; t < idArray.Length; t++)
                        {
                            int id = int.Parse(idArray[t]);
                            idList.Add(id);
                        }


                        if (idList.Contains(candidate.CandidateId))
                        {
                            idList.Remove(candidate.CandidateId);
                        }
                        else
                        {
                            if (idList.Count == 5)
                            {
                                idList.RemoveAt(0);
                            }
                        }
                        idList.Add(candidate.CandidateId);
                        string idString = string.Empty;
                        foreach (int id in idList)
                        {
                            if (idString != string.Empty)
                                idString += "A";
                            idString += id.ToString();
                        }
                        userIDsDic.Add(userAndCanIds[0], idString);                                                
                    }
                    else
                    {
                        userIDsDic.Add(userAndCanIds[0], userAndCanIds[1]);                           
                    }
                }
                if (!userIDsDic.ContainsKey(SessionManager.CurrentUser.UserID.Trim()))
                {
                    userIDsDic.Add(SessionManager.CurrentUser.UserID.Trim(), candidate.CandidateId.ToString());
                }

                lastCanCookie.Expires = DateTime.Now.AddDays(-1);                
                Response.Cookies.Add(lastCanCookie);

                HttpCookie lastCandidatesCookie = new HttpCookie("lastCandidatesCookie");
                lastCandidatesCookie.Expires = DateTime.Today.AddDays(30);
                foreach (KeyValuePair<string, string> item in userIDsDic)
                {
                    lastCandidatesCookie.Values.Add(item.Key, item.Value);
                }
                Response.Cookies.Add(lastCandidatesCookie);
                
            }
            else
            {
                HttpCookie lastCandidatesCookie = new HttpCookie("lastCandidatesCookie");
                lastCandidatesCookie.Expires = DateTime.Today.AddDays(30);
                lastCandidatesCookie.Values.Add(SessionManager.CurrentUser.UserID.Trim(), candidate.CandidateId.ToString());
                Response.Cookies.Add(lastCandidatesCookie);
            }
               
        }
        else
        {
            HttpCookie lastCandidatesCookie = new HttpCookie("lastCandidatesCookie");
            lastCandidatesCookie.Expires = DateTime.Today.AddDays(30);
            lastCandidatesCookie.Values.Add(SessionManager.CurrentUser.UserID.Trim(), candidate.CandidateId.ToString());
            Response.Cookies.Add(lastCandidatesCookie);
        }
        */
    }

    /// <summary>
    /// load data for first view
    /// </summary>
    /// <param name="candidate"></param>
    private void LoadCandidateData(Candidate candidate)
    {
        //load header table data
        LoadHeaderData(candidate);
        
        //load only general tab data 
        if (Request.QueryString["tab"] == "action")
        {
            gridActions.Visible = true;
            LoadActionTabData(candidate);
            radTabStripCandidateProfile.FindTabByValue("ActionView").PageViewID = "ActionView";
            radTabStripCandidateProfile.FindTabByValue("ActionView").Selected = true;
            CandidateProfileMultiPage.FindPageViewByID("ActionView").Selected = true;
        }
        else
        {
            LoadGeneralTabData(candidate);
            radTabStripCandidateProfile.FindTabByValue("GeneralView").PageViewID = "GeneralView";
        }
    }

    private void InitControls()
    {        
        //Load header drop down data
        ParamTypeRepository paramTypeRepo = new ParamTypeRepository();
        ddlUnit.DataValueField = "TypeID";
        ddlUnit.DataTextField = "Label";
        ddlUnit.DataSource = paramTypeRepo.FindAll();
        ddlUnit.DataBind();

        ParamUserRepository paramUserRepo = new ParamUserRepository();
        ddlInterviewer.DataValueField = "UserID";
        ddlInterviewer.DataTextField = "LastName";

        ddlInterviewer.DataSource = paramUserRepo.GetAllUser(true);
        ddlInterviewer.DataBind();

        if (!string.IsNullOrEmpty(Request.QueryString["CandidateID"]))
            lnkCanContactAdd.OnClientClick = string.Format("return OncandidateContactAddClientClicked('{0}')",Request.QueryString["CandidateID"]);
        else
            lnkCanContactAdd.OnClientClick = string.Format("return OncandidateContactAddClientClicked('')");
        //if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
        //{
        //    radTabStripCandidateProfile.OnClientLoad = "javascript:onLoadCandidateProfile();";
        //}

        if (!string.IsNullOrEmpty(Request.QueryString["CandidateID"]))
            lnkAddNewDocument.Attributes.Add("onclick", string.Format("OnDocumentEditClientClicked({0},\"\")", Request.QueryString["CandidateID"]));

        //tab study
        HttpCookie studyGridPageSizeCookie = Request.Cookies.Get("cand_studygrdps");
        if (studyGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(studyGridPageSizeCookie.Value))
                gridStudies.PageSize = Convert.ToInt32(studyGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(studyGridPageSizeCookie.Value) : gridStudies.PageSize;
        }
        HttpCookie experienceGridPageSizeCookie = Request.Cookies.Get("cand_expgrdps");
        if (experienceGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(experienceGridPageSizeCookie.Value))
                gridExperience.PageSize = Convert.ToInt32(experienceGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(experienceGridPageSizeCookie.Value) : gridExperience.PageSize;
        }

        //tab actions
        HttpCookie actionGridPageSizeCookie = Request.Cookies.Get("cand_actgrdps");
        if (actionGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(actionGridPageSizeCookie.Value))
                gridActions.PageSize = Convert.ToInt32(actionGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(actionGridPageSizeCookie.Value) : gridActions.PageSize;
        }
        //tab Document
        HttpCookie docGridPageSizeCookie = Request.Cookies.Get("cand_docgrdps");
        if (docGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(docGridPageSizeCookie.Value))
                grdDocuments.PageSize = Convert.ToInt32(docGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(docGridPageSizeCookie.Value) : grdDocuments.PageSize;
        }
    }

    private void FillLabelLanguage()
    {
        lnkBack.Text = ResourceManager.GetString("backText");
        //RadCandidateGrid.Columns[0].HeaderText = ResourceManager.GetString("columnLastNameCandidateGrid");
        //Common :
        lblCanLastName.Text = ResourceManager.GetString("lblCanLastName");
        lblCanFirstName.Text = ResourceManager.GetString("lblCanFirstName");
        lblCanUnit.Text = ResourceManager.GetString("lblCanUnit");
        lblCanInterviewer.Text = ResourceManager.GetString("lblCanInterviewer");
        lblDateInterview.Text = ResourceManager.GetString("lblDateInterview");
        btnEditSave.Text = ResourceManager.GetString("editText");
        btnCancel.Text = ResourceManager.GetString("cancelText");
        lnkBackToAction.Text = ResourceManager.GetString("lnkBackToAction");

        radTabStripCandidateProfile.Tabs[0].Text = ResourceManager.GetString("tabCanGeneralText");
        radTabStripCandidateProfile.Tabs[1].Text = ResourceManager.GetString("tabCanExpectancyText");
        radTabStripCandidateProfile.Tabs[2].Text = ResourceManager.GetString("tabCanStudyExperienceText");
        radTabStripCandidateProfile.Tabs[3].Text = ResourceManager.GetString("tabCanEvaluationText");
        radTabStripCandidateProfile.Tabs[4].Text = ResourceManager.GetString("tabCanKnowledgeFunctionText");
        radTabStripCandidateProfile.Tabs[5].Text = ResourceManager.GetString("tabCanPresentationText");
        radTabStripCandidateProfile.Tabs[6].Text = ResourceManager.GetString("tabCanActionText");
        radTabStripCandidateProfile.Tabs[7].Text = ResourceManager.GetString("tabCanDocumentText");

        //tab general :
        lblCanAddress.Text = ResourceManager.GetString("lblCanAddress");
        lblCanZip.Text = ResourceManager.GetString("lblCanZip");
        lblCanCity.Text = ResourceManager.GetString("lblCanCity");
        lblCanGenre.Text = ResourceManager.GetString("lblCanGenre");
        lblCanNationality.Text = ResourceManager.GetString("lblCanNationality");
        lblCanDateOfBirth.Text = ResourceManager.GetString("lblCanDateOfBirth");
        lblCanAge.Text = ResourceManager.GetString("lblCanAge");
        lnkCanContactAdd.Text = ResourceManager.GetString("lnkCanContactAdd");
        lblCanCreationDate.Text = ResourceManager.GetString("lblCanCreationDate");
        lblCanStatus.Text = ResourceManager.GetString("lblCanStatus");
        lblCanInactivityReason.Text = ResourceManager.GetString("lblCanInactivityReason");
        lblCanRemarkGeneral.Text = ResourceManager.GetString("lblCanRemarkGeneral");
        lblCandidateWishes.Text = ResourceManager.GetString("lblCandidateWishes");
        lblCanArea.Text = ResourceManager.GetString("lblCanArea");
        btnAddCanArea.Text = ResourceManager.GetString("addText");
        btnRemoveCanArea.Text = ResourceManager.GetString("removeText");
        lblCanSalary.Text = ResourceManager.GetString("lblCanSalary");

        lblCanCompanyGeneral.Text = ResourceManager.GetString("lblCanCompanyGeneral");
        lblCanContractType.Text = ResourceManager.GetString("lblCanContractType");
        lblCanFunction.Text = ResourceManager.GetString("lblCanFunction");
        lblCanMotivation.Text = ResourceManager.GetString("lblCanMotivation");

        gridContact.Columns[0].HeaderText = ResourceManager.GetString("columnTypeCandidateContact");
        gridContact.Columns[1].HeaderText = ResourceManager.GetString("columnZoneCandidateContact");
        gridContact.Columns[2].HeaderText = ResourceManager.GetString("columnPhoneMailCandidateContact");
        gridContact.Columns[3].HeaderText = ResourceManager.GetString("columnPlaceCandidateContact");

        //Tab Expectancies
        expectancyGrid.Columns[0].HeaderText = ResourceManager.GetString("gridKnowFunctionTypeColumn");
        expectancyGrid.Columns[1].HeaderText = ResourceManager.GetString("gridKnowFunctionGroupColumn");
        expectancyGrid.Columns[2].HeaderText = ResourceManager.GetString("lblCanFunctionGrid");
        lblAddRemoveExpectancy.Text = ResourceManager.GetString("lblAddRemoveExpectancy");
        gridCanExpectDestination.Columns[0].HeaderText = ResourceManager.GetString("gridKnowFunctionTypeColumn");
        gridCanExpectDestination.Columns[1].HeaderText = ResourceManager.GetString("gridKnowFunctionGroupColumn");
        gridCanExpectDestination.Columns[2].HeaderText = ResourceManager.GetString("lblCanFunctionGrid");

        lblCanExpectUnit.Text = ResourceManager.GetString("lblKnowFuncUnit");
        lblCanExpectFam.Text = ResourceManager.GetString("lblKnowFuncFam");

        btnCanExpectEdit.Text = ResourceManager.GetString("editText");
        btnCanExpectAdd.Text = ResourceManager.GetString("addText");
        btnCanExpectRemove.Text = ResourceManager.GetString("removeText");
        btnExpectOK.Text = ResourceManager.GetString("okText");
        btnExpectCancel.Text = ResourceManager.GetString("cancelText");

        //Tab Studies/Experience:
        lblCanStudies.Text = ResourceManager.GetString("lblCanStudies");
        lnkAddNewStudy.Text = ResourceManager.GetString("lnkAddNewStudy");
        lblCanExperience.Text = ResourceManager.GetString("lblCanExperience");
        lnkAddNewExperience.Text = ResourceManager.GetString("lnkAddNewExperience");

        gridStudies.Columns[0].HeaderText = ResourceManager.GetString("columnPeriodStudiesCan");
        gridStudies.Columns[1].HeaderText = ResourceManager.GetString("columnTrainingStudiesCan");
        gridStudies.Columns[2].HeaderText = ResourceManager.GetString("columnDiplomaStudiesCan");
        gridStudies.Columns[3].HeaderText = ResourceManager.GetString("columnLevelStudiesCan");
        gridStudies.Columns[4].HeaderText = ResourceManager.GetString("columnSchoolStudiesCan");

        gridExperience.Columns[0].HeaderText = ResourceManager.GetString("columnPeriodExperienceCan");
        gridExperience.Columns[1].HeaderText = ResourceManager.GetString("columnCompanyExperienceCan");
        gridExperience.Columns[2].HeaderText = ResourceManager.GetString("columnSalaryExperienceCan");
        gridExperience.Columns[3].HeaderText = ResourceManager.GetString("columnSalaryPackageExperienceCan");
        gridExperience.Columns[4].HeaderText = ResourceManager.GetString("columnJobTitleExperienceCan");
        gridExperience.Columns[5].HeaderText = ResourceManager.GetString("columnQuitReasonExperienceCan");

        //Tab Evaluation :
        lblCanGlobal.Text = ResourceManager.GetString("lblCanGlobal");
        lblCanVerbal.Text = ResourceManager.GetString("lblCanVerbal");
        lblCanAutonomy.Text = ResourceManager.GetString("lblCanAutonomy");
        lblCanMotivationEval.Text = ResourceManager.GetString("lblCanMotivationEval");
        lblCanPersonality.Text = ResourceManager.GetString("lblCanPersonality");
        lblCanLanguage.Text = ResourceManager.GetString("lblCanLanguage");
        lblCanFrenchLang.Text = ResourceManager.GetString("lblCanFrenchLang");
        lblCanGermanLang.Text = ResourceManager.GetString("lblCanGermanLang");
        lblCanDutchLang.Text = ResourceManager.GetString("lblCanDutchLang");
        lblCanSpanishLang.Text = ResourceManager.GetString("lblCanSpanishLang");
        lblCanEnglishLang.Text = ResourceManager.GetString("lblCanEnglishLang");
        lblCanItalianLang.Text = ResourceManager.GetString("lblCanItalianLang");
        lblCanOtherLang.Text = ResourceManager.GetString("lblCanOtherLang");

        //Tab Knowledge/Function : 
        lblCanKnowledgeGrid.Text = ResourceManager.GetString("lblCanKnowledgeGrid");
        lblCanFunctionGrid.Text = ResourceManager.GetString("lblCanFunctionGrid");

        gridKnowledgeOld.Columns[0].HeaderText = ResourceManager.GetString("gridKnowFunctionTypeColumn");
        gridKnowledgeOld.Columns[1].HeaderText = ResourceManager.GetString("gridKnowFunctionGroupColumn");
        gridKnowledgeOld.Columns[2].HeaderText = ResourceManager.GetString("lblCanKnowledgeGrid");
        gridFunctionOld.Columns[0].HeaderText = ResourceManager.GetString("gridKnowFunctionTypeColumn");
        gridFunctionOld.Columns[1].HeaderText = ResourceManager.GetString("gridKnowFunctionGroupColumn");
        gridFunctionOld.Columns[2].HeaderText = ResourceManager.GetString("lblCanFunctionGrid");

        lblKnowFuncUnit.Text = ResourceManager.GetString("lblKnowFuncUnit");
        lblKnowFuncFam.Text = ResourceManager.GetString("lblKnowFuncFam");

        btnFunctionEdit.Text = ResourceManager.GetString("editText");
        btnKnowledgeEdit.Text = ResourceManager.GetString("editText");
        btnKnowFuncOK.Text = ResourceManager.GetString("okText");
        btnKnowFuncCancel.Text = ResourceManager.GetString("cancelText");
        btnKnowFuncAdd.Text = ResourceManager.GetString("addText");
        btnKnowFuncRemove.Text = ResourceManager.GetString("removeText");

        gridKnowFuncDestination.Columns[0].HeaderText = ResourceManager.GetString("gridKnowFunctionTypeColumn");
        gridKnowFuncDestination.Columns[1].HeaderText = ResourceManager.GetString("gridKnowFunctionGroupColumn");

        //Tab Presentation : 
        btnGeneratePresentation.Text = ResourceManager.GetString("btnGeneratePresentation");
        lblSendPresentation.Text = ResourceManager.GetString("lblSendPresentation");
        lblCompanyPresent.Text = ResourceManager.GetString("lblCompanyPresent");
        lblContactsPresent.Text = ResourceManager.GetString("lblContactsPresent");
        lblAttachedDocumentsPresent.Text = ResourceManager.GetString("lblAttachedDocumentsPresent");
        btnSendPresentation.Text = ResourceManager.GetString("btnSendPresentation");
        grdPresentationContacts.Columns[1].HeaderText = ResourceManager.GetString("columnNameContactsPresent");
        grdPresentationContacts.Columns[2].HeaderText = ResourceManager.GetString("columnEmailContactsPresent");
        grdPresentationAttachedDocs.Columns[1].HeaderText = ResourceManager.GetString("columnDocumentAttachedDocPresent");
        grdPresentationAttachedDocs.Columns[2].HeaderText = ResourceManager.GetString("columnCreatedDateAttachedDocPresent");

        grdComDocuments.Columns[1].HeaderText = ResourceManager.GetString("columnNameDocumentCan");        
        grdComDocuments.Columns[2].HeaderText = ResourceManager.GetString("columnCreatedDateDocumentCan");
        lnkAddNewComDocument.Text = ResourceManager.GetString("lblAddNewDocument");
        lblComDocuments.Text = ResourceManager.GetString("lblCompanyDocumentsPresent");
        //Tab Action : 
        lnkAddNewAction.Text = ResourceManager.GetString("lnkAddNewAction");
        gridActions.Columns[0].HeaderText = ResourceManager.GetString("columnActiveActionCan");
        gridActions.Columns[1].HeaderText = ResourceManager.GetString("columnTaskNbrActionCan");
        gridActions.Columns[2].HeaderText = ResourceManager.GetString("columnDateActionCan");
        gridActions.Columns[3].HeaderText = ResourceManager.GetString("columnHourActionCan");
        gridActions.Columns[4].HeaderText = ResourceManager.GetString("columnTypeActionCan");
        gridActions.Columns[5].HeaderText = ResourceManager.GetString("columnCompanyActionCan");
        gridActions.Columns[6].HeaderText = ResourceManager.GetString("columnDescriptionActionCan");
        gridActions.Columns[7].HeaderText = ResourceManager.GetString("columnResponsibleActionCan");

        //Tab Documents : 
        grdDocuments.Columns[0].HeaderText = ResourceManager.GetString("columnNameDocumentCan");
        grdDocuments.Columns[1].HeaderText = ResourceManager.GetString("columnLegendDocumentCan");
        grdDocuments.Columns[2].HeaderText = ResourceManager.GetString("columnCreatedDateDocumentCan");
        lnkAddNewDocument.Text = ResourceManager.GetString("lblAddNewDocument");
    }

    private void LoadHeaderData(Candidate candidate)
    {        
        if (candidate != null)
        {
            txtFirstName.Text = candidate.FirstName;
            txtLastName.Text = candidate.LastName;
            ddlUnit.SelectedValue = candidate.Unit;
            ddlInterviewer.SelectedValue = candidate.Interviewer;
            datDateInterview.SelectedDate = candidate.DateOfInterView;
        }
    }

    private void LoadGeneralTabData(Candidate candidate)
    {
        ParamPaysRepository paramPaysRepo = new ParamPaysRepository();
        ddlCountry.DataValueField = "PaysID";
        ddlCountry.DataTextField = "Pays";
        ddlCountry.DataSource = paramPaysRepo.FindAll();
        ddlCountry.DataBind();

        ddlGenre.Items.Add(new RadComboBoxItem("M", "M"));
        ddlGenre.Items.Add(new RadComboBoxItem("F", "F"));
        ddlStatus.Items.Add(new RadComboBoxItem("", ""));
        ddlStatus.Items.Add(new RadComboBoxItem("actif", "False"));
        ddlStatus.Items.Add(new RadComboBoxItem("inactif", "True"));

        ParamNationaliteRepository nationalityRepo = new ParamNationaliteRepository();
        ddlNationality.DataValueField = "NationaliteID";
        ddlNationality.DataTextField = "NationaliteID";
        ParamNationalite emptyNation = new ParamNationalite();
        emptyNation.NationaliteID = string.Empty;
        emptyNation.Label = string.Empty;
        IList<ParamNationalite> nationList = nationalityRepo.FindAll();
        nationList.Insert(0, emptyNation);
        ddlNationality.DataSource = nationList;
        ddlNationality.DataBind();

        ddlCanArea.DataValueField = "Location";
        ddlCanArea.DataTextField = "Location";
        ParamLocationsRepository locationRepo = new ParamLocationsRepository();
        ddlCanArea.DataSource = locationRepo.GetAllLocations();
        ddlCanArea.DataBind();

        ddlSalaryWish.Items.Add(new RadComboBoxItem("1500-1999"));
        ddlSalaryWish.Items.Add(new RadComboBoxItem("2000-2999"));
        ddlSalaryWish.Items.Add(new RadComboBoxItem("3000-3999"));
        ddlSalaryWish.Items.Add(new RadComboBoxItem("4000-4999"));
        ddlSalaryWish.Items.Add(new RadComboBoxItem(">5000"));

        ddlCompanyWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("notImportantCompaniesText")));
        ddlCompanyWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("smallAndMediumCompaniesText")));
        ddlCompanyWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("bigCompaniesText")));
        ddlCompanyWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("groupCompaniesText")));
        ddlCompanyWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("multinationalCompaniesText")));

        ddlContractTypeWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("notImportantContractText")));
        ddlContractTypeWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("permanentContractText")));
        ddlContractTypeWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("fixedPeriodeContractText")));
        ddlContractTypeWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("projectMissionContractText")));
        ddlContractTypeWish.Items.Add(new RadComboBoxItem(ResourceManager.GetString("freelanceContractText")));

        if (candidate != null)
        {
            txtAddress.Text = !string.IsNullOrEmpty(candidate.Address) ? candidate.Address : "";
            txtZip.Text = !string.IsNullOrEmpty(candidate.ZipCode) ? candidate.ZipCode : "";
            txtCity.Text = !string.IsNullOrEmpty(candidate.City) ? candidate.City : ""; 
            datDateOfBirth.SelectedDate = candidate.BirthDate;
            if (candidate.BirthDate.HasValue)
            {
                DateTime today = DateTime.Today;                
                DateTime birthDay = candidate.BirthDate.Value;
                int age = today.Year - birthDay.Year;
                if ((today.Month < birthDay.Month) || (today.Month == birthDay.Month) && (today.Day < birthDay.Day))
                    age = age - 1;

                txtAge.Text = age.ToString();
            }

            ddlCountry.SelectedValue = candidate.CountryCode;
            if (candidate.Gender != null)
                ddlGenre.SelectedValue = candidate.Gender.ToUpper();
            ddlNationality.SelectedValue = candidate.Nationlity;
            datCreationDate.SelectedDate = candidate.CreationDate;
            ddlStatus.SelectedValue = candidate.Inactive.ToString();
            txtInactivityReason.Text = candidate.ReasonDesactivation;
            txtRemarkGeneral.Text = candidate.Remark;

            //Tab General : Candidate Wishes :
            CandidateExpectationRepository candidateExpectRepo = new CandidateExpectationRepository();
            CandidateExpectation expect = candidateExpectRepo.GetCandidateExpectation(candidate.CandidateId);
            if (expect != null)
            {
                SessionManager.CandidateExpectation = expect;
                //txtArea.Text = expect.Region;
                if (!string.IsNullOrEmpty(expect.Region))
                {
                    string[] areaArray = expect.Region.Split(';');
                    for (int i = 0; i < areaArray.Length; i++)
                    {
                        listCanArea.Items.Add(new ListItem(areaArray[i].Trim(), areaArray[i].Trim()));
                    }
                }
                hiddenCanAreaList.Value = expect.Region;
                ddlCompanyWish.Text = expect.Company;
                txtFunction.Text = expect.Function;
                ddlSalaryWish.Text = expect.SalaryLevel;
                ddlContractTypeWish.Text = expect.TypeofContract;
                txtMotivation.Text = expect.Motivation;
            }
        }
    }
    private void LoadExpectancyTabData(Candidate candidate)
    {
        BindExpectacyOfCandidate(candidate);
    }
    private void LoadStudyExperienceTabData(Candidate candidate)
    {
        BindStudyGridOfCurrentCandidate(candidate);
        BindExperienceGridOfCurrentCandidate(candidate);
    }
    private void LoadEvaluationTabData(Candidate candidate)
    {
        ParamLangueRepository languageRepo = new ParamLangueRepository();
        ddlOtherLang.DataValueField = "LangueID";
        ddlOtherLang.DataTextField = "LangueID";
        ddlOtherLang.DataSource = languageRepo.FindAll();
        ddlOtherLang.DataBind();

        for (int i = 0; i <= 5; i++)
        {
            ddlFrenchLang.Items.Add(new RadComboBoxItem(
                ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlDutchLang.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlEnglishLang.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlGermanLang.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlSpainishLang.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlItalianLang.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlOtherLangSkill.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));

        }
        if (candidate != null)
        {
            CandidateEvaluationRepository candidateEvalRepo = new CandidateEvaluationRepository();
            CandidateEvaluation evaluation = candidateEvalRepo.GetCandidateEvaluation(candidate.CandidateId);
            if (evaluation != null)
            {
                SessionManager.CandidateEvaluation = evaluation;
                txtPresentationEval.Text = evaluation.PresentationEvaluation;
                txtGlobal.Text = evaluation.GlobalEvaluation;
                txtVerbal.Text = evaluation.ExpressionEvaluation;
                txtOtherEval.Text = evaluation.VariousMatters;
                txtAutonomy.Text = evaluation.SelfEvaluation;
                txtMotivationEval.Text = evaluation.MotivationEvaluation;
                txtPersonality.Text = evaluation.PersonalityEvaluation;
                ddlOtherLang.SelectedValue = evaluation.AdditionLanguage;
                if (evaluation.French.HasValue)
                    ddlFrenchLang.SelectedValue = evaluation.French.Value.ToString();

                if (evaluation.German.HasValue)
                    ddlGermanLang.SelectedValue = evaluation.German.Value.ToString();
                if (evaluation.Dutch.HasValue)
                    ddlDutchLang.SelectedValue = evaluation.Dutch.Value.ToString();
                if (evaluation.Spanish.HasValue)
                    ddlSpainishLang.SelectedValue = evaluation.Spanish.Value.ToString();
                if (evaluation.English.HasValue)
                    ddlEnglishLang.SelectedValue = evaluation.English.Value.ToString();
                if (evaluation.Italian.HasValue)
                    ddlItalianLang.SelectedValue = evaluation.Italian.Value.ToString();
                if (evaluation.AdditionLanguageScore.HasValue)
                    ddlOtherLangSkill.SelectedValue = evaluation.AdditionLanguageScore.Value.ToString();
            }
        }
    }
    private void LoadKnowledgeFunctionTabData(Candidate candidate)
    {
        BindKnowledgeGridOfCurrentCandidate(candidate);
        BindFunctionGridOfCurrentCandidate(candidate);
    }
    private void LoadPresentationTabData(Candidate cand)
    {
        BindDropdownCompanies();
        BindContactGridOfCurrentCandidate(cand);
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            int compID = Convert.ToInt32(ddlCompany.SelectedValue);
            BindContactsGridOfCompany(compID);
            BindDocumentsGridOfCompany(compID);
            lnkAddNewComDocument.Visible = true;
            btnSendPresentation.Enabled = !string.IsNullOrEmpty(Request.QueryString["CandidateId"]);
            lnkAddNewComDocument.Attributes.Add("onclick", string.Format("OnComDocumentEditClientClicked({0},\"\")", compID));
        }
        else
        {
            BindContactsGridOfCompany(0);
            BindDocumentsGridOfCompany(0);
            lnkAddNewComDocument.Visible = false;
            btnSendPresentation.Enabled = false;
        }
        BindAttachedDocumentsOfCandidate(cand);
        if (cand != null)
        {
            txtPresentationText.Text = cand.Presentation;
            //grid documents
            grdDocuments.DataSource = new CandidateDocumentRepository().GetDocumentsOfCandidate(cand.CandidateId);
        }
        else
        {
            txtPresentationText.Text = string.Empty;
            //grid documents
            grdDocuments.DataSource = new List<CandidateDocument>();
        }
        grdDocuments.DataBind();
    }
    private void LoadActionTabData(Candidate candidate)
    {
        BindActionGridOfCurrentCandidate(candidate);
    }
    private void LoadDocumentTabData(Candidate candidate)
    {
        BindGridDocumentsOfCurrentCandidate(candidate);
    }

    protected void OnButtonCandidateEditSaveClicked(object sender, EventArgs e)
    {
        /*string script = "<script type=\"text/javascript\">";
        script += " DisableToolbar();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);*/

        if (btnEditSave.Text == ResourceManager.GetString("editText"))
        {
            //Change mode to Edit mode.
            //btnEditSave.Text = ResourceManager.GetString("saveText");
            //EnableCandidateControls(true);
            //divAddRemoveExpectancy.Visible = false;
            //divKnowledgeFunctionEdit.Visible = false;
            string url = !string.IsNullOrEmpty(Request.QueryString["tab"]) ? Request.Url.PathAndQuery.Replace("&tab=action", "") : Request.Url.PathAndQuery;
            if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                url = url.Replace(Request.QueryString["mode"], "edit");
            else
                url += "&mode=edit";
            Response.Redirect(url, true);
        }
        else
        {
            //Save data.
            Candidate candidate = SaveCandidateProfile();

            //Change mode to View mode
            //btnEditSave.Text = ResourceManager.GetString("editText");
            //EnableCandidateControls(false);
            //divAddRemoveExpectancy.Visible = false;
            //divKnowledgeFunctionEdit.Visible = false;
            if (candidate != null)
            {
                string addBackUrl = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
                {
                    addBackUrl = "&backurl=visible";
                }
                string original = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["originalPage"]) && Request.QueryString["originalPage"] == "Action")
                {
                    original = "&originalPage=Action";
                }
                Response.Redirect(string.Format("~/CandidateProfile.aspx?CandidateID={0}&mode=view" + addBackUrl + original, candidate.CandidateId));
            }
        }
    }


    protected void OnButtonCandidateCancelClicked(object sender, EventArgs e)
    {
        Candidate curCan = SessionManager.CurrentCandidate;
        if (curCan != null)
        {
            //FillData(curCan);
            //FillComboboxWithCandidate(curCan);
            //BindExpectacyOfCandidate(curCan);
            //BindKnowledgeGridOfCurrentCandidate(curCan);
            //BindFunctionGridOfCurrentCandidate(curCan);
            /*btnEditSave.Text = ResourceManager.GetString("editText");
            EnableCandidateControls(false);*/            
            string url = !string.IsNullOrEmpty(Request.QueryString["tab"]) ? Request.Url.PathAndQuery.Replace("&tab=action", "") : Request.Url.PathAndQuery;
            
            if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                url = url.Replace(Request.QueryString["mode"], "view");
            else
                url += "&mode=view";
            Response.Redirect(url, true);
        }
        else if (SessionManager.LastNameSearchCriteria != null)
        {
            string lastname = SessionManager.LastNameSearchCriteria;
            Response.Redirect("~/Candidates.aspx?lastname=" + lastname);
        }
    }

    protected void OnLinkBackToActionClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hidActionUrl.Value)
            && Request.UrlReferrer != null)
        {
            Response.Redirect(hidActionUrl.Value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private Candidate SaveCandidateProfile()
    {
        //Save data.
        Candidate candidate = SaveCandidateData();
        if (candidate != null)
        {
            SaveCandidateExpect(candidate);
            if (SessionManager.NewCandidateTelephoneList != null && SessionManager.NewCandidateTelephoneList.Count > 0)
            {
                SaveContactTelephone(candidate);
            }
            if (!string.IsNullOrEmpty(radTabStripCandidateProfile.FindTabByValue("ExpectancyView").PageViewID))
                SaveCandidateExpectancies(candidate);
            if (!string.IsNullOrEmpty(radTabStripCandidateProfile.FindTabByValue("EvaluationView").PageViewID))
                SaveCandidateEvaluation(candidate);
            if (!string.IsNullOrEmpty(radTabStripCandidateProfile.FindTabByValue("KnowledgeView").PageViewID))
                SaveCandidateKnowledgeFunction(candidate);            
        }
        return candidate;
    }

    

    /// <summary>
    /// Enable or disabled controls on tabs of Candidate.
    /// When the mode is in view mode, all controls are disabled, not to edit.
    /// When the mode in is Edit mode, all controls are enabled, so they can be editted.
    /// </summary>
    /// <param name="enable"></param>
    private void EnableCandidateControls(bool enable)
    {
        btnCancel.Visible = enable;
        if (enable)
            btnEditSave.Text = ResourceManager.GetString("saveText");
        else
            btnEditSave.Text = ResourceManager.GetString("editText");
        //Common
        txtLastName.ReadOnly = !enable;
        txtFirstName.ReadOnly = !enable;
        ddlUnit.Enabled = enable;
        ddlInterviewer.Enabled = enable;
        datDateInterview.Enabled = enable;

        //Tab general
        txtAddress.ReadOnly = !enable;
        txtZip.ReadOnly = !enable;
        txtCity.ReadOnly = !enable;
        ddlCountry.Enabled = enable;
        ddlGenre.Enabled = enable;
        ddlNationality.Enabled = enable;
        datDateOfBirth.Enabled = enable;
        lnkCanContactAdd.Visible = enable;
        ddlCanArea.Enabled = enable;
        btnAddCanArea.Enabled = enable;
        btnRemoveCanArea.Enabled = enable;

        ddlCompanyWish.Enabled = enable;
        txtFunction.ReadOnly = !enable;
        ddlSalaryWish.Enabled = enable;
        ddlContractTypeWish.Enabled = enable;
        txtMotivation.ReadOnly = !enable;
        datCreationDate.Enabled = enable;
        ddlStatus.Enabled = enable;
        txtInactivityReason.ReadOnly = !enable;
        txtRemarkGeneral.ReadOnly = !enable;
        gridContact.Columns[4].Display = enable;
        gridContact.Columns[5].Display = enable;

        //Tab Expectancies
        btnCanExpectEdit.Enabled = enable;
        //divAddRemoveExpectancy.Visible = enable;

        //Tab Studies&Experience
        lnkAddNewStudy.Visible = enable;
        lnkAddNewExperience.Visible = enable;
        gridStudies.Columns[5].Display = enable;
        gridStudies.Columns[6].Display = enable;
        gridExperience.Columns[6].Display = enable;
        gridExperience.Columns[7].Display = enable;

        //Tab Evaluation
        txtPresentationEval.ReadOnly = !enable;
        txtVerbal.ReadOnly = !enable;
        txtAutonomy.ReadOnly = !enable;
        txtPersonality.ReadOnly = !enable;
        txtGlobal.ReadOnly = !enable;
        txtOtherEval.ReadOnly = !enable;
        txtMotivationEval.ReadOnly = !enable;
        ddlFrenchLang.Enabled = enable;
        ddlDutchLang.Enabled = enable;
        ddlEnglishLang.Enabled = enable;
        ddlGermanLang.Enabled = enable;
        ddlSpainishLang.Enabled = enable;
        ddlItalianLang.Enabled = enable;
        ddlOtherLang.Enabled = enable;
        ddlOtherLangSkill.Enabled = enable;

        //Tab Knowledge&Functions
        btnKnowledgeEdit.Enabled = enable;
        btnFunctionEdit.Enabled = enable;
        //divKnowledgeFunctionEdit.Visible = enable;

        //Tab Presentation
        txtPresentationText.ReadOnly = !enable;
        ddlCompany.Enabled = enable;
        btnGeneratePresentation.Enabled = enable;
        btnSendPresentation.Enabled = enable && !string.IsNullOrEmpty(Request.QueryString["CandidateId"]);
        //Tab Action
        lnkAddNewAction.Visible = enable;
        gridActions.Columns[8].Display = enable;
        gridActions.Columns[9].Display = enable;
        //Tab Documents
        grdDocuments.Columns[3].Display = enable;
        grdDocuments.Columns[4].Display = enable;
        lnkAddNewDocument.Visible = enable;

        hidMode.Value = enable ? "edit" : "view";
    }

    /// <summary>
    /// Save general data of candidate to database.
    /// </summary>
    private Candidate SaveCandidateData()
    {
        if (string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtFirstName.Text))
        {
            string message = ResourceManager.GetString("messageCanNameMustNotBeEmpty");
            string script = "<script type=\"text/javascript\">";
            script += " alert(\"" + message + "\")";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
            return null;
        }
        bool isNew = false;
        //Save Candidate
        Candidate candidate = new Candidate();
        if (SessionManager.CurrentCandidate != null)
        {
            candidate = SessionManager.CurrentCandidate;
        }
        else
        {
            isNew = true;
            candidate = new Candidate();
        }
        //Common
        candidate.LastName = txtLastName.Text.Trim();
        candidate.FirstName = txtFirstName.Text.Trim();
        candidate.Unit = ddlUnit.SelectedValue;
        if (!string.IsNullOrEmpty(ddlInterviewer.SelectedValue))
            candidate.Interviewer = ddlInterviewer.SelectedValue;
        else
            candidate.Interviewer = null;
        candidate.DateOfInterView = datDateInterview.SelectedDate;
        candidate.LastModifDate = DateTime.Now;

        //Tab general
        candidate.Address = txtAddress.Text.Trim();
        candidate.ZipCode = txtZip.Text.Trim();
        candidate.City = txtCity.Text.Trim();
        candidate.CountryCode = ddlCountry.SelectedValue;
        candidate.Gender = ddlGenre.SelectedValue;
        if (!string.IsNullOrEmpty(ddlNationality.SelectedValue))
            candidate.Nationlity = ddlNationality.SelectedValue;
        else
            candidate.Nationlity = null;
        candidate.BirthDate = datDateOfBirth.SelectedDate;
        candidate.CreationDate = datCreationDate.SelectedDate;
        if (string.IsNullOrEmpty(ddlStatus.SelectedValue))
            candidate.Inactive = null;
        else
            candidate.Inactive = bool.Parse(ddlStatus.SelectedValue);
        candidate.ReasonDesactivation = txtInactivityReason.Text.Trim();
        candidate.Remark = txtRemarkGeneral.Text.Trim();
        //tab presentation
        if (!string.IsNullOrEmpty(radTabStripCandidateProfile.FindTabByValue("PresentationView").PageViewID))
            candidate.Presentation = txtPresentationText.Text;


        CandidateRepository repo = new CandidateRepository();
        if (isNew)
            repo.Insert(candidate);
        else
            repo.Update(candidate);
        candidate = repo.FindOne(candidate);
        SessionManager.CurrentCandidate = candidate;
        SaveLastViewCandidatesToCookie(candidate);
        if (isNew)
        {
            string script = "<script type='text/javascript'>";
            script += "onSaveOrLoadCandidateProfilePage();";
            script += "</script>";
            if (!ClientScript.IsClientScriptBlockRegistered("onSaveOrLoadCandidateProfilePage"))
                ClientScript.RegisterStartupScript(this.GetType(), "onSaveOrLoadCandidateProfilePage", script);
        }
        return candidate;
    }

    /// <summary>
    /// save the candidate telephone in case adding a new candidate
    /// </summary>
    private void SaveContactTelephone(Candidate candidate)
    {
        if (SessionManager.NewCandidateTelephoneList != null && SessionManager.NewCandidateTelephoneList.Count > 0)
        {
            foreach (CandidateTelephone phone in SessionManager.NewCandidateTelephoneList)
            {
                CandidateTelephoneRepository phoneRepo = new CandidateTelephoneRepository();

                phone.CandidateID = candidate.CandidateId;
                phoneRepo.Insert(phone);
            }
        }
    }
    /// <summary>
    /// Save candidate expectation
    /// </summary>
    /// <param name="candidate"></param>
    private void SaveCandidateExpect(Candidate candidate)
    {
        bool isNew = false;
        CandidateExpectation expect = null;
        if (SessionManager.CandidateExpectation != null)
        {
            expect = SessionManager.CandidateExpectation;
        }
        else
        {
            isNew = true;
            expect = new CandidateExpectation();
            expect.CandidateID = candidate.CandidateId;
        }

        //expect.Region = txtArea.Text.Trim();
        if (!string.IsNullOrEmpty(hiddenCanAreaList.Value))
            expect.Region = hiddenCanAreaList.Value as string;
        else
            expect.Region = null;
        expect.Company = ddlCompanyWish.Text.Trim();
        expect.Function = txtFunction.Text.Trim();
        expect.SalaryLevel = ddlSalaryWish.Text.Trim();
        expect.TypeofContract = ddlContractTypeWish.Text.Trim();
        expect.Motivation = txtMotivation.Text.Trim();
        CandidateExpectationRepository repo = new CandidateExpectationRepository();
        if (isNew)
            //Use the specific insert function.
            repo.InsertNewExpect(expect);
        else
            repo.Update(expect);
        SessionManager.CandidateExpectation = expect;
        listCanArea.Items.Clear();
        if (!string.IsNullOrEmpty(expect.Region))
        {
            string[] areaArray = expect.Region.Split(';');
            for (int i = 0; i < areaArray.Length; i++)
            {
                listCanArea.Items.Add(new ListItem(areaArray[i].Trim(), areaArray[i].Trim()));
            }
        }
    }

    /// <summary>
    /// Save candidate evaluation
    /// </summary>
    /// <param name="candidate"></param>
    private void SaveCandidateEvaluation(Candidate candidate)
    {
        bool isNew = false;
        CandidateEvaluation evaluation = null;
        if (SessionManager.CandidateEvaluation != null)
        {
            evaluation = SessionManager.CandidateEvaluation;
        }
        else
        {
            isNew = true;
            evaluation = new CandidateEvaluation();
            evaluation.CandidateID = candidate.CandidateId;
        }

        evaluation.PresentationEvaluation = txtPresentationEval.Text.Trim();
        evaluation.GlobalEvaluation = txtGlobal.Text.Trim();
        evaluation.ExpressionEvaluation = txtVerbal.Text.Trim();
        evaluation.VariousMatters = txtOtherEval.Text.Trim();
        evaluation.SelfEvaluation = txtAutonomy.Text.Trim();
        evaluation.MotivationEvaluation = txtMotivationEval.Text.Trim();
        evaluation.PersonalityEvaluation = txtPersonality.Text.Trim();
        evaluation.French = Convert.ToInt16(ddlFrenchLang.SelectedValue);
        evaluation.German = Convert.ToInt16(ddlGermanLang.SelectedValue);
        evaluation.Dutch = Convert.ToInt16(ddlDutchLang.SelectedValue);
        evaluation.Spanish = Convert.ToInt16(ddlSpainishLang.SelectedValue);
        evaluation.English = Convert.ToInt16(ddlEnglishLang.SelectedValue);
        evaluation.Italian = Convert.ToInt16(ddlItalianLang.SelectedValue);

        //Other language:
        evaluation.AdditionLanguage = ddlOtherLang.SelectedValue;
        if (evaluation.AdditionLanguage != null)
        {
            evaluation.AdditionLanguageScore = Convert.ToInt16(ddlOtherLangSkill.SelectedValue);
        }

        CandidateEvaluationRepository repo = new CandidateEvaluationRepository();
        if (isNew)
            repo.InsertNewEvaluation(evaluation);
        else
            repo.Update(evaluation);
        SessionManager.CandidateEvaluation = evaluation;
    }

    /// <summary>
    /// Save candidate knowledge and experience.
    /// </summary>
    /// <param name="candidate"></param>
    private void SaveCandidateKnowledgeFunction(Candidate candidate)
    {
        //With knowledge list. 
        CandidateKnowledgeRepository candidateKnowRepo = new CandidateKnowledgeRepository();
        IList<CandidateKnowledge> oldKnowledgeList = candidateKnowRepo.GetCandidateKnowledgeByCandidateID(candidate.CandidateId);
        foreach (CandidateKnowledge newItem in SessionManager.CanKnowledgeOldList)
        {
            bool isNew = true;
            foreach (CandidateKnowledge oldItem in oldKnowledgeList)
            {
                if (oldItem.KnowledgeID.Value == newItem.KnowledgeID.Value)
                {
                    isNew = false;
                    break;
                }
            }
            if (isNew)
            {
                newItem.CandidateID = candidate.CandidateId;
                candidateKnowRepo.Insert(newItem);
            }
        }
        foreach (CandidateKnowledge oldItem in oldKnowledgeList)
        {
            bool isDelete = true;
            foreach (CandidateKnowledge deleteItem in SessionManager.CanKnowledgeOldList)
            {
                if (deleteItem.KnowledgeID.Value == oldItem.KnowledgeID.Value)
                {
                    isDelete = false;
                    break;
                }
            }
            if (isDelete)
                candidateKnowRepo.Delete(oldItem);
        }

        //With Function list.
        CandidateFunctionRepository candidateFuncRepo = new CandidateFunctionRepository();
        IList<CandidateFunction> oldFunctList = candidateFuncRepo.GetCandidateFunctionByCandidateID(candidate.CandidateId);
        foreach (CandidateFunction newIem in SessionManager.CanFunctionOldList)
        {
            bool isNew = true;
            foreach (CandidateFunction oldItem in oldFunctList)
            {
                if (oldItem.FunctionID.Value == newIem.FunctionID.Value)
                {
                    isNew = false;
                    break;
                }
            }
            if (isNew)
            {
                newIem.CandidateID = candidate.CandidateId;
                candidateFuncRepo.Insert(newIem);
            }
        }
        foreach (CandidateFunction oldItem in oldFunctList)
        {
            bool isDelete = true;
            foreach (CandidateFunction deleteItem in SessionManager.CanFunctionOldList)
            {
                if (deleteItem.FunctionID.Value == oldItem.FunctionID.Value)
                {
                    isDelete = false;
                    break;
                }
            }
            if (isDelete)
                candidateFuncRepo.Delete(oldItem);
        }
    }

    private void SaveCandidateExpectancies(Candidate candidate)
    {
        CandidateExpectancyRepository candidateExpectRepo = new CandidateExpectancyRepository();
        IList<CandidateExpectancy> oldExpectList = candidateExpectRepo.GetCandidateExpectancyOfCandidate(candidate.CandidateId);
        foreach (CandidateExpectancy newIem in SessionManager.CanExpectOldList)
        {
            bool isNew = true;
            foreach (CandidateExpectancy oldItem in oldExpectList)
            {
                if (oldItem.FunctionID.Value == newIem.FunctionID.Value)
                {
                    isNew = false;
                    break;
                }
            }
            if (isNew)
            {
                newIem.CandidatID = candidate.CandidateId;
                candidateExpectRepo.Insert(newIem);
            }
        }
        foreach (CandidateExpectancy oldItem in oldExpectList)
        {
            bool isDelete = true;
            foreach (CandidateExpectancy deleteItem in SessionManager.CanExpectOldList)
            {
                if (deleteItem.FunctionID.Value == oldItem.FunctionID.Value)
                {
                    isDelete = false;
                    break;
                }
            }
            if (isDelete)
                candidateExpectRepo.Delete(oldItem);
        }
    }
    #endregion

    #region Tab General
    protected void OnGridContactPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridContact.CurrentPageIndex = e.NewPageIndex;
        BindContactGridOfCurrentCandidate(null);
    }

    private void BindDropdownCompanies()
    {
       /* CompanyRepository companyRepo = new CompanyRepository();
        ddlCompany.DataTextField = "CompanyName";
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataSource = companyRepo.GetAllCompanies();
        ddlCompany.DataBind();
        ddlCompany.SelectedIndex = 0;*/
    }

    private void BindContactGridOfCurrentCandidate(Candidate currentCandidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (currentCandidate != null)
            candidateID = currentCandidate.CandidateId;

        if (candidateID != -1)
        {
            CandidateTelephoneRepository repo = new CandidateTelephoneRepository();
            IList<CandidateTelephone> contactList = repo.GetCandidateTelephonesByCandidateID(candidateID);
            foreach (CandidateTelephone item in contactList)
            {
                if (!string.IsNullOrEmpty(item.Type))
                {
                    if (item.Type == "T")
                        item.TypeLabel = ResourceManager.GetString("candidateContactPhone");
                    else if (item.Type == "F")
                        item.TypeLabel = ResourceManager.GetString("candidateContactFax");
                    else if (item.Type == "G")
                        item.TypeLabel = ResourceManager.GetString("candidateContactMobile");
                    else if (item.Type == "E")
                        item.TypeLabel = ResourceManager.GetString("candidateContactEmail");
                }
            }
            gridContact.DataSource = contactList;
        }
        else //creating a new candidate
        {
            if (SessionManager.NewCandidateTelephoneList != null)
            {
                gridContact.DataSource = SessionManager.NewCandidateTelephoneList;
            }
            else
                gridContact.DataSource = new List<CandidateTelephone>();
        }
        gridContact.DataBind();
    }

    protected void OnGridContactNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;

        if (candidateID != -1)
        {
            CandidateTelephoneRepository repo = new CandidateTelephoneRepository();
            IList<CandidateTelephone> contactList = repo.GetCandidateTelephonesByCandidateID(candidateID);
            foreach (CandidateTelephone item in contactList)
            {
                if (!string.IsNullOrEmpty(item.Type))
                {
                    if (item.Type == "T")
                        item.TypeLabel = ResourceManager.GetString("candidateContactPhone");
                    else if (item.Type == "F")
                        item.TypeLabel = ResourceManager.GetString("candidateContactFax");
                    else if (item.Type == "G")
                        item.TypeLabel = ResourceManager.GetString("candidateContactMobile");
                    else if (item.Type == "E")
                        item.TypeLabel = ResourceManager.GetString("candidateContactEmail");
                }
            }
            gridContact.DataSource = contactList;
        }
        else
        {
            if (string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            {
                gridContact.DataSource = SessionManager.NewCandidateTelephoneList;//new List<CandidateTelephone>();
            }
        }
    }

    protected void OnGridContactItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteCanContactColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((CandidateTelephone)e.Item.DataItem).TelePhoneId.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditCanContactColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
    }

    protected void OnCandidateContactDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int canTelephonID = int.Parse(lnkItem.CommandArgument);
        if (canTelephonID > 0) //existed in database
        {
            CandidateTelephone deleteItem = new CandidateTelephone(canTelephonID);
            CandidateTelephoneRepository repo = new CandidateTelephoneRepository();
            repo.Delete(deleteItem);
        }
        else
        {
            List<CandidateTelephone> list = SessionManager.NewCandidateTelephoneList;
            CandidateTelephone existedItem = list.Find(delegate(CandidateTelephone t) { return t.TelePhoneId == canTelephonID; });
            if (existedItem != null)
            {
                list.Remove(existedItem);
                SessionManager.NewCandidateTelephoneList = list;
            }
        }
        BindContactGridOfCurrentCandidate(null);
    }

    #endregion

    #region Tab Expectancies

    private void BindExpectacyOfCandidate(Candidate candidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (candidate != null)
            candidateID = candidate.CandidateId;

        if (candidateID != -1)
        {
            CandidateExpectancyRepository candExpectRepo = new CandidateExpectancyRepository();
            SessionManager.CanExpectOldList = candExpectRepo.GetCandidateExpectancyOfCandidate(candidate.CandidateId);
            expectancyGrid.DataSource = SessionManager.CanExpectOldList;
        }
        else
            expectancyGrid.DataSource = new List<CandidateExpectancy>();
        expectancyGrid.DataBind();
    }

    protected void OnBtnExpectEditClicked(object sender, EventArgs e)
    {
        divAddRemoveExpectancy.Visible = true;
        btnCanExpectEdit.Enabled = false;

        SessionManager.CanExpectDesList = CopyCanExpectList(SessionManager.CanExpectOldList);
        gridCanExpectDestination.DataSource = SessionManager.CanExpectDesList;
        gridCanExpectDestination.DataBind();

        ParamFunctionFamRepository repo = new ParamFunctionFamRepository();
        ddlCanExpectUnit.DataSource = repo.GetFunctionFamGenreList();
        ddlCanExpectUnit.DataBind();
        OnExpectUnitItemChanged(ddlCanExpectUnit, null);
    }

    private IList<CandidateExpectancy> CopyCanExpectList(IList<CandidateExpectancy> sourceList)
    {
        IList<CandidateExpectancy> result = new List<CandidateExpectancy>();
        foreach (CandidateExpectancy item in sourceList)
        {
            CandidateExpectancy newItem = new CandidateExpectancy();
            newItem.CandidateExpectancyID = item.CandidateExpectancyID;
            newItem.CandidatID = item.CandidatID;
            newItem.FunctionFam = item.FunctionFam;
            newItem.FunctionID = item.FunctionID;
            newItem.Group = item.Group;
            newItem.Type = item.Type;
            result.Add(newItem);
        }
        return result;
    }

    protected void OnExpectUnitItemChanged(object sender, EventArgs e)
    {
        string unit = (string)((RadComboBox)sender).SelectedValue;

        ddlCanExpectFam.DataTextField = "FonctionFamID";
        ddlCanExpectFam.DataValueField = "FonctionFamID";
        ParamFunctionFamRepository repo = new ParamFunctionFamRepository();
        ddlCanExpectFam.DataSource = repo.GetParamFunctionFamByGenre(unit);
        ddlCanExpectFam.DataBind();
        OnExpectFamItemChanged(ddlCanExpectFam, null);
    }

    protected void OnExpectFamItemChanged(object sender, EventArgs e)
    {
        string family = (string)((RadComboBox)sender).SelectedValue;
        listExpectOriginal.DataTextField = "Code";
        listExpectOriginal.DataValueField = "FunctionID";
        CandidateFunctionRepository repo = new CandidateFunctionRepository();
        listExpectOriginal.DataSource = repo.GetAllParamFunctionByFuntionFamID(family);
        listExpectOriginal.DataBind();

    }

    protected void OnBtnExpectAddClicked(object sender, EventArgs e)
    {
        for (int i = listExpectOriginal.Items.Count - 1; i >= 0; i--)
        {
            ListItem selectedItem = listExpectOriginal.Items[i];
            if (selectedItem.Selected)
            {
                string value = selectedItem.Value;

                bool itemExist = false;
                foreach (CandidateExpectancy oldItem in SessionManager.CanExpectDesList)
                {
                    if (oldItem.FunctionID.Value.ToString() == value)
                    {
                        itemExist = true;
                    }
                }
                if (!itemExist)
                {
                    CandidateExpectancy addedItem = new CandidateExpectancy();
                    addedItem.CandidateExpectancyID = -1;
                    addedItem.FunctionID = int.Parse(value);
                    addedItem.FunctionFam = selectedItem.Text;
                    addedItem.Type = ddlCanExpectUnit.SelectedValue;
                    addedItem.Group = ddlCanExpectFam.SelectedValue;
                    SessionManager.CanExpectDesList.Add(addedItem);
                }
            }
        }
        gridCanExpectDestination.DataSource = SessionManager.CanExpectDesList;
        gridCanExpectDestination.DataBind();
    }

    protected void OnBtnExpectRemoveClicked(object sender, EventArgs e)
    {
        foreach (GridItem item in gridCanExpectDestination.SelectedItems)
        {
            int id = (int)item.OwnerTableView.DataKeyValues[item.ItemIndex]["FunctionID"];
            foreach (CandidateExpectancy oldItem in SessionManager.CanExpectDesList)
            {
                if (id == oldItem.FunctionID.Value)
                {
                    SessionManager.CanExpectDesList.Remove(oldItem);
                    break;
                }
            }
        }
        gridCanExpectDestination.DataSource = SessionManager.CanExpectDesList;
        gridCanExpectDestination.DataBind();

    }

    protected void OnBtnExpectOKClicked(object sender, EventArgs e)
    {
        SessionManager.CanExpectOldList = CopyCanExpectList(SessionManager.CanExpectDesList);
        expectancyGrid.DataSource = SessionManager.CanExpectOldList;
        expectancyGrid.DataBind();

        divAddRemoveExpectancy.Visible = false;
        btnCanExpectEdit.Enabled = true;
    }

    protected void OnBtnExpectCancelClicked(object sender, EventArgs e)
    {
        divAddRemoveExpectancy.Visible = false;
        btnCanExpectEdit.Enabled = true;
    }
    #endregion

    #region Tab Study/Experience

    #region Grid Study
    protected void OnGridStudyPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridStudies.CurrentPageIndex = e.NewPageIndex;
        BindStudyGridOfCurrentCandidate(null);
    }

    protected void OnGridStudy_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie studyGridPageSizeCookie = new HttpCookie("cand_studygrdps");
        studyGridPageSizeCookie.Expires.AddDays(30);
        studyGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(studyGridPageSizeCookie);
    }

    private void BindStudyGridOfCurrentCandidate(Candidate currentCandidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (currentCandidate != null)
            candidateID = currentCandidate.CandidateId;

        if (candidateID != -1)
            gridStudies.DataSource = new CandidateTrainingRepository().GetCandidateTrainingByCandidateID(candidateID);
        else
            gridStudies.DataSource = new List<CandidateTraining>();
        gridStudies.DataBind();
    }

    protected void OnGridStudyNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (gridStudies.Visible)
        {
            int candidateID = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
                candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
            else if (SessionManager.CurrentCandidate != null)
                candidateID = SessionManager.CurrentCandidate.CandidateId;

            if (candidateID != -1)
                gridStudies.DataSource = new CandidateTrainingRepository().GetCandidateTrainingByCandidateID(candidateID);
            else
                gridStudies.DataSource = new List<CandidateTraining>();
        }
    }

    protected void OnGridStudyItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteCanStudyColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((CandidateTraining)e.Item.DataItem).CandidateFormationID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditCanStudyColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
    }

    protected void OnCandidateStudyDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int canFormationID = int.Parse(lnkItem.CommandArgument);
        CandidateTraining deleteItem = new CandidateTraining(canFormationID);
        CandidateTrainingRepository repo = new CandidateTrainingRepository();
        repo.Delete(deleteItem);

        BindStudyGridOfCurrentCandidate(null);
    }
    #endregion

    #region Grid Experience
    protected void OnGridExperiencePageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridExperience.CurrentPageIndex = e.NewPageIndex;
        BindExperienceGridOfCurrentCandidate(null);
    }

    protected void OnGridExperience_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie experienceGridPageSizeCookie = new HttpCookie("cand_expgrdps");
        experienceGridPageSizeCookie.Expires.AddDays(30);
        experienceGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(experienceGridPageSizeCookie);
    }

    private void BindExperienceGridOfCurrentCandidate(Candidate currentCandidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (currentCandidate != null)
            candidateID = currentCandidate.CandidateId;

        if (candidateID != -1)
            gridExperience.DataSource = new CandidateExperienceRepository().GetCandidateExperienceByCandidateID(candidateID);
        else
            gridExperience.DataSource = new List<CandidateExperience>();
        gridExperience.DataBind();
    }

    protected void OnGridExperienceNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (gridExperience.Visible)
        {
            int candidateID = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
                candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
            else if (SessionManager.CurrentCandidate != null)
                candidateID = SessionManager.CurrentCandidate.CandidateId;

            if (candidateID != -1)
                gridExperience.DataSource = new CandidateExperienceRepository().GetCandidateExperienceByCandidateID(candidateID);
            else
                gridExperience.DataSource = new List<CandidateExperience>();
        }
    }

    protected void OnGridExperienceItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteCanExperieceColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((CandidateExperience)e.Item.DataItem).ExperienceID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditCanExperienceColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
    }

    protected void OnCandidateExperieceDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int canExperienceID = int.Parse(lnkItem.CommandArgument);
        CandidateExperience deleteItem = new CandidateExperience(canExperienceID);
        CandidateExperienceRepository repo = new CandidateExperienceRepository();
        repo.Delete(deleteItem);

        BindExperienceGridOfCurrentCandidate(null);
    }
    #endregion

    #endregion

    #region Tab KnowLegde/Function
    private void BindKnowledgeGridOfCurrentCandidate(Candidate currentCandidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (currentCandidate != null)
            candidateID = currentCandidate.CandidateId;

        if (candidateID != -1)
        {
            SessionManager.CanKnowledgeOldList = new CandidateKnowledgeRepository().GetCandidateKnowledgeByCandidateID(candidateID);
            gridKnowledgeOld.DataSource = SessionManager.CanKnowledgeOldList;
        }
        else
            gridKnowledgeOld.DataSource = new List<CandidateKnowledge>();
        gridKnowledgeOld.DataBind();
    }

    private void BindFunctionGridOfCurrentCandidate(Candidate currentCandidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (currentCandidate != null)
            candidateID = currentCandidate.CandidateId;

        if (candidateID != -1)
        {
            SessionManager.CanFunctionOldList = new CandidateFunctionRepository().GetCandidateFunctionByCandidateID(candidateID);
            gridFunctionOld.DataSource = SessionManager.CanFunctionOldList;
        }
        else
            gridFunctionOld.DataSource = new List<CandidateFunction>();
        gridFunctionOld.DataBind();
    }

    protected void OnKnowledgeEditClicked(object sender, EventArgs e)
    {
        divKnowledgeFunctionEdit.Visible = true;
        btnFunctionEdit.Enabled = false;
        btnKnowledgeEdit.Enabled = false;
        lblKnowFuncDestination.Text = ResourceManager.GetString("lblCanKnowledgeGrid");
        lblFieldSetKnowFunc.Text = ResourceManager.GetString("lblFieldSetKnowFunc_Knowledge");

        gridKnowFuncDestination.MasterTableView.DataKeyNames = new string[] { "KnowledgeID" };
        gridKnowFuncDestination.MasterTableView.DataMember = "CandidateKnowledge";
        gridKnowFuncDestination.MasterTableView.Columns[2].HeaderText = ResourceManager.GetString("lblCanKnowledgeGrid");

        SessionManager.CanKnowledgeDesList = CopyCanKnowledgeList(SessionManager.CanKnowledgeOldList);
        gridKnowFuncDestination.DataSource = SessionManager.CanKnowledgeDesList;
        gridKnowFuncDestination.DataBind();

        ParamKnowledgeFamRepository repo = new ParamKnowledgeFamRepository();
        ddlKnowFuncUnit.DataSource = repo.GetKnowledgeFamGenreList();
        ddlKnowFuncUnit.DataBind();
        OnKnowFuncUnitItemChanged(ddlKnowFuncUnit, null);
    }

    protected void OnFunctionEditClicked(object sender, EventArgs e)
    {
        divKnowledgeFunctionEdit.Visible = true;
        btnFunctionEdit.Enabled = false;
        btnKnowledgeEdit.Enabled = false;
        lblKnowFuncDestination.Text = ResourceManager.GetString("lblCanFunctionGrid");
        lblFieldSetKnowFunc.Text = ResourceManager.GetString("lblFieldSetKnowFunc_Function");

        gridKnowFuncDestination.MasterTableView.DataKeyNames = new string[] { "FunctionID" };
        gridKnowFuncDestination.MasterTableView.DataMember = "CandidateFunction";
        gridKnowFuncDestination.MasterTableView.Columns[2].HeaderText = ResourceManager.GetString("lblCanFunctionGrid");

        SessionManager.CanFunctionDesList = CopyCanFunctionList(SessionManager.CanFunctionOldList);
        gridKnowFuncDestination.DataSource = SessionManager.CanFunctionDesList;
        gridKnowFuncDestination.DataBind();

        ParamFunctionFamRepository repo = new ParamFunctionFamRepository();
        ddlKnowFuncUnit.DataSource = repo.GetFunctionFamGenreList();
        ddlKnowFuncUnit.DataBind();
        OnKnowFuncUnitItemChanged(ddlKnowFuncUnit, null);
    }

    private IList<CandidateFunction> CopyCanFunctionList(IList<CandidateFunction> sourceList)
    {
        IList<CandidateFunction> result = new List<CandidateFunction>();
        foreach (CandidateFunction item in sourceList)
        {
            CandidateFunction newItem = new CandidateFunction();
            newItem.CandidateFunctionID = item.CandidateFunctionID;
            newItem.CandidateID = item.CandidateID;
            newItem.Code = item.Code;
            newItem.FunctionID = item.FunctionID;
            newItem.Group = item.Group;
            newItem.Type = item.Type;
            result.Add(newItem);
        }
        return result;
    }

    private IList<CandidateKnowledge> CopyCanKnowledgeList(IList<CandidateKnowledge> sourceList)
    {
        IList<CandidateKnowledge> result = new List<CandidateKnowledge>();
        foreach (CandidateKnowledge item in sourceList)
        {
            CandidateKnowledge newItem = new CandidateKnowledge();
            newItem.CandidateKnowledgeID = item.CandidateKnowledgeID;
            newItem.CandidateID = item.CandidateID;
            newItem.Code = item.Code;
            newItem.KnowledgeID = item.KnowledgeID;
            newItem.Group = item.Group;
            newItem.Type = item.Type;
            result.Add(newItem);
        }
        return result;
    }

    protected void OnKnowFuncUnitItemChanged(object sender, EventArgs e)
    {
        string unit = (string)((RadComboBox)sender).SelectedValue;
        if (lblKnowFuncDestination.Text == ResourceManager.GetString("lblCanFunctionGrid"))
        {
            ParamFunctionFamRepository repo = new ParamFunctionFamRepository();
            ddlKnowFuncFam.DataSource = repo.GetParamFunctionFamByGenre(unit);
            ddlKnowFuncFam.DataTextField = "FonctionFamID";
            ddlKnowFuncFam.DataValueField = "FonctionFamID";
        }
        else
        {
            ParamKnowledgeFamRepository repo = new ParamKnowledgeFamRepository();
            ddlKnowFuncFam.DataSource = repo.GetParamKnowledgeFamByGenre(unit);
            ddlKnowFuncFam.DataTextField = "ConFamilleID";
            ddlKnowFuncFam.DataValueField = "ConFamilleID";
        }
        ddlKnowFuncFam.DataBind();
        OnKnowFuncFamItemChanged(ddlKnowFuncFam, null);
    }

    protected void OnKnowFuncFamItemChanged(object sender, EventArgs e)
    {
        string family = (string)((RadComboBox)sender).SelectedValue;
        if (lblKnowFuncDestination.Text == ResourceManager.GetString("lblCanFunctionGrid"))
        {
            listKnowFuncOriginal.DataTextField = "Code";
            listKnowFuncOriginal.DataValueField = "FunctionID";
            CandidateFunctionRepository repo = new CandidateFunctionRepository();
            listKnowFuncOriginal.DataSource = repo.GetAllParamFunctionByFuntionFamID(family);
            listKnowFuncOriginal.DataBind();
        }
        else
        {
            listKnowFuncOriginal.DataTextField = "Code";
            listKnowFuncOriginal.DataValueField = "KnowledgeID";
            CandidateKnowledgeRepository repo = new CandidateKnowledgeRepository();
            listKnowFuncOriginal.DataSource = repo.GetAllParamKnowledgeByKnowledgeFamID(family);
            listKnowFuncOriginal.DataBind();
        }
    }

    protected void OnBtnKnowFuncAddClicked(object sender, EventArgs e)
    {
        if (lblKnowFuncDestination.Text == ResourceManager.GetString("lblCanFunctionGrid"))
        {
            for (int i = listKnowFuncOriginal.Items.Count - 1; i >= 0; i--)
            {
                ListItem selectedItem = listKnowFuncOriginal.Items[i];
                if (selectedItem.Selected)
                {
                    string value = selectedItem.Value;

                    bool itemExist = false;
                    foreach (CandidateFunction oldItem in SessionManager.CanFunctionDesList)
                    {
                        if (oldItem.FunctionID.Value.ToString() == value)
                        {
                            itemExist = true;
                        }
                    }
                    if (!itemExist)
                    {
                        CandidateFunction addedItem = new CandidateFunction();
                        addedItem.CandidateFunctionID = -1;
                        addedItem.FunctionID = int.Parse(value);
                        addedItem.Code = selectedItem.Text;
                        addedItem.Type = ddlKnowFuncUnit.SelectedValue;
                        addedItem.Group = ddlKnowFuncFam.SelectedValue;
                        SessionManager.CanFunctionDesList.Add(addedItem);
                    }
                }
            }
            gridKnowFuncDestination.DataSource = SessionManager.CanFunctionDesList;
            gridKnowFuncDestination.DataBind();
        }
        else
        {
            for (int i = listKnowFuncOriginal.Items.Count - 1; i >= 0; i--)
            {
                ListItem selectedItem = listKnowFuncOriginal.Items[i];
                if (selectedItem.Selected)
                {
                    string value = selectedItem.Value;

                    bool itemExist = false;
                    foreach (CandidateKnowledge oldItem in SessionManager.CanKnowledgeDesList)
                    {
                        if (oldItem.KnowledgeID.Value.ToString() == value)
                        {
                            itemExist = true;
                        }
                    }
                    if (!itemExist)
                    {
                        CandidateKnowledge addedItem = new CandidateKnowledge();
                        addedItem.CandidateKnowledgeID = -1;
                        addedItem.KnowledgeID = int.Parse(value);
                        addedItem.Code = selectedItem.Text;
                        addedItem.Type = ddlKnowFuncUnit.SelectedValue;
                        addedItem.Group = ddlKnowFuncFam.SelectedValue;
                        SessionManager.CanKnowledgeDesList.Add(addedItem);
                    }
                }
            }
            gridKnowFuncDestination.DataSource = SessionManager.CanKnowledgeDesList;
            gridKnowFuncDestination.DataBind();
        }
    }

    protected void OnBtnKnowFuncRemoveClicked(object sender, EventArgs e)
    {
        if (lblKnowFuncDestination.Text == ResourceManager.GetString("lblCanFunctionGrid"))
        {
            foreach (GridItem item in gridKnowFuncDestination.SelectedItems)
            {
                int id = (int)item.OwnerTableView.DataKeyValues[item.ItemIndex]["FunctionID"];
                foreach (CandidateFunction oldItem in SessionManager.CanFunctionDesList)
                {
                    if (id == oldItem.FunctionID.Value)
                    {
                        SessionManager.CanFunctionDesList.Remove(oldItem);
                        break;
                    }
                }
            }
            gridKnowFuncDestination.DataSource = SessionManager.CanFunctionDesList;
            gridKnowFuncDestination.DataBind();
        }
        else
        {
            foreach (GridItem item in gridKnowFuncDestination.SelectedItems)
            {
                int id = (int)item.OwnerTableView.DataKeyValues[item.ItemIndex]["KnowledgeID"];
                foreach (CandidateKnowledge oldItem in SessionManager.CanKnowledgeDesList)
                {
                    if (id == oldItem.KnowledgeID.Value)
                    {
                        SessionManager.CanKnowledgeDesList.Remove(oldItem);
                        break;
                    }
                }
            }
            gridKnowFuncDestination.DataSource = SessionManager.CanKnowledgeDesList;
            gridKnowFuncDestination.DataBind();
        }
    }

    protected void OnBtnKnowFuncOKClicked(object sender, EventArgs e)
    {
        if (lblKnowFuncDestination.Text == ResourceManager.GetString("lblCanFunctionGrid"))
        {
            SessionManager.CanFunctionOldList = CopyCanFunctionList(SessionManager.CanFunctionDesList);
            gridFunctionOld.DataSource = SessionManager.CanFunctionOldList;
            gridFunctionOld.DataBind();
        }
        else
        {
            SessionManager.CanKnowledgeOldList = CopyCanKnowledgeList(SessionManager.CanKnowledgeDesList);
            gridKnowledgeOld.DataSource = SessionManager.CanKnowledgeOldList;
            gridKnowledgeOld.DataBind();
        }
        divKnowledgeFunctionEdit.Visible = false;
        btnFunctionEdit.Enabled = true;
        btnKnowledgeEdit.Enabled = true;
    }

    protected void OnBtnKnowFuncCancelClicked(object sender, EventArgs e)
    {
        divKnowledgeFunctionEdit.Visible = false;
        btnFunctionEdit.Enabled = true;
        btnKnowledgeEdit.Enabled = true;
    }
    #endregion

    #region Tab Action

    protected void OnGridActionPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridActions.CurrentPageIndex = e.NewPageIndex;
        BindActionGridOfCurrentCandidate(null);
    }

    protected void OnActionGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie actionGridPageSizeCookie = new HttpCookie("cand_actgrdps");
        actionGridPageSizeCookie.Expires.AddDays(30);
        actionGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(actionGridPageSizeCookie);
    }

    private void BindActionGridOfCurrentCandidate(Candidate currentCandidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (currentCandidate != null)
            candidateID = currentCandidate.CandidateId;

        if (candidateID != -1)
            gridActions.DataSource = new ActionRepository().GetActionOfCandidate(candidateID);
        else
            gridActions.DataSource = new List<Neos.Data.Action>();
        gridActions.DataBind();
    }

    protected void OnGridActionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (gridActions.Visible)
        {
            int candidateID = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
                candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
            else if (SessionManager.CurrentCandidate != null)
                candidateID = SessionManager.CurrentCandidate.CandidateId;

            if (candidateID != -1)
                gridActions.DataSource = new ActionRepository().GetActionOfCandidate(candidateID);
            else
                gridActions.DataSource = new List<Neos.Data.Action>();
        }
    }

    protected void OnGridActionItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteCanActionColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((Neos.Data.Action)e.Item.DataItem).ActionID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditCanActionColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");

            LinkButton buttonExport = dataItem["TemplateExportActionColumn"].Controls[1] as LinkButton;
            //buttonExport.OnClientClick = "OnActionExportClientClick('" + ResourceManager.GetString("confirmExportAction")
            //    +  "','" + buttonDelete.CommandArgument + "')";
            buttonExport.OnClientClick = "return confirm('" + ResourceManager.GetString("confirmExportAction") + "')";
            buttonExport.CommandArgument = ((Neos.Data.Action)e.Item.DataItem).ActionID.ToString();
            buttonExport.Text = ResourceManager.GetString("exportText");
        }
    }

    protected void OnCandidateActionDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int actionID = int.Parse(lnkItem.CommandArgument);
        Neos.Data.Action deleteItem = new Neos.Data.Action(actionID);
        ActionRepository repo = new ActionRepository();
        repo.Delete(deleteItem);

        BindActionGridOfCurrentCandidate(null);
    }

    protected void OnActionExportClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int actionID = int.Parse(lnkItem.CommandArgument);
        Neos.Data.Action exportItem = new ActionRepository().GetActionByActionID(actionID);
        if (exportItem != null)
        {
            string message = Common.ExportActionToAppoinment(exportItem);
            //string message = "export xong roi";
            // script1 = "<script type='text/javascript'>";
            string script1 = " alert(\"" + message + "\");";
            //script1 += " </script>";
            //Response.Write(script1);
            //if (!this.ClientScript.IsClientScriptBlockRegistered("exportAction"))
            //    this.ClientScript.RegisterStartupScript(this.GetType(), "exportAction", script1);
            MyAjaxManager.ResponseScripts.Add(script1);
        }
    }
    
    #endregion

    #region Tab Presentation
    private void BindAttachedDocumentsOfCandidate(Candidate candidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))        
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);        
        else if (SessionManager.CurrentCandidate != null)        
            candidateID = SessionManager.CurrentCandidate.CandidateId;        
        else if (candidate != null)        
            candidateID = candidate.CandidateId;        

        List<CandidateDocument> docList = new List<CandidateDocument>();
        if(candidateID != -1) 
            docList = new CandidateDocumentRepository().GetDocumentsOfCandidate(candidateID);
        grdPresentationAttachedDocs.DataSource = docList;
        grdPresentationAttachedDocs.DataBind();
    }

    protected void OnCompanyDropdownItemRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        string companyName = e.Text;
        if (!string.IsNullOrEmpty(companyName))
        {
            CompanyRepository companyRepo = new CompanyRepository();
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataSource = companyRepo.FindByName(companyName);// .GetAllCompanies();
            ddlCompany.DataBind();
        }
    }

    protected void OnGridPresentationAttachedDocsPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        grdPresentationAttachedDocs.CurrentPageIndex = e.NewPageIndex;        
        BindAttachedDocumentsOfCandidate(null);
    }

    private void BindContactsGridOfCompany(int companyID)
    {
        if (companyID == 0)
        {
            grdPresentationContacts.DataSource = new List<CompanyContact>();
            grdPresentationContacts.DataBind();
            return;
        }
        List<CompanyContact> contactList = new CompanyContactRepository().GetContactOfCompany(companyID);
        grdPresentationContacts.DataSource = contactList;
        grdPresentationContacts.DataBind();
    }

    protected void OnGridPresentationContactsPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            grdPresentationContacts.CurrentPageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(ddlCompany.SelectedValue)) 
                BindContactsGridOfCompany(Convert.ToInt32(ddlCompany.SelectedValue));
            else
                BindContactsGridOfCompany(0);
        }
    }

    protected void OnGridPresentationAttachedDocsNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;

        if (candidateID != -1)
            grdPresentationAttachedDocs.DataSource = new CandidateDocumentRepository().GetDocumentsOfCandidate(candidateID);
        else
            grdPresentationAttachedDocs.DataSource = new List<CandidateDocument>();
    }

    protected void OnGridPresentationAttachedDocsItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            CandidateDocument doc = e.Item.DataItem as CandidateDocument;
            if (doc != null)
            {
                Literal lblDocName = (Literal)e.Item.FindControl("lblDocName");
                if (lblDocName != null)
                {
                    lblDocName.Text = !string.IsNullOrEmpty(doc.DocumentLegend) ? doc.DocumentLegend : doc.DocumentName;
                }
                Literal lbCreationDate = (Literal)e.Item.FindControl("lbCreationDate");

                if (lbCreationDate != null)
                {
                    lbCreationDate.Text = (doc.CreatedDate.HasValue) ? doc.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt") : "n/a";
                }
            }
        }
    }

    protected void OnGridPresentationContactsItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            CompanyContact contact = (CompanyContact)e.Item.DataItem;
            if (contact != null)
            {
                Literal lblContactName = (Literal)e.Item.FindControl("lblContactName");
                if (lblContactName != null)
                {
                    lblContactName.Text = string.Format("{0} {1}", contact.FirstName, contact.LastName);
                }
                Literal lblContactEmail = (Literal)e.Item.FindControl("lblContactEmail");
                if (lblContactEmail != null)
                {
                    lblContactEmail.Text = (contact.GetContactEmail() != null) ? contact.GetContactEmail().Tel : "n/a";
                }
            }
        }
    }

    protected void OnDropdownCompanySelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue)) 
        {
            int compID = Convert.ToInt32(ddlCompany.SelectedValue);
            BindContactsGridOfCompany(compID);
            BindDocumentsGridOfCompany(compID);
            lnkAddNewComDocument.Visible = true;
            lnkAddNewComDocument.Attributes.Add("onclick", string.Format("OnComDocumentEditClientClicked({0},\"\")", compID));
            btnSendPresentation.Enabled = !string.IsNullOrEmpty(Request.QueryString["CandidateId"]);
        }
        else 
        {
            BindContactsGridOfCompany(0);
            BindDocumentsGridOfCompany(0);
            lnkAddNewComDocument.Visible = false;
            btnSendPresentation.Enabled = false;
        }

        //BindContactsGridOfCompany(Convert.ToInt32(ddlCompany.SelectedValue));
    }

    protected void OnPresentationContactGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            List<CompanyContact> contactList = new CompanyContactRepository().GetContactOfCompany(Int32.Parse(ddlCompany.SelectedValue));
            grdPresentationContacts.DataSource = contactList;
        }
    }

    protected void OnGridComDocumentsNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            grdComDocuments.DataSource = new CompanyDocumentRepository().GetDocumentsOfCompany(Int32.Parse(ddlCompany.SelectedValue));
        }
    }

    protected void OnGridComDocumentsPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            grdComDocuments.CurrentPageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(ddlCompany.SelectedValue)) 
                BindDocumentsGridOfCompany(Convert.ToInt32(ddlCompany.SelectedValue));
            else
                BindDocumentsGridOfCompany(0);
        }
    }

    
    private void BindDocumentsGridOfCompany(int companyID)
    {
        if (companyID == 0)
        {
            grdComDocuments.DataSource = new List<CompanyDocument>();
            grdComDocuments.DataBind();
            return;
        }
        List<CompanyDocument> docList = new CompanyDocumentRepository().GetDocumentsOfCompany(companyID);
        grdComDocuments.DataSource = docList;
        grdComDocuments.DataBind();
    }

    /// <summary>
    /// Neos sends informations of a candidate to a company that is potentially interested in hiring the candidate...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnBtnSendPresentationClicked(object sender, EventArgs e)
    {
         //Find email of company first.
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue)
            && !string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
        {
            PresentationEmailObject emailObject = new PresentationEmailObject();
            emailObject.CompanyId = int.Parse(ddlCompany.SelectedValue);
            emailObject.CandidateId = int.Parse(Request.QueryString["CandidateId"]);
            emailObject.AutoCreateAction = true;
            emailObject.Body = txtPresentationText.Text;
            if (!string.IsNullOrEmpty(SessionManager.CurrentUser.Email))
            {
                emailObject.CcEmails.Add(SessionManager.CurrentUser.Email.Trim());
            }

            foreach (GridDataItem itemContact in grdPresentationContacts.Items)
            {
                if (itemContact.Selected)
                {
                    TableCell cell = itemContact["ContactID"];
                    if (!string.IsNullOrEmpty(cell.Text))
                    {
                        emailObject.ContactId = int.Parse(cell.Text);
                    }
                    Literal lblContactEmail = itemContact["TemplateContactEmailColumn"].FindControl("lblContactEmail") as Literal;
                    if (lblContactEmail != null && Common.IsValidEmailAddress(lblContactEmail.Text.Trim()))
                    {
                        emailObject.MainEmails.Add(lblContactEmail.Text.Trim());
                    }
                }
            }

            CandidateDocumentRepository canDocRepo = new CandidateDocumentRepository();
            foreach (GridDataItem itemCanDoc in grdPresentationAttachedDocs.Items)
            {
                if (itemCanDoc.Selected)
                {
                    TableCell cell = itemCanDoc["DocumentID"];
                    if (!string.IsNullOrEmpty(cell.Text))
                    {
                        int canDocId = int.Parse(cell.Text);
                        CandidateDocument canDoc = canDocRepo.FindOne(new CandidateDocument(canDocId));
                        if (!string.IsNullOrEmpty(canDoc.Type) && canDoc.Type.Trim() == "CV")
                        {
                            emailObject.AttachmentList.Add(canDoc.DocumentName, WebConfig.CVDocumentPhysicalPath + canDoc.DocumentName);
                        }
                        else
                        {
                            emailObject.AttachmentList.Add(canDoc.DocumentName, WebConfig.DocumentPhysicalPath + canDoc.DocumentName);
                        }
                    }
                }
            }

            CompanyDocumentRepository comDocRepo = new CompanyDocumentRepository();
            foreach (GridDataItem itemComDoc in grdComDocuments.Items)
            {
                if (itemComDoc.Selected)
                {
                    TableCell cell = itemComDoc["DocumentID"];
                    if (!string.IsNullOrEmpty(cell.Text))
                    {
                        int comDocId = int.Parse(cell.Text);
                        CompanyDocument comDoc = comDocRepo.FindOne(new CompanyDocument(comDocId));

                        emailObject.AttachmentList.Add(comDoc.DocumentName, WebConfig.CompanyDocumentPhysicalPath + comDoc.DocumentName);

                    }
                }
            }
            SessionManager.PresentationEmailObject = emailObject;
            
            radWinSendPresentation.NavigateUrl = "SendPresentationEmail.aspx";            
            radWinSendPresentation.VisibleOnPageLoad = true;           
        }
    }
    #endregion

    #region Tab Documents
    protected void OnGridDocumentsDeleteCommand(object source, GridCommandEventArgs e)
    {
        CandidateDocumentRepository docRepo = new CandidateDocumentRepository();
        string docID = e.CommandArgument.ToString();
        CandidateDocument doc = docRepo.FindOne(new CandidateDocument(Int32.Parse(docID)));
        if (doc != null)
        {
            docRepo.Delete(doc);
        }
        grdDocuments.Rebind();
    }

    protected void OnDocumentGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie docGridPageSizeCookie = new HttpCookie("cand_docgrdps");
        docGridPageSizeCookie.Expires.AddDays(30);
        docGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(docGridPageSizeCookie);
    }

    protected void OnGridDocumentsPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        grdDocuments.CurrentPageIndex = e.NewPageIndex;
        BindGridDocumentsOfCurrentCandidate(null);
    }

    private void BindGridDocumentsOfCurrentCandidate(Candidate currentCandidate)
    {
        int candidateID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
        else if (SessionManager.CurrentCandidate != null)
            candidateID = SessionManager.CurrentCandidate.CandidateId;
        else if (currentCandidate != null)
            candidateID = currentCandidate.CandidateId;

        if (candidateID != -1)
            grdDocuments.DataSource = new CandidateDocumentRepository().GetDocumentsOfCandidate(candidateID);
        else
            grdDocuments.DataSource = new List<CandidateDocument>();
        grdDocuments.DataBind();
    }

    protected void OnGridDocumentsNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (grdDocuments.Visible)
        {
            int candidateID = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
                candidateID = Int32.Parse(Request.QueryString["CandidateId"]);
            else if (SessionManager.CurrentCandidate != null)
            {
                candidateID = SessionManager.CurrentCandidate.CandidateId;
            }
            grdDocuments.DataSource = new CandidateDocumentRepository().GetDocumentsOfCandidate(candidateID);
        }
    }

    protected void OnGridDocumentsItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            if (dataItem != null)
            {
                LinkButton lnkDocumentName = (LinkButton)dataItem.FindControl("lnkDocumentName");
                if (lnkDocumentName != null)
                {
                    CandidateDocument doc = (CandidateDocument)e.Item.DataItem;
                    lnkDocumentName.CommandArgument = ((CandidateDocument)e.Item.DataItem).DocumentID.ToString();
                    lnkDocumentName.OnClientClick = "return openNeosWindow('" + doc.AbsoluteURL + "');";
                }

                ImageButton button = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                button.Attributes["onclick"] = string.Format("return confirm(\"{0}\")", ResourceManager.GetString("confirmDeleteDocument"));
                button.CommandArgument = ((CandidateDocument)e.Item.DataItem).DocumentID.ToString();

                dataItem["CreatedDate"].Text = (dataItem.DataItem != null && ((CandidateDocument)dataItem.DataItem).CreatedDate != null) ? ((CandidateDocument)dataItem.DataItem).CreatedDate.ToString() : "n/a";
            }
        }
    }

    protected void OnGridDocumentsItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "viewdocument")
        {
            int docID = Int32.Parse(e.CommandArgument.ToString());
            CandidateDocument doc = new CandidateDocumentRepository().FindOne(new CandidateDocument(docID));
            if (doc != null)
            {
                string fileName = doc.AbsoluteURL.Substring(doc.AbsoluteURL.LastIndexOf(@"/") + 1, doc.AbsoluteURL.Length - (doc.AbsoluteURL.LastIndexOf(@"/") + 1));
                string filePath = doc.Type == "CV" ? WebConfig.CVDocumentPhysicalPath + fileName : WebConfig.DocumentPhysicalPath + fileName;
                string script = "<script type='text/javascript'>";
                script += "openNeosWindow('" + filePath + "');";
                script += "</script>";

                if (!ClientScript.IsClientScriptBlockRegistered("openTheFile"))
                    ClientScript.RegisterStartupScript(this.GetType(), "openTheFile", script);

                //Response.ContentType = doc.ContentType;
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                //Response.TransmitFile(filePath);
                //Response.Flush();
                //Response.End();
                //Response.Buffer = false;
                //FileStream inStr = null;
                //byte[] buffer = new byte[1024];
                //long byteCount;

                //inStr = File.OpenRead(filePath);
                //while ((byteCount = inStr.Read(buffer, 0, buffer.Length)) > 0)
                //{
                //    if (Response.IsClientConnected)
                //    {
                //        Response.OutputStream.Write(buffer, 0, buffer.Length);
                //        Response.Flush();
                //    }
                //}
            }
        }
    }

    #endregion

    #region Common events
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindCanDocumentGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, grdDocuments);
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, grdPresentationAttachedDocs);
            grdDocuments.Rebind();
            grdPresentationAttachedDocs.Rebind();

        }
        else if (e.Argument.IndexOf("RebindPresentationContactGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, grdPresentationContacts);
            //grdPresentationContacts.Rebind();
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, divPresentDocuments);
            //grdComDocuments.Rebind();
            OnDropdownCompanySelectedIndexChanged(null, null);
        }
        else if (e.Argument.IndexOf("RebindComDocumentGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, grdComDocuments);
            grdComDocuments.Rebind();
        }
        else if (e.Argument.IndexOf("RebindCanContactGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridContact);
            gridContact.Rebind();
        }
        else if (e.Argument.IndexOf("RebindCanStudyGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridStudies);
            gridStudies.Rebind();

        }
        else if (e.Argument.IndexOf("RebindCanExperienceGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridExperience);
            gridExperience.Rebind();

        }
        else if (e.Argument.IndexOf("RebindCanActionGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridActions);
            gridActions.Rebind();

        }
        else if (e.Argument.IndexOf("radTabClick") != -1)
        {
            switch (e.Argument)
            {
                case "radTabClick_GeneralView": //load general Tab data
                    LoadGeneralTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.FindTabByValue("GeneralView").PageViewID = "GeneralView";
                    radTabStripCandidateProfile.FindTabByValue("GeneralView").PageView.Selected = true;
                    DrawGeneralTab();
                    break;
                case "radTabClick_ExpectancyView":
                    LoadExpectancyTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.Tabs[1].PageViewID = "ExpectancyView";
                    radTabStripCandidateProfile.Tabs[1].PageView.Selected = true;

                    DrawExpectancyTab();
                    break;
                case "radTabClick_StudyExperienceView":
                    LoadStudyExperienceTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.Tabs[2].PageViewID = "StudyExperienceView";
                    radTabStripCandidateProfile.Tabs[2].PageView.Selected = true;

                    DrawStudyExperienceTab();
                    break;
                case "radTabClick_EvaluationView":
                    LoadEvaluationTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.Tabs[3].PageViewID = "EvaluationView";
                    radTabStripCandidateProfile.Tabs[3].PageView.Selected = true;

                    DrawEvaluationTab();
                    break;
                case "radTabClick_KnowledgeView":
                    LoadKnowledgeFunctionTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.Tabs[4].PageViewID = "KnowledgeView";
                    radTabStripCandidateProfile.Tabs[4].PageView.Selected = true;

                    DrawKnowledgeFunctionTab();
                    break;
                case "radTabClick_PresentationView":
                    LoadPresentationTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.Tabs[5].PageViewID = "PresentationView";
                    radTabStripCandidateProfile.Tabs[5].PageView.Selected = true;

                    DrawPresentationTab();
                    break;
                case "radTabClick_ActionView":
                    LoadActionTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.Tabs[6].PageViewID = "ActionView";
                    radTabStripCandidateProfile.Tabs[6].PageView.Selected = true;

                    DrawActionTab();
                    break;
                case "radTabClick_DocumentView":
                    LoadDocumentTabData(SessionManager.CurrentCandidate);
                    radTabStripCandidateProfile.Tabs[7].PageViewID = "DocumentView";
                    radTabStripCandidateProfile.Tabs[7].PageView.Selected = true;
                    DrawDocumentTab();
                    break;
            }
            //MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, radTabStripCandidateProfile);
        }
        else if (e.Argument.IndexOf("ViewEditCandidateProfile") != -1)
        {
            string url = !string.IsNullOrEmpty(Request.QueryString["tab"]) ? Request.Url.PathAndQuery.Replace("&tab=action", "") : Request.Url.PathAndQuery;

            if (btnEditSave.Text == ResourceManager.GetString("editText"))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                    url = url.Replace(Request.QueryString["mode"], "edit");
                else
                    url += "&mode=edit";
                Response.Redirect(url, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                    url = url.Replace(Request.QueryString["mode"], "view");
                else
                    url += "&mode=view";
                Response.Redirect(url, true);
            }
        }
        else if (e.Argument.IndexOf("SaveCandidateProfile") != -1)
        {
            Candidate candidate = SaveCandidateProfile();
            if (candidate != null)
            {
                string addBackUrl = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
                {
                    addBackUrl = "&backurl=visible";
                }
                string original = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["originalPage"]) && Request.QueryString["originalPage"] == "Action")
                {
                    original = "&originalPage=Action";
                }
                Response.Redirect(string.Format("~/CandidateProfile.aspx?CandidateID={0}&mode=view" + addBackUrl + original, candidate.CandidateId));
            }
        }
        else if (e.Argument.IndexOf("ViewCandidateActions") != -1)
        {
            string url = Request.Url.PathAndQuery;
            if (!string.IsNullOrEmpty(Request.QueryString["tab"]))
                url = url.Replace(Request.QueryString["tab"], "action");
            else
                url += "&tab=action";
            Response.Redirect(url, true);
        }
        else if (e.Argument.IndexOf("AddCandidateActions") != -1)
        {
            MyAjaxManager.ResponseScripts.Add("OnAddNewCanActionClientClicked();");
        }
        //else if (e.Argument.IndexOf("ExportAction") != -1)
        //{
        //    string[] agr = e.Argument.Split('-');
        //    int actionID = int.Parse(agr[1]);            
        //    Action exportItem = new ActionRepository().GetActionByActionID(actionID);
        //    if (exportItem != null)
        //    {
        //        string message = Common.ExportActionToAppoinment(exportItem);
        //        string script1 = " alert(\"" + message + "\");";
        //        //if (!this.ClientScript.IsClientScriptBlockRegistered("exportAction"))
        //        //    this.ClientScript.RegisterStartupScript(this.GetType(), "exportAction", script1);                
        //        MyAjaxManager.ResponseScripts.Add("alert('abc');");
        //    }
        //}
    }

    /// <summary>
    /// no need to draw general tab because it's render at page load
    /// </summary>
    private void DrawGeneralTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtAddress, pnlRadAjaxLoading);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtZip);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtCity);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlCountry);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlGenre);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlNationality);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, divDateOfBirthDatePicker);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtAge);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridContact);
        //lnkCanContactAdd
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, divCreateDatePicker);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlStatus);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtInactivityReason);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtRemarkGeneral);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlCanArea);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, listCanArea);
        //btnAddCanArea
        //btnRemoveCanArea
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlSalaryWish);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlCompanyWish);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlContractTypeWish);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtFunction);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtMotivation);
        
    }
    private void DrawExpectancyTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, expectancyGrid, pnlRadAjaxLoading);
        expectancyGrid.Visible = true;
    }
    private void DrawStudyExperienceTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridStudies, pnlRadAjaxLoading);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridExperience);
        gridStudies.Visible = true;
        gridExperience.Visible = true;
    }
    private void DrawEvaluationTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtPresentationEval, pnlRadAjaxLoading);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtGlobal);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtVerbal);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtOtherEval);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtAutonomy);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtMotivationEval);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtPersonality);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlFrenchLang);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlGermanLang);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlDutchLang);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlSpainishLang);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlEnglishLang);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlItalianLang);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlOtherLang);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlOtherLangSkill);

    }
    private void DrawKnowledgeFunctionTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridKnowledgeOld, pnlRadAjaxLoading);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridFunctionOld);
    }
    private void DrawPresentationTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, txtPresentationText, pnlRadAjaxLoading);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlCompany);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, grdPresentationContacts);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, divPresentDocuments);
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, grdPresentationAttachedDocs);
        //MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, lnkAddNewComDocument);
    }
    private void DrawActionTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridActions, pnlRadAjaxLoading);
        gridActions.Visible = true;
    }
    private void DrawDocumentTab()
    {
        MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, grdDocuments, pnlRadAjaxLoading);
        grdDocuments.Visible = true;
    }
    #endregion

    protected void OnLinkBackClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SessionManager.BackUrl) && SessionManager.BackUrl.Contains("Candidates.aspx"))
        {
            Response.Redirect(SessionManager.BackUrl, true);
        }
    }
}
