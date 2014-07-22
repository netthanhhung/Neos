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

public partial class AdminStudyPopup : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(Request.QueryString["StudyID"]))
            {
                int studyID = int.Parse(Request.QueryString["StudyID"]);
                ParamFormation study = new ParamFormationRepository().FindOne(new ParamFormation(studyID));
                txtStudyLabel.Text = study.Label;                
            }
        }
    }

    private void FillLabelLanguage()
    {
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamFormationRepository repo = new ParamFormationRepository();

        ParamFormation saveItem = new ParamFormation();
        saveItem.Label = txtStudyLabel.Text.Trim();        
                
        if (string.IsNullOrEmpty(Request.QueryString["StudyID"]))
        {
            IList<ParamFormation> oldList = repo.GetAllParamFormations(saveItem.Label);
            if (oldList.Count > 0)
            {
                string message = ResourceManager.GetString("itemAlreadyExist");
                string script1 = "<script type=\"text/javascript\">";
                script1 += " alert(\"" + message + "\");";
                script1 += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
            }
            else
            {
                repo.Insert(saveItem);
            }
        }
        else
        {
            saveItem.FormationID = int.Parse(Request.QueryString["StudyID"]);
            repo.Update(saveItem);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
