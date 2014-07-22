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


public partial class AdminLocations : System.Web.UI.Page
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
        ParamLocationsRepository repo = new ParamLocationsRepository();
        gridLocations.DataSource = repo.GetAllLocations();
    }
    #region event

    protected void OnLocationDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string location = lnkItem.CommandArgument;

        ParamLocations deleteItem = new ParamLocations(location);
        ParamLocationsRepository repo = new ParamLocationsRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridLocations.DataBind();                
    }

    protected void OnGridLocationPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridLocations.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridLocations.DataBind();
    }

    protected void OnGridLocationNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridLocationItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteLocationColumn"].Controls[1] as LinkButton;
            
            ParamLocations location = (ParamLocations)e.Item.DataItem;
            buttonDelete.CommandArgument = location.Location;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (location.NumberCodeUsed > 0 || location.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageLocationBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditLocationColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamLocations location = e.Item.DataItem as ParamLocations;
            if (location != null)
            {
                LinkButton lnkLocationEdit = (LinkButton)e.Item.FindControl("lnkLocationEdit");
                if (lnkLocationEdit != null)
                {
                    lnkLocationEdit.OnClientClick = string.Format("return OnLocationEditClientClicked('{0}')", location.Location);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindLocationGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridLocations);
            gridLocations.Rebind();
        }        
    }
}
