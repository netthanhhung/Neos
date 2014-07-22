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


public partial class AdminFunction : System.Web.UI.Page
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
        ParamFunctionRepository repo = new ParamFunctionRepository();
        gridFunction.DataSource = repo.GetAllFunctions();
    }
    #region event

    protected void OnFunctionDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int functionID = int.Parse(lnkItem.CommandArgument);

        ParamFunction deleteItem = new ParamFunction(functionID);
        ParamFunctionRepository repo = new ParamFunctionRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridFunction.DataBind();                
    }

    protected void OnGridFunctionPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridFunction.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridFunction.DataBind();
    }

    protected void OnGridFunctionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridFunctionItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteFunctionColumn"].Controls[1] as LinkButton;
            
            ParamFunction function = (ParamFunction)e.Item.DataItem;
            buttonDelete.CommandArgument = function.FunctionID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (function.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageFunctionBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditFunctionColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamFunction function = e.Item.DataItem as ParamFunction;
            if (function != null)
            {
                LinkButton lnkFunctionEdit = (LinkButton)e.Item.FindControl("lnkFunctionEdit");
                if (lnkFunctionEdit != null)
                {
                    lnkFunctionEdit.OnClientClick = string.Format("return OnFunctionEditClientClicked('{0}')", function.FunctionID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindFunctionGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridFunction);
            gridFunction.Rebind();
        }        
    }
}
