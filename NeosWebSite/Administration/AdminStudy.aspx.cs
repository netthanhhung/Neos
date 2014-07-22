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


public partial class AdminStudy : System.Web.UI.Page
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
        ParamFormationRepository repo = new ParamFormationRepository();
        gridStudy.DataSource = repo.GetAllParamFormations();
    }
    #region event

    protected void OnStudyDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int studyID = int.Parse(lnkItem.CommandArgument);

        ParamFormation deleteItem = new ParamFormation(studyID);
        ParamFormationRepository repo = new ParamFormationRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridStudy.DataBind();                
    }

    protected void OnGridStudyPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridStudy.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridStudy.DataBind();
    }

    protected void OnGridStudyNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridStudyItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteStudyColumn"].Controls[1] as LinkButton;

            ParamFormation study = (ParamFormation)e.Item.DataItem;
            buttonDelete.CommandArgument = study.FormationID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (study.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageStudyBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditStudyColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamFormation study = e.Item.DataItem as ParamFormation;
            if (study != null)
            {
                LinkButton lnkStudyEdit = (LinkButton)e.Item.FindControl("lnkStudyEdit");
                if (lnkStudyEdit != null)
                {
                    lnkStudyEdit.OnClientClick = string.Format("return OnStudyEditClientClicked('{0}')", study.FormationID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindStudyGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridStudy);
            gridStudy.Rebind();
        }        
    }
}
