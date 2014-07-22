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


public partial class AdminNationality : System.Web.UI.Page
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
        ParamNationaliteRepository repo = new ParamNationaliteRepository();
        gridNationality.DataSource = repo.GetAllNationalities();
    }
    #region event

    protected void OnNationalityDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string nationalityID = lnkItem.CommandArgument;

        ParamNationalite deleteItem = new ParamNationalite(nationalityID);
        ParamNationaliteRepository repo = new ParamNationaliteRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridNationality.DataBind();                
    }

    protected void OnGridNationalityPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridNationality.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridNationality.DataBind();
    }

    protected void OnGridNationalityNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridNationalityItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteNationalityColumn"].Controls[1] as LinkButton;

            ParamNationalite nationality = (ParamNationalite)e.Item.DataItem;
            buttonDelete.CommandArgument = nationality.NationaliteID;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (nationality.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageNationalityBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditNationalityColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamNationalite nationality = e.Item.DataItem as ParamNationalite;
            if (nationality != null)
            {
                LinkButton lnkNationalityEdit = (LinkButton)e.Item.FindControl("lnkNationalityEdit");
                if (lnkNationalityEdit != null)
                {
                    lnkNationalityEdit.OnClientClick = string.Format("return OnNationalityEditClientClicked('{0}')", nationality.NationaliteID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindNationalityGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridNationality);
            gridNationality.Rebind();
        }        
    }
}
