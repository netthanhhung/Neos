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
using System.Drawing;

public partial class InvoiceUnpaidPage : System.Web.UI.Page
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
            if (!HavePermission())
            {
                TurnOverMView.ActiveViewIndex = 1;
                return;
            }
            InitControls();
            BindGridData(null);
        }
    }

    private bool HavePermission()
    {
        bool viewInvoicingPermission = false;
        if (SessionManager.CurrentUser != null && SessionManager.CurrentUser.Permissions != null)
        {
            foreach (ParamUserPermission item in SessionManager.CurrentUser.Permissions)
            {
                if (item.PermissionCode == "INVOICING")
                {
                    viewInvoicingPermission = true;
                    break;
                }
            }
        }

        return viewInvoicingPermission;
    }

    private void InitControls()
    {
        HttpCookie invoiceGridPageSizeCookie = Request.Cookies.Get("invoicegrdps");
        if (invoiceGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(invoiceGridPageSizeCookie.Value))
                gridInvoiceUnpaid.PageSize = Convert.ToInt32(invoiceGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(invoiceGridPageSizeCookie.Value) : gridInvoiceUnpaid.PageSize;
        }
    }

    private void BindGridData(GridSortCommandEventArgs sortEventArgs)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "unpaid")
        {
            int pageSize = 20;
            int pageNumber = gridInvoiceUnpaid.CurrentPageIndex + 1;
            string sortExpress = string.Empty;
            string sortExpressInvert = string.Empty;
            foreach (GridSortExpression item in gridInvoiceUnpaid.MasterTableView.SortExpressions)
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

            if (string.IsNullOrEmpty(sortExpress))
            {
                sortExpress = "CompanyName ASC";
                sortExpressInvert = "CompanyName DESC";
            }            

            InvoicesRepository repo = new InvoicesRepository();
            gridInvoiceUnpaid.VirtualItemCount = repo.CountUnpaidInvoices(pageSize, pageNumber, sortExpress, sortExpressInvert);
            IList<InvoiceUnpaid> list = repo.GetUnpaidInvoices(pageSize, pageNumber, sortExpress, sortExpressInvert);
            gridInvoiceUnpaid.DataSource = list;                        
        }
        else
        {
            gridInvoiceUnpaid.DataSource = new List<Invoices>();            
        }        
    }

    private void FillLabelLanguage()
    {
        lblInvoice.Text = ResourceManager.GetString("hypUnpaidInvoice");
        gridInvoiceUnpaid.Columns[1].HeaderText = ResourceManager.GetString("columnInvoiceUnpaidCompanyHeader");
        gridInvoiceUnpaid.Columns[2].HeaderText = ResourceManager.GetString("columnInvoiceUnpaidDueAmountHeader");
        gridInvoiceUnpaid.Columns[3].HeaderText = ResourceManager.GetString("columnInvoiceDetailOldestDateHeader");

        lblNoPermission.Text = ResourceManager.GetString("lblNoPermissionToViewInvoices"); 
    }
    
    #region event
    
    protected void OnInvoiceUnpaidGridPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie invoiceGridPageSizeCookie = new HttpCookie("invoicegrdps");
        invoiceGridPageSizeCookie.Expires.AddDays(30);
        invoiceGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(invoiceGridPageSizeCookie);
    }

    protected void OnInvoiceUnpaidGridPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridInvoiceUnpaid.CurrentPageIndex = e.NewPageIndex;
        BindGridData(null);
        gridInvoiceUnpaid.DataBind();
    }

    protected void OnInvoiceUnpaidGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData(null);
    }

    protected void OnInvoiceUnpaidGridSortCommand(object source, GridSortCommandEventArgs e)
    {
        BindGridData(e);
    }
       
    #endregion
      
}
