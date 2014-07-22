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

public partial class AdminUnitPopup : System.Web.UI.Page
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
            if (Request.QueryString["TypeID"] != null)
            {
                string typeID = Request.QueryString["TypeID"];
                ParamType unit = new ParamTypeRepository().FindOne(new ParamType(typeID));
                txtTypeID.Text = unit.TypeID;
                txtLabel.Text = unit.Label;
            }
        }
    }

    private void FillLabelLanguage()
    {
        //rfvAdminUnit1.ErrorMessage = ResourceManager.GetString("messageUnitMustNotBeEmpty");
        //rfvAdminUnit2.ErrorMessage = ResourceManager.GetString("messageUnitCodeMustNotBeEmpty");
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamTypeRepository repo = new ParamTypeRepository();

        ParamType saveItem = new ParamType();
        saveItem.TypeID = txtTypeID.Text;
        saveItem.Label = txtLabel.Text;
        
        if (Request.QueryString["TypeID"] == null)
        {
            ParamType oldItem = repo.FindOne(new ParamType(txtTypeID.Text));
            if (oldItem == null)
                repo.InserNewUnit(saveItem);
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
            repo.Update(saveItem);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
