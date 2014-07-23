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
using Neos.Data;
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class Actions : System.Web.UI.Page
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
            FillLabelLanguage();
            InitControls();
            BindGridData(null);
            gridActions.DataBind();
            SessionManager.BackUrl = Request.Url.ToString();
        }
    }

    private void InitControls()
    {
        HttpCookie actionGridPageSizeCookie = Request.Cookies.Get("actgrdps");
        if (actionGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(actionGridPageSizeCookie.Value))
                gridActions.PageSize = Convert.ToInt32(actionGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(actionGridPageSizeCookie.Value) : gridActions.PageSize;
        }
    }


    private void FillLabelLanguage()
    {
        lnkAddNewAction.Text = ResourceManager.GetString("lnkAddNewAction");
        gridActions.Columns[0].HeaderText = ResourceManager.GetString("columnActiveActionCan");
        gridActions.Columns[1].HeaderText = ResourceManager.GetString("columnTaskNbrActionCan");
        gridActions.Columns[2].HeaderText = ResourceManager.GetString("columnDateActionCan");
        gridActions.Columns[3].HeaderText = ResourceManager.GetString("columnHourActionCan");
        gridActions.Columns[4].HeaderText = ResourceManager.GetString("columnTypeActionCan");
        gridActions.Columns[5].HeaderText = ResourceManager.GetString("columnCandidateActionCan");
        gridActions.Columns[6].HeaderText = ResourceManager.GetString("columnCompanyActionCan");
        gridActions.Columns[7].HeaderText = ResourceManager.GetString("columnDescriptionActionCan");
        gridActions.Columns[8].HeaderText = ResourceManager.GetString("columnResponsibleActionCan");
        lblActionTitle.Text = ResourceManager.GetString("lblRightPaneActionTitle");
    }

    private void BindGridData(GridSortCommandEventArgs sortEventArgs)
    {
        if (SessionManager.CurrentUser == null)
            return;
        int pageSize = 15;
        int pageNumber = gridActions.CurrentPageIndex + 1;
        string sortExpress = string.Empty;
        string sortExpressInvert = string.Empty;
        foreach (GridSortExpression item in gridActions.MasterTableView.SortExpressions)
        {
            GridSortOrder newSortOrder = item.SortOrder;
            if (sortEventArgs != null && item.FieldName == sortEventArgs.SortExpression)
            {
                newSortOrder = sortEventArgs.NewSortOrder;
            }

            if (!string.IsNullOrEmpty(sortExpress) && newSortOrder != GridSortOrder.None)
            {
                sortExpress += ", ";
                sortExpressInvert += ", ";
            }
            if (newSortOrder == GridSortOrder.Ascending)
            {
                sortExpress += item.FieldName + " ASC";
                sortExpressInvert += item.FieldName + " DESC";
            }
            else if (newSortOrder == GridSortOrder.Descending)
            {
                sortExpress += item.FieldName + " DESC";
                sortExpressInvert += item.FieldName + " ASC";
            }
        }

        if (sortEventArgs != null && !sortExpress.Contains(sortEventArgs.SortExpression))
        {
            if (!string.IsNullOrEmpty(sortExpress) && sortEventArgs.NewSortOrder != GridSortOrder.None)
            {
                sortExpress += ", ";
                sortExpressInvert += ", ";
            }
            if (sortEventArgs.NewSortOrder == GridSortOrder.Ascending)
            {
                sortExpress += sortEventArgs.SortExpression + " ASC";
                sortExpressInvert += sortEventArgs.SortExpression + " DESC";
            }
            else if (sortEventArgs.NewSortOrder == GridSortOrder.Descending)
            {
                sortExpress += sortEventArgs.SortExpression + " DESC";
                sortExpressInvert += sortEventArgs.SortExpression + " ASC";
            }
        }

        if (!string.IsNullOrEmpty(sortExpress))
        {
            if (sortExpress.Contains("Hour"))
            {
                sortExpress = sortExpress.Replace("Hour", "Heure");
                sortExpressInvert = sortExpressInvert.Replace("Hour", "Heure");
            }
            if (sortExpress.Contains("CandidateFullName"))
            {
                sortExpress = sortExpress.Replace("CandidateFullName", "CanLastName");
                sortExpressInvert = sortExpressInvert.Replace("CandidateFullName", "CanLastName");
            }
            if (sortExpress.Contains("CompanyName"))
            {
                sortExpress = sortExpress.Replace("CompanyName", "SocNom");
                sortExpressInvert = sortExpressInvert.Replace("CompanyName", "SocNom");
            }
        }
        else
        {
            sortExpress = "ActionId DESC";
            sortExpressInvert = "ActionId ASC";
        }

        string type = "MyActiveThisWeek";
        if (!string.IsNullOrEmpty(Request.QueryString["type"]))
        {
            type = Request.QueryString["type"];
        }

        ActionSearchCriteria criteria = new ActionSearchCriteria();
        switch (type)
        {
            case "MyActiveThisWeek":
                DateTime dateFrom = Common.GetBeginDayOfWeek(DateTime.Today);                
                criteria.DateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                criteria.Active = "Yes";
                criteria.Responsible = SessionManager.CurrentUser.UserID;
                break;
            case "MyActive":
                criteria.Active = "Yes";
                criteria.Responsible = SessionManager.CurrentUser.UserID;
                break;
            case "MyInactive":
                criteria.Active = "No";
                criteria.Responsible = SessionManager.CurrentUser.UserID;
                break;
            case "MyAllActions":
                criteria.Responsible = SessionManager.CurrentUser.UserID;
                break;
            case "AllActive":
                criteria.Active = "Yes";
                break;
            case "AllInactive":
                criteria.Active = "No";
                break;
            case "AllActions":
                break;
            case "search":
                if (!string.IsNullOrEmpty(Request.QueryString["active"]))
                    criteria.Active = Request.QueryString["active"];
                if (!string.IsNullOrEmpty(Request.QueryString["dateFrom"]))
                    criteria.DateFrom = DateTime.ParseExact(Request.QueryString["dateFrom"], "dd/MM/yyyy",
                        System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                if (!string.IsNullOrEmpty(Request.QueryString["dateTo"]))
                    criteria.DateTo = DateTime.ParseExact(Request.QueryString["dateTo"], "dd/MM/yyyy",
                        System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                if (!string.IsNullOrEmpty(Request.QueryString["candidate"]))
                    criteria.CanName = Request.QueryString["candidate"];
                if (!string.IsNullOrEmpty(Request.QueryString["company"]))
                    criteria.ComName = Request.QueryString["company"];
                if (!string.IsNullOrEmpty(Request.QueryString["typeAction"]))
                    criteria.TypeActionID = int.Parse(Request.QueryString["typeAction"]);
                if (!string.IsNullOrEmpty(Request.QueryString["description"]))
                    criteria.Description = Request.QueryString["description"];
                if (!string.IsNullOrEmpty(Request.QueryString["responsible"]))
                    criteria.Responsible = Request.QueryString["responsible"];
                break;
        }
        ActionRepository repo = new ActionRepository();
        gridActions.VirtualItemCount = repo.CountActions(criteria, pageSize, pageNumber, sortExpress, sortExpressInvert);
        IList<Neos.Data.Action> list = repo.SearchActions(criteria, pageSize, pageNumber, sortExpress, sortExpressInvert);
        gridActions.DataSource = list;

    }

    #region Event Action
    
    protected void OnActionGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie actionGridPageSizeCookie = new HttpCookie("actgrdps");
        actionGridPageSizeCookie.Expires.AddDays(30);
        actionGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(actionGridPageSizeCookie);
    }
    protected void OnCandidateClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string id = lnkItem.CommandArgument;
        Response.Redirect("~/CandidateProfile.aspx?CandidateID=" + id + "&mode=edit" + "&originalPage=Action");
    }

    protected void OnCompanyClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string id = lnkItem.CommandArgument;
        Response.Redirect("~/CompanyProfile.aspx?CompanyId=" + id + "&originalPage=Action&mode=edit");
    }

    protected void OnActionDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int actionID = int.Parse(lnkItem.CommandArgument);
        Neos.Data.Action deleteItem = new Neos.Data.Action(actionID);
        ActionRepository repo = new ActionRepository();
        repo.Delete(deleteItem);

        BindGridData(null);
        gridActions.DataBind();
    }

    protected void OnActionExportClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        int actionID = int.Parse(lnkItem.CommandArgument);
        Neos.Data.Action exportItem = new ActionRepository().GetActionByActionID(actionID);
        if (exportItem != null)
        {
            string message = Common.ExportActionToAppoinment(exportItem);
            //string script1 = "<script type=\"text/javascript\">";
            string script1 = " alert(\"" + message + "\")";
            //script1 += " </script>";
            //if (!this.ClientScript.IsClientScriptBlockRegistered("exportAction"))
            //    this.ClientScript.RegisterStartupScript(this.GetType(), "exportAction", script1);            
            ActionAjaxManager.ResponseScripts.Add(script1);
        }
    }

    protected void OnGridActionPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridActions.CurrentPageIndex = e.NewPageIndex;
        BindGridData(null);
        gridActions.DataBind();
    }
    
    protected void OnGridActionNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData(null);
    }

    protected void OnGridActionItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteActionColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((Neos.Data.Action)e.Item.DataItem).ActionID.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            //LinkButton buttonEdit = dataItem["TemplateEditActionColumn"].Controls[1] as LinkButton;
            //buttonEdit.Text = ResourceManager.GetString("editText");
            HyperLink lnkActionEdit = (HyperLink)e.Item.FindControl("lnkActionEdit");
            if(lnkActionEdit != null)
                lnkActionEdit.Text = ResourceManager.GetString("editText");

            LinkButton buttonExport = dataItem["TemplateExportActionColumn"].Controls[1] as LinkButton;
            buttonExport.OnClientClick = "return confirm('" + ResourceManager.GetString("confirmExportAction") + "')";
            buttonExport.CommandArgument = ((Neos.Data.Action)e.Item.DataItem).ActionID.ToString();
            buttonExport.Text = ResourceManager.GetString("exportText");
        }

    }

    protected void OnRadActionGridSortCommand(object source, GridSortCommandEventArgs e)
    {
        BindGridData(e);
    }
    #endregion

    #region ajax manager events
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindActionGrid") != -1)
        {
            ActionAjaxManager.AjaxSettings.AddAjaxSetting(ActionAjaxManager, gridActions);
            gridActions.Rebind();

        }
        else if (e.Argument.IndexOf("OpenSelectedAction") != -1)
        {
            if (gridActions.SelectedItems.Count == 1)
            {
                Response.Redirect(string.Format("~/ActionDetails.aspx?ActionID={0}&type=action&mode=edit&backurl=visible", GetSelectedActionID()), true);
            }
        }
        else if (e.Argument.IndexOf("DeleteSelectedAction") != -1)
        {
            ActionAjaxManager.AjaxSettings.AddAjaxSetting(ActionAjaxManager, gridActions);
            Neos.Data.ActionRepository actionRepo = new Neos.Data.ActionRepository();
            actionRepo.Delete(new Neos.Data.Action(GetSelectedActionID()));
            gridActions.Rebind();
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private int GetSelectedActionID()
    {
        GridDataItem selectedItem = gridActions.SelectedItems[0] as GridDataItem;
        TableCell actionIDCell = selectedItem["TaskNbr"];
        if (!string.IsNullOrEmpty(actionIDCell.Text))
            return Convert.ToInt32(actionIDCell.Text);
        return 0;
    }
    #endregion
}
