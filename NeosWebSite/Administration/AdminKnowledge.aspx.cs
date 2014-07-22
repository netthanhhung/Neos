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


public partial class AdminKnowledge : System.Web.UI.Page
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
        ParamKnowledgeRepository repo = new ParamKnowledgeRepository();
        gridKnowledge.DataSource = repo.GetAllKnowledges();
    }
    #region event

    protected void OnKnowledgeDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int knowledgeID = int.Parse(lnkItem.CommandArgument);

        ParamKnowledge deleteItem = new ParamKnowledge(knowledgeID);
        ParamKnowledgeRepository repo = new ParamKnowledgeRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridKnowledge.DataBind();                
    }

    protected void OnGridKnowledgePageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridKnowledge.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridKnowledge.DataBind();
    }

    protected void OnGridKnowledgeNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridKnowledgeItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteKnowledgeColumn"].Controls[1] as LinkButton;
            
            ParamKnowledge knowledge = (ParamKnowledge)e.Item.DataItem;
            buttonDelete.CommandArgument = knowledge.KnowledgeID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (knowledge.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageKnowledgeBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditKnowledgeColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamKnowledge knowledge = e.Item.DataItem as ParamKnowledge;
            if (knowledge != null)
            {
                LinkButton lnkKnowledgeEdit = (LinkButton)e.Item.FindControl("lnkKnowledgeEdit");
                if (lnkKnowledgeEdit != null)
                {
                    lnkKnowledgeEdit.OnClientClick = string.Format("return OnKnowledgeEditClientClicked('{0}')", knowledge.KnowledgeID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindKnowledgeGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridKnowledge);
            gridKnowledge.Rebind();
        }        
    }
}
