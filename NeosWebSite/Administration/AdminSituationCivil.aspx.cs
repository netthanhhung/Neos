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


public partial class AdminSituationCivil : System.Web.UI.Page
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
        ParamSituationCivilRepository repo = new ParamSituationCivilRepository();
        gridSituationCivil.DataSource = repo.GetAllSituationCivils();
    }
    #region event

    protected void OnSituationCivilDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string code = lnkItem.CommandArgument;

        ParamSituationCivil deleteItem = new ParamSituationCivil(code);
        ParamSituationCivilRepository repo = new ParamSituationCivilRepository();
        repo.Delete(deleteItem);

        BindGridData();
        gridSituationCivil.DataBind();                
    }

    protected void OnGridSituationCivilPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridSituationCivil.CurrentPageIndex = e.NewPageIndex;
        BindGridData();
        gridSituationCivil.DataBind();
    }

    protected void OnGridSituationCivilNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData();
    }

    protected void OnGridSituationCivilItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteSituationCivilColumn"].Controls[1] as LinkButton;

            ParamSituationCivil situationCivil = (ParamSituationCivil)e.Item.DataItem;
            buttonDelete.CommandArgument = situationCivil.Code;
            buttonDelete.Text = ResourceManager.GetString("deleteText");
            if (situationCivil.NumberIDUsed > 0)
            {
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageSituationCivilBeingUsed"));
            }
            else
            {
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            }
            LinkButton buttonEdit = dataItem["TemplateEditSituationCivilColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            ParamSituationCivil situationCivil = e.Item.DataItem as ParamSituationCivil;
            if (situationCivil != null)
            {
                LinkButton lnkSituationCivilEdit = (LinkButton)e.Item.FindControl("lnkSituationCivilEdit");
                if (lnkSituationCivilEdit != null)
                {
                    lnkSituationCivilEdit.OnClientClick = string.Format("return OnSituationCivilEditClientClicked('{0}')", situationCivil.Code);
                }
            }
        }
    }
    
    #endregion
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindSituationCivilGrid") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, gridSituationCivil);
            gridSituationCivil.Rebind();
        }        
    }
}
