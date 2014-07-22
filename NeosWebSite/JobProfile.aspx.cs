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
using Neos.Data.Enums;

public partial class JobPopup : System.Web.UI.Page
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
            Common.RedirectToLoginPage(this);
            return;
        }
        if (!IsPostBack)
        {
            SessionManager.CurrentJob = null;
            FillLabelTexts();
            InitControls();
            if (!string.IsNullOrEmpty(SessionManager.BackUrl)
                && SessionManager.BackUrl.Contains("Jobs.aspx")
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

            BindData();

        }
        string script = "<script type='text/javascript'>";
        script += "onLoadJobProfilePage();";
        script += "</script>";
        if (!ClientScript.IsClientScriptBlockRegistered("LoadJobProfilePage"))
            ClientScript.RegisterStartupScript(this.GetType(), "LoadJobProfilePage", script);
    }

    private void FillLabelTexts()
    {
        lnkBack.Text = ResourceManager.GetString("backText");
        lblProfile.Text = ResourceManager.GetString("lblJobProfile");
        lblLocation.Text = ResourceManager.GetString("lblJobLocation");
        lblResponsible.Text = ResourceManager.GetString("lblJobResponsible");
        lblActivatedDate.Text = ResourceManager.GetString("lblJobActivatedDate");
        lblFunction.Text = ResourceManager.GetString("lblJobFunction");
        lblConfidential.Text = ResourceManager.GetString("lblJobConfidential");
        rdoSelectEmail.Text = ResourceManager.GetString("lblJobEmail");
        lblExpiredDate.Text = ResourceManager.GetString("lblJobExpiredDate");
        lblCompany.Text = ResourceManager.GetString("lblJobCompany");
        lblActive.Text = ResourceManager.GetString("lblJobActive");
        rdoSelectURL.Text = ResourceManager.GetString("lblJobUrl");
        lblRemindDate.Text = ResourceManager.GetString("lblJobRemindDate");

        lblJobRef.Text = ResourceManager.GetString("lblJobRef");
        lblNumberApplications.Text = ResourceManager.GetString("lblNumberApplications");
        lblNumberOfVisits.Text = ResourceManager.GetString("lblNumberOfVisits");
        lblLastModifDate.Text = ResourceManager.GetString("lblJobLastModifDate");

        lblTitle.Text = ResourceManager.GetString("lblJobTitle");
        lblCompanyDesc.Text = ResourceManager.GetString("lblJobCompanyDescription");
        lblJobDesc.Text = ResourceManager.GetString("lblJobDescription");
        lblPersonalDesc.Text = ResourceManager.GetString("lblJobPersonalDescription");
        lblPackageDesc.Text = ResourceManager.GetString("lblJobPackageDescription");

        lblTitleEN.Text = ResourceManager.GetString("lblJobTitleEN");
        lblCompanyDescEN.Text = ResourceManager.GetString("lblJobCompanyDescriptionEN");
        lblJobDescEN.Text = ResourceManager.GetString("lblJobDescriptionEN");
        lblPersonalDescEN.Text = ResourceManager.GetString("lblJobPersonalDescriptionEN");
        lblPackageDescEN.Text = ResourceManager.GetString("lblJobPackageDescriptionEN");

        lblTitleNL.Text = ResourceManager.GetString("lblJobTitleNL");
        lblCompanyDescNL.Text = ResourceManager.GetString("lblJobCompanyDescriptionNL");
        lblJobDescNL.Text = ResourceManager.GetString("lblJobDescriptionNL");
        lblPersonalDescNL.Text = ResourceManager.GetString("lblJobPersonalDescriptionNL");
        lblPackageDescNL.Text = ResourceManager.GetString("lblJobPackageDescriptionNL");
    }

    private void InitControls()
    {
        txtURL.Attributes.Add("disabled", "true");
        rdoSelectEmail.Attributes.Add("onclick", string.Format("enableInputControl('{0}','{1}','{2}')", rdoSelectEmail.ClientID, txtEmail.ClientID, txtURL.ClientID));
        rdoSelectURL.Attributes.Add("onclick", string.Format("enableInputControl('{0}','{1}','{2}')", rdoSelectURL.ClientID, txtURL.ClientID, txtEmail.ClientID));
    }

    private void BindData()
    {
        FillDropDownListData();
        btnPreview.Visible = (Request.QueryString["JobId"] != null);

        if (!string.IsNullOrEmpty(Request.QueryString["JobId"])) //edit
        {
            Job currentJob = new JobRepository().FindOne(new Job(Int32.Parse(Request.QueryString["JobId"])));
            if (currentJob != null)
            {
                SessionManager.CurrentJob = currentJob;
                txtJobRef.Text = currentJob.JobID.ToString();
                txtNumberOfApplications.Text = currentJob.NrOfApplications.HasValue ? currentJob.NrOfApplications.Value.ToString() : "0";
                txtNumberOfVisits.Text = currentJob.NrOfVisites.HasValue ? currentJob.NrOfVisites.Value.ToString() : "0";
                calLastModif.SelectedDate = currentJob.LastModifiedDate;

                ddlProfile.SelectedValue = currentJob.ProfileID.HasValue ? currentJob.ProfileID.Value.ToString() : "";
                ddlLocation.SelectedValue = currentJob.Location;
                ddlResponsible.SelectedValue = currentJob.CareerManager;
                ddlCompany.SelectedValue = currentJob.CompanyID.HasValue ? currentJob.CompanyID.Value.ToString() : "";
                ddlFunction.SelectedValue = currentJob.FamilyFunctionID;
                calActivatedDate.SelectedDate = currentJob.ActivatedDate;
                calExpiredDate.SelectedDate = currentJob.ExpiredDate;
                calRemindDate.SelectedDate = currentJob.RemindDate;
                txtURL.Text = currentJob.URL;
                chkIsActive.Checked = currentJob.IsActive;
                chkIsConfidential.Checked = currentJob.IsConfidential.HasValue ? currentJob.IsConfidential.Value : false;
                //
                rdoSelectEmail.Checked = !string.IsNullOrEmpty(currentJob.CareerManagerEmail);
                rdoSelectURL.Checked = !string.IsNullOrEmpty(currentJob.URL);
                //
                txtEmail.Text = currentJob.CareerManagerEmail;
                txtURL.Text = currentJob.URL;

                txtTitle.Text = currentJob.Title;
                txtTitleNL.Text = currentJob.Title_NL;
                txtTitleEN.Text = currentJob.Title_EN;
                btnPreview.OnClientClick = string.Format("return onButtonPreview_ClientClick('{0}');", WebConfig.NeosJobDetailURL + currentJob.JobID);
                //show title
                lblJobProfileTitle.Text = currentJob.Title;
            }

            if (Request.QueryString["mode"] == "view")
            {
                EnableControls(false);
            }
            else
                EnableControls(true);
        }
        else
        {
            ddlResponsible.SelectedValue = SessionManager.CurrentUser.UserID;
            EnableControls(true);

            //show title
            lblJobProfileTitle.Text = ResourceManager.GetString("lblRightPaneAddNewJob");
        }
        if (!string.IsNullOrEmpty(Request.QueryString["CompanyID"]))
        {
            ddlCompany.SelectedValue = Request.QueryString["CompanyID"];
            lnkBackToCompany.NavigateUrl = string.IsNullOrEmpty(Request.UrlReferrer.PathAndQuery) ? Request.UrlReferrer.PathAndQuery : string.Format("~/CompanyProfile.aspx?CompanyId={0}&tab=job&mode=edit", Request.QueryString["CompanyID"]);
        }
    }



    private void FillDropDownListData()
    {
        //fill profile dropdown list
        ddlProfile.DataSource = new ParamProfileRepository().FindAll();
        ddlProfile.DataBind();
        ddlProfile.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("select", ""));
        //fill location dropdown list
        ddlLocation.DataSource = new ParamLocationsRepository().GetAllLocations();
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("select", ""));
        //fill responsible dropdown list
        ddlResponsible.DataSource = new ParamUserRepository().GetAllUser(false);
        ddlResponsible.DataBind();
        ddlResponsible.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("select", ""));
        //fill company dropdownlist
        ddlCompany.DataSource = new CompanyRepository().GetAllCompanies();
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("select", ""));
        //fill function dropdownlist 
        ddlFunction.DataSource = new ParamFunctionFamRepository().FindAll();
        ddlFunction.DataBind();
        ddlFunction.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("select", ""));


    }

    protected void OnDropdownCompany_ItemsRequested(object o, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        ddlCompany.DataMember = "Company";
        ddlCompany.DataTextField = "CompanyName";
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataSource = new CompanyRepository().GetAllCompanies();
        ddlCompany.DataBind();
    }

    protected void OnButtonCancel_Click(object sender, EventArgs e)
    {
        //EnableControls(false);
        string url = Request.Url.PathAndQuery;

        if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
            url = url.Replace(Request.QueryString["mode"], "view");
        else
            url += "&mode=view";
        Response.Redirect(url, true);

    }

    protected void OnButtonSave_Click(object sender, EventArgs e)
    {
        Neos.Data.Job job = SaveJobInfo();
        if (job != null)
        {
            string addBackUrl = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
            {
                addBackUrl = "&backurl=visible";
            }
            Response.Redirect(string.Format("~/JobProfile.aspx?JobId={0}&mode=view" + addBackUrl, job.JobID));
        }
        //EnableControls(false);        
    }

    private Neos.Data.Job SaveJobInfo()
    {
        UserMessageRepository messageRepo = new UserMessageRepository();
        JobRepository jobRepo = new JobRepository();
        Neos.Data.Job job = new Neos.Data.Job();

        if (!string.IsNullOrEmpty(Request.QueryString["JobId"])) //edit
        {
            job = jobRepo.FindOne(new Job(Int32.Parse(Request.QueryString["JobId"])));
            if (job != null)
            {
                string responsibleUser = job.CareerManager;
                job = SetInfoForJob(job);
                jobRepo.Update(job);

                if (responsibleUser != ddlResponsible.SelectedValue) // in case select another responsible user
                {
                    if (job.RemindDate.HasValue && job.ExpiredDate.HasValue)
                    {
                        UserMessage message = messageRepo.FindMessagesByRef(job.JobID.ToString());
                        if (message != null)
                        {
                            message.UserID = job.CareerManager;
                            messageRepo.Update(message);
                        }
                    }
                }
            }
        }
        else //edit
        {
            job = SetInfoForJob(job);
            job.CreatedDate = DateTime.Now;
            jobRepo.Insert(job);
            //insert a notification message
            if (job.RemindDate.HasValue && job.ExpiredDate.HasValue)
            {
                UserMessage message = new UserMessage();
                message.UserID = ddlResponsible.SelectedValue;
                message.Type = UserMessageType.JobResponsibility;
                message.Subject = ResourceManager.GetString("message.JobReminderSubject");
                message.MessageContent = string.Format(ResourceManager.GetString("message.JobReminderContent"), job.ExpiredDate.Value.ToString("dd/MM/yyyy hh:mm tt"), "JobProfile.aspx?JobId=" + job.JobID);
                message.RemindDate = job.RemindDate;
                message.CreatedDate = DateTime.Now;
                message.IsUnread = true;
                message.RefID = job.JobID.ToString();
                messageRepo.Insert(message);
            }
        }
        return job;
    }

    protected void OnButtonEdit_Click(object sender, EventArgs e)
    {
        //EnableControls(true);
        string url = Request.Url.PathAndQuery;
        if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
            url = url.Replace(Request.QueryString["mode"], "edit");
        else
            url += "&mode=edit";
        Response.Redirect(url, true);
    }

    private void EnableControls(bool enable)
    {
        lblJobRef.Visible = txtJobRef.Visible = !enable;
        lblNumberOfVisits.Visible = txtNumberOfVisits.Visible = !enable;
        lblNumberApplications.Visible = txtNumberOfApplications.Visible = !enable;
        lblLastModifDate.Visible = calLastModif.Visible = !enable;

        ddlProfile.Enabled = enable;
        ddlLocation.Enabled = enable;
        ddlResponsible.Enabled = enable;
        ddlCompany.Enabled = !string.IsNullOrEmpty(Request.QueryString["CompanyID"]) ? false : enable;
        lnkBackToCompany.Visible = !string.IsNullOrEmpty(Request.QueryString["CompanyID"]);

        ddlFunction.Enabled = enable;
        calActivatedDate.Enabled = enable;
        calExpiredDate.Enabled = enable;
        calRemindDate.Enabled = enable;
        txtURL.Enabled = enable;
        chkIsActive.Enabled = enable;
        chkIsConfidential.Enabled = enable;
        txtTitle.Enabled = enable;
        rdoSelectEmail.Enabled = enable;
        rdoSelectURL.Enabled = enable;
        txtEmail.Enabled = enable;
        txtTitleNL.Enabled = enable;
        txtTitleEN.Enabled = enable;
        txtCompanyDesc.Visible = enable;
        txtCompanyDescView.Visible = !enable;
        txtJobDesc.Visible = enable;
        txtJobDescView.Visible = !enable;
        txtPersonalDesc.Visible = enable;
        txtPersonalDescView.Visible = !enable;
        txtPackageDesc.Visible = enable;
        txtPackageDescView.Visible = !enable;

        txtCompanyDescEN.Visible = enable;
        txtCompanyDescENView.Visible = !enable;
        txtJobDescEN.Visible = enable;
        txtJobDescENView.Visible = !enable;
        txtPersonalDescEN.Visible = enable;
        txtPersonalDescENView.Visible = !enable;
        txtPackageDescEN.Visible = enable;
        txtPackageDescENView.Visible = !enable;

        txtCompanyDescNL.Visible = enable;
        txtCompanyDescNLView.Visible = !enable;
        txtJobDescNL.Visible = enable;
        txtJobDescNLView.Visible = !enable;
        txtPersonalDescNL.Visible = enable;
        txtPersonalDescNLView.Visible = !enable;
        txtPackageDescNL.Visible = enable;
        txtPackageDescNLView.Visible = !enable;

        if (enable)
        {
            txtCompanyDesc.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.CompanyDescription : "";
            txtJobDesc.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.JobDescription : "";
            txtPersonalDesc.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PersonalDescription : "";
            txtPackageDesc.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PackageDescription : "";

            txtCompanyDescEN.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.CompanyDescription_EN : "";
            txtJobDescEN.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.JobDescription_EN : "";
            txtPersonalDescEN.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PersonalDescription_EN : "";
            txtPackageDescEN.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PackageDescription_EN : "";

            txtCompanyDescNL.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.CompanyDescription_NL : "";
            txtJobDescNL.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.JobDescription_NL : "";
            txtPersonalDescNL.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PersonalDescription_NL : "";
            txtPackageDescNL.Content = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PackageDescription_NL : "";
        }
        else
        {
            txtCompanyDescView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.CompanyDescription : "";
            txtJobDescView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.JobDescription : "";
            txtPersonalDescView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PersonalDescription : "";
            txtPackageDescView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PackageDescription : "";

            txtCompanyDescENView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.CompanyDescription_EN : "";
            txtJobDescENView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.JobDescription_EN : "";
            txtPersonalDescENView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PersonalDescription_EN : "";
            txtPackageDescENView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PackageDescription_EN : "";

            txtCompanyDescNLView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.CompanyDescription_NL : "";
            txtJobDescNLView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.JobDescription_NL : "";
            txtPersonalDescNLView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PersonalDescription_NL : "";
            txtPackageDescNLView.Text = SessionManager.CurrentJob != null ? SessionManager.CurrentJob.PackageDescription_NL : "";
        }

        btnSave.Visible = enable;
        btnCancel.Visible = enable;
        btnEdit.Visible = !enable;

        hidMode.Value = enable ? "edit" : "view";
    }

    private Job SetInfoForJob(Job job)
    {
        //Job job = new Job();
        job.IsActive = chkIsActive.Checked;
        job.Title = txtTitle.Text;
        job.CompanyDescription = txtCompanyDesc.Content;
        job.JobDescription = txtJobDesc.Content;
        job.PersonalDescription = txtPersonalDesc.Content;
        job.PackageDescription = txtPackageDesc.Content;
        if (!string.IsNullOrEmpty(ddlProfile.SelectedValue))
        {
            job.ProfileID = int.Parse(ddlProfile.SelectedValue);
        }

        job.Location = ddlLocation.SelectedValue;
        job.CareerManager = ddlResponsible.SelectedValue;


        ParamUser responsible = new ParamUserRepository().GetUserById(ddlResponsible.SelectedValue);
        if (responsible != null)
        {
            job.CareerManagerTitle = responsible.Gender == "M" ? "Monsieur" : "Madame";
            job.CareerManagerLastName = responsible.LastName;
            job.CareerManagerTelephone = responsible.Telephone;

        }
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            job.CompanyID = int.Parse(ddlCompany.SelectedValue); //nullalbe
        }
        job.FamilyFunctionID = ddlFunction.SelectedValue;
        job.IsConfidential = chkIsConfidential.Checked;
        job.ExpiredDate = calExpiredDate.SelectedDate;
        job.LastModifiedDate = DateTime.Now;
        if (rdoSelectEmail.Checked)
        {
            job.CareerManagerEmail = txtEmail.Text;
            job.URL = "";
        }
        else
        {
            job.URL = txtURL.Text;
            job.CareerManagerEmail = "";
        }

        job.ActivatedDate = calActivatedDate.SelectedDate;
        job.RemindDate = calRemindDate.SelectedDate;

        job.Title_NL = txtTitleNL.Text;
        job.CompanyDescription_NL = txtCompanyDescNL.Content;
        job.JobDescription_NL = txtJobDescNL.Content;
        job.PersonalDescription_NL = txtPersonalDescNL.Content;
        job.PackageDescription_NL = txtPackageDescNL.Content;

        job.Title_EN = txtTitleEN.Text;
        job.CompanyDescription_EN = txtCompanyDescEN.Content;
        job.JobDescription_EN = txtJobDescEN.Content;
        job.PersonalDescription_EN = txtPersonalDescEN.Content;
        job.PackageDescription_EN = txtPackageDescEN.Content;

        job.TitleTrack = txtTitle.Text;

        return job;
    }

    protected void OnMyAjaxManager_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("ddlProfileClientChanged") > -1)
        {
            JobProfileAjaxManager.AjaxSettings.AddAjaxSetting(JobProfileAjaxManager, ddlFunction);
            int profileID = -1;
            if (int.TryParse(ddlProfile.SelectedValue, out profileID))
            {
                ParamProfile profile = new ParamProfileRepository().FindOne(new ParamProfile(Int32.Parse(ddlProfile.SelectedValue)));
                if (profile != null)
                {
                    ddlFunction.DataSource = new ParamFunctionFamRepository().GetParamFunctionFamByGenre(profile.ProfileCode);
                    ddlFunction.DataBind();
                    ddlFunction.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("select", ""));
                }
            }
            else
            {
                ddlFunction.DataSource = new ParamFunctionFamRepository().FindAll();
                ddlFunction.DataBind();
                ddlFunction.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("select", ""));
            }
        }
        else if (e.Argument.IndexOf("ViewEditJobProfile") > -1)
        {
            string url = Request.Url.PathAndQuery;
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
        else if (e.Argument.IndexOf("SaveJob") > -1)
        {
            Neos.Data.Job job = SaveJobInfo();
            if (job != null)
            {
                string addBackUrl = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
                {
                    addBackUrl = "&backurl=visible";
                }
                Response.Redirect(string.Format("~/JobProfile.aspx?JobId={0}&mode=view" + addBackUrl, job.JobID));
            }
        }
        else if (e.Argument.IndexOf("PreviewJob") > -1)
        {
            string script = string.Format("openPopUp('{0}')", WebConfig.NeosJobDetailURL + Request.QueryString["JobID"]);
            JobProfileAjaxManager.ResponseScripts.Add(script);
        }
    }

    protected void OnLinkBackClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SessionManager.BackUrl) && SessionManager.BackUrl.Contains("Jobs.aspx"))
        {
            Response.Redirect(SessionManager.BackUrl, true);
        }
    }
}
