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
using System.Collections.Generic;

public partial class CandidateAdvancedSearch : System.Web.UI.Page
{
    PageStatePersister _pers;
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            if (_pers == null)
            {
                _pers = new SessionPageStatePersister(this);
            }
            return _pers;
        }
    }

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
            FillCombobox();
            FillOldData();
        }        
    }

    private void FillLabelLanguage()
    {
        btnSearch.Text = ResourceManager.GetString("btnSearchCandidates");
        //General
        lblGeneralField.Text = ResourceManager.GetString("lblGeneralFieldAdvancedSearch");
        lblLastName.Text = ResourceManager.GetString("lblCanLastName");
        lblFirstName.Text = ResourceManager.GetString("lblCanFirstName");
        lblAgeFrom.Text = ResourceManager.GetString("lblCanAge");
        lblAgeTo.Text = ResourceManager.GetString("toText");
        lblActiveCan.Text = ResourceManager.GetString("lblActiveAdvancedSearch");        
        lblActiveCanBoth.Text = ResourceManager.GetString("bothText");
        lblActiveCanNo.Text = ResourceManager.GetString("noText");
        lblActiveCanYes.Text = ResourceManager.GetString("yesText");
        lblInterviewer.Text = ResourceManager.GetString("lblCanInterviewer");
        lblDateInterviewFrom.Text = ResourceManager.GetString("lblDateInterview");
        lblDateInterTo.Text = ResourceManager.GetString("toText");

        //Location
        lblSignage.Text = ResourceManager.GetString("lblSignageFieldAdvancedSearch");
        lblLocation.Text = ResourceManager.GetString("lblLocationAdvancedSearch");

        //Formation
        lblFormation.Text = ResourceManager.GetString("lblFormationFieldAdvancedSearch");
        lblStudyBackgroundLevel.Text = ResourceManager.GetString("lblBackgroundLevelAdvancedSearch");
        lblStudyOrentation.Text = ResourceManager.GetString("lblOrientationAdvancedSearch");
        lblStudySelection.Text = ResourceManager.GetString("lblStudySelectionAdvancedSearch");
        btnStudyAdd.Text = ResourceManager.GetString("addText");
        btnStudyRemove.Text = ResourceManager.GetString("removeText");
        lblStudyHaveOne.Text = ResourceManager.GetString("lblMustHaveOneAdvancedSearch");
        lblStudyHaveAll.Text = ResourceManager.GetString("lblMustHaveAllAdvancedSearch");
        
        //Knowledge        
        lblUnit.Text = ResourceManager.GetString("lblKnowFuncUnit");
        lblKnowlegde.Text = ResourceManager.GetString("lblCanKnowledgeGrid");
        lblFrenchLang.Text = ResourceManager.GetString("lblCanFrenchLang");
        lblGermanLang.Text = ResourceManager.GetString("lblCanGermanLang");
        lblDutchLang.Text = ResourceManager.GetString("lblCanDutchLang");
        lblSpanishLange.Text = ResourceManager.GetString("lblCanSpanishLang");
        lblEnglishLang.Text = ResourceManager.GetString("lblCanEnglishLang");
        lblItalianLang.Text = ResourceManager.GetString("lblCanItalianLang");
        lblKnowledgeSelection.Text = ResourceManager.GetString("lblKnowledgeSelectionAdvancedSearch");
        btnKnowledgeAdd.Text = ResourceManager.GetString("addText");
        btnKnowledgeRemove.Text = ResourceManager.GetString("removeText");
        lblKnowlegdeHaveOne.Text = ResourceManager.GetString("lblMustHaveOneAdvancedSearch");
        lblKnowlegdeHaveAll.Text = ResourceManager.GetString("lblMustHaveAllAdvancedSearch");

        //Function
        lblFunction.Text = ResourceManager.GetString("lblCanFunctionGrid");
        lblFunctionSelection.Text = ResourceManager.GetString("lblFunctionSelectionAdvancedSearch");
        btnFunctionAdd.Text = ResourceManager.GetString("addText");
        btnFunctionRemove.Text = ResourceManager.GetString("removeText");
        lblFunctionHaveOne.Text = ResourceManager.GetString("lblMustHaveOneAdvancedSearch");
        lblFunctionHaveAll.Text = ResourceManager.GetString("lblMustHaveAllAdvancedSearch");
    }

    private void FillCombobox()
    {        
        ParamTypeRepository paramTypeRepo = new ParamTypeRepository();
        ddlUnit.DataValueField = "TypeID";
        ddlUnit.DataTextField = "Label";
        IList<ParamType> unitList = paramTypeRepo.FindAll();
        IList<ParamType> unitList2 = new List<ParamType>();
        foreach (ParamType item in unitList)
        {
            if (!string.IsNullOrEmpty(item.TypeID))
                unitList2.Add(item);
        }
        ddlUnit.DataSource = unitList2;
        ddlUnit.DataBind();
        OnKnowFuncUnitItemChanged(ddlUnit, null);

        //Common
        int nowYear = DateTime.Today.Year;
        ddlAgeFrom.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
        ddlAgeTo.Items.Add(new RadComboBoxItem(string.Empty, string.Empty));
        for (int i = nowYear; i >= 1900; i--)
        {
            ddlAgeFrom.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            ddlAgeTo.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
        }

        ParamUserRepository paramUserRepo = new ParamUserRepository();
        ddlInterviewer.DataValueField = "UserID";
        ddlInterviewer.DataTextField = "LastName";
        ddlInterviewer.DataSource = paramUserRepo.GetAllUser(true);
        ddlInterviewer.DataBind();

        //Location
        listLocation.DataValueField = "CodeLocation";        
        listLocation.DataTextField = "Location";
        listLocation.DataSource = new ParamLocationsRepository().GetAllLocations();
        listLocation.DataBind();

        //Formation
        ddlStudyBackgroundLevel.DataValueField = "SchoolID";
        ddlStudyBackgroundLevel.DataTextField = "Label";
        ParamStudyLevelRepository studyLevelRepo = new ParamStudyLevelRepository();
        ddlStudyBackgroundLevel.DataSource = studyLevelRepo.FindAll();
        ddlStudyBackgroundLevel.DataBind();

        ddlStudyOrentation.DataValueField = "FormationID";
        ddlStudyOrentation.DataTextField = "Label";
        ParamFormationRepository formationRepo = new ParamFormationRepository();
        ddlStudyOrentation.DataSource = formationRepo.FindAllWithAscSort();
        ddlStudyOrentation.DataBind();

        //Knowledge
        for (int i = 0; i <= 5; i++)
        {
            ddlFrenchLangLevel.Items.Add(new RadComboBoxItem(
                ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlDutchLangLevel.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlEnglishLangLevel.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlGermanLangLevel.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlSpanishLangeLevel.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlItalianLangLevel.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));
            ddlOtherLangLevel.Items.Add(new RadComboBoxItem(
                            ResourceManager.GetString("languageSkillLevel" + i), i.ToString()));            
        }

        ParamLangueRepository languageRepo = new ParamLangueRepository();
        ddlOtherLang.DataValueField = "LangueID";
        ddlOtherLang.DataTextField = "LangueID";
        ddlOtherLang.DataSource = languageRepo.FindAll();
        ddlOtherLang.DataBind();
    }

    private void FillOldData()
    {
        if (SessionManager.CandidateSearchCriteria != null)
        {
            CandidateSearchCriteria criteria = SessionManager.CandidateSearchCriteria;
            txtLastName.Text = criteria.LastName;
            txtFirstName.Text = criteria.FirstName;
            if(criteria.AgeFrom.HasValue)
                ddlAgeFrom.SelectedValue = criteria.AgeFrom.Value.Year.ToString();
            else 
                ddlAgeFrom.SelectedValue = string.Empty;
            if (criteria.AgeTo.HasValue)
                ddlAgeTo.SelectedValue = criteria.AgeTo.Value.Year.ToString();
            else
                ddlAgeTo.SelectedValue = string.Empty;
            if (criteria.Active == "Yes")
            {
                radActiveCanBoth.Checked = false;
                radActiveCanYes.Checked = true;
                radActiveCanNo.Checked = false;
            }
            else if (criteria.Active == "No")
            {
                radActiveCanBoth.Checked = false;
                radActiveCanYes.Checked = false;
                radActiveCanNo.Checked = true;
            }
            else
            {
                radActiveCanBoth.Checked = true;
                radActiveCanYes.Checked = false;                
                radActiveCanNo.Checked = false;
            }
            if (!string.IsNullOrEmpty(criteria.Interviewer))
                ddlInterviewer.SelectedValue = criteria.Interviewer;
            else
                ddlInterviewer.SelectedValue = string.Empty;
            datInterviewFrom.SelectedDate = criteria.DateInterviewerFrom;
            datInterviewTo.SelectedDate = criteria.DateInterviewerTo;

            //Locations
            foreach (ListItem item in listLocation.Items)
            {
                bool isSelected = false;
                foreach (string location in criteria.Locations)
                {
                    if (location == item.Text)
                    {
                        isSelected = true;
                        break;
                    }
                }
                item.Selected = isSelected;
            }

            //Study
            listStudyDestination.Items.Clear();
            if (criteria.StudyAndLevelIDs != null && criteria.StudyAndLevelIDs.Length > 0)
            {
                for (int i = 0; i < criteria.StudyAndLevelIDs.Length; i++)
                {
                    ListItem item = new ListItem(criteria.StudyAndLevelTexts[i], criteria.StudyAndLevelIDs[i]);
                    listStudyDestination.Items.Add(item);
                }
            }
            radStudyOne.Checked = criteria.StudyHaveOne;
            radStudyAll.Checked = !criteria.StudyHaveOne;

            //Knowledge
            ddlFrenchLangLevel.SelectedValue = criteria.FrenchLevel.ToString();
            ddlDutchLangLevel.SelectedValue = criteria.DutchLevel.ToString();
            ddlEnglishLangLevel.SelectedValue = criteria.EnglishLevel.ToString();
            ddlGermanLangLevel.SelectedValue = criteria.GermanLevel.ToString();
            ddlSpanishLangeLevel.SelectedValue = criteria.SpanishLevel.ToString();
            ddlItalianLangLevel.SelectedValue = criteria.ItalianLevel.ToString();
            if (!string.IsNullOrEmpty(criteria.OtherLang))
            {
                ddlOtherLang.SelectedValue = criteria.OtherLang;
                ddlOtherLangLevel.SelectedValue = criteria.OtherLevel.ToString();
            }
            listKnowledgeDestination.Items.Clear();
            if (criteria.KnowledgeIDs != null && criteria.KnowledgeIDs.Length > 0)
            {
                for (int i = 0; i < criteria.KnowledgeIDs.Length; i++)
                {
                    ListItem item = new ListItem(criteria.KnowledgeTexts[i], criteria.KnowledgeIDs[i].ToString());
                    listKnowledgeDestination.Items.Add(item);
                }
            }
            radKnowledgeOne.Checked = criteria.KnowledgeHaveOne;
            radKnowledgeAll.Checked = !criteria.KnowledgeHaveOne;

            //Function
            listFunctionDestination.Items.Clear();
            if (criteria.FunctionIDs != null && criteria.FunctionIDs.Length > 0)
            {
                for (int i = 0; i < criteria.FunctionIDs.Length; i++)
                {
                    ListItem item = new ListItem(criteria.FunctionTexts[i], criteria.FunctionIDs[i].ToString());
                    listFunctionDestination.Items.Add(item);
                }
            }
            radFunctionOne.Checked = criteria.FunctionHaveOne;
            radFunctionAll.Checked = !criteria.FunctionHaveOne;
        }

    }
    #region Events
    protected void OnBtnSearchClicked(object sender, EventArgs e)
    {
        CandidateSearchCriteria criteria = new CandidateSearchCriteria();

        //General        
        if (!string.IsNullOrEmpty(txtLastName.Text))
            criteria.LastName = txtLastName.Text;
        if (!string.IsNullOrEmpty(txtFirstName.Text))
            criteria.FirstName = txtFirstName.Text;
        if (!string.IsNullOrEmpty(ddlAgeFrom.SelectedValue))
            criteria.AgeFrom = new DateTime(int.Parse(ddlAgeFrom.SelectedValue), 1, 1);        
        if (!string.IsNullOrEmpty(ddlAgeTo.SelectedValue))        
            criteria.AgeTo = new DateTime(int.Parse(ddlAgeTo.SelectedValue), 12, 31);        
        if (radActiveCanYes.Checked)
            criteria.Active = "Yes";
        else if (radActiveCanNo.Checked)
            criteria.Active = "No";
        if (!string.IsNullOrEmpty(ddlInterviewer.SelectedValue))
            criteria.Interviewer = ddlInterviewer.SelectedValue;
        criteria.DateInterviewerFrom = datInterviewFrom.SelectedDate;
        criteria.DateInterviewerTo = datInterviewTo.SelectedDate;

        //Locations        
        foreach (ListItem item in listLocation.Items)
        {
            if (item.Selected)
                criteria.Locations.Add(item.Text);
        }

        //Study   
        List<string> studyIDList = new List<string>();
        List<string> studyTextList = new List<string>();     
        foreach (ListItem item in listStudyDestination.Items)
        {            
            studyIDList.Add(item.Value);
            studyTextList.Add(item.Text);
        }
        criteria.StudyAndLevelIDs = studyIDList.ToArray();
        criteria.StudyAndLevelTexts = studyTextList.ToArray();
        criteria.StudyHaveOne = radStudyOne.Checked;

        //Knowledge
        criteria.FrenchLevel = int.Parse(ddlFrenchLangLevel.SelectedValue);
        criteria.DutchLevel = int.Parse(ddlDutchLangLevel.SelectedValue);
        criteria.EnglishLevel = int.Parse(ddlEnglishLangLevel.SelectedValue);
        criteria.GermanLevel = int.Parse(ddlGermanLangLevel.SelectedValue);
        criteria.SpanishLevel = int.Parse(ddlSpanishLangeLevel.SelectedValue);
        criteria.ItalianLevel = int.Parse(ddlItalianLangLevel.SelectedValue);
        criteria.OtherLevel = int.Parse(ddlOtherLangLevel.SelectedValue);
        criteria.OtherLang = ddlOtherLang.SelectedValue;

        List<int> knowIDs = new List<int>();
        List<string> knowTexts = new List<string>();
        foreach (ListItem item in listKnowledgeDestination.Items)
        {
            knowIDs.Add(int.Parse(item.Value));
            knowTexts.Add(item.Text);
        }
        criteria.KnowledgeIDs = knowIDs.ToArray();
        criteria.KnowledgeTexts = knowTexts.ToArray();
        criteria.KnowledgeHaveOne = radKnowledgeOne.Checked;

        //Function
        List<int> functIDs = new List<int>();
        List<string> funcTexts = new List<string>();
        foreach (ListItem item in listFunctionDestination.Items)
        {
            functIDs.Add(int.Parse(item.Value));
            funcTexts.Add(item.Text);
        }
        criteria.FunctionIDs = functIDs.ToArray();
        criteria.FunctionTexts = funcTexts.ToArray();
        criteria.FunctionHaveOne = radFunctionOne.Checked;

        SessionManager.CandidateSearchCriteria = criteria;
        Response.Redirect("~/Candidates.aspx?type=AdvancedSearch");
    }

    private System.Collections.Generic.List<string> List<T1>()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    protected void OnKnowFuncUnitItemChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string unit = (string)((RadComboBox)sender).SelectedValue;
        
        ParamFunctionFamRepository repoFunc = new ParamFunctionFamRepository();
        ddlFunctionFam.DataTextField = "FonctionFamID";
        ddlFunctionFam.DataValueField = "FonctionFamID";
        ddlFunctionFam.DataSource = repoFunc.GetParamFunctionFamByGenre(unit);
        ddlFunctionFam.DataBind();

        ParamKnowledgeFamRepository repoKnow = new ParamKnowledgeFamRepository();
        ddlKnowledgeFam.DataTextField = "ConFamilleID";
        ddlKnowledgeFam.DataValueField = "ConFamilleID";
        ddlKnowledgeFam.DataSource = repoKnow.GetParamKnowledgeFamByGenre(unit);        
        ddlKnowledgeFam.DataBind();

        if (string.IsNullOrEmpty(unit))
        {
            ddlFunctionFam.Text = string.Empty;
            ddlKnowledgeFam.Text = string.Empty;
        }

        OnFunctionFamItemChanged(ddlFunctionFam, null);
        OnKnowledgeFamItemChanged(ddlKnowledgeFam, null);        
    }

    protected void OnFunctionFamItemChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string family = (string)((RadComboBox)sender).SelectedValue;
        listFunctionOriginal.DataTextField = "Code";
        listFunctionOriginal.DataValueField = "FunctionID";
        CandidateFunctionRepository repo = new CandidateFunctionRepository();
        listFunctionOriginal.DataSource = repo.GetAllParamFunctionByFuntionFamID(family);
        listFunctionOriginal.DataBind();
    }

    protected void OnKnowledgeFamItemChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string family = (string)((RadComboBox)sender).SelectedValue;

        listKnowledgeOriginal.DataTextField = "Code";
        listKnowledgeOriginal.DataValueField = "KnowledgeID";
        CandidateKnowledgeRepository repo = new CandidateKnowledgeRepository();
        listKnowledgeOriginal.DataSource = repo.GetAllParamKnowledgeByKnowledgeFamID(family);
        listKnowledgeOriginal.DataBind();
    }

    protected void OnBtnStudyAddClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlStudyBackgroundLevel.SelectedValue)
            && !string.IsNullOrEmpty(ddlStudyOrentation.SelectedValue))
        {
            ListItem addItem = new ListItem();
            addItem.Value = ddlStudyOrentation.SelectedValue + ";" + ddlStudyBackgroundLevel.SelectedValue;
            addItem.Text = ddlStudyOrentation.Text + ";" + ddlStudyBackgroundLevel.Text;
            bool isExist = false;
            foreach (ListItem item in listStudyDestination.Items)
            {
                if (item.Value == addItem.Value)
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
                listStudyDestination.Items.Add(addItem);
        }        
    }

    protected void OnBtnStudyRemoveClicked(object sender, EventArgs e)
    {
        for (int i = listStudyDestination.Items.Count - 1; i >= 0; i--)
        {
            ListItem item = listStudyDestination.Items[i];
            if (item.Selected)
            {
                listStudyDestination.Items.Remove(item);
            }
        }
    }

    protected void OnBtnKnowledgeAddClicked(object sender, EventArgs e)
    {
        foreach (ListItem itemOri in listKnowledgeOriginal.Items)
        {
            if (itemOri.Selected)
            {
                bool isExist = false;
                foreach (ListItem itemDes in listKnowledgeDestination.Items)
                {
                    if (itemDes.Value == itemOri.Value)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    ListItem addItem = new ListItem(itemOri.Text, itemOri.Value);
                    listKnowledgeDestination.Items.Add(addItem);
                }
            }
        }
    }
    
    protected void OnBtnKnowledgeRemoveClicked(object sender, EventArgs e)
    {
        for (int i = listKnowledgeDestination.Items.Count - 1; i >= 0; i--)
        {
            ListItem item = listKnowledgeDestination.Items[i];
            if (item.Selected)
            {
                listKnowledgeDestination.Items.Remove(item);
            }
        }
    }

    protected void OnBtnFunctionAddClicked(object sender, EventArgs e)
    {
        foreach (ListItem itemOri in listFunctionOriginal.Items)
        {
            if (itemOri.Selected)
            {
                bool isExist = false;
                foreach (ListItem itemDes in listFunctionDestination.Items)
                {
                    if (itemDes.Value == itemOri.Value)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    ListItem addItem = new ListItem(itemOri.Text, itemOri.Value);
                    listFunctionDestination.Items.Add(addItem);
                }
            }
        }
    }
    
    protected void OnBtnFunctionRemoveClicked(object sender, EventArgs e)
    {
        for (int i = listFunctionDestination.Items.Count - 1; i >= 0; i--)
        {
            ListItem item = listFunctionDestination.Items[i];
            if (item.Selected)
            {
                listFunctionDestination.Items.Remove(item);
            }
        }
    }

    protected void OnMyAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("ddlUnitClientChanged") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlFunctionFam);
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlKnowledgeFam);
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, listFunctionOriginal);
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, listKnowledgeOriginal);
            
            OnKnowFuncUnitItemChanged(ddlUnit, null);
        }
        else if (e.Argument.IndexOf("ddlKnowledgeFamClientChanged") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, listKnowledgeOriginal);
            OnKnowledgeFamItemChanged(ddlKnowledgeFam, null);
        }
        else if (e.Argument.IndexOf("ddlFunctionFamClientChanged") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, listFunctionOriginal);
            OnFunctionFamItemChanged(ddlFunctionFam, null);
        }
    }
    #endregion
}
