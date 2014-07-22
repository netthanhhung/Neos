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
using Telerik.Web.UI.GridExcelBuilder;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
public partial class InvoicesPage : System.Web.UI.Page
{
    
    /*PageStatePersister _pers;
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
    }*/

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }       
        if (!IsPostBack)
        {
            FillLabelLanguage();
            if (!HavePermission())
            {
                InvoicingMView.ActiveViewIndex = 2;                
                return;
            }
            
            InitControls();
            BindGridData(null);
            SessionManager.BackUrl = Request.Url.ToString();
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
                gridInvoice.PageSize = Convert.ToInt32(invoiceGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(invoiceGridPageSizeCookie.Value) : gridInvoice.PageSize;
        }
        if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "future")
        {
            gridInvoice.Columns[13].Display = true;
        }
    }

    public int pageSize
    {
        get { return gridInvoice.PageSize; }
        set { gridInvoice.PageSize = value; }
    }

    private void BindGridData(GridSortCommandEventArgs sortEventArgs)
    {
        InvoicesRepository repo = new InvoicesRepository();
        IList<Invoices> list = new List<Invoices>();
        if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "search")
        {
            int pageNumber = gridInvoice.CurrentPageIndex + 1;
            string sortExpress = string.Empty;
            string sortExpressInvert = string.Empty;
            foreach (GridSortExpression item in gridInvoice.MasterTableView.SortExpressions)
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
                if (sortExpress.Contains("CompanyName"))
                {
                    sortExpress = sortExpress.Replace("CompanyName", "SocNom");
                    sortExpressInvert = sortExpressInvert.Replace("CompanyName", "SocNom");
                }
                if (sortExpress.Contains("CompanyId"))
                {
                    sortExpress = sortExpress.Replace("CompanyId", "SocieteID");
                    sortExpressInvert = sortExpressInvert.Replace("CompanyId", "SocieteID");
                }
                if (sortExpress.Contains("InvoiceDate"))
                {
                    sortExpress = sortExpress.Replace("InvoiceDate", "Date");
                    sortExpressInvert = sortExpressInvert.Replace("InvoiceDate", "Date");
                }
            }
            else
            {
                sortExpress = "IdYear DESC, IdFactNumber DESC, IdTypeInvoice ASC";
                sortExpressInvert = "IdYear ASC, IdFactNumber ASC, IdTypeInvoice DESC";
            }


            InvoicesSearchCriteria criteria = GetSearchCriteria();

            if (!string.IsNullOrEmpty(Request.QueryString["invoiceNumberFrom"]))
                criteria.InvoiceNumberFrom = int.Parse(Request.QueryString["invoiceNumberFrom"]);
            if (!string.IsNullOrEmpty(Request.QueryString["invoiceNumberTo"]))
                criteria.InvoiceNumberTo = int.Parse(Request.QueryString["invoiceNumberTo"]);

            if (!string.IsNullOrEmpty(Request.QueryString["fiscalYear"]))
                criteria.FiscalYear = int.Parse(Request.QueryString["fiscalYear"]);

            if (!string.IsNullOrEmpty(Request.QueryString["dateFrom"]))
                criteria.InvoiceDateFrom = DateTime.ParseExact(Request.QueryString["dateFrom"], "dd/MM/yyyy",
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
            if (!string.IsNullOrEmpty(Request.QueryString["dateTo"]))
                criteria.InvoiceDateTo = DateTime.ParseExact(Request.QueryString["dateTo"], "dd/MM/yyyy",
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);

            if (!string.IsNullOrEmpty(Request.QueryString["invoiceType"]))
                criteria.InvoiceType = Request.QueryString["invoiceType"];

            if (!string.IsNullOrEmpty(Request.QueryString["customer"]))
                criteria.Customer = int.Parse(Request.QueryString["customer"]);

            gridInvoice.VirtualItemCount = repo.CountInvoices(criteria, pageSize, pageNumber, sortExpress, sortExpressInvert);
            list = repo.SearchInvoices(criteria, pageSize, pageNumber, sortExpress, sortExpressInvert);            
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "future")
        {
            lblTitle.Text = ResourceManager.GetString("lblInvoiceFutureGridHeader");
            list = repo.GetFutureInvoices(int.Parse(WebConfig.FirstNumberFutureInvoice));
            
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
        {
            int companyID = int.Parse(Request.QueryString["CompanyId"]);            
            list = repo.GetUnpaidInvoicesOfCompany(companyID, false);
        }
        else
        {
            int notConfirmedFuture = repo.CountNotConfirmedFutureInvoices(int.Parse(WebConfig.FirstNumberFutureInvoice));
            if (notConfirmedFuture > 0)
            {
                Response.Redirect("InvoicesPage.aspx?type=future");
            }
        }

        gridInvoice.DataSource = list;
        btnPrintAll.Visible = (list.Count > 0);
        btnExcelExport.Visible = (list.Count > 0);        
        //btnPrintSelection.Visible = false;
    }

    private InvoicesSearchCriteria GetSearchCriteria()
    {
        InvoicesSearchCriteria criteria = new InvoicesSearchCriteria();

        if (!string.IsNullOrEmpty(Request.QueryString["invoiceNumberFrom"]))
            criteria.InvoiceNumberFrom = int.Parse(Request.QueryString["invoiceNumberFrom"]);
        if (!string.IsNullOrEmpty(Request.QueryString["invoiceNumberTo"]))
            criteria.InvoiceNumberTo = int.Parse(Request.QueryString["invoiceNumberTo"]);

        if (!string.IsNullOrEmpty(Request.QueryString["fiscalYear"]))
            criteria.FiscalYear = int.Parse(Request.QueryString["fiscalYear"]);

        if (!string.IsNullOrEmpty(Request.QueryString["dateFrom"]))
            criteria.InvoiceDateFrom = DateTime.ParseExact(Request.QueryString["dateFrom"], "dd/MM/yyyy",
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
        if (!string.IsNullOrEmpty(Request.QueryString["dateTo"]))
            criteria.InvoiceDateTo = DateTime.ParseExact(Request.QueryString["dateTo"], "dd/MM/yyyy",
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);

        if (!string.IsNullOrEmpty(Request.QueryString["invoiceType"]))
            criteria.InvoiceType = Request.QueryString["invoiceType"];

        if (!string.IsNullOrEmpty(Request.QueryString["customer"]))
            criteria.Customer = int.Parse(Request.QueryString["customer"]);
        return criteria;
    }

    private IList<Invoices> GetSearchInvoiceList()
    {
        InvoicesRepository repo = new InvoicesRepository();
        IList<Invoices> list = new List<Invoices>();
        if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "search")
        {
            InvoicesSearchCriteria criteria = GetSearchCriteria();
            list = repo.SearchInvoicesWithoutPage(criteria);
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "future")
        {            
            list = repo.GetFutureInvoices(int.Parse(WebConfig.FirstNumberFutureInvoice));

        }
        else if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
        {
            int companyID = int.Parse(Request.QueryString["CompanyId"]);
            list = repo.GetUnpaidInvoicesOfCompany(companyID, false);
        }
        return list;
    }
    private void FillLabelLanguage()
    {
        lblTitle.Text = ResourceManager.GetString("lblInvoiceGridHeader");
        gridInvoice.Columns[1].HeaderText = ResourceManager.GetString("columnCompanyNameHeader");
        gridInvoice.Columns[2].HeaderText = ResourceManager.GetString("columnCompanyRefHeader");
        gridInvoice.Columns[3].HeaderText = ResourceManager.GetString("columnInvoiveCreditNumberHeader");        
        gridInvoice.Columns[4].HeaderText = ResourceManager.GetString("columnInvoiceTypeHeader");
        gridInvoice.Columns[5].HeaderText = ResourceManager.GetString("columnInvoiceDateHeader");
        gridInvoice.Columns[6].HeaderText = ResourceManager.GetString("columnAmountVATExcludeHeader");
        gridInvoice.Columns[7].HeaderText = ResourceManager.GetString("columnVATAmountHeader");
        gridInvoice.Columns[8].HeaderText = ResourceManager.GetString("columnTotalVATHeader");
        gridInvoice.Columns[9].HeaderText = ResourceManager.GetString("columnInvoiceRemarkHeader");
        gridInvoice.Columns[10].HeaderText = ResourceManager.GetString("columnInvoiceInternalRemarkHeader");

        lnkAddInvoice.Text = ResourceManager.GetString("lnkAddInvoice");

        lblInvoiceDetail.Text = ResourceManager.GetString("lblInvoiceDetail");
        gridInvoiceDetails.Columns[0].HeaderText = ResourceManager.GetString("columnInvoiceDetailDescriptionHeader");
        gridInvoiceDetails.Columns[1].HeaderText = ResourceManager.GetString("columnInvoiceDetailQuantityHeader");
        gridInvoiceDetails.Columns[2].HeaderText = ResourceManager.GetString("columnInvoiceDetailUnitPriceHeader");
        gridInvoiceDetails.Columns[3].HeaderText = ResourceManager.GetString("columnInvoiceDetailAmountHeader");
        gridInvoiceDetails.Columns[4].HeaderText = ResourceManager.GetString("columnInvoiceDetailCodeVATHeader");
        gridInvoiceDetails.Columns[5].HeaderText = ResourceManager.GetString("columnInvoiceDetailPercentVATHeader");
        gridInvoiceDetails.Columns[6].HeaderText = ResourceManager.GetString("columnInvoiceDetailAmountVATHeader");
        gridInvoiceDetails.Columns[7].HeaderText = ResourceManager.GetString("columnInvoiceDetailTotalAmountHeader");

        btnPrintAll.Text = ResourceManager.GetString("btnInvoicePrintAll");
        btnEmail.Text = ResourceManager.GetString("btnInvoiceEmailSelection");
        btnExcelExport.Text = ResourceManager.GetString("btnInvoiceExcelExport");
        lblNoPermission.Text = ResourceManager.GetString("lblNoPermissionToViewInvoices");
        btnDownloadAllInvoices.Text = ResourceManager.GetString("lblDownloadZipFile"); 

    }
    
    #region event
    protected void OnInvoiceDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string invoiceID = lnkItem.CommandArgument;
        if (!string.IsNullOrEmpty(invoiceID))
        {
            string[] key = invoiceID.Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);                                                

            //Delete Invoice's payments first.
            InvoicePaymentsRepository payRepo = new InvoicePaymentsRepository();
            payRepo.DeleteInvoicePaymentsOfInvoice(idFactNumber, type, idYear);

            //Delete invoice's details
            InvoiceDetailsRepository detailRepo = new InvoiceDetailsRepository();
            detailRepo.DeleteInvoiceDetails(idFactNumber, type, idYear, null);

            //Delete Invoice        
            InvoicesRepository invoiceRepo = new InvoicesRepository();
            invoiceRepo.DeleteInvoice(idFactNumber, type, idYear);

            BindGridData(null);
            gridInvoice.DataBind();
        }
    }

    protected void OnInvoiceGridPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie invoiceGridPageSizeCookie = new HttpCookie("invoicegrdps");
        invoiceGridPageSizeCookie.Expires.AddDays(30);
        invoiceGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(invoiceGridPageSizeCookie);
    }

    protected void OnInvoiceGridPageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gridInvoice.CurrentPageIndex = e.NewPageIndex;
        BindGridData(null);
        gridInvoice.DataBind();
    }

    protected void OnInvoiceGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridData(null);
    }

    protected void OnInvoiceGridItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteInvoiceColumn"].Controls[1] as LinkButton;

            //Check that this invoice is the last invoice of a fiscal year.
            string invoiceIdPK = ((Invoices)e.Item.DataItem).InvoiceIdPK;
            string[] key = invoiceIdPK.Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);
            bool canDelete = true;
            InvoicesRepository invoiceRepo = new InvoicesRepository();
            int firstFutureNumber = int.Parse(WebConfig.FirstNumberFutureInvoice);
            if (firstFutureNumber > idFactNumber)
            {
                Invoices lastNormalInvoice = invoiceRepo.GetInvoicesWithMaxNumber(
                        idYear, type, false, firstFutureNumber);
                if (lastNormalInvoice != null && idFactNumber != lastNormalInvoice.IdFactNumber)
                {
                    canDelete = false;                  
                }
            }

            if(canDelete) 
                buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            else
                buttonDelete.OnClientClick = string.Format("javascript:alert('{0}'); return false;", ResourceManager.GetString("messageInvoiceCanNotDelete"));                

            if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "future")
            {
                buttonDelete.OnClientClick = string.Empty;
            }

            buttonDelete.CommandArgument = ((Invoices)e.Item.DataItem).InvoiceIdPK;
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            HyperLink hypEdit = dataItem["TemplateEditInvoiceColumn"].Controls[1] as HyperLink;
            hypEdit.Text = ResourceManager.GetString("editText");

            HyperLink hypInvoice = dataItem["TemplateInvoiceInvoiceColumn"].Controls[1] as HyperLink;
            hypInvoice.Text = ResourceManager.GetString("lblInvoiceInvoice");
        }
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Invoices invoice = (Invoices)e.Item.DataItem;
            if (invoice.Payement.HasValue && invoice.Payement.Value)
            {
                e.Item.BackColor = Color.YellowGreen;
            }
        }
    }

    protected void OnGridInvoiceDetailNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (gridInvoice.SelectedItems.Count == 1)
        {
            GridDataItem dataItem = ((GridDataItem)gridInvoice.SelectedItems[0]);
            if (dataItem != null)
            {
                string invoiceID = dataItem["InvoiceIdPK"].Text;
                if (!string.IsNullOrEmpty(invoiceID))
                {
                    string[] key = invoiceID.Split('-');
                    IList<InvoiceDetails> detailList =
                        new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(
                                int.Parse(key[0]), key[1], int.Parse(key[2]), null);
                    gridInvoiceDetails.DataSource = detailList;
                }
                else
                {
                    gridInvoiceDetails.DataSource = new List<InvoiceDetails>();
                }
            }
        }
        else
        {
            gridInvoiceDetails.DataSource = new List<InvoiceDetails>();
        }
    }

    protected void OnInvoiceGridSortCommand(object source, GridSortCommandEventArgs e)
    {
        BindGridData(e);
    }

    protected void OnAddNewInvoiceClick(object sender, EventArgs e)
    {
        Response.Redirect("InvoiceProfile.aspx?mode=edit&backurl=visible", false);
    }

    protected void OnButtonInvoicePrintAllClicked(object sender, EventArgs e)
    {
        List<InvoicingFile> fileList = new List<InvoicingFile>();

        if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "search")
        {
            InvoicesSearchCriteria criteria = new InvoicesSearchCriteria();
            string sortExpress = "IdFactNumber DESC, IdTypeInvoice ASC, IdYear DESC";
            string sortExpressInvert = "IdFactNumber ASC, IdTypeInvoice DESC, IdYear ASC";

            if (!string.IsNullOrEmpty(Request.QueryString["invoiceNumberFrom"]))
                criteria.InvoiceNumberFrom = int.Parse(Request.QueryString["invoiceNumberFrom"]);
            if (!string.IsNullOrEmpty(Request.QueryString["invoiceNumberTo"]))
                criteria.InvoiceNumberTo = int.Parse(Request.QueryString["invoiceNumberTo"]);

            if (!string.IsNullOrEmpty(Request.QueryString["fiscalYear"]))
                criteria.FiscalYear = int.Parse(Request.QueryString["fiscalYear"]);

            if (!string.IsNullOrEmpty(Request.QueryString["dateFrom"]))
                criteria.InvoiceDateFrom = DateTime.ParseExact(Request.QueryString["dateFrom"], "dd/MM/yyyy",
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
            if (!string.IsNullOrEmpty(Request.QueryString["dateTo"]))
                criteria.InvoiceDateTo = DateTime.ParseExact(Request.QueryString["dateTo"], "dd/MM/yyyy",
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);

            if (!string.IsNullOrEmpty(Request.QueryString["invoiceType"]))
                criteria.InvoiceType = Request.QueryString["invoiceType"];

            if (!string.IsNullOrEmpty(Request.QueryString["customer"]))
                criteria.Customer = int.Parse(Request.QueryString["customer"]);

            InvoicesRepository repo = new InvoicesRepository();
            gridInvoice.VirtualItemCount = repo.CountInvoices(criteria, 10, 1, sortExpress, sortExpressInvert);
            IList<Invoices> list = repo.SearchInvoices(criteria, gridInvoice.VirtualItemCount, 
                1, sortExpress, sortExpressInvert);
            foreach (Invoices item in list)
            {
                string filePath = Common.ExportInvoices(item, WebConfig.AddressFillInInvoice, 
                    WebConfig.AbsoluteExportDirectory);

                InvoicingFile file = new InvoicingFile();
                file.InvoiceIdPK = item.InvoiceIdPK;
                file.FilePath = filePath;

                fileList.Add(file);
            }
            InvoicingMView.ActiveViewIndex = 1;
            ProcessPrintedFiles(fileList);
            
            /*string message = ResourceManager.GetString("messageExportSuccessfully");
            string script1 = "<script type=\"text/javascript\">";
            script1 += " alert(\"" + message + "\")";
            script1 += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);*/
        }
    }

    protected void OnButtonInvoicePrintSelectionClicked(object sender, EventArgs e)
    {
        List<InvoicingFile> fileList = new List<InvoicingFile>();
        GridItemCollection col = gridInvoice.SelectedItems;
        foreach (GridDataItem item in col)
        {
            TableCell cell = item["InvoiceIdPK"];
            if (!string.IsNullOrEmpty(cell.Text))
            {
                string[] key = cell.Text.Split('-');
                int idFactNumber = int.Parse(key[0]);
                string type = key[1];
                int idYear = int.Parse(key[2]);
                Invoices invoice = new InvoicesRepository().GetInvoiceByID(idFactNumber, type, idYear);
                string fileName = Common.ExportInvoices(invoice, WebConfig.AddressFillInInvoice, 
                    WebConfig.AbsoluteExportDirectory);
                
                InvoicingFile file = new InvoicingFile();
                file.InvoiceIdPK = invoice.InvoiceIdPK;
                file.FilePath = fileName;
                fileList.Add(file);
            }
        }
        InvoicingMView.ActiveViewIndex = 1;
        ProcessPrintedFiles(fileList);

        /*string message = ResourceManager.GetString("messageExportSuccessfully");
        string script1 = "<script type=\"text/javascript\">";
        script1 += " alert(\"" + message + "\")";
        script1 += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);*/
    }

    private void ProcessPrintedFiles(List<InvoicingFile> fileList) 
    {
        if (fileList.Count == 1)//prompt download if there is only one file
        {
            InvoicingFile file = fileList[0];
            string originalFilename = file.FilePath.Substring(file.FilePath.LastIndexOf("\\") + 1, file.FilePath.Length - file.FilePath.LastIndexOf('\\') - 1);
            FileStream liveStream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);

            byte[] buffer = new byte[(int)liveStream.Length];
            liveStream.Read(buffer, 0, (int)liveStream.Length);
            liveStream.Close();

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + originalFilename);
            Response.BinaryWrite(buffer);
            Response.End();

        }
        else
        {
            GridInvoiceFile.DataSource = fileList;
            GridInvoiceFile.DataBind();
        }
        
    }

    protected void OnButtonDownloadAllInvoices_Click(object sender, EventArgs e)
    {
        string fileName = WebConfig.AbsoluteExportDirectory + "\\" + DateTime.Now.ToString("ddMMyyhhmmss") + "_invoices.zip";

        using (Ionic.Utils.Zip.ZipFile zip = new Ionic.Utils.Zip.ZipFile())
        {
            zip.AddDirectoryByName("Invoices");
            foreach (GridDataItem item in GridInvoiceFile.Items)
            {
                TableCell cell = item["FilePath"];
                if (!string.IsNullOrEmpty(cell.Text))
                {
                    string filePath = cell.Text;
                    if (File.Exists(filePath))
                    {
                        //string shortName = filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.Length - filePath.LastIndexOf('\\') - 1);
                        //string dirPath = filePath.Substring(0, filePath.Length - shortName.Length);
                        //zip.AddFile(filePath);
                        zip.AddFile(filePath, "Invoices");
                    }
                }
            }
            if(zip.Count >0)
            {
                zip.Save(fileName);
            }
        }
        if (File.Exists(fileName))
        {
            string originalFilename = "invoices.zip";
            FileStream liveStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            byte[] buffer = new byte[(int)liveStream.Length];
            liveStream.Read(buffer, 0, (int)liveStream.Length);
            liveStream.Close();

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + originalFilename);
            Response.BinaryWrite(buffer);
            Response.End();
        }
    }

    protected void OnGridInvoiceFile_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            InvoicingFile file = e.Item.DataItem as InvoicingFile;
            if (file != null)
            {
                ImageButton downloadButton = ((GridDataItem)e.Item)["Download"].Controls[0] as ImageButton;
                if (downloadButton != null)
                {
                    downloadButton.CommandArgument = file.FilePath;
                }               
            }
        }
    }

    protected void OnGridInvoiceFile_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "download")
        {
            string filePath = e.CommandArgument as string;
            if (!string.IsNullOrEmpty(filePath))
            {
                string originalFilename = filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.Length - filePath.LastIndexOf('\\') - 1);
                FileStream liveStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                byte[] buffer = new byte[(int)liveStream.Length];
                liveStream.Read(buffer, 0, (int)liveStream.Length);
                liveStream.Close();

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Length", buffer.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + originalFilename);
                Response.BinaryWrite(buffer);
                Response.End();
            }
        }
    }

    protected void OnButtonInvoiceEmailSelectionClicked(object sender, EventArgs e)
    {
        GridItemCollection col = gridInvoice.SelectedItems;
        IList<Invoices> invoiceList = new List<Invoices>();
        string email = string.Empty;
        foreach (GridDataItem item in col)
        {
            TableCell cell = item["InvoiceIdPK"];
            if (!string.IsNullOrEmpty(cell.Text))
            {
                string[] key = cell.Text.Split('-');
                int idFactNumber = int.Parse(key[0]);
                string type = key[1];
                int idYear = int.Parse(key[2]);
                Invoices invoice = new InvoicesRepository().GetInvoiceByID(idFactNumber, type, idYear);
                invoiceList.Add(invoice);
                CompanyAddress address = new CompanyAddressRepository().FindOne(
                    new CompanyAddress(invoice.RefCustomerNumber.Value));

                if (!string.IsNullOrEmpty(address.Email))
                {
                    if (email == string.Empty)
                    {
                        email = address.Email.Trim();
                    }
                    else if (email != address.Email.Trim())
                    {
                        string message = ResourceManager.GetString("messageInvoicesNotHaveSameEmail");
                        string script1 = "<script type=\"text/javascript\">";
                        script1 += " alert(\"" + message + "\")";
                        script1 += " </script>";

                        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
                        return;
                    }
                }
                else
                {
                    string message = ResourceManager.GetString("messageInvoiceNotHaveAnyEmail");
                    string script1 = "<script type=\"text/javascript\">";
                    script1 += " alert(\"" + message + "\")";
                    script1 += " </script>";

                    if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                        ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
                    return;
                }                                
            }
        }

        Microsoft.Office.Interop.Outlook.Application outlookApp =
                        new Microsoft.Office.Interop.Outlook.Application();
        Microsoft.Office.Interop.Outlook.MailItem mailItem =
            (Microsoft.Office.Interop.Outlook.MailItem)
            outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
        mailItem.To = email;
        mailItem.Subject = "Send invoice";
        
        foreach (Invoices item in invoiceList)
        {
            string fileName = Common.ExportInvoices(
                item, WebConfig.AddressFillInInvoice, WebConfig.AbsoluteExportDirectory);
            mailItem.Attachments.Add(fileName,
                Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);       
        }
        mailItem.Display(true);
    }

    #region Excel export    
    protected void OnGridInvoiceExcelMLExportStylesCreated(object source,
            GridExportExcelMLStyleCreatedArgs e)
    {
        foreach (StyleElement style in e.Styles)
        {
            if (style.Id == "headerStyle")
            {
                style.FontStyle.Bold = true;
                style.FontStyle.Color = System.Drawing.Color.Gainsboro;
                style.InteriorStyle.Color = System.Drawing.Color.Wheat;
                style.InteriorStyle.Pattern = InteriorPatternType.Solid;
            }
            else if (style.Id == "itemStyle")
            {
                style.InteriorStyle.Color = System.Drawing.Color.WhiteSmoke;
                style.InteriorStyle.Pattern = InteriorPatternType.Solid;
            }
            else if (style.Id == "alternatingItemStyle")
            {
                style.InteriorStyle.Color = System.Drawing.Color.LightGray;
                style.InteriorStyle.Pattern = InteriorPatternType.Solid;
            }
        }
        StyleElement myStyle = new StyleElement("MyCustomStyle");
        myStyle.FontStyle.Bold = true;
        myStyle.FontStyle.Italic = true;
        myStyle.InteriorStyle.Color = System.Drawing.Color.Gray;
        myStyle.InteriorStyle.Pattern = InteriorPatternType.Solid;
        e.Styles.Add(myStyle);
    }

    protected void OnGridInvoiceExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        if (e.RowType == GridExportExcelMLRowType.DataRow)
        {
            //if (e.Row.Cells[0] != null && ((string)e.Row.Cells[0].Data.DataItem).Contains("U"))
            //{
            //    e.Row.Cells[0].StyleValue = "MyCustomStyle";
            //}
            //else
            //{

            //}
        }
        else if (e.RowType == GridExportExcelMLRowType.HeaderRow)
        {
            if (e.Row.Cells[0] != null)
            {
                e.Row.Cells[0].StyleValue = "MyCustomStyle";
            }
        }
    }

    protected void OnButtonInvoiceExcelExportClicked(object sender, EventArgs e)
    {
        //ExportToExcel();

        string fileName = "Invoices-ExcelExport-" + DateTime.Today.Year;
        if (DateTime.Today.Month < 10)
        {
            fileName += "0";
        }
        fileName += DateTime.Today.Month.ToString();
        if (DateTime.Today.Day < 10)
        {
            fileName += "0";
        }
        fileName += DateTime.Today.Day.ToString();

        gridInvoice.MasterTableView.Columns.FindByUniqueName("TemplateEditInvoiceColumn").Visible = false;
        gridInvoice.MasterTableView.Columns.FindByUniqueName("TemplateDeleteInvoiceColumn").Visible = false;
        gridInvoice.MasterTableView.Columns.FindByUniqueName("TemplateInvoiceInvoiceColumn").Visible = false;
        gridInvoice.AllowSorting = false;
        //gridInvoice.PageSize = gridInvoice.MasterTableView.VirtualItemCount;
        gridInvoice.ExportSettings.IgnorePaging = true;
        gridInvoice.ExportSettings.FileName = fileName;
        gridInvoice.ExportSettings.OpenInNewWindow = false;
        gridInvoice.ExportSettings.ExportOnlyData = true;

        gridInvoice.MasterTableView.ExportToExcel();
        gridInvoice.AllowSorting = true;
    }

    private void ExportToExcel()
    {
        //string fileName = "C:\\Temp\\NeosExcelExport\\ExcelExport-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + ".xls";
        string fileName = WebConfig.AbsoluteExportDirectory + "\\Invoices-ExcelExport-" + DateTime.Today.Year;
        if (DateTime.Today.Month < 10)
        {
            fileName += "0";
        }
        fileName += DateTime.Today.Month.ToString();
        if (DateTime.Today.Day < 10)
        {
            fileName += "0";
        }
        fileName += DateTime.Today.Day.ToString();
        fileName += ".xls";

        BuildInvoiceExcelExportFile(fileName);

        string originalFilename = fileName.Substring(fileName.LastIndexOf("\\") + 1, fileName.Length - fileName.LastIndexOf('\\') - 1);
        FileStream liveStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        
        byte[] buffer = new byte[(int)liveStream.Length];
        liveStream.Read(buffer, 0, (int)liveStream.Length);
        liveStream.Close();

        //Delete the temp file.
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Length", buffer.Length.ToString());
        Response.AddHeader("Content-Disposition", "attachment; filename=" + originalFilename);
        Response.BinaryWrite(buffer);
        Response.End();
    }

    private void BuildInvoiceExcelExportFile(string fileName)
    {
        IList<Invoices> invoiceList = GetSearchInvoiceList();
        Excel.ApplicationClass excelApp = new Excel.ApplicationClass();
        excelApp.Application.Workbooks.Add(Type.Missing);
        Excel.Worksheet activeSheet = (Excel.Worksheet)excelApp.ActiveSheet;
        activeSheet.Name = ResourceManager.GetString("lblInvoiceGridHeader");
        string currencyFormat = "#,##0.00";
        activeSheet.Columns.ColumnWidth = 20;
        //Build invoice list.
        //Build header.
        activeSheet.get_Range(activeSheet.Cells[1, 1], activeSheet.Cells[1, 10]).Font.Bold = true;

        activeSheet.get_Range(activeSheet.Cells[1, 9], activeSheet.Cells[65535, 9]).ColumnWidth = 50;
        activeSheet.get_Range(activeSheet.Cells[1, 10], activeSheet.Cells[65535, 10]).ColumnWidth = 50;        

        activeSheet.get_Range(activeSheet.Cells[2, 6], activeSheet.Cells[65535, 6]).EntireColumn.NumberFormat = currencyFormat;
        activeSheet.get_Range(activeSheet.Cells[2, 7], activeSheet.Cells[65535, 7]).EntireColumn.NumberFormat = currencyFormat;
        activeSheet.get_Range(activeSheet.Cells[2, 8], activeSheet.Cells[65535, 8]).EntireColumn.NumberFormat = currencyFormat;

        activeSheet.Cells[1, 1] = ResourceManager.GetString("columnCompanyNameHeader");
        activeSheet.Cells[1, 2] = ResourceManager.GetString("columnCompanyRefHeader");
        activeSheet.Cells[1, 3] = ResourceManager.GetString("columnInvoiveCreditNumberHeader");
        activeSheet.Cells[1, 4] = ResourceManager.GetString("columnInvoiceTypeHeader");
        activeSheet.Cells[1, 5] = ResourceManager.GetString("columnInvoiceDateHeader");
        activeSheet.Cells[1, 6] = ResourceManager.GetString("columnAmountVATExcludeHeader");
        activeSheet.Cells[1, 7] = ResourceManager.GetString("columnVATAmountHeader");
        activeSheet.Cells[1, 8] = ResourceManager.GetString("columnTotalVATHeader");
        activeSheet.Cells[1, 9] = ResourceManager.GetString("columnInvoiceRemarkHeader");
        activeSheet.Cells[1, 10] = ResourceManager.GetString("columnInvoiceInternalRemarkHeader");
        int i = 2;
        foreach (Invoices invoice in invoiceList)
        {
            activeSheet.Cells[i, 1] = invoice.CompanyName;
            activeSheet.Cells[i, 2] = invoice.CompanyId;
            activeSheet.Cells[i, 3] = invoice.IdFactNumber;
            activeSheet.Cells[i, 4] = invoice.IdTypeInvoice;
            activeSheet.Cells[i, 5] = invoice.Date.HasValue ? invoice.Date.Value.ToString("dd/MM/yyyy") : string.Empty;
            activeSheet.Cells[i, 6] = invoice.TotalHtvaEuro.HasValue ? invoice.TotalHtvaEuro.Value : 0;
            activeSheet.Cells[i, 7] = invoice.AmountVatEuro.HasValue ? invoice.AmountVatEuro.Value : 0;
            activeSheet.Cells[i, 8] = invoice.TotalAmountIncludeVatEuro.HasValue ? invoice.TotalAmountIncludeVatEuro.Value : 0;
            activeSheet.Cells[i, 9] = invoice.Remark;
            activeSheet.Cells[i, 10] = invoice.Remark_Internal;
            i++;
        }

        //Build invoice detail list;
        Excel.Worksheet detailSheet = (Excel.Worksheet)excelApp.Sheets.Add(activeSheet, Type.Missing, Type.Missing, Type.Missing);
        detailSheet.Name = ResourceManager.GetString("lblInvoiceDetail");        
        detailSheet.Columns.ColumnWidth = 15;
        detailSheet.get_Range(detailSheet.Cells[1, 1], detailSheet.Cells[1, 11]).Font.Bold = true;
        detailSheet.get_Range(detailSheet.Cells[1, 4], detailSheet.Cells[65535, 4]).ColumnWidth = 40;

        detailSheet.get_Range(detailSheet.Cells[2, 5], detailSheet.Cells[65535, 5]).EntireColumn.NumberFormat = currencyFormat;
        detailSheet.get_Range(detailSheet.Cells[2, 6], detailSheet.Cells[65535, 6]).EntireColumn.NumberFormat = currencyFormat;
        detailSheet.get_Range(detailSheet.Cells[2, 7], detailSheet.Cells[65535, 7]).EntireColumn.NumberFormat = currencyFormat;        
        detailSheet.get_Range(detailSheet.Cells[2, 10], detailSheet.Cells[65535, 10]).EntireColumn.NumberFormat = currencyFormat;
        detailSheet.get_Range(detailSheet.Cells[2, 11], detailSheet.Cells[65535, 11]).EntireColumn.NumberFormat = currencyFormat;

        detailSheet.Cells[1, 1] = ResourceManager.GetString("columnInvoiveCreditNumberHeader");
        detailSheet.Cells[1, 2] = ResourceManager.GetString("lblFiscalYear");
        detailSheet.Cells[1, 3] = ResourceManager.GetString("columnInvoiceTypeHeader");
        detailSheet.Cells[1, 4] = ResourceManager.GetString("columnInvoiceDetailDescriptionHeader");
        detailSheet.Cells[1, 5] = ResourceManager.GetString("columnInvoiceDetailQuantityHeader");
        detailSheet.Cells[1, 6] = ResourceManager.GetString("columnInvoiceDetailUnitPriceHeader");
        detailSheet.Cells[1, 7] = ResourceManager.GetString("columnInvoiceDetailAmountHeader");
        detailSheet.Cells[1, 8] = ResourceManager.GetString("columnInvoiceDetailCodeVATHeader");
        detailSheet.Cells[1, 9] = ResourceManager.GetString("columnInvoiceDetailPercentVATHeader");
        detailSheet.Cells[1, 10] = ResourceManager.GetString("columnInvoiceDetailAmountVATHeader");
        detailSheet.Cells[1, 11] = ResourceManager.GetString("columnInvoiceDetailTotalAmountHeader");

        i = 2;
        foreach (Invoices invoice in invoiceList)
        {            
            IList<InvoiceDetails> detailList = new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(
                invoice.IdFactNumber, invoice.IdTypeInvoice, invoice.IdYear, null);
            if (detailList != null && detailList.Count > 0)
            {
                foreach (InvoiceDetails detail in detailList)
                {
                    detailSheet.Cells[i, 1] = detail.IdFactNumber;
                    detailSheet.Cells[i, 2] = detail.IdYear;
                    detailSheet.Cells[i, 3] = detail.IdTypeInvoice;
                    detailSheet.Cells[i, 4] = detail.Description;
                    detailSheet.Cells[i, 5] = detail.Quantity.HasValue ? detail.Quantity.Value : 0;
                    detailSheet.Cells[i, 6] = detail.UnitPriceEuro.HasValue ? detail.UnitPriceEuro.Value : 0;
                    detailSheet.Cells[i, 7] = detail.AmountEuro.HasValue ? detail.AmountEuro.Value : 0;
                    detailSheet.Cells[i, 8] = detail.VatCode.HasValue ? detail.VatCode.Value : 0;
                    detailSheet.Cells[i, 9] = detail.VatRate.HasValue ? detail.VatRate.Value : 0;
                    detailSheet.Cells[i, 10] = detail.AmountVAT.HasValue ? detail.AmountVAT.Value : 0;
                    detailSheet.Cells[i, 11] = detail.TotalAmountVAT.HasValue ? detail.TotalAmountVAT.Value : 0;
                    i++;
                }
                i++;
            }
        }

        activeSheet.Select(Type.Missing);

        excelApp.ActiveWorkbook.SaveCopyAs(fileName);        
        excelApp.ActiveWorkbook.Saved = true;                

        excelApp.Quit();        
    }
    #endregion

    #endregion

    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindInvoiceDetailGrid") != -1)
        {
            InvoiceAjaxManager.AjaxSettings.AddAjaxSetting(InvoiceAjaxManager, gridInvoiceDetails);
            //InvoiceAjaxManager.AjaxSettings.AddAjaxSetting(InvoiceAjaxManager, btnPrintSelection);
            string[] args = e.Argument.Split('/');
            if (args.Length == 3)
            {
                try
                {
                    //string userID = args[1];
                    int rowIndex = Int32.Parse(args[2]);
                    gridInvoice.MasterTableView.ClearSelectedItems();
                    gridInvoice.MasterTableView.Items[rowIndex - 1].Selected = true;
                }
                catch (System.Exception ex) { throw ex; }
                gridInvoiceDetails.Rebind();
                //btnPrintSelection.Enabled = true;
            }
        }
        else if (e.Argument.IndexOf("InvoiceGridMultiSelected") != -1)
        {
            //InvoiceAjaxManager.AjaxSettings.AddAjaxSetting(InvoiceAjaxManager, btnPrintSelection);
            //btnPrintSelection.Enabled = true;

            bool isSameReceiptionEmail = true;
            string email = "";
            string[] args = e.Argument.Split('/');
            if (args.Length == 2)
            {
                List<string> invoices = new List<string>(args[1].TrimEnd(';').Split(';'));
                foreach (string invoiceId in invoices)
                {
                    if (!string.IsNullOrEmpty(invoiceId))
                    {
                        string[] key = invoiceId.Split('-');
                        int idFactNumber = int.Parse(key[0]);
                        string type = key[1];
                        int idYear = int.Parse(key[2]);
                        Invoices invoice = new InvoicesRepository().GetInvoiceByID(idFactNumber, type, idYear);
                        if (string.IsNullOrEmpty(email))
                        {
                            email = GetEmailOfCompany(invoice.CompanyId.Value);
                        }
                        else
                            if (email.CompareTo(GetEmailOfCompany(invoice.CompanyId.Value)) != 0)
                            {
                                isSameReceiptionEmail = false;
                                break;
                            }
                        //InvoiceAjaxManager.ResponseScripts.Add(string.Format("alert('{0}: {1}')", invoice.InvoiceIdPK, GetEmailOfCompany(invoice.CompanyId.Value)));
                    }
                }
            }          
            if (isSameReceiptionEmail)
            {
                InvoiceAjaxManager.ResponseScripts.Add("processInvoiceToolBar(\"InvoiceGridSelectedSameReceiptionEmail\")");
            }
            else
                InvoiceAjaxManager.ResponseScripts.Add("processInvoiceToolBar(\"InvoiceGridMultiSelected\")");
                
        }
        else if (e.Argument.IndexOf("RebindInvoiceGrid") != -1)
        {
            InvoiceAjaxManager.AjaxSettings.AddAjaxSetting(InvoiceAjaxManager, gridInvoice);
            InvoiceAjaxManager.AjaxSettings.AddAjaxSetting(InvoiceAjaxManager, gridInvoiceDetails);
            gridInvoice.Rebind();
            gridInvoiceDetails.Rebind();
        }
        else
        {
            switch (e.Argument)
            {
                case "OpenSelectedInvoice":
                    Response.Redirect(string.Format("~/InvoiceProfile.aspx?InvoiceIdPK={0}&mode=edit&backurl=visible", GetSelectedInvoiceIdPK()), true);
                    break;
                case "DeleteSelectedInvoice":
                    InvoiceAjaxManager.AjaxSettings.AddAjaxSetting(InvoiceAjaxManager, gridInvoice);
                    InvoiceAjaxManager.AjaxSettings.AddAjaxSetting(InvoiceAjaxManager, gridInvoiceDetails);
                    foreach (GridDataItem selectedItem in gridInvoice.SelectedItems)
                    {
                        TableCell invoiceIDCell = selectedItem["InvoiceIdPK"];                        
                        string invoiceID = selectedItem["InvoiceIdPK"].Text;
                        if (!string.IsNullOrEmpty(invoiceID))
                        {
                            string[] key = invoiceID.Split('-');
                            int idFactNumber = int.Parse(key[0]);
                            string type = key[1];
                            int idYear = int.Parse(key[2]);
                            //Delete Invoice's payments first.
                            InvoicePaymentsRepository payRepo = new InvoicePaymentsRepository();
                            payRepo.DeleteInvoicePaymentsOfInvoice(idFactNumber, type, idYear);

                            //Delete invoice's details
                            InvoiceDetailsRepository detailRepo = new InvoiceDetailsRepository();
                            detailRepo.DeleteInvoiceDetails(idFactNumber, type, idYear, null);

                            //Delete Invoice        
                            InvoicesRepository invoiceRepo = new InvoicesRepository();
                            invoiceRepo.DeleteInvoice(idFactNumber, type, idYear);
                        }                        
                    }
                    BindGridData(null);
                    gridInvoice.DataBind();
                    break;
                case "PrintInvoice":
                    
                    List<InvoicingFile> fileList = new List<InvoicingFile>();
                    GridItemCollection col = gridInvoice.SelectedItems;
                    foreach (GridDataItem item in col)
                    {
                        TableCell cell = item["InvoiceIdPK"];
                        if (!string.IsNullOrEmpty(cell.Text))
                        {
                            string[] key = cell.Text.Split('-');
                            int idFactNumber = int.Parse(key[0]);
                            string type = key[1];
                            int idYear = int.Parse(key[2]);
                            Invoices invoice = new InvoicesRepository().GetInvoiceByID(idFactNumber, type, idYear);
                            string fileName = Common.ExportInvoices(invoice, WebConfig.AddressFillInInvoice, 
                                WebConfig.AbsoluteExportDirectory);

                            InvoicingFile file = new InvoicingFile();
                            file.InvoiceIdPK = invoice.InvoiceIdPK;
                            file.FilePath = fileName;
                            fileList.Add(file);
                        }
                    }
                    InvoicingMView.ActiveViewIndex = 1;
                    ProcessPrintedFiles(fileList);
                    
                    break;
                case "EmailInvoice":
                    string selectedInvoiceIDs = "";
                    foreach (GridDataItem selectedItem in gridInvoice.SelectedItems)
                    {
                        TableCell invoiceIDCell = selectedItem["InvoiceIdPK"];
                        string invoiceID = selectedItem["InvoiceIdPK"].Text;
                        if (!string.IsNullOrEmpty(invoiceID))
                        {
                            selectedInvoiceIDs += invoiceID + ";";
                        }
                    }
                    selectedInvoiceIDs = selectedInvoiceIDs.TrimEnd(';');
                    string url = "SendEmail.aspx?type=invoice&ids=" + selectedInvoiceIDs;
                    InvoiceAjaxManager.ResponseScripts.Add(string.Format("OnSendInvoiceByEmail('{0}')", url));
                    
                    break;
                case "CopyInvoice":
                    Response.Redirect(string.Format("~/InvoiceProfile.aspx?type=copy&from={0}&mode=edit", GetSelectedInvoiceIdPK()));
                    break;
            }
        }
    }

    private string GetEmailOfCompany(int companyID)
    {
        IList<CompanyAddress> addressList = new CompanyAddressRepository().GetAddressesOfCompany(companyID);
        foreach (CompanyAddress addr in addressList)
        {
            if (!string.IsNullOrEmpty(addr.Email))
                return addr.Email;
        }
        Company com = new CompanyRepository().FindOne(new Company(companyID));
        if (com != null)
            return com.Email;
        return "";
    }

    private string GetSelectedInvoiceIdPK()
    {
        GridDataItem selectedItem = gridInvoice.SelectedItems[0] as GridDataItem;
        TableCell invoiceIDCell = selectedItem["InvoiceIdPK"];
        if (!string.IsNullOrEmpty(invoiceIDCell.Text))
            return invoiceIDCell.Text;
        return "";
    }
}
class InvoicingFile
{
    private string invoiceIdPK;

    public string InvoiceIdPK
    {
        get { return invoiceIdPK; }
        set { invoiceIdPK = value; }
    }
    private string filePath;

    public string FilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }

    public string FileName
    {
        get 
        {
            if (string.IsNullOrEmpty(filePath)) return "";
            return filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.Length - filePath.LastIndexOf('\\') - 1);
        }
    }
    

}
