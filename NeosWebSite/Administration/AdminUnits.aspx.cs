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


public partial class AdminUnits : System.Web.UI.Page
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
        ParamTypeRepository repo = new ParamTypeRepository();
        gridUnits.DataSource = repo.FindAll();
    }
    #region event

    protected void OnUnitDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string unit = lnkItem.CommandArgument;

        ParamType deleteItem = new ParamType();
        deleteItem.TypeID = unit;
        ParamTypeRepository repo = new ParamTypeRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridUnits.DataBind();                
    }

    protected void OnGridUnitPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridUnits.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridUnits.DataBind();
    }

    protected void OnGridUnitNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridUnitItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteUnitColumn"].Controls[1] as LinkButton;

            ParamType unit = (ParamType)e.Item.DataItem;
            buttonDelete.CommandArgument = unit.TypeID;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";            
            LinkButton buttonEdit = dataItem["TemplateEditUnitColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamType unit = e.Item.DataItem as ParamType;
            if (unit != null)
            {
                LinkButton lnkUnitEdit = (LinkButton)e.Item.FindControl("lnkUnitEdit");
                if (lnkUnitEdit != null)
                {
                    lnkUnitEdit.OnClientClick = string.Format("return OnUnitEditClientClicked('{0}')", unit.TypeID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindUnitGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridUnits);
            gridUnits.Rebind();
        }        
    }
}
