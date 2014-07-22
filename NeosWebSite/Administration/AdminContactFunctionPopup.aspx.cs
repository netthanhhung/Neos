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

public partial class AdminContactFunctionPopup : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(Request.QueryString["ContactFunctionID"]))
            {
                int contactFunctionID = int.Parse(Request.QueryString["ContactFunctionID"]);
                ParamContactFunction contactFunction = new ParamContactFunctionRepository().FindOne(new ParamContactFunction(contactFunctionID));
                txtFunction.Text = contactFunction.FunctionName;
                txtLogicalOrder.Text = contactFunction.LogicalOrder;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamContactFunctionRepository repo = new ParamContactFunctionRepository();

        ParamContactFunction saveItem = new ParamContactFunction();
        saveItem.FunctionName = txtFunction.Text.Trim();
        saveItem.LogicalOrder = txtLogicalOrder.Text.Trim();
                
        if (string.IsNullOrEmpty(Request.QueryString["ContactFunctionID"]))
        {            
            repo.Insert(saveItem);            
        }
        else
        {
            saveItem.ContactFunctionID = int.Parse(Request.QueryString["ContactFunctionID"]);
            repo.Update(saveItem);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
