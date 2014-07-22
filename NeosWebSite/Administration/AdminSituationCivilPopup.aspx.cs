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

public partial class AdminSituationCivilPopup : System.Web.UI.Page
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
            if (Request.QueryString["Code"] != null)
            {
                string code = Request.QueryString["Code"];
                ParamSituationCivil situationCivil = new ParamSituationCivilRepository().GetSituationCivil(code);
                txtCode.Text = situationCivil.Code;
                txtCodeType.Text = situationCivil.CodeType;
                txtLabel.Text = situationCivil.Label;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamSituationCivilRepository repo = new ParamSituationCivilRepository();

        ParamSituationCivil saveItem = new ParamSituationCivil();
        saveItem.Code = txtCode.Text;
        saveItem.CodeType = txtCodeType.Text;
        saveItem.Label = txtLabel.Text;
                
        if (Request.QueryString["Code"] == null)
        {
            //Insert new record
            ParamSituationCivil oldItem = repo.GetSituationCivil(txtCode.Text);
            
            if (oldItem == null)
                repo.InserNewSituationCivil(saveItem);
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
            
            if (Request.QueryString["Code"] == txtCode.Text)
            {
                repo.Update(saveItem);
            }
            else 
            {
                ParamSituationCivil oldItem = repo.GetSituationCivil(Request.QueryString["Code"]);
                if (oldItem.NumberIDUsed <= 0)
                {
                    repo.Delete(oldItem);
                    repo.InserNewSituationCivil(saveItem);
                }
                else
                {
                    string message = ResourceManager.GetString("messageSituationCivilBeingUsed");
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
