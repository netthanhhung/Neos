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
//using Microsoft.Office.Interop.Outlook;

public partial class ActionDetails : System.Web.UI.Page
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
            InitControls();

            if (!string.IsNullOrEmpty(SessionManager.BackUrl)
                && SessionManager.BackUrl.Contains("Actions.aspx")
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
            //Fill data for combobox
            ddlTypeAction.DataValueField = "ParamActionID";
            ddlTypeAction.DataTextField = "Label";
            ParamTypeActionRepository paramTypeActionRepo = new ParamTypeActionRepository();
            ddlTypeAction.DataSource = paramTypeActionRepo.FindAll();
            ddlTypeAction.DataBind();

            ddlResponsible.DataValueField = "UserID";
            ddlResponsible.DataTextField = "LastName";
            ParamUserRepository userRepo = new ParamUserRepository();
            ddlResponsible.DataSource = userRepo.GetAllUser(true);
            ddlResponsible.DataBind();
            ddlResponsible.SelectedValue = SessionManager.CurrentUser.UserID;

            bool isCompany = false;
            bool isCandidate = false;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                if (Request.QueryString["type"] == "company")
                {
                    isCompany = true;
                    btnCompany.Enabled = false;
                    ddlCompany.Enabled = false;
                }
                else if (Request.QueryString["type"] == "candidate")
                {
                    isCandidate = true;
                    btnCandidate.Enabled = false;
                    ddlCandidate.Enabled = false;
                }
                else if (Request.QueryString["type"] == "action"
                        && string.IsNullOrEmpty(Request.QueryString["ActionID"]))
                {
                    if (SessionManager.CurrentUser != null)
                        ddlResponsible.SelectedValue = SessionManager.CurrentUser.UserID;
                }
            }

            if (SessionManager.CurrentCompany != null && isCompany)
            {
                ddlCompany.Items.Clear();
                ddlCompany.Items.Add(new RadComboBoxItem(SessionManager.CurrentCompany.CompanyName, SessionManager.CurrentCompany.CompanyID.ToString()));
                ddlCompany.SelectedIndex = 0;

                hiddenCompanyId.Value = SessionManager.CurrentCompany.CompanyID.ToString();
                txtCompany.Text = SessionManager.CurrentCompany.CompanyName;
                BindContactListByCompany(SessionManager.CurrentCompany.CompanyID);
            }
            if (SessionManager.CurrentCandidate != null && isCandidate)
            {
                hiddenCandidateId.Value = SessionManager.CurrentCandidate.CandidateId.ToString();
                txtCandidate.Text = SessionManager.CurrentCandidate.FirstName + " " + SessionManager.CurrentCandidate.LastName;
                ddlCandidate.Items.Add(new RadComboBoxItem(SessionManager.CurrentCandidate.FirstName + " " + SessionManager.CurrentCandidate.LastName, SessionManager.CurrentCandidate.CandidateId.ToString()));
                ddlCandidate.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["ActionID"]))
            {

                int actionID = int.Parse(Request.QueryString["ActionID"]);
                ActionRepository repo = new ActionRepository();
                Action action = repo.GetActionByActionID(actionID);
                if (action.CompanyID.HasValue)
                {
                    hiddenCompanyId.Value = action.CompanyID.Value.ToString();
                    BindContactListByCompany(action.CompanyID.Value);
                }
                if (action.CandidateID.HasValue)
                    hiddenCandidateId.Value = action.CandidateID.Value.ToString();
                if (action.ContactID.HasValue)
                {
                    ddlContact.SelectedValue = action.ContactID.Value.ToString();
                }
                else
                    ddlContact.SelectedValue = "-1";

                txtTaskNbr.Text = action.ActionID.ToString();
                chkActive.Checked = action.Actif;
                datDateAction.SelectedDate = action.DateAction;
                //datHour.SelectedDate = action.Hour;
                radTimeHour.SelectedDate = action.Hour;

                if (action.TypeAction.HasValue)
                    ddlTypeAction.SelectedValue = action.TypeAction.Value.ToString();
                datCreationDate.SelectedDate = action.DateCreation;
                txtCompany.Text = action.CompanyName;
                txtCandidate.Text = action.CandidateFullName;

                txtAppointmentPlace.Text = action.LieuRDV;
                txtCompanyResult.Text = action.ResultCompany;
                txtCandidateResult.Text = action.ResultCandidate;
                txtDescription.Text = action.DescrAction;
                ddlResponsible.SelectedValue = action.Responsable;

                //Company dropdownlist
                if (action.CompanyID.HasValue)
                {
                    ddlCompany.Items.Add(new RadComboBoxItem(action.CompanyName, action.CompanyID.ToString()));
                    ddlCompany.SelectedIndex = 0;
                }
                //candidate dropdownlist
                if (action.CandidateID.HasValue)
                {
                    ddlCandidate.Items.Add(new RadComboBoxItem(action.CandidateFullName, action.CandidateID.ToString()));
                    ddlCandidate.SelectedIndex = 0;
                }
            }
        }

        string script = "<script type='text/javascript'>";
        script += "onLoadActionDetailPage();";
        script += "</script>";
        if (!ClientScript.IsClientScriptBlockRegistered("LoadActionDetailPage"))
            ClientScript.RegisterStartupScript(this.GetType(), "LoadActionDetailPage", script);

        
    }

    protected void OnDropdownCandidate_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        string name = e.Text;
        if (!string.IsNullOrEmpty(name))
        {
            if(name.Length >= 3)
            {
                List<Candidate> list = new CandidateRepository().SearchCandidatesOnName(name);
                ddlCandidate.DataTextField = "FullName";
                ddlCandidate.DataValueField = "CandidateId";
                ddlCandidate.DataSource = list;
                ddlCandidate.DataBind();
            }
        }
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

    private void InitControls()
    {
        if (Request.UrlReferrer.PathAndQuery != null)
        {
            urlReferrer.Value = Request.UrlReferrer.PathAndQuery;
        }

        if (Request.QueryString["type"] == "action")
        {
            //ActionID=19120
            if (!string.IsNullOrEmpty(Request.QueryString["ActionID"]))
            {
               if(Request.QueryString["mode"] == "view")
                   EnableControls(false);
               else
                   EnableControls(true);
            }
            else
            {
                EnableControls(true);
            }
        }
        else
        {
            btnEdit.Visible = false;
            btnCancel.Attributes.Add("onclick", "return OnBtnCancelClientClicked();");
        }

        datCreationDate.Enabled = false;
        datCreationDate.Calendar.Enabled = false;
        datCreationDate.SelectedDate = DateTime.Now;
    }

    private void FillLabelLanguage()
    {
        lnkBack.Text = ResourceManager.GetString("backText");

        this.Page.Title = ResourceManager.GetString("candidateActionPopupTitle");
        btnSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");
        btnEdit.Text = ResourceManager.GetString("editText");
        lblActive.Text = ResourceManager.GetString("columnActiveActionCan");
        lblTaskNbr.Text = ResourceManager.GetString("columnTaskNbrActionCan");
        lblDateAction.Text = ResourceManager.GetString("columnDateActionCan");
        lblHour.Text = ResourceManager.GetString("columnHourActionCan");
        lblTypeAction.Text = ResourceManager.GetString("columnTypeActionCan");
        lblCandidate.Text = ResourceManager.GetString("columnCandidateActionCan");
        lblCompany.Text = ResourceManager.GetString("columnCompanyActionCan");        
        lblResponsible.Text = ResourceManager.GetString("columnResponsibleActionCan");

        lblAppointmentPlace.Text = ResourceManager.GetString("lblAppointmentPlace");
        lblCompanyResult.Text = ResourceManager.GetString("lblCompanyResult");
        lblCandidateResult.Text = ResourceManager.GetString("lblCandidateResult");
        lblActionDescription.Text = ResourceManager.GetString("lblActionDescription");
        if (!string.IsNullOrEmpty(Request.QueryString["ActionID"]))
            lblActionDetailTitle.Text = ResourceManager.GetString("lblRightPaneActionDetailTitle");
        else
            lblActionDetailTitle.Text = ResourceManager.GetString("lblRightPaneAddNewActionTitle");
    }

    private void EnableControls(bool enable)
    {
        ddlTypeAction.Enabled = enable;
        datCreationDate.Enabled = enable;
        datDateAction.Enabled = enable;
        radTimeHour.Enabled = enable;
        btnCandidate.Enabled = enable;
        btnCompany.Enabled = enable;
        ddlCompany.Enabled = enable;
        ddlCandidate.Enabled = enable;
        ddlContact.Enabled = enable;
        ddlResponsible.Enabled = enable;
        txtAppointmentPlace.ReadOnly = !enable;
        chkActive.Enabled = enable;
        txtCompanyResult.ReadOnly = !enable;
        txtCandidateResult.ReadOnly = !enable;
        txtDescription.ReadOnly = !enable;
        chkExportToOutlook.Enabled = enable;

        btnSave.Visible = enable;
        btnCancel.Visible = enable;
        btnEdit.Visible = !enable;

        hidMode.Value = enable ? "edit" : "view";
    }

    private void BindContactListByCompany(int companyId)
    {
        ddlContact.DataTextField = "FullName";
        ddlContact.DataValueField = "ContactID";
        List<CompanyContact> list = new CompanyContactRepository().GetContactOfCompany(companyId);
        list.Insert(0, new CompanyContact(-1));

        ddlContact.DataSource = list;
        ddlContact.DataBind();
    }

    protected void OnBtnEdit_Clicked(object sender, EventArgs e)
    {
        //EnableControls(true);        
        string url = Request.Url.PathAndQuery;

        if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
            url = url.Replace(Request.QueryString["mode"], "edit");
        else
            url += "&mode=edit";
        Response.Redirect(url, true);

    }

    protected void OnBtnCancel_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(urlReferrer.Value) || urlReferrer.Value.Contains("Home.aspx"))
        {
            Response.Redirect("~/Actions.aspx", true);
        }
        else if(urlReferrer.Value.Contains("ActionDetails.aspx"))
        {
            EnableControls(false);
        }
        else
            Response.Redirect(urlReferrer.Value, true);
    }
    protected void OnBtnSave_Clicked(object sender, EventArgs e)
    {
        Action saveItem = SaveAction();
        if (saveItem == null) return;
        if (Request.QueryString["type"] == "action")
        {
            string addBackUrl = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
            {
                addBackUrl = "&backurl=visible";
            }
            Response.Redirect(string.Format("~/ActionDetails.aspx?ActionID={0}&type=action&mode=view" + addBackUrl, saveItem.ActionID), true);
        }
        else
        {
            string script = "<script type=\"text/javascript\">";
            script += " OnBtnSaveClientClicked();";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("saveAction"))
                ClientScript.RegisterStartupScript(this.GetType(), "saveAction", script);
        }
    }

    private Action SaveAction()
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return null;
        }

        ActionRepository repo = new ActionRepository();
        Action saveItem = GetAction();
        if (string.IsNullOrEmpty(Request.QueryString["ActionID"]))
        {
            //Insert new record
            repo.Insert(saveItem);
        }
        else
        {
            //Update the record.
            saveItem.ActionID = int.Parse(Request.QueryString["ActionID"]);
            repo.Update(saveItem);
        }
        if (chkExportToOutlook.Checked)
        {

            string message = Common.ExportActionToAppoinment(saveItem);
            //string script1 = "<script type=\"text/javascript\">";
            string script1 = " alert(\"" + message + "\")";
            //script1 += " </script>";
            //if (!this.ClientScript.IsClientScriptBlockRegistered("exportAction"))
            //    this.ClientScript.RegisterStartupScript(this.GetType(), "exportAction", script1); 
            ActionDetailAjaxManager.ResponseScripts.Add(script1);

        }

        return saveItem;
    }    

    private Action GetAction()
    {
        Action saveItem = new Action();
        if (!string.IsNullOrEmpty(ddlTypeAction.SelectedValue))
        {
            saveItem.TypeAction = int.Parse(ddlTypeAction.SelectedValue);
            saveItem.TypeActionLabel = ddlTypeAction.Text;
        }
        saveItem.DateCreation = datCreationDate.SelectedDate;
        saveItem.DateAction = datDateAction.SelectedDate;
        if (saveItem.DateAction.HasValue && radTimeHour.SelectedDate.HasValue)
        {
            DateTime date = saveItem.DateAction.Value;
            DateTime hour = new DateTime(date.Year, date.Month, date.Day,
                radTimeHour.SelectedDate.Value.Hour, radTimeHour.SelectedDate.Value.Minute, radTimeHour.SelectedDate.Value.Second);
            saveItem.Hour = hour;
        }
        else
        {
            saveItem.Hour = radTimeHour.SelectedDate;// datHour.SelectedDate;
        }

        /*if(!string.IsNullOrEmpty(hiddenCompanyId.Value))
            saveItem.CompanyID = int.Parse(hiddenCompanyId.Value);
        if (!string.IsNullOrEmpty(hiddenCandidateId.Value))
            saveItem.CandidateID = int.Parse(hiddenCandidateId.Value);*/
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            /*saveItem.CandidateFullName = txtCandidate.Text.Trim();
            saveItem.CompanyName = txtCompany.Text.Trim();*/

            saveItem.CompanyID = int.Parse(ddlCompany.SelectedValue);
            saveItem.CompanyName = ddlCompany.Text;
        }
        if (!string.IsNullOrEmpty(ddlCandidate.SelectedValue))
        {
            saveItem.CandidateID = int.Parse(ddlCandidate.SelectedValue);
            saveItem.CandidateFullName = ddlCandidate.Text;
        }
        if (!string.IsNullOrEmpty(ddlContact.SelectedValue) && ddlContact.SelectedValue != "-1")
            saveItem.ContactID = int.Parse(ddlContact.SelectedValue);
        saveItem.ResultCompany = txtCompanyResult.Text.Trim();
        saveItem.ResultCandidate = txtCandidateResult.Text.Trim();
        saveItem.DescrAction = txtDescription.Text.Trim();
        if(!string.IsNullOrEmpty(ddlResponsible.SelectedValue)) 
            saveItem.Responsable = ddlResponsible.SelectedValue;
        saveItem.LieuRDV = txtAppointmentPlace.Text.Trim();
        saveItem.Actif = chkActive.Checked;
        return saveItem;
    }

    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindContactListByCompany") != -1)
        {
            string[] param = e.Argument.Split('-');
            if (param.Length == 2)
            {
                ActionDetailAjaxManager.AjaxSettings.AddAjaxSetting(ActionDetailAjaxManager, ddlContact);
                BindContactListByCompany(int.Parse(param[1]));
            }
        }
        else if (e.Argument.IndexOf("ViewEditActionDetail") != -1)
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
        else if (e.Argument.IndexOf("SaveActionDetail") != -1)
        {
            Action saveItem = SaveAction();
            if (saveItem != null)
            {
                if (Request.QueryString["type"] == "action")
                {
                    string addBackUrl = string.Empty;
                    if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
                    {
                        addBackUrl = "&backurl=visible";
                    }
                    Response.Redirect(string.Format("~/ActionDetails.aspx?ActionID={0}&type=action&mode=view" + addBackUrl, saveItem.ActionID), true);
                }
            }
        }
    }
    protected void OnLinkBackClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SessionManager.BackUrl) && SessionManager.BackUrl.Contains("Actions.aspx"))
        {
            Response.Redirect(SessionManager.BackUrl, true);
        }
    }
}
