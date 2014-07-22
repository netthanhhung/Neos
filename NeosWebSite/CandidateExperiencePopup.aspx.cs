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
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class CandidateExperiencePopup : System.Web.UI.Page
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
            InitControls();
            //Fill data for combobox
            ddlPeriodeMonthFrom.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
            ddlPeriodeMonthTo.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
            ddlPeriodeYearFrom.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
            ddlPeriodeYearTo.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
            for (int i = 1; i <= 12; i++)
            {
                string text = i.ToString();
                if (i < 10)
                    text = "0" + text;
                ddlPeriodeMonthFrom.Items.Add(new RadComboBoxItem(text, text));
                ddlPeriodeMonthTo.Items.Add(new RadComboBoxItem(text, text));
            }
            int nowYear = DateTime.Today.Year;
            for (int i = nowYear; i >= 1900; i--)
            {
                ddlPeriodeYearFrom.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                ddlPeriodeYearTo.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }

            ddlSalary.Items.Add(new RadComboBoxItem("1500-1999"));
            ddlSalary.Items.Add(new RadComboBoxItem("2000-2999"));
            ddlSalary.Items.Add(new RadComboBoxItem("3000-3999"));
            ddlSalary.Items.Add(new RadComboBoxItem("4000-4999"));
            ddlSalary.Items.Add(new RadComboBoxItem(">5000"));

            ParamKnowledgeFamRepository repo = new ParamKnowledgeFamRepository();
            ddlFuncUnit.DataSource = repo.GetKnowledgeFamGenreList();
            ddlFuncUnit.DataBind();
            OnFuncUnitItemChanged(ddlFuncUnit, null);
        }
        if (!string.IsNullOrEmpty(Request.QueryString["ExperienceID"]))
        {
            if (!IsPostBack)
            {
                int experienceID = int.Parse(Request.QueryString["ExperienceID"]);
                CandidateExperienceRepository repo = new CandidateExperienceRepository();
                CandidateExperience experience = repo.FindOne(new CandidateExperience(experienceID));

                if (!string.IsNullOrEmpty(experience.Period))
                {
                    txtPeriodeString.Text = experience.Period;

                    string expression = @"\d{2}/\d{4}\s*-\s*\d{2}/\d{4}";
                    if (Regex.IsMatch(experience.Period, expression))
                    {
                        string[] fromTo = experience.Period.Split('-');
                        string[] from = fromTo[0].Trim().Split('/');
                        string[] to = fromTo[1].Trim().Split('/');
                        if (1 <= int.Parse(from[0]) && int.Parse(from[0]) <= 12)
                            ddlPeriodeMonthFrom.SelectedValue = from[0];
                        else
                            ddlPeriodeMonthFrom.SelectedValue = string.Empty;

                        if (1900 <= int.Parse(from[1]) && int.Parse(from[1]) <= DateTime.Now.Year)
                            ddlPeriodeYearFrom.SelectedValue = from[1];
                        else
                            ddlPeriodeYearFrom.SelectedValue = string.Empty;

                        if (1 <= int.Parse(to[0]) && int.Parse(to[0]) <= 12)
                            ddlPeriodeMonthTo.SelectedValue = to[0];
                        else
                            ddlPeriodeMonthTo.SelectedValue = string.Empty;

                        if (1900 <= int.Parse(to[1]) && int.Parse(to[1]) <= DateTime.Now.Year)
                            ddlPeriodeYearTo.SelectedValue = to[1];
                        else
                            ddlPeriodeYearTo.SelectedValue = string.Empty;
                    }
                    else
                    {                        
                        ddlPeriodeMonthFrom.SelectedValue = string.Empty;
                        ddlPeriodeYearFrom.SelectedValue = string.Empty;
                        ddlPeriodeMonthTo.SelectedValue = string.Empty;
                        ddlPeriodeYearTo.SelectedValue = string.Empty;
                    }
                }
                txtCompany.Text = experience.Company;
                ddlSalary.Text = experience.Salary;
                txtSalaryPackage.Text = experience.ExtraAdvantage;
                txtJobTitle.Text = experience.FunctionDesc;
                txtQuitReason.Text = experience.LeftReason;

                if (experience.FunctionID.HasValue && experience.FunctionID.Value > 0)
                {
                    CandidateFunction canFunc = new CandidateFunctionRepository().GetFunctionByFunctionID(experience.FunctionID.Value);
                    ddlFuncUnit.SelectedValue = canFunc.Type;
                    OnFuncUnitItemChanged(ddlFuncUnit, null);
                    ddlFuncFam.SelectedValue = canFunc.Group;
                    OnFuncFamItemChanged(ddlFuncFam, null);
                    ddlFunction.SelectedValue = experience.FunctionID.Value.ToString();
                }
                else
                    ddlFunction.SelectedValue = "-1";
            }
        }        
    }

    private void InitControls()
    {
        
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("candidateExperiencePopupTitle");
        lblPeriode.Text = ResourceManager.GetString("columnPeriodExperienceCan") + ":";
        lblPeriodeFrom.Text = ResourceManager.GetString("fromText");
        lblPeriodeTo.Text = ResourceManager.GetString("toText");
        lblCompany.Text = ResourceManager.GetString("columnCompanyExperienceCan");
        lblSalary.Text = ResourceManager.GetString("columnSalaryExperienceCan");
        lblSalaryPackage.Text = ResourceManager.GetString("columnSalaryPackageExperienceCan");
        lblJobTitle.Text = ResourceManager.GetString("columnJobTitleExperienceCan");
        lblQuitReason.Text = ResourceManager.GetString("columnQuitReasonExperienceCan");
        lblFunction.Text = ResourceManager.GetString("columnFunctionExperienceCan");
        lblFuncUnit.Text = ResourceManager.GetString("columnFunctionUnitExperienceCan") + ":";
        lblFuncFam.Text = ResourceManager.GetString("columnFunctionFamExperienceCan") + ":";
        lblFunctionChild.Text = ResourceManager.GetString("columnFunctionExperienceCan") + ":";
        
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        bool isPeriodeValid = true;
        if ((string.IsNullOrEmpty(ddlPeriodeYearTo.SelectedValue)
            && string.IsNullOrEmpty(ddlPeriodeYearFrom.SelectedValue)
            && string.IsNullOrEmpty(ddlPeriodeMonthFrom.SelectedValue)
            && string.IsNullOrEmpty(ddlPeriodeMonthTo.SelectedValue))
            || (!string.IsNullOrEmpty(ddlPeriodeYearTo.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeYearFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeMonthFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeMonthTo.SelectedValue)))
        {
            isPeriodeValid = true;
        }
        else
        {
            isPeriodeValid = false;
        }

        if (isPeriodeValid && (!string.IsNullOrEmpty(ddlPeriodeYearTo.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeYearFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeMonthFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeMonthTo.SelectedValue)))
        {
            DateTime periodeFrom = new DateTime(int.Parse(ddlPeriodeYearFrom.SelectedValue), int.Parse(ddlPeriodeMonthFrom.SelectedValue), 1);
            DateTime periodeTo = new DateTime(int.Parse(ddlPeriodeYearTo.SelectedValue), int.Parse(ddlPeriodeMonthTo.SelectedValue), 1);
            if (periodeTo < periodeFrom)
            {
                isPeriodeValid = false;
            }
        }

        if (isPeriodeValid)
        {
            CandidateExperienceRepository repo = new CandidateExperienceRepository();
            CandidateExperience saveItem = GetCadidateExperience();
            if (string.IsNullOrEmpty(Request.QueryString["ExperienceID"]))
            {
                //Insert new record
                repo.Insert(saveItem);
            }
            else
            {
                //Update the record.
                saveItem.ExperienceID = int.Parse(Request.QueryString["ExperienceID"]);
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
            script += " alert(\"" + ResourceManager.GetString("messageExperiodePeriodeIsNotValid") + "\")";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
        }
    }

    private CandidateExperience GetCadidateExperience()
    {
        CandidateExperience saveItem = new CandidateExperience();
        if (SessionManager.CurrentCandidate != null)
            saveItem.CandidateID = SessionManager.CurrentCandidate.CandidateId;
        if (!string.IsNullOrEmpty(ddlPeriodeYearTo.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeYearFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeMonthFrom.SelectedValue)
            && !string.IsNullOrEmpty(ddlPeriodeMonthTo.SelectedValue))
        {
            saveItem.Period = ddlPeriodeMonthFrom.SelectedValue + "/" + ddlPeriodeYearFrom.SelectedValue
                            + " - " + ddlPeriodeMonthTo.SelectedValue + "/" + ddlPeriodeYearTo.SelectedValue;
        }
        else 
        {
            saveItem.Period = txtPeriodeString.Text;
        }
        saveItem.Company = txtCompany.Text.Trim();
        saveItem.Salary = ddlSalary.Text.Trim();
        saveItem.ExtraAdvantage = txtSalaryPackage.Text.Trim();
        saveItem.FunctionDesc = txtJobTitle.Text.Trim();
        saveItem.LeftReason = txtQuitReason.Text.Trim();
        if (!string.IsNullOrEmpty(ddlFunction.SelectedValue) && ddlFunction.SelectedValue != "-1")
            saveItem.FunctionID = int.Parse(ddlFunction.SelectedValue);
        else
            saveItem.FunctionID = null;
        return saveItem;
    }

    protected void OnFuncUnitItemChanged(object sender, EventArgs e)
    {
        string unit = (string)((RadComboBox)sender).SelectedValue;

        ParamFunctionFamRepository repo = new ParamFunctionFamRepository();
        ddlFuncFam.DataSource = repo.GetParamFunctionFamByGenre(unit);
        ddlFuncFam.DataTextField = "FonctionFamID";
        ddlFuncFam.DataValueField = "FonctionFamID";
        ddlFuncFam.DataBind();
        OnFuncFamItemChanged(ddlFuncFam, null);
    }

    protected void OnFuncFamItemChanged(object sender, EventArgs e)
    {
        string family = (string)((RadComboBox)sender).SelectedValue;
        ddlFunction.DataValueField = "FunctionID";
        ddlFunction.DataTextField = "Code";
        CandidateFunctionRepository repo = new CandidateFunctionRepository();
        IList<CandidateFunction> list = new List<CandidateFunction>();
        list = repo.GetAllParamFunctionByFuntionFamID(family);
        CandidateFunction firstItem = new CandidateFunction();
        firstItem.FunctionID = -1;
        firstItem.Code = string.Empty;
        list.Insert(0, firstItem);
        ddlFunction.Text = null;
        ddlFunction.DataSource = list;
        ddlFunction.DataBind();
    }
}
