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

public partial class AdminStudyLevelPopup : System.Web.UI.Page
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
            ParamStudyHierarchyRepository repoHie = new ParamStudyHierarchyRepository();
            ddlHierarchy.DataTextField = "Label";
            ddlHierarchy.DataValueField = "HiararchyID";
            ddlHierarchy.DataSource = repoHie.FindAll();
            ddlHierarchy.DataBind();

            if (!string.IsNullOrEmpty(Request.QueryString["StudyLevelID"]))
            {
                int studyLevelID = int.Parse(Request.QueryString["StudyLevelID"]);
                ParamStudyLevel studyLevel = new ParamStudyLevelRepository().FindOne(new ParamStudyLevel(studyLevelID));
                if(studyLevel.ValueHierarchy.HasValue) 
                    ddlHierarchy.SelectedValue = studyLevel.ValueHierarchy.Value.ToString();
                txtLabel.Text = studyLevel.Label;                
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamStudyLevelRepository repo = new ParamStudyLevelRepository();

        ParamStudyLevel saveItem = new ParamStudyLevel();
        saveItem.ValueHierarchy = int.Parse(ddlHierarchy.SelectedValue);
        saveItem.Label = txtLabel.Text.Trim();        
                
        if (string.IsNullOrEmpty(Request.QueryString["StudyLevelID"]))
        {
            repo.Insert(saveItem);
        }
        else
        {
            saveItem.SchoolID = int.Parse(Request.QueryString["StudyLevelID"]);
            repo.Update(saveItem);            
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
