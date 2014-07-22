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

public partial class CandidateTelephonePopup : System.Web.UI.Page
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
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactPhone"), "T"));
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactFax"), "F"));
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactMobile"), "G"));
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactEmail"), "E"));

        }
        if (!string.IsNullOrEmpty(Request.QueryString["TelePhoneId"]))
        {
            if (!IsPostBack)
            {
                CandidateTelephone telephone = new CandidateTelephone();
                int telephoneID = int.Parse(Request.QueryString["TelePhoneId"]);
                if (telephoneID > 0) //existed in database
                {
                    CandidateTelephoneRepository repo = new CandidateTelephoneRepository();
                    telephone = repo.FindOne(new CandidateTelephone(telephoneID));

                    
                }
                else //get from session data
                {
                    telephone = SessionManager.NewCandidateTelephoneList.Find(delegate(CandidateTelephone t) { return t.TelePhoneId == telephoneID; });
                }

                ddlType.SelectedValue = telephone.Type;
                txtZone.Text = telephone.PhoneArea;
                txtPhoneMail.Text = telephone.PhoneMail;
                txtPlace.Text = telephone.Location;
            }
        }
    }

    private void InitControls()
    {
        txtZone.Focus();
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("candidateTelephonePopupTitle");
        lblType.Text = ResourceManager.GetString("columnTypeCandidateContact");
        lblZone.Text = ResourceManager.GetString("columnZoneCandidateContact");
        lblPhoneMail.Text = ResourceManager.GetString("columnPhoneMailCandidateContact");
        lblPlace.Text = ResourceManager.GetString("columnPlaceCandidateContact");  
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        CandidateTelephoneRepository repo = new CandidateTelephoneRepository();
        CandidateTelephone saveItem = GetCadidateTelephone();
        if (string.IsNullOrEmpty(Request.QueryString["TelePhoneId"]))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["candidateID"]))
            {
                //Insert new record
                repo.Insert(saveItem);
            }
            else
            {
                //save to session
                List<CandidateTelephone> list = SessionManager.NewCandidateTelephoneList;                
                saveItem.TelePhoneId = 0 - list.Count - 2;
                list.Add(saveItem);                
                SessionManager.NewCandidateTelephoneList = list;
            }
        }
        else
        {
            int telephoneID = int.Parse(Request.QueryString["TelePhoneId"]);
            if (telephoneID > 0)//existed in database
            {
                //Update the record.
                saveItem.TelePhoneId = telephoneID;
                repo.Update(saveItem);
            }
            else //get from session data
            {
                List<CandidateTelephone> list = SessionManager.NewCandidateTelephoneList;                
                CandidateTelephone existedItem = list.Find(delegate(CandidateTelephone t) { return t.TelePhoneId == telephoneID; });
                if (existedItem != null)
                {
                    int index = list.IndexOf(existedItem);
                    list.Remove(existedItem);
                    saveItem.TelePhoneId = existedItem.TelePhoneId;
                    list.Insert(index, saveItem);
                    SessionManager.NewCandidateTelephoneList = list;
                }
            }
        }
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";        
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);

        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.Page.ClientID, script);   
    }

    private CandidateTelephone GetCadidateTelephone()
    {
        CandidateTelephone saveItem = new CandidateTelephone();
        if (!string.IsNullOrEmpty(Request.QueryString["TelePhoneId"]))
        {
            int telId = int.Parse(Request.QueryString["TelePhoneId"]);
            if (telId > 0)
            {
                saveItem = new CandidateTelephoneRepository().FindOne(new CandidateTelephone(telId));
            }
        }
        if (!string.IsNullOrEmpty(Request.QueryString["candidateID"]))
            saveItem.CandidateID = Convert.ToInt32(Request.QueryString["candidateID"]);
        saveItem.Type = ddlType.SelectedValue;
        saveItem.TypeLabel = ddlType.Text;
        saveItem.PhoneArea = txtZone.Text.Trim();
        saveItem.PhoneMail = txtPhoneMail.Text.Trim();
        saveItem.Location = txtPlace.Text.Trim();        
        return saveItem;
    }
}
