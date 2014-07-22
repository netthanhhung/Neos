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

public partial class AdminFunctionFamPopup : System.Web.UI.Page
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
            if (Request.QueryString["FunctionFamID"] != null)
            {
                string functionFamID = Request.QueryString["FunctionFamID"];
                ParamFunctionFam functionFam = new ParamFunctionFamRepository().GetFunctionFamsByID(functionFamID);
                txtFunctionFamID.Text = functionFam.FonctionFamID;
                txtGenre.Text = functionFam.Genre;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamFunctionFamRepository repo = new ParamFunctionFamRepository();

        ParamFunctionFam saveItem = new ParamFunctionFam();
        saveItem.FonctionFamID = txtFunctionFamID.Text;
        saveItem.Genre = txtGenre.Text;
                
        if (Request.QueryString["FunctionFamID"] == null)
        {
            //Insert new record
            ParamFunctionFam oldItem = repo.GetFunctionFamsByID(txtFunctionFamID.Text);
            
            if (oldItem == null)
                repo.InserNewFunctionFam(saveItem);
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
            
            if (Request.QueryString["FunctionFamID"] == txtFunctionFamID.Text)
            {
                repo.Update(saveItem);
            }
            else 
            {
                ParamFunctionFam oldItem = repo.GetFunctionFamsByID(Request.QueryString["FunctionFamID"]);
                if (oldItem.NumberIDUsed <= 0)
                {
                    repo.Delete(oldItem);
                    repo.InserNewFunctionFam(saveItem);
                }
                else
                {
                    string message = ResourceManager.GetString("messageFunctionFamBeingUsed");
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
