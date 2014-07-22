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
using Telerik.Web.UI;
using Neos.Data;
using System.Collections.Generic;

public partial class ContactInfoPopup : System.Web.UI.Page
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
            BindData();
        }
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("companyContactInfoPopupTitle");
        lblType.Text = ResourceManager.GetString("lblType");
        lblZone.Text = ResourceManager.GetString("lblPhoneArea");
        lblPhoneMail.Text = ResourceManager.GetString("lblInfo");
        lblPlace.Text = ResourceManager.GetString("columnPlaceCandidateContact");

    }

    private void BindData()
    {
        if (!IsPostBack)
        {
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactPhone"), "T"));
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactFax"), "F"));
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactMobile"), "G"));
            ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactEmail"), "E"));
        }
        if (!string.IsNullOrEmpty(Request.QueryString["contactInfoId"]))
        {
            try
            {
                CompanyContactTelephone contactInfo = null;
                int contactInfoID = Int32.Parse(Request.QueryString["contactInfoId"]);
                if (contactInfoID > 0)
                {
                    contactInfo = new CompanyContactTelephoneRepository().FindOne(new CompanyContactTelephone(contactInfoID));                    
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["contactId"]))
                    {
                        int contactId = Int16.Parse(Request.QueryString["contactId"]);
                        CompanyContact existedItem = SessionManager.NewCompanyContactList.Find(delegate(CompanyContact t) { return t.ContactID == contactId; });
                        if (existedItem != null)
                        {
                            List<CompanyContactTelephone> listTel = existedItem.ContactInfo;
                            contactInfo = listTel.Find(delegate(CompanyContactTelephone a) { return a.ContactTelephoneID == contactInfoID; });                            
                        }
                    }
                }
                if (contactInfo != null)
                {
                    ddlType.SelectedValue = contactInfo.Type;
                    txtZone.Text = contactInfo.TelephoneZone;
                    txtPhoneMail.Text = contactInfo.Tel;
                    txtPlace.Text = contactInfo.Location;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    protected void OnButtonSaveClick(object sender, EventArgs e)
    {
        CompanyContactTelephoneRepository contactInfoRepo = new CompanyContactTelephoneRepository();
        CompanyContactTelephone contactInfo = new CompanyContactTelephone();
        if (!string.IsNullOrEmpty(Request.QueryString["contactInfoId"]))
        {
            try
            {
                int contactInfoID = Int32.Parse(Request.QueryString["contactInfoId"]);
                contactInfo = contactInfoRepo.FindOne(new CompanyContactTelephone(contactInfoID));
                if (contactInfo != null)
                {
                    contactInfo.Type = ddlType.SelectedValue;
                    contactInfo.TelephoneZone = txtZone.Text.Trim();
                    contactInfo.Tel = txtPhoneMail.Text.Trim();
                    contactInfo.Location = txtPlace.Text.Trim();

                    contactInfoRepo.Update(contactInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["contactId"]))
        {
            contactInfo.Type = ddlType.SelectedValue;
            contactInfo.TelephoneZone = txtZone.Text.Trim();
            contactInfo.Tel = txtPhoneMail.Text.Trim();
            contactInfo.Location = txtPlace.Text.Trim();

            int contactId = Int16.Parse(Request.QueryString["contactId"]);
            if (contactId > 0)
            {
                contactInfo.ContactID = contactId;
                contactInfoRepo.Insert(contactInfo);
            }
            else
            {
                CompanyContact existedItem = SessionManager.NewCompanyContactList.Find(delegate(CompanyContact t) { return t.ContactID == contactId; });
                if (existedItem != null)
                {
                    contactInfo.ContactID = existedItem.ContactID;
                    contactInfo.ContactTelephoneID = 0 - existedItem.ContactInfo.Count - 2;
                    existedItem.ContactInfo.Add(contactInfo);
                }
            }                        
        }

        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("saveAndCloseWindow"))
            ClientScript.RegisterStartupScript(this.GetType(), "saveAndCloseWindow", script);
    }
}
