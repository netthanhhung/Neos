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


public partial class AdminTypeAction : System.Web.UI.Page
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
        ParamTypeActionRepository repo = new ParamTypeActionRepository();
        gridTypeAction.DataSource = repo.GetAllParamTypeActions();
    }
    #region event

    protected void OnTypeActionDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int typeActionID = int.Parse(lnkItem.CommandArgument);

        ParamTypeAction deleteItem = new ParamTypeAction(typeActionID);
        ParamTypeActionRepository repo = new ParamTypeActionRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridTypeAction.DataBind();                
    }

    protected void OnGridTypeActionPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridTypeAction.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridTypeAction.DataBind();
    }

    protected void OnGridTypeActionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridTypeActionItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteTypeActionColumn"].Controls[1] as LinkButton;

            ParamTypeAction typeAction = (ParamTypeAction)e.Item.DataItem;
            buttonDelete.CommandArgument = typeAction.ParamActionID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (typeAction.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageTypeActionBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditTypeActionColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamTypeAction typeAction = e.Item.DataItem as ParamTypeAction;
            if (typeAction != null)
            {
                LinkButton lnkTypeActionEdit = (LinkButton)e.Item.FindControl("lnkTypeActionEdit");
                if (lnkTypeActionEdit != null)
                {
                    lnkTypeActionEdit.OnClientClick = string.Format("return OnTypeActionEditClientClicked('{0}')", typeAction.ParamActionID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindTypeActionGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridTypeAction);
            gridTypeAction.Rebind();
        }        
    }
}
