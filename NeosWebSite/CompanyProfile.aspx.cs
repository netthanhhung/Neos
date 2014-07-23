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
using System.Xml;

public partial class CompanyProfile : System.Web.UI.Page
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

    #region Common
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
            InitData();
            if (!string.IsNullOrEmpty(SessionManager.BackUrl)
                && SessionManager.BackUrl.Contains("Companies.aspx")
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
            
            if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            {
                lnkAddNewDocument.Attributes.Add("onclick", string.Format("OnComDocumentEditClientClicked({0},\"\")", Request.QueryString["CompanyID"]));

                int companyD = int.Parse(Request.QueryString["CompanyId"]);
                CompanyRepository companyRepo = new CompanyRepository();
                Company currentCompany = companyRepo.FindOne(companyD);
                SessionManager.CurrentCompany = currentCompany;
                SaveLastViewCompaniesToCookie(currentCompany);
                string script1 = "<script type='text/javascript'>";
                script1 += "onSaveOrLoadCompanyProfilePage();";
                script1 += "</script>";
                if (!ClientScript.IsClientScriptBlockRegistered("onSaveOrLoadCompanyProfilePage"))
                    ClientScript.RegisterStartupScript(this.GetType(), "onSaveOrLoadCompanyProfilePage", script1);

                //Fill data for grids.                
                FillDataGrid(currentCompany);
                FillCurrentCompanyInfo(currentCompany);
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
                {
                    EnableCompanyControls(false);
                }
                else
                    EnableCompanyControls(true);

                if (!string.IsNullOrEmpty(Request.QueryString["tab"]))// == "action")
                {
                    switch (Request.QueryString["tab"])
                    {
                        case "action":
                            radTabStripCompany.FindTabByValue("Actions").Selected = true;
                            radTabStripCompany.FindTabByValue("Actions").PageView.Selected = true;
                            break;
                        case "job":
                            radTabStripCompany.FindTabByValue("Job").Selected = true;
                            radTabStripCompany.FindTabByValue("Job").PageView.Selected = true;
                            break;
                        case "invoice":
                            radTabStripCompany.FindTabByValue("Invoice").Selected = true;
                            radTabStripCompany.FindTabByValue("Invoice").PageView.Selected = true;
                            break;
                    }
                }
                //show the company title
                lblCompanyProfileTitle.Text = string.Format(ResourceManager.GetString("lblRightPaneCompanyProfileTitle"), currentCompany.CompanyName);
            }
            else
            {
                SessionManager.CurrentCompany = null;
                SessionManager.NewCompanyContactList = new List<CompanyContact>();

                FillDataGrid(null);
                EnableCompanyControls(true);

                btnEditSave.Text = ResourceManager.GetString("saveText");
                //lnkAddContact.Visible = false;
                //lnkAddContactInfo.Visible = false;
                lnkAddContact.OnClientClick = "return OnAddNewCompanyContactClientClicked('')";
                SessionManager.NewCompanyContactList = null;
                lnkAddNewAction.Visible = false;
                chkRemoveLogo.Visible = false;
                lnkAddNewDocument.Visible = false;
                //show the title    
                lblCompanyProfileTitle.Text = ResourceManager.GetString("lblRightPaneAddNewCompanyTitle");
            }
        }
        string script = "<script type='text/javascript'>";
        script += "onLoadCompanyProfilePage();";
        script += "</script>";
        if (!ClientScript.IsClientScriptBlockRegistered("LoadCompanyProfilePage"))
            ClientScript.RegisterStartupScript(this.GetType(), "LoadCompanyProfilePage", script);
    }

    private void SaveLastViewCompaniesToCookie(Company company)
    {
        XmlDocument doc = new XmlDocument();
        try
        {
            doc.Load(Server.MapPath("~/App_Data/LastViewedCompanies.xml"));

            XmlElement rootNode = doc.DocumentElement;
            if (rootNode != null)
            {
                XmlNode userNode = rootNode.SelectSingleNode("User[@id='" + SessionManager.CurrentUser.UserID + "']");
                if (userNode != null) //user node existed
                {
                    string viewedCompanies = userNode.Attributes["viewed-companies"].Value;
                    if (!string.IsNullOrEmpty(viewedCompanies))
                    {
                        List<string> viewedCompanyList = new List<string>(viewedCompanies.Split('&'));

                        viewedCompanyList.Remove(company.CompanyID.ToString());
                        viewedCompanyList.Insert(0, company.CompanyID.ToString());
                        if (viewedCompanyList.Count > WebConfig.NumberOfRecentCompany)
                            viewedCompanyList.RemoveAt(viewedCompanyList.Count - 1);

                        viewedCompanies = "";
                        foreach (string companyID in viewedCompanyList)
                        {
                            viewedCompanies += companyID + "&";
                        }
                        viewedCompanies = viewedCompanies.TrimEnd('&');

                        userNode.Attributes["viewed-companies"].Value = viewedCompanies;
                    }
                    else
                    {
                        userNode.Attributes["viewed-companies"].Value = company.CompanyID.ToString();
                    }
                }
                else //create new user node
                {
                    userNode = doc.CreateElement("User");


                    XmlAttribute id = doc.CreateAttribute("id");
                    id.Value = SessionManager.CurrentUser.UserID;


                    XmlAttribute viewedCompanyNode = doc.CreateAttribute("viewed-companies");
                    viewedCompanyNode.Value = company.CompanyID.ToString();

                    userNode.Attributes.Append(id);
                    userNode.Attributes.Append(viewedCompanyNode);

                    rootNode.AppendChild(userNode);
                }
            }

            doc.Save(Server.MapPath("~/App_Data/LastViewedCompanies.xml"));
        }
        catch (Exception ex)
        {
            throw ex;
        }

        #region commented
        /*
        HttpCookie lastComCookie = Request.Cookies.Get("lastCompaniesCookie");

        if (lastComCookie != null)
        {
            string[] values = lastComCookie.Values.ToString().Split('&');
            IDictionary<string, string> userIDsDic = new Dictionary<string, string>();
            if (values.Length > 0 && !string.IsNullOrEmpty(values[0]))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    string[] userAndComIds = values[i].Split('=');
                    if (userAndComIds[0] == SessionManager.CurrentUser.UserID.Trim())
                    {
                        string idsOfUser = userAndComIds[1];
                        IList<int> idList = new List<int>();
                        string[] idArray = idsOfUser.Split('A');
                        for (int t = 0; t < idArray.Length; t++)
                        {
                            int id = int.Parse(idArray[t]);
                            idList.Add(id);
                        }


                        if (idList.Contains(company.CompanyID))
                        {
                            idList.Remove(company.CompanyID);
                        }
                        else
                        {
                            if (idList.Count == 5)
                            {
                                idList.RemoveAt(0);
                            }
                        }
                        idList.Add(company.CompanyID);
                        string idString = string.Empty;
                        foreach (int id in idList)
                        {
                            if (idString != string.Empty)
                                idString += "A";
                            idString += id.ToString();
                        }
                        userIDsDic.Add(userAndComIds[0], idString);
                    }
                    else
                    {
                        userIDsDic.Add(userAndComIds[0], userAndComIds[1]);
                    }
                }
                if (!userIDsDic.ContainsKey(SessionManager.CurrentUser.UserID.Trim()))
                {
                    userIDsDic.Add(SessionManager.CurrentUser.UserID.Trim(), company.CompanyID.ToString());
                }

                lastComCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(lastComCookie);

                HttpCookie lastCompaniesCookie = new HttpCookie("lastCompaniesCookie");
                lastCompaniesCookie.Expires = DateTime.Today.AddDays(30);
                foreach (KeyValuePair<string, string> item in userIDsDic)
                {
                    lastCompaniesCookie.Values.Add(item.Key, item.Value);
                }
                Response.Cookies.Add(lastCompaniesCookie);

            }
            else
            {
                HttpCookie lastCompaniesCookie = new HttpCookie("lastCompaniesCookie");
                lastCompaniesCookie.Expires = DateTime.Today.AddDays(30);
                lastCompaniesCookie.Values.Add(SessionManager.CurrentUser.UserID.Trim(), company.CompanyID.ToString());
                Response.Cookies.Add(lastCompaniesCookie);
            }

        }
        else
        {
            HttpCookie lastCompaniesCookie = new HttpCookie("lastCompaniesCookie");
            lastCompaniesCookie.Expires = DateTime.Today.AddDays(30);
            lastCompaniesCookie.Values.Add(SessionManager.CurrentUser.UserID.Trim(), company.CompanyID.ToString());
            Response.Cookies.Add(lastCompaniesCookie);
        }
        */
        #endregion
    }


    /// <summary>
    /// init dropdown list fields
    /// </summary>
    private void InitData()
    {
        //fill reposible field        
        ddlNeosResp.DataTextField = "LastName";
        ddlNeosResp.DataValueField = "UserID";
        ddlNeosResp.DataSource = new ParamUserRepository().GetAllUser(true);
        ddlNeosResp.DataBind();
        //ddlNeosResp.Items.Insert(0, new RadComboBoxItem("", ""));
        //field unit code field
        ddlUnit.DataTextField = "Label";
        ddlUnit.DataValueField = "TypeID";
        ddlUnit.DataSource = new ParamTypeRepository().FindAll() as List<ParamType>;
        ddlUnit.DataBind();
        //fill status (type)
        ddlParamClientStatus.DataTextField = "Status";
        ddlParamClientStatus.DataValueField = "StatusID";
        ddlParamClientStatus.DataSource = new ParamClientStatusRepository().FindAll();
        ddlParamClientStatus.DataBind();
        ddlParamClientStatus.Items.Insert(0, new RadComboBoxItem("", ""));
        //fill legal form
        ddlCompanyLegalForm.DataTextField = "F";
        ddlCompanyLegalForm.DataValueField = "F";
        ddlCompanyLegalForm.DataSource = new LegalFormRepository().FindAll();
        ddlCompanyLegalForm.DataBind();
        ddlCompanyLegalForm.Items.Insert(0, new RadComboBoxItem("", ""));
    }

    private void FillLabelLanguage()
    {
        lnkBack.Text = ResourceManager.GetString("backText");
        //Common
        btnEditSave.Text = ResourceManager.GetString("editText");
        btnCancel.Text = ResourceManager.GetString("cancelText");

        lnkBackToAction.Text = ResourceManager.GetString("lnkBackToAction");
        //header
        lblNameCom.Text = ResourceManager.GetString("lblNameCom");
        lblNeosResp.Text = ResourceManager.GetString("lblNeosResponsible");
        lblWebsite.Text = ResourceManager.GetString("lblWebsite");
        lnkWebsiteTitle.Text = ResourceManager.GetString("lblWebsite");
        //tab Contact info
        lblAddress.Text = ResourceManager.GetString("lblCanAddress");
        lblLegalForm.Text = ResourceManager.GetString("lblLegalForm");
        lblGroup.Text = ResourceManager.GetString("lblGroup");
        lblZipcode.Text = ResourceManager.GetString("lblZipcode");
        lblLocality.Text = ResourceManager.GetString("lblLocality");
        lblPhoneArea.Text = ResourceManager.GetString("lblPhoneArea");
        lblPhone.Text = ResourceManager.GetString("lblPhone");
        lblEmail.Text = ResourceManager.GetString("lblEmail");
        lblType.Text = ResourceManager.GetString("lblType");
        lblFax.Text = ResourceManager.GetString("lblFax");
        lblVATNumber.Text = ResourceManager.GetString("lblVATNumber");
        lblContacts.Text = ResourceManager.GetString("lblContactsPresent");
        lblContactInfo.Text = ResourceManager.GetString("lblContactInfo");
        lnkAddContact.Text = ResourceManager.GetString("lblAddContact");
        lnkAddContactInfo.Text = ResourceManager.GetString("lblAddContactInfo");

        CompanyContactGrid.Columns[1].HeaderText = ResourceManager.GetString("columnLastNameCandidateGrid");
        CompanyContactGrid.Columns[2].HeaderText = ResourceManager.GetString("columnFirstNameCandidateGrid");
        CompanyContactGrid.Columns[3].HeaderText = ResourceManager.GetString("lblCanFunction");
        CompanyContactGrid.Columns[4].HeaderText = ResourceManager.GetString("lblGender");
        CompanyContactGrid.Columns[5].HeaderText = ResourceManager.GetString("lblCanLanguage");

        ContactInfoGrid.Columns[0].HeaderText = ResourceManager.GetString("columnTypeActionCan");
        ContactInfoGrid.Columns[1].HeaderText = ResourceManager.GetString("lblPhoneArea");
        ContactInfoGrid.Columns[2].HeaderText = ResourceManager.GetString("lblInfo");
        ContactInfoGrid.Columns[3].HeaderText = ResourceManager.GetString("columnPlaceCandidateContact");

        //tab client info
        lblActivity.Text = ResourceManager.GetString("lblActivity");
        lblRemark.Text = ResourceManager.GetString("lblCanRemarkGeneral");
        lblCreatedDate.Text = ResourceManager.GetString("columnCreatedDateAttachedDocPresent");
        lblUnit.Text = ResourceManager.GetString("lblCanUnit");
        lblSponsor.Text = ResourceManager.GetString("lblSponsor");

        //tab Action
        lnkAddNewAction.Text = ResourceManager.GetString("lnkAddNewAction");
        gridActions.Columns[0].HeaderText = ResourceManager.GetString("columnActiveActionCan");
        gridActions.Columns[1].HeaderText = ResourceManager.GetString("columnTaskNbrActionCan");
        gridActions.Columns[2].HeaderText = ResourceManager.GetString("columnDateActionCan");
        gridActions.Columns[3].HeaderText = ResourceManager.GetString("columnHourActionCan");
        gridActions.Columns[4].HeaderText = ResourceManager.GetString("columnTypeActionCan");
        gridActions.Columns[5].HeaderText = ResourceManager.GetString("columnCandidateActionCan");
        gridActions.Columns[6].HeaderText = ResourceManager.GetString("columnDescriptionActionCan");
        gridActions.Columns[7].HeaderText = ResourceManager.GetString("columnResponsibleActionCan");

        //TabStrip
        radTabStripCompany.Tabs[0].Text = ResourceManager.GetString("lblCompanyProfileContactInfo");
        radTabStripCompany.Tabs[1].Text = ResourceManager.GetString("lblCompanyProfileClientInfo");
        radTabStripCompany.Tabs[2].Text = ResourceManager.GetString("lblCompanyProfileActions");
        radTabStripCompany.Tabs[3].Text = ResourceManager.GetString("lblCompanyProfileInvoiceCoordinates");
        radTabStripCompany.Tabs[4].Text = ResourceManager.GetString("lblCompanyProfileJobs");
        radTabStripCompany.Tabs[5].Text = ResourceManager.GetString("lblCompanyDocuments");

        //Tab Invoice Coordinate
        gridInvoiceCoordinate.Columns[0].HeaderText = ResourceManager.GetString("lblInvoiceAddressName");
        gridInvoiceCoordinate.Columns[1].HeaderText = ResourceManager.GetString("lblInvoiceAddress");
        gridInvoiceCoordinate.Columns[2].HeaderText = ResourceManager.GetString("lblInvoiceZipCode");
        gridInvoiceCoordinate.Columns[3].HeaderText = ResourceManager.GetString("lblInvoiceCity");
        gridInvoiceCoordinate.Columns[4].HeaderText = ResourceManager.GetString("lblInvoiceVATNumber");
        gridInvoiceCoordinate.Columns[5].HeaderText = ResourceManager.GetString("lblPhone");
        gridInvoiceCoordinate.Columns[6].HeaderText = ResourceManager.GetString("lblFax");
        gridInvoiceCoordinate.Columns[7].HeaderText = ResourceManager.GetString("lblEmail");
        gridInvoiceCoordinate.Columns[8].HeaderText = ResourceManager.GetString("lblCoordinateDefault");

        lnkAddNewInvoiceCoordinate.Text = ResourceManager.GetString("lnkAddNewInvoiceCoordinate");

        //Tab documents
        grdDocuments.Columns[0].HeaderText = ResourceManager.GetString("columnNameDocumentCan");
        grdDocuments.Columns[1].HeaderText = ResourceManager.GetString("columnLegendDocumentCan");
        grdDocuments.Columns[2].HeaderText = ResourceManager.GetString("columnCreatedDateDocumentCan");
        lnkAddNewDocument.Text = ResourceManager.GetString("lblAddNewDocument");

    }

    private void FillCurrentCompanyInfo(Company currentCompany)
    {
        //header
        txtCompanyName.Text = currentCompany.CompanyName;
        txtCompanyID.Text = currentCompany.CompanyID.ToString();
        txtWebsite.Text = currentCompany.WebLink;
        lnkWebsite.Text = !string.IsNullOrEmpty(currentCompany.WebLink) ? currentCompany.WebLink : "n/a";
        lnkWebsite.NavigateUrl = !string.IsNullOrEmpty(currentCompany.WebLink) ? (currentCompany.WebLink.StartsWith("http://") ? currentCompany.WebLink : "http://" + currentCompany.WebLink) : "";

        lnkWebsiteTitle.NavigateUrl = !string.IsNullOrEmpty(currentCompany.WebLink) ? (currentCompany.WebLink.StartsWith("http://") ? currentCompany.WebLink : "http://" + currentCompany.WebLink) : "";

        CompanyLogo logo = new CompanyLogoRepository().FindOne(new CompanyLogo(currentCompany.CompanyID));
        if (logo != null)
        {
            lnkLogo.Text = !string.IsNullOrEmpty(logo.LogoPath) ? logo.LogoPath.Substring(logo.LogoPath.LastIndexOf('/') + 1) : "n/a";
            lnkLogo.NavigateUrl = "#";
            imgCompanyLogo.ImageUrl = logo.LogoPath;
        }
        else
        {
            imgCompanyLogo.Visible = false;
        }

        ddlNeosResp.SelectedValue = currentCompany.Responsible;
        //tab contact info
        txtCompanyAddress.Text = currentCompany.Address;
        txtCompanyZipCode.Text = currentCompany.ZipCode;
        txtCompanyCity.Text = currentCompany.City;
        txtCompanyEmail.Text = currentCompany.Email;
        txtCompanyPhoneArea.Text = currentCompany.TelephoneZone;
        txtCompanyGroup.Text = currentCompany.Group;
        txtCompanyPhone.Text = currentCompany.Telephone;
        txtCompanyFax.Text = currentCompany.Fax;
        txtCompanyVAT.Text = currentCompany.NVAT;
        lnkAddContact.OnClientClick = string.Format("return OnAddNewCompanyContactClientClicked('{0}')", currentCompany.CompanyID);
        ddlCompanyLegalForm.SelectedValue = currentCompany.LegalForm;
        ddlParamClientStatus.SelectedValue = currentCompany.Status.HasValue ? currentCompany.Status.Value.ToString() : "";
        //tab client info
        txtActivity.Text = currentCompany.Activity;
        txtRemark.Text = currentCompany.Remark;
        datCreatedDate.SelectedDate = currentCompany.CreatedDate;
        ddlUnit.SelectedValue = currentCompany.UnitCode;
        chkSponsor.Checked = currentCompany.SponsorArea.HasValue ? currentCompany.SponsorArea.Value : false;

        //tab job
        lnkAddNewJob.NavigateUrl = string.Format("~/JobProfile.aspx?CompanyId={0}&mode=edit", currentCompany.CompanyID);
    }

    private void FillDataGrid(Company company)
    {
        BindActionGridOfCurrentCompany(company);
        BindContactGridOfCurrentCompany(company);
        BindContactInfo(-1);
        BindJobGrid(company);

        BindGridDocumentsOfCurrentCompany(company);
    }

    private void EnableCompanyControls(bool enable)
    {
        btnCancel.Visible = enable;
        if (enable)
            btnEditSave.Text = ResourceManager.GetString("saveText");
        else
            btnEditSave.Text = ResourceManager.GetString("editText");

        txtCompanyName.ReadOnly = !enable;
        ddlNeosResp.Enabled = enable;
        txtWebsite.Visible = enable;
        lnkWebsite.Visible = !enable;
        txtCompanyID.Visible = !enable;
        MViewLogo.ActiveViewIndex = Convert.ToInt32(enable);
        lblWebsite.Visible = !enable;
        lnkWebsiteTitle.Visible = enable;

        //tab contact info
        txtCompanyAddress.ReadOnly = !enable;
        txtCompanyZipCode.ReadOnly = !enable;
        txtCompanyCity.ReadOnly = !enable;
        txtCompanyEmail.ReadOnly = !enable;
        txtCompanyPhoneArea.ReadOnly = !enable;
        txtCompanyGroup.ReadOnly = !enable;
        txtCompanyPhone.ReadOnly = !enable;
        txtCompanyFax.ReadOnly = !enable;
        txtCompanyVAT.ReadOnly = !enable;
        lnkAddContact.Visible = enable;

        ddlCompanyLegalForm.Enabled = enable;
        ddlParamClientStatus.Enabled = enable;
        lnkAddContactInfo.Visible = (enable && CompanyContactGrid.SelectedItems.Count >0);

        CompanyContactGrid.Columns[6].Display = enable;
        CompanyContactGrid.Columns[7].Display = enable;
        ContactInfoGrid.Columns[4].Display = enable;
        ContactInfoGrid.Columns[5].Display = enable;

        gridActions.Columns[8].Display = enable;
        gridActions.Columns[9].Display = enable;
        //tab client info
        txtActivity.ReadOnly = !enable;
        txtRemark.ReadOnly = !enable;
        datCreatedDate.Enabled = enable;
        ddlUnit.Enabled = enable;
        chkSponsor.Enabled = enable;

        //Tab Actions
        lnkAddNewAction.Visible = enable;

        hidMode.Value = enable ? "edit" : "view";

        //Tab Invoice coordinate
        gridInvoiceCoordinate.Columns[9].Display = enable;
        gridInvoiceCoordinate.Columns[10].Display = enable;
        lnkAddNewInvoiceCoordinate.Visible = !string.IsNullOrEmpty(Request.QueryString["CompanyID"]) ? enable : false;
        //Tab jobs
        lnkAddNewJob.Visible = !string.IsNullOrEmpty(Request.QueryString["CompanyID"]) ? enable : false;

        //Tab Documents
        grdDocuments.Columns[3].Display = enable;
        grdDocuments.Columns[4].Display = enable;
        lnkAddNewDocument.Visible = enable;
    }

    protected void onButtonCompanyEditSaveClicked(object sender, EventArgs e)
    {
        if (btnEditSave.Text == ResourceManager.GetString("editText"))
        {
            //Change mode to Edit mode.
            //btnEditSave.Text = ResourceManager.GetString("saveText");
            //EnableCompanyControls(true);

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
            Company company = SaveCompanyData();
            
            //Change to view mode
            if (company != null)
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
                Response.Redirect(string.Format("~/CompanyProfile.aspx?CompanyId={0}&mode=view" + addBackUrl + original, company.CompanyID));
            }
        }
    }

    protected void OnButtonCompanyCancelClicked(object sender, EventArgs e)
    {
        Company curCom = SessionManager.CurrentCompany;
        if (curCom != null)
        {
            //FillCurrentCompanyInfo(curCom);
            //btnEditSave.Text = ResourceManager.GetString("editText");
            //EnableCompanyControls(false);

            string url = !string.IsNullOrEmpty(Request.QueryString["tab"]) ? Request.Url.PathAndQuery.Replace("&tab=action", "") : Request.Url.PathAndQuery;

            if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                url = url.Replace(Request.QueryString["mode"], "view");
            else
                url += "&mode=view";
            Response.Redirect(url, true);
        }
        else
        {
            //string lastname = SessionManager.LastNameSearchCriteria;
            Response.Redirect("~/Companies.aspx");
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

    private Company SaveCompanyData()
    {
        if (string.IsNullOrEmpty(txtCompanyName.Text))
        {
            string message = ResourceManager.GetString("messageComNameMustNotBeEmpty");
            string script = "<script type=\"text/javascript\">";
            script += " alert(\"" + message + "\")";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
            return null;
        }
        bool isNew = false;
        //Save Company
        Company currentCompany = new Company();
        if (SessionManager.CurrentCompany != null)
        {
            currentCompany = SessionManager.CurrentCompany;
        }
        else
        {
            isNew = true;
            currentCompany = new Company();
        }

        //header
        currentCompany.CompanyName = txtCompanyName.Text.Trim();
        currentCompany.WebLink = txtWebsite.Text.Trim();
        if (!string.IsNullOrEmpty(ddlNeosResp.SelectedValue))
            currentCompany.Responsible = ddlNeosResp.SelectedValue;
        else
            currentCompany.Responsible = null;
        //tab contact info
        currentCompany.Address = txtCompanyAddress.Text.Trim();
        currentCompany.ZipCode = txtCompanyZipCode.Text.Trim();
        currentCompany.City = txtCompanyCity.Text.Trim();
        currentCompany.Email = txtCompanyEmail.Text.Trim();
        currentCompany.TelephoneZone = txtCompanyPhoneArea.Text.Trim();
        currentCompany.Group = txtCompanyGroup.Text.Trim();
        currentCompany.Telephone = txtCompanyPhone.Text.Trim();
        currentCompany.Fax = txtCompanyFax.Text.Trim();
        currentCompany.NVAT = txtCompanyVAT.Text.Trim();
        if (!string.IsNullOrEmpty(ddlCompanyLegalForm.SelectedValue))
            currentCompany.LegalForm = ddlCompanyLegalForm.SelectedValue;
        else
            currentCompany.LegalForm = null;

        if (!string.IsNullOrEmpty(ddlParamClientStatus.SelectedValue))
            currentCompany.Status = int.Parse(ddlParamClientStatus.SelectedValue);
        else
            currentCompany.Status = null;

        //tab client info
        currentCompany.Activity = txtActivity.Text.Trim();
        currentCompany.Remark = txtRemark.Text.Trim();
        currentCompany.CreatedDate = datCreatedDate.SelectedDate;
        if (!string.IsNullOrEmpty(ddlUnit.SelectedValue))
            currentCompany.UnitCode = ddlUnit.SelectedValue;
        else
            currentCompany.UnitCode = null;
        currentCompany.SponsorArea = chkSponsor.Checked;

        CompanyRepository repo = new CompanyRepository();
        if (isNew)
        {
            currentCompany.CreatedDate = DateTime.Now;
            repo.Insert(currentCompany);
        }
        else
            repo.Update(currentCompany);
        //insert company logo
        if (chkRemoveLogo.Checked)
        {
            CompanyLogoRepository companyLogoRepo = new CompanyLogoRepository();
            companyLogoRepo.Delete(new CompanyLogo(currentCompany.CompanyID));
        }
        else
        {
            if (fileCompanyLogo.HasFile)
            {
                string fileName = string.Format("{0}_{1}", currentCompany.CompanyID, System.IO.Path.GetFileName(fileCompanyLogo.PostedFile.FileName.ToString()));//fileCompanyLogo.PostedFile.FileName
                CompanyLogoRepository companyLogoRepo = new CompanyLogoRepository();
                CompanyLogo logo = companyLogoRepo.FindOne(new CompanyLogo(currentCompany.CompanyID));
                if (logo != null)
                {
                    logo.LogoPath = WebConfig.UserImagePath + fileName;
                    companyLogoRepo.Update(logo);
                }
                else
                {
                    logo = new CompanyLogo();
                    logo.CompanyID = currentCompany.CompanyID;
                    logo.LogoPath = WebConfig.UserImagePath + fileName;
                    companyLogoRepo.Insert(logo);
                }
                fileCompanyLogo.SaveAs(WebConfig.UserImages + fileName);
            }
        }
        currentCompany = repo.FindOne(currentCompany);
        SessionManager.CurrentCompany = currentCompany;
        SaveLastViewCompaniesToCookie(currentCompany);
        if (isNew)
        {
            string script1 = "<script type='text/javascript'>";
            script1 += "onSaveOrLoadCompanyProfilePage();";
            script1 += "</script>";
            if (!ClientScript.IsClientScriptBlockRegistered("onSaveOrLoadCompanyProfilePage"))
                ClientScript.RegisterStartupScript(this.GetType(), "onSaveOrLoadCompanyProfilePage", script1);
        }
               
        //Save company contact
        SaveCompanyContact(currentCompany);

        return currentCompany;
    }

    private void SaveCompanyContact(Company currentCompany)
    {        
        if (SessionManager.NewCompanyContactList != null 
            && SessionManager.NewCompanyContactList.Count > 0
            && currentCompany != null)
        {
            CompanyContactRepository contactRepo = new CompanyContactRepository();
            foreach (CompanyContact contact in SessionManager.NewCompanyContactList)
            {                
                contact.CompanyID = currentCompany.CompanyID;
                contactRepo.Insert(contact);
                CompanyContact realContact = contactRepo.FindOne(contact);

                CompanyContactTelephoneRepository telephoneRepo = new CompanyContactTelephoneRepository();
                foreach (CompanyContactTelephone telephone in contact.ContactInfo)
                {
                    telephone.ContactID = realContact.ContactID;
                    telephoneRepo.Insert(telephone);
                }

            }
        }
    }
    #endregion

    #region Tab Action

    protected void OnGridActionPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridActions.CurrentPageIndex = e.NewPageIndex;
        BindActionGridOfCurrentCompany(null);
    }

    private void BindActionGridOfCurrentCompany(Company currentCompany)
    {
        int companyID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            companyID = Int32.Parse(Request.QueryString["CompanyId"]);
        else if (SessionManager.CurrentCompany != null)
            companyID = SessionManager.CurrentCompany.CompanyID;
        else if (currentCompany != null)
            companyID = currentCompany.CompanyID;

        if (companyID != -1)
            gridActions.DataSource = new ActionRepository().GetActionOfCompany(companyID);
        else
            gridActions.DataSource = new List<Neos.Data.Action>();
        gridActions.DataBind();
    }

    protected void OnGridActionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        int companyID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            companyID = Int32.Parse(Request.QueryString["CompanyId"]);
        else if (SessionManager.CurrentCompany != null)
            companyID = SessionManager.CurrentCompany.CompanyID;

        if (companyID != -1)
            gridActions.DataSource = new ActionRepository().GetActionOfCompany(companyID);
        else
            gridActions.DataSource = new List<Neos.Data.Action>();
    }

    protected void OnGridActionItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteComActionColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((Neos.Data.Action)e.Item.DataItem).ActionID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditComActionColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");

            LinkButton buttonExport = dataItem["TemplateExportActionColumn"].Controls[1] as LinkButton;
            buttonExport.OnClientClick = "return confirm('" + ResourceManager.GetString("confirmExportAction") + "')";
            buttonExport.CommandArgument = ((Neos.Data.Action)e.Item.DataItem).ActionID.ToString();
            buttonExport.Text = ResourceManager.GetString("exportText");
        }
    }

    protected void OnCompanyActionDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int actionID = int.Parse(lnkItem.CommandArgument);
        Neos.Data.Action deleteItem = new Neos.Data.Action(actionID);
        ActionRepository repo = new ActionRepository();
        repo.Delete(deleteItem);

        BindActionGridOfCurrentCompany(null);
    }

    protected void OnActionExportClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int actionID = int.Parse(lnkItem.CommandArgument);
        Neos.Data.Action exportItem = new ActionRepository().GetActionByActionID(actionID);
        if (exportItem != null)
        {
            string message = Common.ExportActionToAppoinment(exportItem);
            //string script1 = "<script type=\"text/javascript\">";
            string script1 = " alert(\"" + message + "\")";
            //script1 += " </script>";
            //if (!this.ClientScript.IsClientScriptBlockRegistered("exportAction"))
            //    this.ClientScript.RegisterStartupScript(this.GetType(), "exportAction", script1); 
            CompanyProfileAjaxManager.ResponseScripts.Add(script1);
        }
    }

    #endregion

    #region Tab Contact Info
    private void BindContactGridOfCurrentCompany(Company currentCompany)
    {
        int companyID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            companyID = Int32.Parse(Request.QueryString["CompanyId"]);
        else if (SessionManager.CurrentCompany != null)
            companyID = SessionManager.CurrentCompany.CompanyID;
        else if (currentCompany != null)
            companyID = currentCompany.CompanyID;

        if (companyID != -1)
        {
            List<CompanyContact> contactList = new CompanyContactRepository().GetContactOfCompany(companyID);
            CompanyContactGrid.DataSource = contactList;
        }
        else
        {            
            if (SessionManager.NewCompanyContactList != null)
            {
                CompanyContactGrid.DataSource = SessionManager.NewCompanyContactList;
            }
            else
                CompanyContactGrid.DataSource = new List<CompanyContact>();
        }
        CompanyContactGrid.DataBind();
    }

    protected void OnCompanyContactGridDeleteCommand(object source, GridCommandEventArgs e)
    {
        int index = -1;
        if (CompanyContactGrid.SelectedIndexes.Count == 1)
            index = Int32.Parse(CompanyContactGrid.SelectedIndexes[0]);

        CompanyContactRepository contactRepo = new CompanyContactRepository();
        int contactID = Int32.Parse(e.CommandArgument.ToString());
        if (contactID > 0)
        {
            contactRepo.Delete(new CompanyContact(contactID));
            //CompanyContactGrid.Rebind();
        }
        else
        {
            List<CompanyContact> list = SessionManager.NewCompanyContactList;
            CompanyContact existedItem = list.Find(delegate(CompanyContact t) { return t.ContactID == contactID; });
            if (existedItem != null)
            {
                list.Remove(existedItem);
                SessionManager.NewCompanyContactList = list;
            }
        }

        if (index > -1)
        {
            if (index < CompanyContactGrid.MasterTableView.Items.Count)
                CompanyContactGrid.MasterTableView.Items[index].Selected = true;
            else
            {
                CompanyContactGrid.MasterTableView.Items[index - 1].Selected = true;
            }
        }
        //CompanyContactGrid.MasterTableView.Items[0].Selected = true;
        BindContactGridOfCurrentCompany(null);
        ContactInfoGrid.Rebind();
        if (btnEditSave.Text == ResourceManager.GetString("saveText"))
        {
            lnkAddContactInfo.Visible = (CompanyContactGrid.SelectedItems.Count > 0);
        }
    }

    protected void OnContactInfoGridDeleteCommand(object source, GridCommandEventArgs e)
    {
        CompanyContactTelephoneRepository contactInfoRepo = new CompanyContactTelephoneRepository();
        string[] idArray = e.CommandArgument.ToString().Split('&');
        int contactInfoId = Int32.Parse(idArray[0]);
        int contactID = Int32.Parse(idArray[1]);
        if (contactInfoId > 0)
        {
            contactInfoRepo.Delete(new CompanyContactTelephone(contactInfoId));
        }
        else
        {
            List<CompanyContact> list = SessionManager.NewCompanyContactList;
            CompanyContact existedItem = list.Find(delegate(CompanyContact t) { return t.ContactID == contactID; });
            if (existedItem != null)
            {
                List<CompanyContactTelephone> listTel = existedItem.ContactInfo;
                CompanyContactTelephone existInfo = listTel.Find(delegate(CompanyContactTelephone a) { return a.ContactTelephoneID == contactInfoId; });
                if (existInfo != null)
                {
                    existedItem.ContactInfo.Remove(existInfo);
                }
            }
            SessionManager.NewCompanyContactList = list;
        }
        ContactInfoGrid.Rebind();
    }

    private void BindContactInfo(int contactID)
    {
        if (contactID > 0)
        {
            List<CompanyContactTelephone> contactInfoList = new CompanyContactTelephoneRepository().GetContactInfo(contactID);
            ContactInfoGrid.DataSource = contactInfoList;
        }
        else
        {
            List<CompanyContact> list = SessionManager.NewCompanyContactList;
            CompanyContact existedItem = list.Find(delegate(CompanyContact t) { return t.ContactID == contactID; });
            if (existedItem != null)
            {
                ContactInfoGrid.DataSource = existedItem.ContactInfo;
            }
            else
            {
                ContactInfoGrid.DataSource = new List<CompanyContactTelephone>();
            }
        }
        ContactInfoGrid.DataBind();
    }
    protected void OnContactInfoGridItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            CompanyContactTelephone contactInfo = e.Item.DataItem as CompanyContactTelephone;
            if (contactInfo != null)
            {
                TableCell type = ((GridDataItem)e.Item)["Type"] as TableCell;
                if (type != null)
                {
                    if (!string.IsNullOrEmpty(contactInfo.Type))
                    {
                        switch (contactInfo.Type.ToUpper())
                        {
                            case "T":
                                type.Text = ResourceManager.GetString("candidateContactPhone");
                                break;
                            case "F":
                                type.Text = ResourceManager.GetString("candidateContactFax");
                                break;
                            case "G":
                                type.Text = ResourceManager.GetString("candidateContactMobile");
                                break;
                            case "E":
                                type.Text = ResourceManager.GetString("candidateContactEmail");
                                break;
                        }
                    }
                }
                LinkButton lnkContactEdit = (LinkButton)e.Item.FindControl("lnkContactEdit");
                if (lnkContactEdit != null)
                {
                    lnkContactEdit.OnClientClick = string.Format("return OnEditContactInfoClientClicked('{0}', '{1}')", contactInfo.ContactTelephoneID, contactInfo.ContactID);
                }
                GridDataItem item = e.Item as GridDataItem;
                ImageButton button = item["DeleteColumn"].Controls[0] as ImageButton;
                button.Attributes["onclick"] = string.Format("return confirm(\"{0}\")", ResourceManager.GetString("confirmDeleteContactInfo"));
                button.CommandArgument = contactInfo.ContactTelephoneID.ToString() + "&" + contactInfo.ContactID.ToString();
            }

        }
    }
    protected void OnCompanyContactGridItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            CompanyContact contact = (CompanyContact)e.Item.DataItem;
            if (contact != null)
            {
                //Literal lblContactFunction = (Literal)e.Item.FindControl("lblContactFunction");
                //if (lblContactFunction != null)
                //{
                //    lblContactFunction.Text = contact.Position;
                //}                

                // OnClientClick='<%# Eval("ContactID","return OnEditCompanyContactClientClicked({0})") %>' CommandArgument='<%# Eval("ContactID") %>'
                LinkButton lnkContactEdit = (LinkButton)e.Item.FindControl("lnkContactEdit");
                if (lnkContactEdit != null)
                {
                    lnkContactEdit.OnClientClick = string.Format("return OnEditCompanyContactClientClicked('{0}')", contact.ContactID);
                }
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    ImageButton button = item["DeleteColumn"].Controls[0] as ImageButton;
                    button.Attributes["onclick"] = string.Format("return confirm(\"{0}\")", ResourceManager.GetString("confirmDeleteContact"));
                    button.CommandArgument = contact.ContactID.ToString();
                }
            }
        }
    }

    protected void OnCompanyContactGridPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        CompanyContactGrid.CurrentPageIndex = e.NewPageIndex;
        BindContactGridOfCurrentCompany(null);
    }

    protected void OnCompanyContactGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
        {
            List<CompanyContact> contactList = new CompanyContactRepository().GetContactOfCompany(Int32.Parse(Request.QueryString["CompanyId"]));
            CompanyContactGrid.DataSource = contactList;
        }
        else
        {
            CompanyContactGrid.DataSource = SessionManager.NewCompanyContactList;
        }
    }

    protected void OnContactInfoGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (CompanyContactGrid.SelectedItems.Count == 1)
        {
            GridDataItem dataItem = ((GridDataItem)CompanyContactGrid.SelectedItems[0]);
            if (dataItem != null)
            {
                int contactID = Int32.Parse(dataItem["ContactID"].Text);
                if (contactID > 0)
                {
                    List<CompanyContactTelephone> contactInfoList = new CompanyContactTelephoneRepository().GetContactInfo(contactID);
                    ContactInfoGrid.DataSource = contactInfoList;
                }
                else
                {
                    List<CompanyContact> list = SessionManager.NewCompanyContactList;
                    CompanyContact existedItem = list.Find(delegate(CompanyContact t) { return t.ContactID == contactID; });
                    if (existedItem != null)
                    {
                        ContactInfoGrid.DataSource = existedItem.ContactInfo;
                    }
                }
            }
        }
        else
        {
            ContactInfoGrid.DataSource = new List<CompanyContactTelephone>();
        }
    }



    #endregion

    #region Tab Invoice Coordinates

    protected void OnGridInvoiceCoordinatePageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridInvoiceCoordinate.CurrentPageIndex = e.NewPageIndex;
        BindInvoiceCoordinateGridOfCurrentCompany(null);
    }

    private void BindInvoiceCoordinateGridOfCurrentCompany(Company currentCompany)
    {
        int companyID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            companyID = Int32.Parse(Request.QueryString["CompanyId"]);
        else if (SessionManager.CurrentCompany != null)
            companyID = SessionManager.CurrentCompany.CompanyID;
        else if (currentCompany != null)
            companyID = currentCompany.CompanyID;

        if (companyID != -1)
            gridInvoiceCoordinate.DataSource = new CompanyAddressRepository().GetAddressesOfCompany(companyID);
        else
            gridInvoiceCoordinate.DataSource = new List<CompanyAddress>();
        gridInvoiceCoordinate.DataBind();
    }

    protected void OnGridInvoiceCoordinateNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        int companyID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            companyID = Int32.Parse(Request.QueryString["CompanyId"]);
        else if (SessionManager.CurrentCompany != null)
            companyID = SessionManager.CurrentCompany.CompanyID;

        if (companyID != -1)
            gridInvoiceCoordinate.DataSource = new CompanyAddressRepository().GetAddressesOfCompany(companyID);
        else
            gridInvoiceCoordinate.DataSource = new List<CompanyAddress>();
    }

    protected void OnGridInvoiceCoordinateItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteInvoiceCoordinateColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((CompanyAddress)e.Item.DataItem).AddressID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditInvoiceCoordinateColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
    }

    protected void OnInvoiceCoordinateDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int addressId = int.Parse(lnkItem.CommandArgument);
        CompanyAddress deleteItem = new CompanyAddress(addressId);
        CompanyAddressRepository repo = new CompanyAddressRepository();
        repo.Delete(deleteItem);

        BindInvoiceCoordinateGridOfCurrentCompany(null);
    }

    #endregion


    #region Tab Jobs
    private void BindJobGrid(Company company)
    {
        if (company != null)
        {
            List<Job> jobList = new JobRepository().GetJobsOfCompany(company.CompanyID);
            CompanyJobGrid.DataSource = jobList;
            CompanyJobGrid.DataBind();
        }
    }

    protected void OnCompanyJobGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        List<Job> jobList = new JobRepository().GetJobsOfCompany(Convert.ToInt32(Request.QueryString["CompanyId"]));
        CompanyJobGrid.DataSource = jobList;
    }

    protected void OnCompanyJobGrid_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "delete")
        {
            int jobID = Int32.Parse(e.CommandArgument.ToString());
            JobRepository jobRepo = new JobRepository();
            jobRepo.Delete(new Job(jobID));

            CompanyJobGrid.Rebind();
        }
    }
    protected void OnCompanyJobGrid_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        CompanyJobGrid.CurrentPageIndex = e.NewPageIndex;
        BindJobGrid(new Company(Convert.ToInt32(Request.QueryString["CompanyId"])));
    }

    protected void OnCompanyJobGrid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Job job = e.Item.DataItem as Job;
            if (job != null)
            {
                //NavigateUrl='<%#Eval("JobID","~/JobProfile.aspx?jobID={0}") %>'
                HyperLink lnkJobTitle = (HyperLink)e.Item.FindControl("lnkJobTitle");
                if (lnkJobTitle != null)
                {
                    lnkJobTitle.NavigateUrl = string.Format("~/JobProfile.aspx?JobId={0}&CompanyId={1}&mode=edit", job.JobID, job.CompanyID);
                }
            }
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            if (dataItem != null)
            {
                ImageButton delButton = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                delButton.Attributes["onclick"] = string.Format("return confirm(\"{0}\")", ResourceManager.GetString("confirmDeleteJob"));
                delButton.CommandArgument = ((Job)e.Item.DataItem).JobID.ToString();
            }
        }
    }

    #endregion

    #region Tab Documents
    protected void OnGridDocumentsDeleteCommand(object source, GridCommandEventArgs e)
    {
        CompanyDocumentRepository docRepo = new CompanyDocumentRepository();
        string docID = e.CommandArgument.ToString();
        CompanyDocument doc = docRepo.FindOne(new CompanyDocument(Int32.Parse(docID)));
        if (doc != null)
        {
            docRepo.Delete(doc);
        }
        grdDocuments.Rebind();
    }

    //protected void OnDocumentGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    //{
    //    HttpCookie docGridPageSizeCookie = new HttpCookie("comp_docgrdps");
    //    docGridPageSizeCookie.Expires.AddDays(30);
    //    docGridPageSizeCookie.Value = e.NewPageSize.ToString();
    //    Response.Cookies.Add(docGridPageSizeCookie);
    //}

    protected void OnGridDocumentsPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        grdDocuments.CurrentPageIndex = e.NewPageIndex;
        BindGridDocumentsOfCurrentCompany(null);
    }

    private void BindGridDocumentsOfCurrentCompany(Company currentCompany)
    {
        int companyID = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            companyID = Int32.Parse(Request.QueryString["CompanyId"]);
        else if (SessionManager.CurrentCompany != null)
            companyID = SessionManager.CurrentCompany.CompanyID;
        else if (currentCompany != null)
            companyID = currentCompany.CompanyID;

        if (companyID != -1)
            grdDocuments.DataSource = new CompanyDocumentRepository().GetDocumentsOfCompany(companyID);
        else
            grdDocuments.DataSource = new List<CompanyDocument>();
        grdDocuments.DataBind();
    }

    protected void OnGridDocumentsNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (grdDocuments.Visible)
        {
            int companyID = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
                companyID = Int32.Parse(Request.QueryString["CompanyId"]);
            else if (SessionManager.CurrentCompany != null)
            {
                companyID = SessionManager.CurrentCompany.CompanyID;
            }
            grdDocuments.DataSource = new CompanyDocumentRepository().GetDocumentsOfCompany(companyID);
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
                    CompanyDocument doc = (CompanyDocument)e.Item.DataItem;
                    lnkDocumentName.CommandArgument = ((CompanyDocument)e.Item.DataItem).DocumentID.ToString();
                    lnkDocumentName.OnClientClick = "return openNeosWindow('" + doc.AbsoluteURL + "');";
                }

                ImageButton button = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                button.Attributes["onclick"] = string.Format("return confirm(\"{0}\")", ResourceManager.GetString("confirmDeleteDocument"));
                button.CommandArgument = ((CompanyDocument)e.Item.DataItem).DocumentID.ToString();

                dataItem["CreatedDate"].Text = (dataItem.DataItem != null && ((CompanyDocument)dataItem.DataItem).CreatedDate != null) ? ((CompanyDocument)dataItem.DataItem).CreatedDate.ToString() : "n/a";
            }
        }
    }

    protected void OnGridDocumentsItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "viewdocument")
        {
            int docID = Int32.Parse(e.CommandArgument.ToString());
            CompanyDocument doc = new CompanyDocumentRepository().FindOne(new CompanyDocument(docID));
            if (doc != null)
            {
                string fileName = doc.AbsoluteURL.Substring(doc.AbsoluteURL.LastIndexOf(@"/") + 1, doc.AbsoluteURL.Length - (doc.AbsoluteURL.LastIndexOf(@"/") + 1));
                string filePath = doc.Type == "CV" ? WebConfig.CVDocumentPhysicalPath + fileName : WebConfig.DocumentPhysicalPath + fileName;
                Response.ContentType = doc.ContentType;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.TransmitFile(filePath);
                Response.End();
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

    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindComDocumentGrid") > -1)
        {
            CompanyProfileAjaxManager.AjaxSettings.AddAjaxSetting(CompanyProfileAjaxManager, grdDocuments);            
            grdDocuments.Rebind();            

        }
        else if (e.Argument.IndexOf("RebindComActionGrid") > -1)
        {
            gridActions.Rebind();
        }
        else if (e.Argument.IndexOf("RebindContactInfoGrid") > -1)
        {
            CompanyProfileAjaxManager.AjaxSettings.AddAjaxSetting(CompanyProfileAjaxManager, ContactInfoGrid);
            CompanyProfileAjaxManager.AjaxSettings.AddAjaxSetting(CompanyProfileAjaxManager, lnkAddContactInfo);


            string[] args = e.Argument.Split('/');
            if (args.Length == 3)
            {
                try
                {
                    int contactID = Int32.Parse(args[1]);
                    int rowIndex = Int32.Parse(args[2]);
                    BindContactInfo(contactID);
                    CompanyContactGrid.MasterTableView.ClearSelectedItems();
                    CompanyContactGrid.MasterTableView.Items[rowIndex - 1].Selected = true;

                    if (btnEditSave.Text == ResourceManager.GetString("saveText"))
                    {
                        lnkAddContactInfo.Visible = true;
                        lnkAddContactInfo.OnClientClick = string.Format("return OnAddNewContactInfoClientClicked('{0}');", contactID);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ContactInfoGrid.Rebind();
            }
        }
        else if (e.Argument.IndexOf("RebindCompanyContactGrid") > -1)
        {
            CompanyProfileAjaxManager.AjaxSettings.AddAjaxSetting(CompanyProfileAjaxManager, CompanyContactGrid);
            
            int index = -1;
            if (CompanyContactGrid.SelectedIndexes.Count == 1)
                index = Int32.Parse(CompanyContactGrid.SelectedIndexes[0]);
            CompanyContactGrid.Rebind();
        }
        else if (e.Argument.IndexOf("ViewEditCompanyProfile") > -1)
        {
            string url = !string.IsNullOrEmpty(Request.QueryString["tab"]) ? Request.Url.PathAndQuery.Replace("&tab=action", "") : Request.Url.PathAndQuery;

            if (hidMode.Value == "view")
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
        else if (e.Argument.IndexOf("SaveCompanyProfile") > -1)
        {
            Company company = SaveCompanyData();
            if (company != null)
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
                Response.Redirect(string.Format("~/CompanyProfile.aspx?CompanyId={0}&mode=view" + addBackUrl + original, company.CompanyID));
            }
        }
        else if (e.Argument.IndexOf("ViewCompanyActions") > -1)
        {
            string url = Request.Url.PathAndQuery;
            if (!string.IsNullOrEmpty(Request.QueryString["tab"]))
                url = url.Replace(Request.QueryString["tab"], "action");
            else
                url += "&tab=action";
            Response.Redirect(url, true);
        }
        else if (e.Argument.IndexOf("AddCompanyAction") > -1)
        {
            string script = "OnAddNewComActionClientClicked()";
            CompanyProfileAjaxManager.ResponseScripts.Add(script);
        }
        else if (e.Argument.IndexOf("ViewCompanyJobs") > -1)
        {
            string url = Request.Url.PathAndQuery;
            if (!string.IsNullOrEmpty(Request.QueryString["tab"]))
                url = url.Replace(Request.QueryString["tab"], "job");
            else
                url += "&tab=job";
            Response.Redirect(url, true);
        }
        else if (e.Argument.IndexOf("AddCompanyJob") > -1)
        {
            Response.Redirect(string.Format("~/JobProfile.aspx?CompanyId={0}&mode=edit", Request.QueryString["CompanyId"]));
        }
        else if (e.Argument.IndexOf("ViewCompanyInvoices") > -1)
        {
            //url = Request.Url.PathAndQuery;
            //if (!string.IsNullOrEmpty(Request.QueryString["tab"]))
            //    url = url.Replace(Request.QueryString["tab"], "invoice");
            //else
            //    url += "&tab=invoice";
            Response.Redirect(string.Format("~/InvoicesPage.aspx?type=search&customer={0}", Request.QueryString["CompanyId"]), true);

        }
        else if (e.Argument.IndexOf("AddCompanyInvoice") > -1)
        {
            /*
            script = "OnAddNewInvoiceCoordinateClientClicked()";
            CompanyProfileAjaxManager.ResponseScripts.Add(script);*/
            Response.Redirect(string.Format("~/InvoiceProfile.aspx?type=addnew&customer={0}&mode=edit", Request.QueryString["CompanyId"]), true);
        }
        else if (e.Argument.IndexOf("RebindInvoiceCoordinateGrid") > -1)
        {
            gridInvoiceCoordinate.Rebind();
        }

    }
    protected void OnLinkBackClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SessionManager.BackUrl) && SessionManager.BackUrl.Contains("Companies.aspx"))
        {
            Response.Redirect(SessionManager.BackUrl, true);
        }
    }
}
