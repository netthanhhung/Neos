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

public partial class SendPresentationEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }

        if (!IsPostBack)
        {
            FillLabelTexts();
            BindData();
        }
    }

    private void FillLabelTexts()
    {  
        chkAutoCreateAction.Text = ResourceManager.GetString("lblAutomaticallyCreateAction");
        lblTo.Text = ResourceManager.GetString("lblSendMailTo");
        lblCC.Text = ResourceManager.GetString("lblSendMailCC");
        lblSubject.Text = ResourceManager.GetString("lblSendMailSubject");
        btnSendMail.Text = ResourceManager.GetString("btnSendMailSendText");
        lblAttach.Text = ResourceManager.GetString("lblSendMailAttachments");        
    }

    private void BindData()
    {
        if (SessionManager.PresentationEmailObject != null)
        {
            PresentationEmailObject emailObject = SessionManager.PresentationEmailObject;
            chkAutoCreateAction.Checked = emailObject.AutoCreateAction;
            string toMail = string.Empty;
            foreach (string mainEmail in emailObject.MainEmails)
            {
                toMail += mainEmail + ";";
            }
            txtTo.Text = toMail;

            string ccMail = string.Empty;
            foreach (string ccItem in emailObject.CcEmails)
            {
                ccMail += ccItem + ";";
            }
            txtCC.Text = ccMail;

            txtEmailContent.Content = emailObject.Body;

            List<string> attachmentsFileNameList = new List<string>();
            foreach (KeyValuePair<string, string> pair in emailObject.AttachmentList)
            {
                attachmentsFileNameList.Add(pair.Key);
            }
            rptAttachFiles.DataSource = attachmentsFileNameList;
            rptAttachFiles.DataBind();
        }
    }


    protected void onButtonSendMail_Click(object sender, EventArgs e)
    {
        List<string> toEmails = new List<string>();
        List<string> ccEmails = new List<string>();
        List<string> attachedFiles = new List<string>();

        if (!string.IsNullOrEmpty(txtTo.Text))
        {
            toEmails = new List<string>(txtTo.Text.Split(';'));
        }

        if (!string.IsNullOrEmpty(txtCC.Text))
        {
            ccEmails = new List<string>(txtCC.Text.Split(';'));
        }
        foreach (RepeaterItem item in rptAttachFiles.Items)
        {
            CheckBox chkSelectFile = item.FindControl("chkSelectFile") as CheckBox;
            if (chkSelectFile.Checked)
            {
                //attachedFiles.Add(WebConfig.AbsoluteExportDirectory + "\\" + chkSelectFile.Text);
                string fileName = chkSelectFile.Text;
                string absolutePath = SessionManager.PresentationEmailObject.AttachmentList[fileName];
                attachedFiles.Add(absolutePath);
            }
        }
        string message = "";
        bool isValid = true;

        if (toEmails.Count + ccEmails.Count <= 0)
        {   
            //msgMissingEmailAddress
            message = ResourceManager.GetString("msgMissingEmailAddress");
            isValid = false;
        }
        else
        {
            List<string> emailsToCheck = new List<string>();
            emailsToCheck.AddRange(toEmails);
            emailsToCheck.AddRange(ccEmails);

            foreach (string email in emailsToCheck)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    if (!Common.IsValidEmailAddress(email))
                    {
                        message = ResourceManager.GetString("msgInvalidEmailFormat");
                        isValid = false;
                        break;
                    }
                }
            }           
        }

        string script = "<script type='text/javascript'>";
        if (isValid)
        {
            Common.SendEmail(toEmails, ccEmails, attachedFiles, txtSubject.Text, txtEmailContent.Content);
            if (chkAutoCreateAction.Checked)
            {
                CreateAction();
            }
            message = ResourceManager.GetString("msgEmailSent");
            script += "closeWindow();";
        }
        script += string.Format("alert(\"{0}\");", message);
        script += "</script>";
        if (!ClientScript.IsClientScriptBlockRegistered("ShowMessage"))
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script);
    }

    private void CreateAction()
    {
        if (SessionManager.PresentationEmailObject != null)
        {
            PresentationEmailObject emailObject = SessionManager.PresentationEmailObject;
            Neos.Data.Action newAction = new Neos.Data.Action();
            newAction.CandidateID = emailObject.CandidateId;
            newAction.CompanyID = emailObject.CompanyId;
            if(emailObject.ContactId > 0) 
                newAction.ContactID = emailObject.ContactId;
            newAction.Responsable = SessionManager.CurrentUser.UserID;
            newAction.DateCreation = DateTime.Today;
            newAction.DateAction = DateTime.Today;
            newAction.Hour = DateTime.Now;
            newAction.Actif = true;
            //action type="CV envoyé"
            ParamTypeAction typeAction = new ParamTypeActionRepository().GetParamTypeActionByLibelle("CV envoyé");
            if(typeAction != null)
                newAction.TypeAction = typeAction.ParamActionID;
            new ActionRepository().Insert(newAction);
        }
    }

    protected void OnRepeaterAttachFiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            string fileName = (string)e.Item.DataItem;
            if (!string.IsNullOrEmpty(fileName))
            {
                string name = fileName.Substring(fileName.LastIndexOf("\\") + 1, fileName.Length - fileName.LastIndexOf('\\') - 1);
                CheckBox chkSelectFile = (CheckBox)e.Item.FindControl("chkSelectFile");
                if (chkSelectFile != null)
                {
                    chkSelectFile.Text = name;
                }
            }
        }
    }
}
