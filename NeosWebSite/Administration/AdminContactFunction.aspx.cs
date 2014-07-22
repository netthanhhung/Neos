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


public partial class AdminContactFunction : System.Web.UI.Page
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
        ParamContactFunctionRepository repo = new ParamContactFunctionRepository();
        gridContactFunction.DataSource = repo.FindAllWithAscSort();
    }
    #region event

    protected void OnContactFunctionDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int contactFunctionID = int.Parse(lnkItem.CommandArgument);

        ParamContactFunction deleteItem = new ParamContactFunction(contactFunctionID);
        ParamContactFunctionRepository repo = new ParamContactFunctionRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridContactFunction.DataBind();                
    }

    protected void OnGridContactFunctionPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridContactFunction.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridContactFunction.DataBind();
    }

    protected void OnGridContactFunctionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridContactFunctionItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteContactFunctionColumn"].Controls[1] as LinkButton;

            ParamContactFunction contactFunction = (ParamContactFunction)e.Item.DataItem;
            buttonDelete.CommandArgument = contactFunction.ContactFunctionID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            
            LinkButton buttonEdit = dataItem["TemplateEditContactFunctionColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamContactFunction contactFunction = e.Item.DataItem as ParamContactFunction;
            if (contactFunction != null)
            {
                LinkButton lnkContactFunctionEdit = (LinkButton)e.Item.FindControl("lnkContactFunctionEdit");
                if (lnkContactFunctionEdit != null)
                {
                    lnkContactFunctionEdit.OnClientClick = string.Format("return OnContactFunctionEditClientClicked('{0}')", contactFunction.ContactFunctionID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindContactFunctionGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridContactFunction);
            gridContactFunction.Rebind();
        }        
    }
}
