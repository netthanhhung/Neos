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


public partial class AdminLanguage : System.Web.UI.Page
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
        ParamLangueRepository repo = new ParamLangueRepository();
        gridLanguage.DataSource = repo.GetAllLanguages();
    }
    #region event

    protected void OnLanguageDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string languageID = lnkItem.CommandArgument;

        ParamLangue deleteItem = new ParamLangue(languageID);
        ParamLangueRepository repo = new ParamLangueRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridLanguage.DataBind();                
    }

    protected void OnGridLanguagePageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridLanguage.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridLanguage.DataBind();
    }

    protected void OnGridLanguageNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridLanguageItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteLanguageColumn"].Controls[1] as LinkButton;

            ParamLangue language = (ParamLangue)e.Item.DataItem;
            buttonDelete.CommandArgument = language.LangueID;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (language.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageLanguageBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditLanguageColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamLangue language = e.Item.DataItem as ParamLangue;
            if (language != null)
            {
                LinkButton lnkLanguageEdit = (LinkButton)e.Item.FindControl("lnkLanguageEdit");
                if (lnkLanguageEdit != null)
                {
                    lnkLanguageEdit.OnClientClick = string.Format("return OnLanguageEditClientClicked('{0}')", language.LangueID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindLanguageGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridLanguage);
            gridLanguage.Rebind();
        }        
    }
}
