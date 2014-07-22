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

public partial class AdminLanguagePopup : System.Web.UI.Page
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
            if (Request.QueryString["LanguageID"] != null)
            {
                string languageID = Request.QueryString["LanguageID"];
                ParamLangue language = new ParamLangueRepository().GetLanguageByID(languageID);
                txtLanguageID.Text = language.LangueID;
                txtLabel.Text = language.Label;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamLangueRepository repo = new ParamLangueRepository();

        ParamLangue saveItem = new ParamLangue();
        saveItem.LangueID = txtLanguageID.Text;
        saveItem.Label = txtLabel.Text;
                
        if (Request.QueryString["LanguageID"] == null)
        {
            //Insert new record
            ParamLangue oldItem = repo.GetLanguageByID(txtLanguageID.Text);
            
            if (oldItem == null)
                repo.InserNewLanguage(saveItem);
            else
            {
                string message = ResourceManager.GetString("itemAlreadyExist");
                string script1 = "<script type=\"text/javascript\">";
                script1 += " alert(\"" + message + "\");";
                script1 += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
            }
        }
        else
        {
            
            if (Request.QueryString["LanguageID"] == txtLanguageID.Text)
            {
                repo.Update(saveItem);
            }
            else 
            {
                ParamLangue oldItem = repo.GetLanguageByID(Request.QueryString["LanguageID"]);
                if (oldItem.NumberIDUsed <= 0)
                {
                    repo.Delete(oldItem);
                    repo.InserNewLanguage(saveItem);
                }
                else
                {
                    string message = ResourceManager.GetString("messageLanguageBeingUsed");
                    string script1 = "<script type=\"text/javascript\">";
                    script1 += " alert(\"" + message + "\");";
                    script1 += " </script>";

                    if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                        ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
                }
            }
            
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
