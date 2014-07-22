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


public partial class AdminKnowledgeFam : System.Web.UI.Page
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
        ParamKnowledgeFamRepository repo = new ParamKnowledgeFamRepository();
        gridKnowledgeFam.DataSource = repo.GetAllKnowledgeFams();
    }
    #region event

    protected void OnKnowledgeFamDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string knowledgeFamID = lnkItem.CommandArgument;

        ParamKnowledgeFam deleteItem = new ParamKnowledgeFam(knowledgeFamID);
        ParamKnowledgeFamRepository repo = new ParamKnowledgeFamRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridKnowledgeFam.DataBind();                
    }

    protected void OnGridKnowledgeFamPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridKnowledgeFam.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridKnowledgeFam.DataBind();
    }

    protected void OnGridKnowledgeFamNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridKnowledgeFamItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteKnowledgeFamColumn"].Controls[1] as LinkButton;
            
            ParamKnowledgeFam knowledgeFam = (ParamKnowledgeFam)e.Item.DataItem;
            buttonDelete.CommandArgument = knowledgeFam.ConFamilleID;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (knowledgeFam.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageKnowledgeFamBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditKnowledgeFamColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamKnowledgeFam knowledgeFam = e.Item.DataItem as ParamKnowledgeFam;
            if (knowledgeFam != null)
            {
                LinkButton lnkKnowledgeFamEdit = (LinkButton)e.Item.FindControl("lnkKnowledgeFamEdit");
                if (lnkKnowledgeFamEdit != null)
                {
                    lnkKnowledgeFamEdit.OnClientClick = string.Format("return OnKnowledgeFamEditClientClicked('{0}')", knowledgeFam.ConFamilleID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindKnowledgeFamGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridKnowledgeFam);
            gridKnowledgeFam.Rebind();
        }        
    }
}
