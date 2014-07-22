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


public partial class AdminClientStatus : System.Web.UI.Page
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
        ParamClientStatusRepository repo = new ParamClientStatusRepository();
        gridClientStatus.DataSource = repo.GetAllClientStatuses();
    }
    #region event

    protected void OnClientStatusDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int clientStatusID = int.Parse(lnkItem.CommandArgument);

        ParamClientStatus deleteItem = new ParamClientStatus(clientStatusID);
        ParamClientStatusRepository repo = new ParamClientStatusRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridClientStatus.DataBind();                
    }

    protected void OnGridClientStatusPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridClientStatus.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridClientStatus.DataBind();
    }

    protected void OnGridClientStatusNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridClientStatusItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteClientStatusColumn"].Controls[1] as LinkButton;

            ParamClientStatus clientStatus = (ParamClientStatus)e.Item.DataItem;
            buttonDelete.CommandArgument = clientStatus.StatusID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (clientStatus.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageClientStatusBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditClientStatusColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamClientStatus clientStatus = e.Item.DataItem as ParamClientStatus;
            if (clientStatus != null)
            {
                LinkButton lnkClientStatusEdit = (LinkButton)e.Item.FindControl("lnkClientStatusEdit");
                if (lnkClientStatusEdit != null)
                {
                    lnkClientStatusEdit.OnClientClick = string.Format("return OnClientStatusEditClientClicked('{0}')", clientStatus.StatusID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindClientStatusGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridClientStatus);
            gridClientStatus.Rebind();
        }        
    }
}
