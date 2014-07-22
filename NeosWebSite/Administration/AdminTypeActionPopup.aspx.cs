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

public partial class AdminTypeActionPopup : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(Request.QueryString["TypeActionID"]))
            {
                int typeActionID = int.Parse(Request.QueryString["TypeActionID"]);
                ParamTypeAction typeAction = new ParamTypeActionRepository().FindOne(new ParamTypeAction(typeActionID));
                txtTypeActionLabel.Text = typeAction.Label;
                txtUnitCode.Text = typeAction.UnitCode;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamTypeActionRepository repo = new ParamTypeActionRepository();

        ParamTypeAction saveItem = new ParamTypeAction();
        saveItem.Label = txtTypeActionLabel.Text.Trim();
        saveItem.UnitCode = txtUnitCode.Text.Trim();
                
        if (string.IsNullOrEmpty(Request.QueryString["TypeActionID"]))
        {
            repo.Insert(saveItem);
        }
        else
        {
            saveItem.ParamActionID = int.Parse(Request.QueryString["TypeActionID"]);
            repo.Update(saveItem);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
