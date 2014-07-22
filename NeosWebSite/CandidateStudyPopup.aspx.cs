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
using Telerik.Web.UI;
using System.Text.RegularExpressions;

public partial class CandidateStudyPopup : System.Web.UI.Page
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
            //Fill data from drop down list.
            int nowYear = DateTime.Today.Year;
            ddlPeriodeFrom.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
            ddlPeriodeTo.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
            
            for (int i = nowYear; i >= 1900; i--)
            {
                ddlPeriodeFrom.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                ddlPeriodeTo.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }            

            ddlTraining.DataValueField = "FormationID";
            ddlTraining.DataTextField = "Label";
            ParamFormationRepository formationRepo = new ParamFormationRepository();
            ddlTraining.DataSource = formationRepo.FindAllWithAscSort();      
            ddlTraining.DataBind();

            ddlLevel.DataValueField = "SchoolID";
            ddlLevel.DataTextField = "Label";
            ParamStudyLevelRepository studyLevelRepo = new ParamStudyLevelRepository();
            ddlLevel.DataSource = studyLevelRepo.FindAll();
            ddlLevel.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, string.Empty));
            ddlLevel.DataBind();
        }
        if (!string.IsNullOrEmpty(Request.QueryString["CandidateFormationID"]))
        {
            if (!IsPostBack)
            {
                int candidateFormationID = int.Parse(Request.QueryString["CandidateFormationID"]);
                CandidateTrainingRepository repo = new CandidateTrainingRepository();
                CandidateTraining training = repo.FindOne(new CandidateTraining(candidateFormationID));
          
                if (!string.IsNullOrEmpty(training.Period))
                {
                    txtPeriodeString.Text = training.Period;

                    string expression = @"\d{4}\s*-\s*\d{4}";
                    if (Regex.IsMatch(training.Period, expression))
                    {
                        string[] yearArray = training.Period.Split('-');
                        if (1900 <= int.Parse(yearArray[0]) && int.Parse(yearArray[0]) <= DateTime.Now.Year)
                            ddlPeriodeFrom.SelectedValue = yearArray[0];
                        else
                            ddlPeriodeFrom.SelectedValue = string.Empty;
                        if (1900 <= int.Parse(yearArray[1]) && int.Parse(yearArray[1]) <= DateTime.Now.Year)
                            ddlPeriodeTo.SelectedValue = yearArray[1];
                        else
                            ddlPeriodeTo.SelectedValue = string.Empty;
                    }                    
                }
                if(training.FormationID.HasValue) 
                    ddlTraining.SelectedValue = training.FormationID.Value.ToString();
                txtDiploma.Text = training.Diplome;
                if(training.StudyLevelID.HasValue) 
                    ddlLevel.SelectedValue = training.StudyLevelID.Value.ToString();
                txtSchool.Text = training.School;
            }
        }        
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("candidateStudyPopupTitle");
        lblPeriode.Text = ResourceManager.GetString("columnPeriodStudiesCan") + ":";
        lblPeriodeFrom.Text = ResourceManager.GetString("fromText");
        lblPeriodeTo.Text = ResourceManager.GetString("toText");
        lblTraining.Text = ResourceManager.GetString("columnTrainingStudiesCan");
        lblDiploma.Text = ResourceManager.GetString("columnDiplomaStudiesCan");
        lblLevel.Text = ResourceManager.GetString("columnLevelStudiesCan");
        lblSchool.Text = ResourceManager.GetString("columnSchoolStudiesCan");        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        bool isPeriodeValid = true;
        if ((string.IsNullOrEmpty(ddlPeriodeFrom.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodeTo.SelectedValue))
            || (!string.IsNullOrEmpty(ddlPeriodeFrom.SelectedValue) && string.IsNullOrEmpty(ddlPeriodeTo.SelectedValue)))
        {
            isPeriodeValid = false;
        }
        else if (!string.IsNullOrEmpty(ddlPeriodeFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeTo.SelectedValue)
            && int.Parse(ddlPeriodeFrom.SelectedValue) > int.Parse(ddlPeriodeTo.SelectedValue))
        {
            isPeriodeValid = false;
        }
        if (isPeriodeValid)
        {
            CandidateTrainingRepository repo = new CandidateTrainingRepository();
            CandidateTraining saveItem = GetCadidateTraining();
            if (string.IsNullOrEmpty(Request.QueryString["CandidateFormationID"]))
            {
                //Insert new record
                repo.Insert(saveItem);
            }
            else
            {
                //Update the record.
                saveItem.CandidateFormationID = int.Parse(Request.QueryString["CandidateFormationID"]);
                repo.Update(saveItem);
            }
            string script = "<script type=\"text/javascript\">";
            script += " OnBtnSaveClientClicked();";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
        }
        else
        {
            string script = "<script type=\"text/javascript\">";
            script += " alert(\"" + ResourceManager.GetString("messageStudyPeriodeIsNotValid") + "\")";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
        }

        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.Page.ClientID, script);   
    }

    private CandidateTraining GetCadidateTraining()
    {
        CandidateTraining saveItem = new CandidateTraining();
        if(SessionManager.CurrentCandidate != null)
            saveItem.CandidateID = SessionManager.CurrentCandidate.CandidateId;
        if (!string.IsNullOrEmpty(ddlPeriodeFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeTo.SelectedValue))
        {
            saveItem.Period = ddlPeriodeFrom.SelectedValue + "-" + ddlPeriodeTo.SelectedValue;
        }
        else
        {
            saveItem.Period = txtPeriodeString.Text;
        }
        if (!string.IsNullOrEmpty(ddlTraining.SelectedValue))
            saveItem.FormationID = int.Parse(ddlTraining.SelectedValue);
        else
            saveItem.FormationID = null;
        saveItem.Diplome = txtDiploma.Text.Trim();
        if (!string.IsNullOrEmpty(ddlLevel.SelectedValue))
            saveItem.StudyLevelID = int.Parse(ddlLevel.SelectedValue);
        else
            saveItem.StudyLevelID = null;
        saveItem.School = txtSchool.Text.Trim();

        return saveItem;        
    }
}
