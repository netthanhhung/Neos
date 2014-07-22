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

public partial class CompanyContactPopup : System.Web.UI.Page
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
        this.Page.Title = ResourceManager.GetString("companyContactPopupTitle");
        lblFirstName.Text = ResourceManager.GetString("columnFirstNameCandidateGrid");
        lblLastName.Text = ResourceManager.GetString("columnLastNameCandidateGrid");
        lblFunction.Text = ResourceManager.GetString("lblCanFunction");
        lblGender.Text = ResourceManager.GetString("lblGender");
        lblLanguage.Text = ResourceManager.GetString("lblCanLanguage");
        btnSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");
        lblRemark.Text = ResourceManager.GetString("lblCanRemarkGeneral");
        chkReceiveNeosNewsletter.Text = ResourceManager.GetString("lblReceiveNeosNewsletter");
    }

    private void BindData()
    {
        //load function list
        ddlFunction.DataSource = new ParamContactFunctionRepository().FindAllWithAscSort();
        ddlFunction.DataBind();
        //load gender
        RadComboBoxItem nullItem = new RadComboBoxItem("", "");
        RadComboBoxItem fItem = new RadComboBoxItem("F","F");
        RadComboBoxItem mItem = new RadComboBoxItem("M", "M");
        ddlGender.Items.Add(nullItem);
        ddlGender.Items.Add(fItem);
        ddlGender.Items.Add(mItem);

        if (!string.IsNullOrEmpty(Request.QueryString["contactID"])) //edit contact
        {
            int contactId = Int32.Parse(Request.QueryString["contactID"]);
            CompanyContact contact = new CompanyContact();
            if (contactId > 0)
            {
                CompanyContactRepository contactRepo = new CompanyContactRepository();
                contact = contactRepo.FindOne(new CompanyContact(Int32.Parse(Request.QueryString["contactID"])));
            }
            else
            {
                contact = SessionManager.NewCompanyContactList.Find(delegate(CompanyContact t) { return t.ContactID == contactId; });
            }

            txtFirstName.Text = contact.FirstName;
            txtLastName.Text = contact.LastName;
            ddlFunction.Text = contact.Position;
            ddlGender.SelectedValue = contact.Gender;
            txtLanguage.Text = contact.Language;
            txtRemark.Text = contact.Remark;
            chkReceiveNeosNewsletter.Checked = contact.Newsletter;

        }

    }

    protected void OnButtonSaveClick(object sender, EventArgs e)
    {
        CompanyContactRepository contactRepo = new CompanyContactRepository();
        CompanyContact contact = GetCompanyContact();
        if (!string.IsNullOrEmpty(Request.QueryString["contactID"])) //edit contact
        {
            int contactID = Int32.Parse(Request.QueryString["contactID"]);
            if (contactID > 0)
            {
                contact.ContactID = contactID;
                contactRepo.Update(contact);
            }
            else
            {
                List<CompanyContact> list = SessionManager.NewCompanyContactList;
                CompanyContact existedItem = list.Find(delegate(CompanyContact t) { return t.ContactID == contactID; });
                if (existedItem != null)
                {
                    int index = list.IndexOf(existedItem);
                    contact.ContactInfo = existedItem.ContactInfo;
                    list.Remove(existedItem);
                    contact.ContactID = existedItem.ContactID;
                    list.Insert(index, contact);
                    SessionManager.NewCompanyContactList = list;
                }
            }
            
        }
        else //add new
        {            
            if (!string.IsNullOrEmpty(Request.QueryString["companyID"]))
            {
                contact.CompanyID = Int32.Parse(Request.QueryString["companyID"]);
                contactRepo.Insert(contact);
            }
            else
            {
                //save to session
                List<CompanyContact> list = SessionManager.NewCompanyContactList;
                contact.ContactID = 0 - list.Count - 2;
                list.Add(contact);
                SessionManager.NewCompanyContactList = list;
            }
        }
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("saveAndCloseWindow"))
            ClientScript.RegisterStartupScript(this.GetType(), "saveAndCloseWindow", script);
    }

    private CompanyContact GetCompanyContact()
    {
        CompanyContact contact = new CompanyContact();
        if (!string.IsNullOrEmpty(Request.QueryString["contactID"]))
        {
            int contactId = int.Parse(Request.QueryString["contactID"]);
            if (contactId > 0)
            {
                contact = new CompanyContactRepository().FindOne(new CompanyContact(contactId));   
            }
        }
        //contact.CompanyID = Int32.Parse(Request.QueryString["companyID"]);
        contact.LastName = txtLastName.Text.Trim();
        contact.FirstName = txtFirstName.Text.Trim();
        contact.Position = ddlFunction.Text;
        contact.Gender = ddlGender.SelectedValue;
        contact.Language = txtLanguage.Text.Trim();
        contact.Remark = txtRemark.Text;
        contact.Newsletter = chkReceiveNeosNewsletter.Checked;
        return contact;
    }
}