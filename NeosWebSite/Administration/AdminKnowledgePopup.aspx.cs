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

public partial class AdminKnowledgePopup : System.Web.UI.Page
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
            ParamKnowledgeFamRepository repoKnowFam = new ParamKnowledgeFamRepository();
            ddlKnowledgeFam.DataTextField = "ConFamilleID";
            ddlKnowledgeFam.DataValueField = "ConFamilleID";
            ddlKnowledgeFam.DataSource = repoKnowFam.GetAllKnowledgeFams();
            ddlKnowledgeFam.DataBind();

            if (!string.IsNullOrEmpty(Request.QueryString["KnowledgeID"]))
            {
                int knowledgeID = int.Parse(Request.QueryString["KnowledgeID"]);
                ParamKnowledge knowledge = new ParamKnowledgeRepository().FindOne(new ParamKnowledge(knowledgeID));
                ddlKnowledgeFam.SelectedValue = knowledge.KnowledgeFamID;
                txtCode.Text = knowledge.Code;
                txtDefinition.Text = knowledge.Definition;
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamKnowledgeRepository repo = new ParamKnowledgeRepository();

        ParamKnowledge saveItem = new ParamKnowledge();
        saveItem.KnowledgeFamID = ddlKnowledgeFam.SelectedValue;
        saveItem.Code = txtCode.Text.Trim();
        saveItem.Definition = txtDefinition.Text.Trim();
                
        if (string.IsNullOrEmpty(Request.QueryString["KnowledgeID"]))
        {
            repo.Insert(saveItem);
        }
        else
        {
            saveItem.KnowledgeID = int.Parse(Request.QueryString["KnowledgeID"]);
            repo.Update(saveItem);            
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
