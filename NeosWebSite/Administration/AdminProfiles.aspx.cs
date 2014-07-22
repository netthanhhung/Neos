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


public partial class AdminProfiles : System.Web.UI.Page
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
        ParamProfileRepository repo = new ParamProfileRepository();
        gridProfiles.DataSource = repo.GetAllProfiles();
    }
    #region event

    protected void OnProfileDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int profileID = int.Parse(lnkItem.CommandArgument);

        ParamProfile deleteItem = new ParamProfile(profileID);
        ParamProfileRepository repo = new ParamProfileRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridProfiles.DataBind();                
    }

    protected void OnGridProfilePageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridProfiles.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridProfiles.DataBind();
    }

    protected void OnGridProfileNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridProfileItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteProfileColumn"].Controls[1] as LinkButton;

            ParamProfile profile = (ParamProfile)e.Item.DataItem;
            buttonDelete.CommandArgument = profile.ProfileID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (profile.NumberProfileUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;",
                    ResourceManager.GetString("messageProfileBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditProfileColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamProfile profile = e.Item.DataItem as ParamProfile;
            if (profile != null)
            {
                LinkButton lnkProfileEdit = (LinkButton)e.Item.FindControl("lnkProfileEdit");
                if (lnkProfileEdit != null)
                {
                    lnkProfileEdit.OnClientClick = string.Format("return OnProfileEditClientClicked('{0}')", profile.ProfileID);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindProfileGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridProfiles);
            gridProfiles.Rebind();
        }        
    }
}
