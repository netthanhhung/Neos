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

public partial class AdminPermissionPopup : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(Request.QueryString["Code"]))
            {
                string code = Request.QueryString["Code"];
                ParamPermission permission = new ParamPermissionRepository().FindOne(new ParamPermission(code));
                txtCode.Text = permission.PermissionCode;
                txtDescription.Text = permission.PermissionDescription;
            }
        }
    }

    private void FillLabelLanguage()
    {
        rfvAdminPermission.ErrorMessage = ResourceManager.GetString("messagePermissionCodeMustNotBeEmpty");
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamPermissionRepository repo = new ParamPermissionRepository();

        ParamPermission saveItem = new ParamPermission();
        saveItem.PermissionCode = txtCode.Text.Trim();
        saveItem.PermissionDescription = txtDescription.Text.Trim();
        string perCode = Request.QueryString["Code"];
        if (string.IsNullOrEmpty(perCode))
        {
            //Insert new record
            ParamPermission oldItem = repo.FindOne(new ParamPermission(txtCode.Text.Trim()));
            if (oldItem == null)
                repo.Insert(saveItem);
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
            perCode = perCode.Trim();
            if (saveItem.PermissionCode.Trim() == perCode)
            {
                //Update the record.            
                repo.Update(saveItem);
            }
            else
            {
                IList<ParamUserPermission> userPerList =
                    new ParamUserPermissionRepository().GetUsersHavePermission(perCode);
                if (userPerList.Count == 0)
                {
                    //Delete the old one and add the new one.
                    ParamPermission deleteItem = new ParamPermission(Request.QueryString["Code"].Trim());
                    repo.Delete(deleteItem);
                    repo.Insert(saveItem);
                }
                else
                {
                    string message = ResourceManager.GetString("messagePermissionBeingUsed");
                    string scriptMes = "<script type=\"text/javascript\">";
                    scriptMes += " alert(\"" + message + "\")";
                    scriptMes += " </script>";

                    if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                        ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", scriptMes);
                    return;
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
