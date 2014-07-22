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


public partial class AdminFunctionFam : System.Web.UI.Page
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
        ParamFunctionFamRepository repo = new ParamFunctionFamRepository();
        gridFunctionFam.DataSource = repo.GetAllFunctionFams();
    }
    #region event

    protected void OnFunctionFamDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string functionFamID = lnkItem.CommandArgument;

        ParamFunctionFam deleteItem = new ParamFunctionFam(functionFamID);
        ParamFunctionFamRepository repo = new ParamFunctionFamRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridFunctionFam.DataBind();                
    }

    protected void OnGridFunctionFamPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridFunctionFam.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridFunctionFam.DataBind();
    }

    protected void OnGridFunctionFamNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridFunctionFamItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteFunctionFamColumn"].Controls[1] as LinkButton;
            
            ParamFunctionFam functionFam = (ParamFunctionFam)e.Item.DataItem;
            buttonDelete.CommandArgument = functionFam.FonctionFamID;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (functionFam.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageFunctionFamBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditFunctionFamColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamFunctionFam functionFam = e.Item.DataItem as ParamFunctionFam;
            if (functionFam != null)
            {
                LinkButton lnkFunctionFamEdit = (LinkButton)e.Item.FindControl("lnkFunctionFamEdit");
                if (lnkFunctionFamEdit != null)
                {
                    lnkFunctionFamEdit.OnClientClick = string.Format("return OnFunctionFamEditClientClicked('{0}')", functionFam.FonctionFamID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindFunctionFamGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridFunctionFam);
            gridFunctionFam.Rebind();
        }        
    }
}
