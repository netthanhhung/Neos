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
using System.Collections.Generic;

public partial class ChooseContactPopup : System.Web.UI.Page
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
            IList<CompanyContact> conList = new List<CompanyContact>();
            gridContact.DataSource = conList;
            gridContact.DataBind();
            txtConLastName.Focus();
        }
    }

    protected void OnContactSearchClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtConLastName.Text) || !string.IsNullOrEmpty(txtConFirstName.Text))
        {
            IList<CompanyContact> conList = new CompanyContactRepository().SearchCompanyContact(
                                                txtConLastName.Text, txtConFirstName.Text);
            gridContact.DataSource = conList;
            gridContact.DataBind();
            if (conList.Count > 0)
                gridContact.Items[0].Selected = true;
        }
    }

    protected void OnBtnOkClicked(object sender, EventArgs e)
    {
        if (gridContact.SelectedValue != null)
        {
            int contactID = (int)gridContact.SelectedValue;
            CompanyContact selectedContact = new CompanyContactRepository().FindOne(new CompanyContact(contactID));
            string argument = selectedContact.ContactID.ToString() + "/"
                + selectedContact.LastName + " " + selectedContact.FirstName;
            string script = "<script type=\"text/javascript\">";
            script += " OnBtnOkClientClicked(\"" + argument + "\");";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
        }
    }
}
