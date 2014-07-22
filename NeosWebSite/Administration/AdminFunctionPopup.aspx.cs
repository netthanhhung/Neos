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

public partial class AdminFunctionPopup : System.Web.UI.Page
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
            ParamFunctionFamRepository repoFunc = new ParamFunctionFamRepository();
            ddlFunctionFam.DataTextField = "FonctionFamID";
            ddlFunctionFam.DataValueField = "FonctionFamID";
            ddlFunctionFam.DataSource = repoFunc.GetAllFunctionFams();
            ddlFunctionFam.DataBind();

            if (!string.IsNullOrEmpty(Request.QueryString["FunctionID"]))
            {
                int functionID = int.Parse(Request.QueryString["FunctionID"]);
                ParamFunction function = new ParamFunctionRepository().FindOne(new ParamFunction(functionID));
                ddlFunctionFam.SelectedValue = function.FunctionFamID;
                txtLabel.Text = function.Label;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamFunctionRepository repo = new ParamFunctionRepository();

        ParamFunction saveItem = new ParamFunction();
        saveItem.FunctionFamID = ddlFunctionFam.SelectedValue;
        saveItem.Label = txtLabel.Text.Trim();
                
        if (string.IsNullOrEmpty(Request.QueryString["FunctionID"]))
        {
            repo.Insert(saveItem);
        }
        else
        {
            saveItem.FunctionID = int.Parse(Request.QueryString["FunctionID"]);
            repo.Update(saveItem);            
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
