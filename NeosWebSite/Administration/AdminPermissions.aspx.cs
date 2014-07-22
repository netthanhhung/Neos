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


public partial class AdminPermissions : System.Web.UI.Page
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
        ParamPermissionRepository repo = new ParamPermissionRepository();
        gridPermissions.DataSource = repo.GetAllPermission();
    }
    #region event

    protected void OnPermissionDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string perCode = lnkItem.CommandArgument;
        
        ParamPermission deleteItem = new ParamPermission(perCode);
        ParamPermissionRepository repo = new ParamPermissionRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridPermissions.DataBind();                
    }

    protected void OnGridPermissionPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridPermissions.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridPermissions.DataBind();
    }

    protected void OnGridPermissionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridPermissionItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeletePermissionColumn"].Controls[1] as LinkButton;
            
            ParamPermission permission = (ParamPermission)e.Item.DataItem;
            buttonDelete.CommandArgument = permission.PermissionCode;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (permission.NbrUserUsed > 0)
            {

                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messagePermissionBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditPermissionColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamPermission per = e.Item.DataItem as ParamPermission;
            if (per != null)
            {
                LinkButton lnkPermissionEdit = (LinkButton)e.Item.FindControl("lnkPermissionEdit");
                if (lnkPermissionEdit != null)
                {
                    lnkPermissionEdit.OnClientClick = string.Format("return OnPermissionEditClientClicked('{0}')", per.PermissionCode);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindPermissionGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridPermissions);
            gridPermissions.Rebind();
        }        
    }
}
