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
using Telerik.Web.UI;
using Neos.Data;
using System.Collections.Generic;


public partial class AdminStudyLevel : System.Web.UI.Page
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
            BindGridData();
        }
    }

    private void BindGridData()
    {
        ParamStudyLevelRepository repo = new ParamStudyLevelRepository();
        gridStudyLevel.DataSource = repo.GetAllStudyLevels();
    }
    #region event

    protected void OnStudyLevelDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int studyLevelID = int.Parse(lnkItem.CommandArgument);

        ParamStudyLevel deleteItem = new ParamStudyLevel(studyLevelID);
        ParamStudyLevelRepository repo = new ParamStudyLevelRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridStudyLevel.DataBind();                
    }

    protected void OnGridStudyLevelPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridStudyLevel.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridStudyLevel.DataBind();
    }

    protected void OnGridStudyLevelNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridStudyLevelItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteStudyLevelColumn"].Controls[1] as LinkButton;
            
            ParamStudyLevel studyLevel = (ParamStudyLevel)e.Item.DataItem;
            buttonDelete.CommandArgument = studyLevel.SchoolID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (studyLevel.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageStudyLevelBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditStudyLevelColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamStudyLevel studyLevel = e.Item.DataItem as ParamStudyLevel;
            if (studyLevel != null)
            {
                LinkButton lnkStudyLevelEdit = (LinkButton)e.Item.FindControl("lnkStudyLevelEdit");
                if (lnkStudyLevelEdit != null)
                {
                    lnkStudyLevelEdit.OnClientClick = string.Format("return OnStudyLevelEditClientClicked('{0}')", studyLevel.SchoolID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindStudyLevelGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridStudyLevel);
            gridStudyLevel.Rebind();
        }        
    }
}
