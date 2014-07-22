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
using ASPPDFLib;
using System.Reflection;
using System.IO;

public partial class InvoiceProfile : System.Web.UI.Page
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
    #region Common
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
            if (!string.IsNullOrEmpty(SessionManager.BackUrl)
                && SessionManager.BackUrl.Contains("InvoicesPage.aspx")
                && !string.IsNullOrEmpty(Request.QueryString["backurl"])
                && Request.QueryString["backurl"] == "visible")
            {
                lnkBack.Visible = true;
            }
            else
            {
                SessionManager.BackUrl = null;
                lnkBack.Visible = false;
            }

            if (!HavePermission())
            {
                InvoicingMView.ActiveViewIndex = 1;
                return;
            }

            if (Request.QueryString["mode"] == "view")
                EnableInvoiceControls(false);
            else
                EnableInvoiceControls(true);
            
            /////////////////////////////////////////////////////////
            if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
            {
                BindInvoiceData(Request.QueryString["InvoiceIdPK"]);
            }
            else if (Request.QueryString["type"] == "copy")
            {
                string invoiceId = Request.QueryString["from"];
                CopyInvoiceData(invoiceId);
                SessionManager.CurrentInvoice = null;
            }
            else if (Request.QueryString["type"] == "addnew")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["customer"]))
                {
                    int companyId = Convert.ToInt32(Request.QueryString["customer"]);
                    BindCompanyInfo(companyId);

                    gridInvoiceDetails.DataSource = new List<InvoiceDetails>();
                    gridInvoicePayments.DataSource = new List<InvoicePayments>();
                }
            }
            else
            {
                datInvoiceDate.SelectedDate = DateTime.Today;
                txtTotalHTVA.Value = 0;
                txtTotalVAT.Value = 0;
                txtTotal.Value = 0;
                txtPayment.Value = 0;
                gridInvoiceDetails.DataSource = new List<InvoiceDetails>();
                gridInvoicePayments.DataSource = new List<InvoicePayments>();
                lnkAddInvoiceDetail.Visible = false;
                lnkAddNewPayment.Visible = false;
                SessionManager.CurrentInvoice = null;
            }
            gridInvoiceDetails.DataBind();
            gridInvoicePayments.DataBind();
        }

        string script = "<script type='text/javascript'>";
        script += "onLoadInvoiceProfilePage();";
        script += "</script>";
        if (!ClientScript.IsClientScriptBlockRegistered("LoadInvoiceProfilePage"))
            ClientScript.RegisterStartupScript(this.GetType(), "LoadInvoiceProfilePage", script);       
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

    private void FillLabelLanguage()
    {
        lblInvoice.Text = ResourceManager.GetString("lblInvoiceInvoice");
        lblInvoiceTitle.Text = ResourceManager.GetString("lblInvoiceInvoice");
        //Common
        if(Request.QueryString["mode"] == "view")
            btnEditSave.Text = ResourceManager.GetString("editText");
        else
            btnEditSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");

        btnEditSave.OnClientClick = "return onBtnSaveClientClicked('" 
            + ResourceManager.GetString("CompanyAddressMustNotNull") + "')";

        lblTypeInvoice.Text = ResourceManager.GetString("lblInvoiceInvoice");
        lblTypeCreditNote.Text = ResourceManager.GetString("lblInvoiceCreditNote");

        lblInvoiceHeader.Text = ResourceManager.GetString("headerText");
        lblCustomer.Text = ResourceManager.GetString("lblInvoiceCustomer");
        lblIdCustomer.Text = ResourceManager.GetString("lblInvoiceIDCustomer");
        lblAddressName.Text = ResourceManager.GetString("lblInvoiceAddressName");
        lblVatNumber.Text = ResourceManager.GetString("lblInvoiceVATNumber");
        lblAddress.Text = ResourceManager.GetString("lblInvoiceAddress");
        lblZipCode.Text = ResourceManager.GetString("lblInvoiceZipCode");
        lblCity.Text = ResourceManager.GetString("lblInvoiceCity");
        lblDate.Text = ResourceManager.GetString("lblInvoiceDate");
        lblInvoiceNumber.Text = ResourceManager.GetString("lblInvoiceNumber");
        btnChooseCustomer.Text = ResourceManager.GetString("btnChooseCustomer");
        btnChooseAddress.Text = ResourceManager.GetString("btnChooseAddress");

        radTabStripInvoice.Tabs[0].Text = ResourceManager.GetString("tabInvoiceDetail");
        radTabStripInvoice.Tabs[1].Text = ResourceManager.GetString("tabInvoicePayment");
        radTabStripInvoice.Tabs[2].Text = ResourceManager.GetString("columnInvoiceInternalRemarkHeader");        

        gridInvoiceDetails.Columns[0].HeaderText = ResourceManager.GetString("columnInvoiceDetailDescriptionHeader");
        gridInvoiceDetails.Columns[1].HeaderText = ResourceManager.GetString("columnInvoiceDetailQuantityHeader");
        gridInvoiceDetails.Columns[2].HeaderText = ResourceManager.GetString("columnInvoiceDetailUnitPriceHeader") + "(€)";
        gridInvoiceDetails.Columns[3].HeaderText = ResourceManager.GetString("columnInvoiceDetailAmountHeader") + "(€)";
        gridInvoiceDetails.Columns[4].HeaderText = ResourceManager.GetString("columnInvoiceDetailCodeVATHeader");
        gridInvoiceDetails.Columns[5].HeaderText = ResourceManager.GetString("columnInvoiceDetailPercentVATHeader");
        gridInvoiceDetails.Columns[6].HeaderText = ResourceManager.GetString("columnInvoiceDetailAmountVATHeader") + "(€)";
        gridInvoiceDetails.Columns[7].HeaderText = ResourceManager.GetString("columnInvoiceDetailTotalAmountHeader") + "(€)";
        lnkAddInvoiceDetail.Text = ResourceManager.GetString("lnkAddInvoiceDetail");

        gridInvoicePayments.Columns[0].HeaderText = ResourceManager.GetString("columnInvoicePaymentDateHeader");
        gridInvoicePayments.Columns[1].HeaderText = ResourceManager.GetString("columnInvoicePaymentAmountHeader");
        gridInvoicePayments.Columns[2].HeaderText = ResourceManager.GetString("columnInvoicePaymentRemarkHeader");
        lnkAddNewPayment.Text = ResourceManager.GetString("lnkAddNewInvoicePayment");

        lblTotalHTVA.Text = ResourceManager.GetString("lblInvoiceTotalHTVA");
        lblTotalVAT.Text = ResourceManager.GetString("lblInvoiceTotalVAT");
        lblTotal.Text = ResourceManager.GetString("lblInvoiceTotal");
        lblPaymentDate.Text = ResourceManager.GetString("lblInvoicePaymentDate");
        lblPayment.Text = ResourceManager.GetString("lblInvoicePayment");

        lblNoPermission.Text = ResourceManager.GetString("lblNoPermissionToViewInvoices"); 
    }
    /// <summary>
    /// binding company info when add invoice of a company
    /// </summary>
    /// <param name="companyID"></param>
    private void BindCompanyInfo(int companyID)
    {
        Company company = new CompanyRepository().FindOne(new Company(companyID));
        if (company == null) return ;
        txtCustomerName.Text = company.CompanyName;
        txtIdCustomer.Text = company.CompanyID.ToString();
        btnChooseCustomer.Enabled = false;
        
        txtIdCustomer.ReadOnly = true;
        txtCustomerName.ReadOnly = true;

        IList<CompanyAddress> comAdrList = new CompanyAddressRepository().GetAddressesOfCompany(companyID);       

        foreach (CompanyAddress item in comAdrList)
        {
            if (item.IsDefault)
            {
                hiddenCompanyAddressId.Value = item.AddressID.ToString();
                txtAddressName.Text = item.Name;
                txtAddress.Text = item.Address;
                txtZipCode.Text = item.ZipCode;
                txtCity.Text = item.City;
                txtVatNumber.Text = item.VatNumber;
                break;
            }
        }

        datInvoiceDate.SelectedDate = DateTime.Now;
        lnkAddInvoiceDetail.Visible = false;
        lnkAddNewPayment.Visible = false;
        btnChooseAddress.Enabled = true;
    }


    private void BindInvoiceData(string InvoiceIdPK)
    {
        string[] key = InvoiceIdPK.Split('-');
        int idFactNumber = int.Parse(key[0]);
        string type = key[1];
        int idYear = int.Parse(key[2]);

        //Get invoice
        Invoices invoice = new InvoicesRepository().GetInvoiceByID(idFactNumber, type, idYear);
        SessionManager.CurrentInvoice = invoice;
        if (invoice.IdTypeInvoice == "I")
            radInvoice.Checked = true;
        else
            radCreditNote.Checked = true;
        txtCustomerName.Text = invoice.CompanyName;
        if (invoice.CompanyId.HasValue)
            txtIdCustomer.Text = invoice.CompanyId.Value.ToString();
        txtInvoiceNumber.Text = invoice.IdFactNumber.ToString();
        chkFactoring.Checked = invoice.Factoring.HasValue ? invoice.Factoring.Value : false;
        datInvoiceDate.SelectedDate = invoice.Date;
        txtRemark.Text = invoice.Remark;
        txtInternalRemark.Text = invoice.Remark_Internal;
        txtTotalHTVA.Value = invoice.TotalHtvaEuro;
        txtTotalVAT.Value = invoice.AmountVatEuro;
        txtTotal.Value = invoice.TotalAmountIncludeVatEuro;

        ddlCustomer.Items.Add(new RadComboBoxItem(invoice.CompanyName, invoice.CompanyId.ToString()));
        ddlCustomer.SelectedIndex = 0;
        //Payment : Choose option 1 : not use these fields any more.
        //datPaymentDate.SelectedDate = invoice.DateOfPayement;
        //if (invoice.Payement.HasValue)
        //    chkPayment.Checked = invoice.Payement.Value;
        //else
        //    chkPayment.Checked = false;   
        InvoicePaymentsRepository paymentRepo = new InvoicePaymentsRepository();
        IList<InvoicePayments> paymentList = paymentRepo.GetInvoicePaymentsOfInvoice(idFactNumber, type, idYear);
        gridInvoicePayments.DataSource = paymentList;
        double payment = paymentRepo.GetSumPaymentOfInvoice(idFactNumber, type, idYear);
        txtPayment.Value = payment;
        if (invoice.TotalAmountIncludeVatEuro.HasValue && paymentList.Count > 0
            && invoice.TotalAmountIncludeVatEuro.Value <= payment)
        {
            chkPayment.Checked = true;
            DateTime? paymentDate =
                new InvoicePaymentsRepository().GetLatestDatePaymentOfInvoice(idFactNumber, type, idYear);
            if (paymentDate.HasValue)
                txtPaymentDate.Text = paymentDate.Value.ToString("dd/MM/yyyy");
        }

        //Get invoice address
        if (invoice.RefCustomerNumber.HasValue)
        {
            CompanyAddress compAdr = new CompanyAddressRepository().FindOne(
                new CompanyAddress(invoice.RefCustomerNumber.Value));
            hiddenCompanyAddressId.Value = compAdr.AddressID.ToString();
            txtAddressName.Text = compAdr.Name;
            txtAddress.Text = compAdr.Address;
            txtCity.Text = compAdr.City;
            txtZipCode.Text = compAdr.ZipCode;
            txtVatNumber.Text = compAdr.VatNumber;
        }
        //Get invoice details
        IList<InvoiceDetails> detailList =
            new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(
                        idFactNumber, type, idYear, null);
        gridInvoiceDetails.DataSource = detailList;
        //lnkAddInvoiceDetail.Visible = true;
        //lnkAddNewPayment.Visible = true;
        lnkAddInvoiceDetail.OnClientClick = "return OnAddNewInvoiceDetailClientClicked('"
            + SessionManager.CurrentInvoice.InvoiceIdPK + "')";
        lnkAddNewPayment.OnClientClick = "return OnAddNewInvoicePaymentClientClicked('"
            + SessionManager.CurrentInvoice.InvoiceIdPK + "')";

        //For future invoice
        if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "future")
        {
            datInvoiceDate.SelectedDate = DateTime.Today;
        }
    }
    /// <summary>
    /// copy information and invoice detail from another invoice
    /// </summary>
    /// <param name="InvoiceIdPK"></param>
    private void CopyInvoiceData(string sourceInvoiceId) 
    {
        string[] key = sourceInvoiceId.Split('-');
        int idFactNumber = int.Parse(key[0]);
        string type = key[1];
        int idYear = int.Parse(key[2]);

        //Get invoice
        Invoices invoice = new InvoicesRepository().GetInvoiceByID(idFactNumber, type, idYear);
        SessionManager.CurrentInvoice = invoice;
        if (invoice.IdTypeInvoice == "I")
            radInvoice.Checked = true;
        else
            radCreditNote.Checked = true;
        txtCustomerName.Text = invoice.CompanyName;
        if (invoice.CompanyId.HasValue)
            txtIdCustomer.Text = invoice.CompanyId.Value.ToString();
        txtInvoiceNumber.Text = "";
        datInvoiceDate.SelectedDate = DateTime.Now;
        txtRemark.Text = invoice.Remark;
        txtTotalHTVA.Value = invoice.TotalHtvaEuro;
        txtTotalVAT.Value = invoice.AmountVatEuro;
        txtTotal.Value = invoice.TotalAmountIncludeVatEuro;

        gridInvoicePayments.DataSource = new List<InvoicePayments>();
        gridInvoicePayments.DataBind();
        //Get invoice address
        if (invoice.RefCustomerNumber.HasValue)
        {
            CompanyAddress compAdr = new CompanyAddressRepository().FindOne(
                new CompanyAddress(invoice.RefCustomerNumber.Value));
            hiddenCompanyAddressId.Value = compAdr.AddressID.ToString();
            txtAddressName.Text = compAdr.Name;
            txtAddress.Text = compAdr.Address;
            txtCity.Text = compAdr.City;
            txtZipCode.Text = compAdr.ZipCode;
            txtVatNumber.Text = compAdr.VatNumber;
        }
        //Get invoice details
        IList<InvoiceDetails> detailList = new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(idFactNumber, type, idYear, null);
        gridInvoiceDetails.DataSource = detailList;
        lnkAddInvoiceDetail.Visible = false;
        lnkAddNewPayment.Visible = false;
        gridInvoiceDetails.Columns.FindByUniqueName("TemplateEditInvoiceDetailColumn").Display = false;
        gridInvoiceDetails.Columns.FindByUniqueName("TemplateDeleteInvoiceDetailColumn").Display = false;
    }

    private void EnableInvoiceControls(bool enable)
    {
        btnCancel.Visible = enable;

        radInvoice.Enabled = enable;
        radCreditNote.Enabled = enable;
        btnChooseCustomer.Enabled = enable;               
        btnChooseAddress.Enabled = !string.IsNullOrEmpty(txtIdCustomer.Text) && enable;        
        datInvoiceDate.Enabled = enable;
        txtRemark.ReadOnly = !enable;
        btnExport.Visible = !enable;
        txtInternalRemark.ReadOnly = !enable;
        ddlCustomer.Enabled = enable;
        chkFactoring.Enabled = enable;

        lnkAddInvoiceDetail.Visible = enable;
        lnkAddNewPayment.Visible = enable;

        gridInvoiceDetails.Columns.FindByUniqueName("TemplateEditInvoiceDetailColumn").Display = enable;
        gridInvoiceDetails.Columns.FindByUniqueName("TemplateDeleteInvoiceDetailColumn").Display = enable;
        //datPaymentDate.Enabled = enable;
        //chkPayment.Enabled = enable;     
        hidMode.Value = enable ? "edit" : "view";
    }

    protected void OnButtonInvoiceEditSaveClicked(object sender, EventArgs e)
    {
        if (btnEditSave.Text == ResourceManager.GetString("editText"))
        {
            //Change mode to Edit mode.
            //btnEditSave.Text = ResourceManager.GetString("saveText");
            //EnableInvoiceControls(true);
            //lnkAddInvoiceDetail.OnClientClick = "return OnAddNewInvoiceDetailClientClicked('"
            //    + SessionManager.CurrentInvoice.InvoiceIdPK + "')";
            //lnkAddNewPayment.OnClientClick = "return OnAddNewInvoicePaymentClientClicked('"
            //    + SessionManager.CurrentInvoice.InvoiceIdPK + "')";
            //btnExport.Visible = false;
            string url = Request.Url.PathAndQuery;

            if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                url = url.Replace(Request.QueryString["mode"], "edit");
            else
                url += "&mode=edit";
            Response.Redirect(url, true);
        }
        else if (Request.QueryString["type"] == "copy")
        {
            string message;
            SessionManager.CurrentInvoice = null;
            Invoices saveItem = GetInvoice(out message);
            if (!string.IsNullOrEmpty(message))
            {
                string script = "<script type=\"text/javascript\">";
                script += " alert(\"" + message + "\")";
                script += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
                return;
            }
            InvoicesRepository repo = new InvoicesRepository();
            repo.InserNewInvoices(saveItem);
            //insert invoice details
            if (!string.IsNullOrEmpty(Request.QueryString["from"]))
            {
                string invoideIDPK = Request.QueryString["from"];
                string[] key = invoideIDPK.Split('-');
                int idFactNumber = int.Parse(key[0]);
                string type = key[1];
                int idYear = int.Parse(key[2]);

                InvoiceDetailsRepository detailRepo = new InvoiceDetailsRepository();
                IList<InvoiceDetails> detailList = detailRepo.GetInvoiceDetailsOfInvoice(idFactNumber, type, idYear, null);
                foreach (InvoiceDetails detail in detailList)
                {
                    detail.IdFactNumber = saveItem.IdFactNumber;
                    detail.IdTypeInvoice = saveItem.IdTypeInvoice;
                    detail.IdYear = saveItem.IdYear;

                    detailRepo.InserNewInvoiceDetails(detail);
                }
            }

            Response.Redirect("~/InvoiceProfile.aspx?InvoiceIdPK=" + saveItem.InvoiceIdPK + "&mode=edit", true);
        }
        else
        {
            string message;
            //Save data.
            Invoices saveItem = GetInvoice(out message);
            if (!string.IsNullOrEmpty(message))
            {
                string script = "<script type=\"text/javascript\">";
                script += " alert(\"" + message + "\")";
                script += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
                return;
            }
            InvoicesRepository repo = new InvoicesRepository();

            //if (SessionManager.CurrentInvoice != null)
            if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
            {
                repo.UpdateInvoices(saveItem);
            }
            else
            {
                repo.InserNewInvoices(saveItem);
            }

            if (saveItem.IdFactNumberNew.HasValue)
                SessionManager.CurrentInvoice = repo.GetInvoiceByID(saveItem.IdFactNumberNew.Value, saveItem.IdTypeInvoice, saveItem.IdYear);
            else
                SessionManager.CurrentInvoice = repo.GetInvoiceByID(saveItem.IdFactNumber, saveItem.IdTypeInvoice, saveItem.IdYear);

            if (SessionManager.CurrentInvoice.CompanyId.HasValue)
            {
                txtIdCustomer.Text = SessionManager.CurrentInvoice.CompanyId.ToString();
                txtCustomerName.Text = SessionManager.CurrentInvoice.CompanyName.Trim();
            }
            txtInvoiceNumber.Text = SessionManager.CurrentInvoice.IdFactNumber.ToString();
            //    //Change mode to View mode

            string addBackUrl = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
            {
                addBackUrl = "&backurl=visible";
            }
            Response.Redirect(string.Format("~/InvoiceProfile.aspx?InvoiceIdPK={0}&mode=view" + addBackUrl, SessionManager.CurrentInvoice.InvoiceIdPK), true);
        }
        
    }

    protected void OnButtonInvoiceCancelClicked(object sender, EventArgs e)
    {
        Invoices invoice = SessionManager.CurrentInvoice;
        if (invoice != null)
        {
            //FillCurrentCompanyInfo(curCom);
            //btnEditSave.Text = ResourceManager.GetString("editText");
            //EnableInvoiceControls(false);
            //btnExport.Visible = true;
            //Response.Redirect(string.Format("~/InvoiceProfile.aspx?InvoiceIdPK={0}&mode=view", invoice.InvoiceIdPK), true);

            string url = Request.Url.PathAndQuery;

            if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                url = url.Replace(Request.QueryString["mode"], "view");
            else
                url += "&mode=view";
            Response.Redirect(url, true);
        }
        else
        {
            //string lastname = SessionManager.LastNameSearchCriteria;
            Response.Redirect("~/InvoicesPage.aspx");
            btnExport.Visible = false;
        }
    }

    private Invoices GetInvoice(out string message)
    {
        message = string.Empty;
        InvoicesRepository invoiceRepo = new InvoicesRepository();
        if (!datInvoiceDate.SelectedDate.HasValue)
        {
            message = ResourceManager.GetString("messageInvoiceDateNotNull");
            return null;
        }

        string[] fiscalKey = WebConfig.FiscalDate.Split('/');        

        Invoices saveItem = null;        
        int firstFutureNumber = int.Parse(WebConfig.FirstNumberFutureInvoice);
        //if (SessionManager.CurrentInvoice != null)
        if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
        {
            saveItem = SessionManager.CurrentInvoice;
            DateTime fiscalDateMin = new DateTime(saveItem.IdYear, int.Parse(fiscalKey[1]), int.Parse(fiscalKey[0]));
            DateTime fiscalDateMax = new DateTime(saveItem.IdYear + 1, int.Parse(fiscalKey[1]), int.Parse(fiscalKey[0]));
            if (datInvoiceDate.SelectedDate.Value < fiscalDateMin
                || datInvoiceDate.SelectedDate.Value >= fiscalDateMax)
            {
                message = string.Format(ResourceManager.GetString("messageInvoiceDateNotValidFiscalDate"), 
                    fiscalDateMin.ToString("dd/MM/yyyy"), fiscalDateMax.ToString("dd/MM/yyyy"));
                return null;
            }
            //if this is a future invoice.
            if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "future")
            {
                //If change the date less than or equal today, we will change this future invoice to normal invoice.
                if (datInvoiceDate.SelectedDate.Value <= DateTime.Today)
                {
                    Invoices lastNormalInvoice = invoiceRepo.GetInvoicesWithMaxNumber(
                        saveItem.IdYear, saveItem.IdTypeInvoice, false, firstFutureNumber);
                    if (lastNormalInvoice != null && lastNormalInvoice.Date.HasValue
                        && lastNormalInvoice.Date.Value > datInvoiceDate.SelectedDate.Value)
                    {
                        message = ResourceManager.GetString("messageInvoiceDateMustHigherthan") + lastNormalInvoice.Date.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        saveItem.IdFactNumberNew = new InvoicesRepository().GetMaxInvoiceNumber(
                            saveItem.IdYear, saveItem.IdTypeInvoice, false, firstFutureNumber);
                        if (!saveItem.IdFactNumberNew.HasValue)
                        {
                            if (saveItem.IdTypeInvoice == "I")
                                saveItem.IdFactNumberNew = int.Parse(WebConfig.FirstNumberInvoice);
                            else
                                saveItem.IdFactNumberNew = int.Parse(WebConfig.FirstNumberCreditNote);
                        }
                        else
                        {
                            saveItem.IdFactNumberNew = saveItem.IdFactNumberNew.Value + 1;
                        }
                    }
                }
            }
            else
            {
                //If the date is changed.
                if (datInvoiceDate.SelectedDate.Value != saveItem.Date.Value)
                {
                    bool isFuture = saveItem.IdFactNumber >= firstFutureNumber;
                    Invoices nextInvoice = invoiceRepo.GetNextInvoices(saveItem.IdFactNumber,
                        saveItem.IdYear, saveItem.IdTypeInvoice, isFuture, firstFutureNumber);
                    if (nextInvoice != null && nextInvoice.Date.HasValue
                        && nextInvoice.Date.Value < datInvoiceDate.SelectedDate.Value)
                    {
                        message = ResourceManager.GetString("messageInvoiceDateMustHigherthan") + nextInvoice.Date.Value.ToString("dd/MM/yyyy");
                    }

                    Invoices previousInvoice = invoiceRepo.GetPreviousInvoices(saveItem.IdFactNumber,
                        saveItem.IdYear, saveItem.IdTypeInvoice, isFuture, firstFutureNumber);
                    if (previousInvoice != null && previousInvoice.Date.HasValue
                        && previousInvoice.Date.Value > datInvoiceDate.SelectedDate.Value)
                    {
                        message = ResourceManager.GetString("messageInvoiceDateMustLowerthan") + previousInvoice.Date.Value.ToString("dd/MM/yyyy");
                        
                    }
                    //If this is the lasted normal invocie, then change the date to future, we have to change this invoice to future invoice.
                    if (string.IsNullOrEmpty(message))
                    {
                        int? maxNumber = invoiceRepo.GetMaxInvoiceNumber(
                            saveItem.IdYear, saveItem.IdTypeInvoice, false, firstFutureNumber);
                        if (maxNumber.HasValue && maxNumber.Value == saveItem.IdFactNumber
                            && datInvoiceDate.SelectedDate.Value > DateTime.Today)
                        {
                            int? maxNumberFuture = invoiceRepo.GetMaxInvoiceNumber(
                                saveItem.IdYear, saveItem.IdTypeInvoice, true, firstFutureNumber);
                            if (maxNumberFuture.HasValue)
                                saveItem.IdFactNumberNew = maxNumberFuture.Value + 1;
                            else
                                saveItem.IdFactNumberNew = firstFutureNumber;
                        }

                    }
                }
            }
        }
        else
        {
            saveItem = new Invoices();
            int idYear = 0;
            string type = "C";
            int idFactNumber = 0;
            if (radInvoice.Checked)
            {
                type = "I";
                idFactNumber = int.Parse(WebConfig.FirstNumberInvoice);
            }
            else
                idFactNumber = int.Parse(WebConfig.FirstNumberCreditNote);

            DateTime today = DateTime.Today;   
            DateTime fiscalDate = new DateTime(DateTime.Today.Year, int.Parse(fiscalKey[1]), int.Parse(fiscalKey[0]));
            //If Current date is lower than FiscalDate in the current civil year:
            //    IdYear=Year(Current Date) – 1
            //Elseif Current date is higher than FiscalDate in the current civil year:
            //    IdYear= Year(Current Date)
            if (today < fiscalDate)
                idYear = today.Year - 1;
            else
                idYear = today.Year;
                
            int? maxNbr = 1;
            if (datInvoiceDate.SelectedDate.HasValue && datInvoiceDate.SelectedDate.Value > today)
            {
                maxNbr = new InvoicesRepository().GetMaxInvoiceNumber(idYear, type, true, firstFutureNumber);
                idFactNumber = firstFutureNumber;
            }
            else
            {
                maxNbr = new InvoicesRepository().GetMaxInvoiceNumber(idYear, type, false, firstFutureNumber);
            }
            
            if (maxNbr.HasValue)
                idFactNumber = maxNbr.Value + 1;

            saveItem.IdFactNumber = idFactNumber;
            saveItem.IdYear = idYear;
            saveItem.IdTypeInvoice = type;

            bool isFuture = saveItem.IdFactNumber >= firstFutureNumber;
            Invoices lastNormalInvoice = invoiceRepo.GetInvoicesWithMaxNumber(
                       saveItem.IdYear, saveItem.IdTypeInvoice, isFuture, firstFutureNumber);
            if (lastNormalInvoice != null && lastNormalInvoice.Date.HasValue
                    && lastNormalInvoice.Date.Value > datInvoiceDate.SelectedDate.Value)
            {
                message = ResourceManager.GetString("messageInvoiceDateMustHigherthan") + lastNormalInvoice.Date.Value.ToString("dd/MM/yyyy");
            }
        }

        saveItem.InvoiceIdPK = saveItem.IdFactNumber.ToString() + "-" + saveItem.IdTypeInvoice + "-" + saveItem.IdYear;

        saveItem.RefCustomerNumber = int.Parse(hiddenCompanyAddressId.Value);
        saveItem.Date = datInvoiceDate.SelectedDate;
        saveItem.Currency = WebConfig.Currency;
        if (!string.IsNullOrEmpty(txtTotalVAT.Text))
        {
            saveItem.AmountVatEuro = Convert.ToDouble(txtTotalVAT.Text, Common.GetDoubleFormatProvider());            
        }
        else
        {
            saveItem.AmountVatEuro = 0;
        }
        if (!string.IsNullOrEmpty(txtTotalHTVA.Text))
        {
            saveItem.TotalHtvaEuro = Convert.ToDouble(txtTotalHTVA.Text, Common.GetDoubleFormatProvider());
        }
        else
        {
            saveItem.TotalHtvaEuro = 0;
        }
        saveItem.Remark = txtRemark.Text;
        saveItem.Remark_Internal = txtInternalRemark.Text;
        saveItem.Payement = chkPayment.Checked;
        saveItem.Factoring = chkFactoring.Checked;
        if (!string.IsNullOrEmpty(txtPaymentDate.Text))
            saveItem.DateOfPayement = DateTime.ParseExact(txtPaymentDate.Text, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);        
        return saveItem;
    }

    protected void OnDropdownCustomer_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        List<Company> list = new List<Company>();
        string companyName = e.Text;
        if (companyName.Length < 3)
        {
            //list = new CompanyRepository().FindByNameWithStartCharacter(companyName, 15, 1, "SocNom ASC", "SocNom DESC");//.FindByName(companyName);
            list = new CompanyRepository().FindByName(companyName);
        }
        else if (!string.IsNullOrEmpty(companyName))
        {
            list = new CompanyRepository().FindByName(companyName);
        }
        ddlCustomer.DataSource = list;
        ddlCustomer.DataBind();
    }

    #region Export invoice
    protected void OnButtonInvoiceExportClicked(object sender, EventArgs e)
    {
        string filePath = Common.ExportInvoices(SessionManager.CurrentInvoice, WebConfig.AddressFillInInvoice, 
            WebConfig.AbsoluteExportDirectory);


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
        
        /*string script1 = "<script type=\"text/javascript\">";
        script1 += "processInvoiceToolBar(\"ViewInvoiceProfile\");";
        script1 += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("a"))
            ClientScript.RegisterStartupScript(this.GetType(), "a", script1);*/
        
    }

    protected void OnButtonInvoiceEmailClicked(object sender, EventArgs e)
    {
        Invoices currentInvoice = SessionManager.CurrentInvoice;
        CompanyAddress comAddress = new CompanyAddressRepository().FindOne(
            new CompanyAddress(currentInvoice.RefCustomerNumber.Value));
        if (comAddress != null)
        {
            string email = string.Empty;
            if (!string.IsNullOrEmpty(comAddress.Email))
            {
                email = comAddress.Email.Trim();
            }
            else
            {
                Company company = new CompanyRepository().FindOne(new Company(comAddress.CompanyID));
                if (company != null && !string.IsNullOrEmpty(company.Email))
                {
                    email = company.Email.Trim();
                }
            }
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    string fileName = Common.ExportInvoices(currentInvoice,
                        WebConfig.AddressFillInInvoice, WebConfig.AbsoluteExportDirectory);
                    //Microsoft.Office.Interop.Outlook.Application outlookApp =
                    //    new Microsoft.Office.Interop.Outlook.Application();
                    //Microsoft.Office.Interop.Outlook.MailItem mailItem =
                    //    (Microsoft.Office.Interop.Outlook.MailItem)
                    //    outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                    //mailItem.To = email;
                    //mailItem.Subject = "Send invoice";
                    //mailItem.Attachments.Add(fileName,
                    //    Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
                    //mailItem.Display(true);
                    string script = "<script type=\"text/javascript\">";
                    script += " OpenOutlookSendMail(" + "'" + "Send invoice" + "',"
                                                 + "'" + email + "',"
                                                 + "'" + fileName + "'" 
                                                 + ");";
                    //script += " OpenMail();";
                    script += " </script>";

                    if (!ClientScript.IsClientScriptBlockRegistered("saveAction"))
                        ClientScript.RegisterStartupScript(this.GetType(), "saveAction", script);

                }
                catch (System.Exception ex)
                {
                    string script2 = "<script type=\"text/javascript\">";

                    script2 += " alert(\"" + ex.Message + "\")";
                    script2 += " </script>";

                    if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                        ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script2);                    
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
            }
        }
    }
    #endregion

    #region Invoice detail
    private void BindGridDetailData()
    {
        if (SessionManager.CurrentInvoice != null)
        {
            Invoices invoice = SessionManager.CurrentInvoice;
            
            IList<InvoiceDetails> detailList =
                new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(
                            invoice.IdFactNumber, invoice.IdTypeInvoice, invoice.IdYear, null);
            gridInvoiceDetails.DataSource = detailList;

            double totalHTVA = 0;
            double totalVAT = 0;
            foreach (InvoiceDetails item in detailList)
            {
                if (item.AmountEuro.HasValue)
                    totalHTVA += item.AmountEuro.Value;
                if (item.AmountVAT.HasValue)
                    totalVAT += item.AmountVAT.Value;
            }
            txtTotalHTVA.Value = totalHTVA;
            txtTotalVAT.Value = totalVAT;
            txtTotal.Value = ((double)(totalVAT + totalHTVA));

            //refresh payment section.
            InvoicePaymentsRepository repo = new InvoicePaymentsRepository();            
            double payment = repo.GetSumPaymentOfInvoice(invoice.IdFactNumber, invoice.IdTypeInvoice, invoice.IdYear);
            txtPayment.Value = payment;
            if ((totalVAT + totalHTVA) <= payment && payment > 0)
            {
                chkPayment.Checked = true;
                DateTime? paymentDate = repo.GetLatestDatePaymentOfInvoice(
                        invoice.IdFactNumber, invoice.IdTypeInvoice, invoice.IdYear);
                if (paymentDate.HasValue)
                    txtPaymentDate.Text = paymentDate.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                chkPayment.Checked = false;
                txtPaymentDate.Text = null;
            }
        }
    }

    protected void OnGridInvoiceDetails_RowDrop(object sender, GridDragDropEventArgs e)
    {
        if (string.IsNullOrEmpty(e.HtmlElement))
        {
            if (e.DraggedItems.Count >0 && e.DestDataItem != null) //[0].OwnerGridID == gridInvoiceDetails.ID && e.DestDataItem != null && e.DestDataItem.OwnerGridID == gridInvoiceDetails.ClientID)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
                {
                    string[] args = Request.QueryString["InvoiceIdPK"].Split('-');
                    if(args.Length == 3)
                    {
                        List<InvoiceDetails> detailList = new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(
                                                                                        Convert.ToInt32(args[0]), 
                                                                                        args[1], 
                                                                                        Convert.ToInt32(args[2]),
                                                                                        null) as List<InvoiceDetails>;
                        if (detailList.Count > 0)
                        {
                            InvoiceDetails invoiceDetail = GetInvoiceDetailFromList(detailList, (string)e.DestDataItem.GetDataKeyValue("InvoiceDetailsId"));
                            int destinationIndex = detailList.IndexOf(invoiceDetail);

                            List<InvoiceDetails> listToMove = new List<InvoiceDetails>();
                            foreach (GridDataItem dragItem in e.DraggedItems)
                            {
                                InvoiceDetails detail = GetInvoiceDetailFromList(detailList, (string)dragItem.GetDataKeyValue("InvoiceDetailsId"));

                                detailList.Remove(detail);
                                detailList.Insert(destinationIndex, detail);
                            }
                            //save the idLignNumber into database
                            for (int i = 0; i < detailList.Count; i++)
                            {
                                InvoiceDetails saveItem = detailList[i];
                                saveItem.IdLigneNumber = i + 1;
                                new InvoiceDetailsRepository().UpdateInvoiceDetails(saveItem);
                            }
                        }

                        gridInvoiceDetails.Rebind();
                    }
                }
            }
        }
        /*
         if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdPendingOrders.ClientID)
        {
            //reorder items in pending  grid
            IList<Order> pendingOrders = PendingOrders;
            Order order = GetOrder(pendingOrders, (int)e.DestDataItem.GetDataKeyValue("OrderId"));
            int destinationIndex = pendingOrders.IndexOf(order);

            List<Order> ordersToMove = new List<Order>();
            foreach (GridDataItem draggedItem in e.DraggedItems)
            {
                Order tmpOrder = GetOrder(pendingOrders, (int)draggedItem.GetDataKeyValue("OrderId"));
                if (tmpOrder != null)
                    ordersToMove.Add(tmpOrder);
            }

            foreach (Order orderToMove in ordersToMove)
            {
                pendingOrders.Remove(orderToMove);
                pendingOrders.Insert(destinationIndex, orderToMove);
            }
            PendingOrders = pendingOrders;
            grdPendingOrders.Rebind();
            e.DestDataItem.Selected = true;
        }
         */
    }

    private InvoiceDetails GetInvoiceDetailFromList(List<InvoiceDetails> list, string Id)
    {
        foreach (InvoiceDetails detail in list)
        {
            if (detail.InvoiceDetailsId == Id)
            {
                return detail;
            }
        }
        return null;
    }

    protected void OnGridInvoiceDetailNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridDetailData();
    }

    protected void OnGridInvoiceDetailItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteInvoiceDetailColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((InvoiceDetails)e.Item.DataItem).InvoiceDetailsId.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditInvoiceDetailColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            InvoiceDetails detail = e.Item.DataItem as InvoiceDetails;
            if (detail != null)
            {
                LinkButton lnkDetailEdit = (LinkButton)e.Item.FindControl("lnkInvoiceDetailEdit");
                if (lnkDetailEdit != null)
                {
                    lnkDetailEdit.OnClientClick = string.Format("return OnInvoiceDetailEditClientClicked('{0}')", detail.InvoiceDetailsId);
                }
            }
        }
    }

    protected void OnInvoiceDetailDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string invoiceID = lnkItem.CommandArgument;
        if (!string.IsNullOrEmpty(invoiceID))
        {
            string[] key = invoiceID.Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);
            int idLigneNumber = int.Parse(key[3]);

            InvoiceDetailsRepository repo = new InvoiceDetailsRepository();
            repo.DeleteInvoiceDetails(idFactNumber, type, idYear, idLigneNumber);
            new InvoicesRepository().ReComputeAmountOfInvoice(idFactNumber, type, idYear);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoiceDetails, txtTotalHTVA);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoiceDetails, txtTotalVAT);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoiceDetails, txtTotal);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoiceDetails, txtPaymentDate);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoiceDetails, txtPayment);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoiceDetails, chkPayment);
            BindGridDetailData();
            gridInvoiceDetails.DataBind();
        }
    }

    #endregion

    #region Payments
    private void BindGridPaymentData()
    {
        if (SessionManager.CurrentInvoice != null)
        {
            Invoices invoice = SessionManager.CurrentInvoice;
            InvoicePaymentsRepository repo = new InvoicePaymentsRepository();
            
            IList<InvoicePayments> paymentList = repo.GetInvoicePaymentsOfInvoice(
                            invoice.IdFactNumber, invoice.IdTypeInvoice, invoice.IdYear);
            gridInvoicePayments.DataSource = paymentList;

            double payment = repo.GetSumPaymentOfInvoice(invoice.IdFactNumber, invoice.IdTypeInvoice, invoice.IdYear);
            txtPayment.Value = payment;
            double totalVat = double.Parse(txtTotal.Text.Trim());
            if (totalVat <= payment && payment > 0)
            {
                chkPayment.Checked = true;
                DateTime? paymentDate = repo.GetLatestDatePaymentOfInvoice(
                        invoice.IdFactNumber, invoice.IdTypeInvoice, invoice.IdYear);
                if (paymentDate.HasValue)
                    txtPaymentDate.Text = paymentDate.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                chkPayment.Checked = false;
                txtPaymentDate.Text = null;
            }
        }
    }

    protected void OnGridInvoicePaymentNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        BindGridPaymentData();
    }

    protected void OnGridInvoicePaymentItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            LinkButton buttonDelete = dataItem["TemplateDeleteInvoicePaymentColumn"].Controls[1] as LinkButton;
            buttonDelete.OnClientClick = "return confirm('" + ResourceManager.GetString("deleteConfirmText") + "')";
            buttonDelete.CommandArgument = ((InvoicePayments)e.Item.DataItem).IdPayment.ToString();
            buttonDelete.Text = ResourceManager.GetString("deleteText");

            LinkButton buttonEdit = dataItem["TemplateEditInvoicePaymentColumn"].Controls[1] as LinkButton;
            buttonEdit.Text = ResourceManager.GetString("editText");
        }
    }

    protected void OnInvoicePaymentDeleteClicked(object sender, EventArgs e)
    {
        LinkButton lnkItem = (LinkButton)sender;
        string idPaymentStr = lnkItem.CommandArgument;
        if (!string.IsNullOrEmpty(idPaymentStr))
        {
            int idPayment = int.Parse(idPaymentStr);
            //Delete Invoice's payments first.
            InvoicePaymentsRepository payRepo = new InvoicePaymentsRepository();
            payRepo.Delete(new InvoicePayments(idPayment));
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoicePayments, txtPaymentDate);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoicePayments, txtPayment);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(gridInvoicePayments, chkPayment);            
            BindGridPaymentData();
            gridInvoicePayments.DataBind();
        }
    }

    #endregion

    #endregion
    protected void OnInvoiceProfileAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("DataBindCompanyAddress") != -1)
        {
            string[] param = e.Argument.Split('-');
            if (param.Length == 2)
            {
                invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, divHeader);
                //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtAddressName);
                //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtAddress);
                //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtZipCode);
                //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtCity);
                //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtVatNumber);
                //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, btnChooseAddress);

                invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, hiddenCompanyAddressId);
                int companyID = int.Parse(param[1]);
                Company company = new CompanyRepository().FindOne(new Company(companyID));
                txtCustomerName.Text = company.CompanyName;
                txtIdCustomer.Text = companyID.ToString();

                IList<CompanyAddress> comAdrList = new CompanyAddressRepository().GetAddressesOfCompany(companyID);
                hiddenCompanyAddressId.Value = null;
                
                txtAddressName.Text = null;
                txtAddress.Text = null;
                txtZipCode.Text = null;
                txtCity.Text = null;
                txtVatNumber.Text = null;
                foreach (CompanyAddress item in comAdrList)
                {
                    if (item.IsDefault)
                    {
                        hiddenCompanyAddressId.Value = item.AddressID.ToString();
                        txtAddressName.Text = item.Name;
                        txtAddress.Text = item.Address;
                        txtZipCode.Text = item.ZipCode;
                        txtCity.Text = item.City;
                        txtVatNumber.Text = item.VatNumber;
                        break;
                    }
                }
                btnChooseAddress.Enabled = true;
            }            
        }

        else if (e.Argument.IndexOf("RebindInvoiceDetailData") != -1)
        {
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, divTotal);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtTotalHTVA);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtTotalVAT);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtTotal);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtPaymentDate);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtPayment);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, chkPayment);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, gridInvoiceDetails);            
            gridInvoiceDetails.Rebind();
        }
        else if (e.Argument.IndexOf("RebindInvoicePaymentData") != -1)
        {
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, divTotal);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtPaymentDate);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, txtPayment);
            //invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, chkPayment);
            invoiceProfileAjaxManager.AjaxSettings.AddAjaxSetting(invoiceProfileAjaxManager, gridInvoicePayments);
            gridInvoicePayments.Rebind();
        }
        else if (e.Argument.IndexOf("ViewEditInvoice") != -1)
        {
            string url = Request.Url.PathAndQuery;

            if (hidMode.Value == "view")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                    url = url.Replace(Request.QueryString["mode"], "edit");
                else
                    url += "&mode=edit";
                Response.Redirect(url, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                    url = url.Replace(Request.QueryString["mode"], "view");
                else
                    url += "&mode=view";
                Response.Redirect(url, true);
            }
        }
        else if (e.Argument.IndexOf("SaveInvoice") != -1)
        {
            string message;
            Invoices saveItem = GetInvoice(out message);
            if (!string.IsNullOrEmpty(message))
            {
                string script = "<script type=\"text/javascript\">";
                script += " alert(\"" + message + "\")";
                script += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
                return;
            }
            InvoicesRepository repo = new InvoicesRepository();
            if (SessionManager.CurrentInvoice != null)
            {
                repo.UpdateInvoices(saveItem);
            }
            else
            {
                repo.InserNewInvoices(saveItem);
            }
            SessionManager.CurrentInvoice = saveItem;

            string addBackUrl = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["backurl"]) && Request.QueryString["backurl"] == "visible")
            {
                addBackUrl = "&backurl=visible";
            }
            Response.Redirect(string.Format("~/InvoiceProfile.aspx?InvoiceIdPK={0}&mode=view" + addBackUrl, SessionManager.CurrentInvoice.InvoiceIdPK), true);
            //string url = Request.Url.PathAndQuery;
            //if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
            //    url = url.Replace(Request.QueryString["mode"], "view");
            //else
            //    url += "&mode=view";
            //Response.Redirect(url, true);
        }
        else if (e.Argument.IndexOf("PrintInvoice") != -1)
        {
           /* Common.ExportInvoices(SessionManager.CurrentInvoice,
            WebConfig.AddressFillInInvoice, WebConfig.AbsoluteExportDirectory);
            string script = " alert(\"" + ResourceManager.GetString("messageExportSuccessfully") + "\")";
            invoiceProfileAjaxManager.ResponseScripts.Add(script);*/
        }
        else if (e.Argument.IndexOf("EmailInvoice") != -1)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
            {
                string selectedInvoiceIDs = Request.QueryString["InvoiceIdPK"];
                string url = "SendEmail.aspx?type=invoice&ids=" + selectedInvoiceIDs;
                invoiceProfileAjaxManager.ResponseScripts.Add(string.Format("OnSendInvoiceByEmail('{0}')", url));
            }
        }
    }
    protected void OnLinkBackClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SessionManager.BackUrl) && SessionManager.BackUrl.Contains("InvoicesPage.aspx"))
        {
            Response.Redirect(SessionManager.BackUrl, true);
        }
    }
}

    
