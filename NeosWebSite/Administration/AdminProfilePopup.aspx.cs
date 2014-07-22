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

public partial class AdminProfilePopup : System.Web.UI.Page
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
            if (Request.QueryString["ProfileID"] != null)
            {
                int profileID = int.Parse(Request.QueryString["ProfileID"]);
                ParamProfile profile = new ParamProfileRepository().FindOne(new ParamProfile(profileID));
                txtProfile.Text = profile.Profile;
                txtCode.Text = profile.ProfileCode;
            }
        }
    }

    private void FillLabelLanguage()
    {
        //rfvAdminProfile1.ErrorMessage = ResourceManager.GetString("messageProfileMustNotBeEmpty");
        //rfvAdminProfile2.ErrorMessage = ResourceManager.GetString("messageProfileCodeMustNotBeEmpty");
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamProfileRepository repo = new ParamProfileRepository();

        ParamProfile saveItem = new ParamProfile();
        saveItem.Profile = txtProfile.Text.Trim();
        saveItem.ProfileCode = txtCode.Text.Trim();
        
        if (string.IsNullOrEmpty(Request.QueryString["ProfileID"]))
        {            
            repo.Insert(saveItem);            
        }
        else
        {
            saveItem.ProfileID = int.Parse(Request.QueryString["ProfileID"]);
            repo.Update(saveItem);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
