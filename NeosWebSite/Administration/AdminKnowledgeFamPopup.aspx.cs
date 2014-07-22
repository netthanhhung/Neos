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

public partial class AdminKnowledgeFamPopup : System.Web.UI.Page
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
            if (Request.QueryString["KnowledgeFamID"] != null)
            {
                string knowledgeFamID = Request.QueryString["KnowledgeFamID"];
                ParamKnowledgeFam knowledgeFam = new ParamKnowledgeFamRepository().GetKnowledgeFamByID(knowledgeFamID);
                txtKnowledgeFamID.Text = knowledgeFam.ConFamilleID;
                txtGenre.Text = knowledgeFam.Genre;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamKnowledgeFamRepository repo = new ParamKnowledgeFamRepository();

        ParamKnowledgeFam saveItem = new ParamKnowledgeFam();
        saveItem.ConFamilleID = txtKnowledgeFamID.Text;
        saveItem.Genre = txtGenre.Text;
                
        if (Request.QueryString["KnowledgeFamID"] == null)
        {
            //Insert new record
            ParamKnowledgeFam oldItem = repo.GetKnowledgeFamByID(txtKnowledgeFamID.Text);
            
            if (oldItem == null)
                repo.InserNewKnowledgeFam(saveItem);
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
            
            if (Request.QueryString["KnowledgeFamID"] == txtKnowledgeFamID.Text)
            {
                repo.Update(saveItem);
            }
            else 
            {
                ParamKnowledgeFam oldItem = repo.GetKnowledgeFamByID(Request.QueryString["KnowledgeFamID"]);
                if (oldItem.NumberIDUsed <= 0)
                {
                    repo.Delete(oldItem);
                    repo.InserNewKnowledgeFam(saveItem);
                }
                else
                {
                    string message = ResourceManager.GetString("messageKnowledgeFamBeingUsed");
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
