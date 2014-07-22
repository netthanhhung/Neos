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

public partial class AdminNationalityPopup : System.Web.UI.Page
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
            if (Request.QueryString["NationalityID"] != null)
            {
                string nationalityID = Request.QueryString["NationalityID"];
                ParamNationalite nationality = new ParamNationaliteRepository().GetNationalityByID(nationalityID);
                txtNationalityID.Text = nationality.NationaliteID;
                txtLabel.Text = nationality.Label;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamNationaliteRepository repo = new ParamNationaliteRepository();

        ParamNationalite saveItem = new ParamNationalite();
        saveItem.NationaliteID = txtNationalityID.Text;
        saveItem.Label = txtLabel.Text;
                
        if (Request.QueryString["NationalityID"] == null)
        {
            //Insert new record
            ParamNationalite oldItem = repo.GetNationalityByID(txtNationalityID.Text);
            
            if (oldItem == null)
                repo.InserNewNationality(saveItem);
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
            
            if (Request.QueryString["NationalityID"] == txtNationalityID.Text)
            {
                repo.Update(saveItem);
            }
            else 
            {
                ParamNationalite oldItem = repo.GetNationalityByID(Request.QueryString["NationalityID"]);
                if (oldItem.NumberIDUsed <= 0)
                {
                    repo.Delete(oldItem);
                    repo.InserNewNationality(saveItem);
                }
                else
                {
                    string message = ResourceManager.GetString("messageNationalityBeingUsed");
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
