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

public partial class AdminLegalFormPopup : System.Web.UI.Page
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
            if (Request.QueryString["FormID"] != null)
            {
                string formID = Request.QueryString["FormID"];
                ParamLegalForm form = new ParamLegalFormRepository().FindOne(new ParamLegalForm(formID));
                txtFormID.Text = form.FormID;                
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamLegalFormRepository repo = new ParamLegalFormRepository();

        ParamLegalForm saveItem = new ParamLegalForm();
        saveItem.FormID = txtFormID.Text;
                
        if (Request.QueryString["FormID"] == null)
        {
            ParamLegalForm oldItem = repo.FindOne(saveItem);
            if (oldItem != null)
            {
                string message = ResourceManager.GetString("itemAlreadyExist");
                string script1 = "<script type=\"text/javascript\">";
                script1 += " alert(\"" + message + "\");";
                script1 += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
            }
            else
            {                
                repo.Insert(saveItem);
            }
        }
        else
        {
            repo.UpdateLegalForm(txtFormID.Text, Request.QueryString["FormID"]);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
