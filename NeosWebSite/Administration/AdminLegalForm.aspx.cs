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


public partial class AdminLegalForm : System.Web.UI.Page
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
        ParamLegalFormRepository repo = new ParamLegalFormRepository();
        gridLegalForm.DataSource = repo.FindAll();
    }
    #region event

    protected void OnLegalFormDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string formID = lnkItem.CommandArgument;

        ParamLegalForm deleteItem = new ParamLegalForm(formID);
        ParamLegalFormRepository repo = new ParamLegalFormRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridLegalForm.DataBind();                
    }

    protected void OnGridLegalFormPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridLegalForm.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridLegalForm.DataBind();
    }

    protected void OnGridLegalFormNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridLegalFormItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteLegalFormColumn"].Controls[1] as LinkButton;

            ParamLegalForm form = (ParamLegalForm)e.Item.DataItem;
            buttonDelete.CommandArgument = form.FormID;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            
            LinkButton buttonEdit = dataItem["TemplateEditLegalFormColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamLegalForm form = e.Item.DataItem as ParamLegalForm;
            if (form != null)
            {
                LinkButton lnkLegalFormEdit = (LinkButton)e.Item.FindControl("lnkLegalFormEdit");
                if (lnkLegalFormEdit != null)
                {
                    lnkLegalFormEdit.OnClientClick = string.Format("return OnLegalFormEditClientClicked('{0}')", form.FormID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindLegalFormGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridLegalForm);
            gridLegalForm.Rebind();
        }        
    }
}
