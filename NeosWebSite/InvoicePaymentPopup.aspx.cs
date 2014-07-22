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

public partial class InvoicePaymentPopup : System.Web.UI.Page
{
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
            BindData();
        }
    }   
    
    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("lblInvoicePaymentHeader");
        lblPaymentDate.Text = ResourceManager.GetString("columnInvoicePaymentDateHeader");
        lblAmount.Text = ResourceManager.GetString("columnInvoicePaymentAmountHeader");
        lblRemark.Text = ResourceManager.GetString("columnInvoicePaymentRemarkHeader");

        btnSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");
    }

    private void BindData()
    {
        datPaymentDate.SelectedDate = DateTime.Today;
        if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
        {
            string[] key = Request.QueryString["InvoiceIdPK"].Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);
            double sumDetail = new InvoiceDetailsRepository().GetSumTotalDetailOfInvoice(idFactNumber, type, idYear);
            double sumPayment = new InvoicePaymentsRepository().GetSumPaymentOfInvoice(idFactNumber, type, idYear);
            double remain = sumDetail - sumPayment;
            if (remain > -0.05 && remain < 0)
                remain = 0;
            txtAmount.Value = remain;
        }
        if (!string.IsNullOrEmpty(Request.QueryString["IdPayment"]))
        {
            int idPayment = int.Parse(Request.QueryString["IdPayment"]);
            InvoicePayments payment = new InvoicePaymentsRepository().GetInvoicePaymentByID(idPayment);
            datPaymentDate.SelectedDate = payment.DatePayment;
            txtAmount.Value = payment.Amount;
            txtRemark.Text = payment.Remark;
        }        
    }   

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        InvoicePaymentsRepository repo = new InvoicePaymentsRepository();        
        if (!string.IsNullOrEmpty(Request.QueryString["IdPayment"]))
        {
            int idPayment = int.Parse(Request.QueryString["IdPayment"]);
            InvoicePayments payment = new InvoicePaymentsRepository().GetInvoicePaymentByID(idPayment);
            payment.DatePayment = datPaymentDate.SelectedDate;
            payment.Amount = txtAmount.Value;
            payment.Remark = txtRemark.Text.Trim();
            repo.Update(payment);
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
        {
            string[] key = Request.QueryString["InvoiceIdPK"].Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);
            
            InvoicePayments payment = new InvoicePayments();
            payment.IdFactNumber = idFactNumber;
            payment.IdTypeInvoice = type;
            payment.IdYear = idYear;

            payment.DatePayment = datPaymentDate.SelectedDate;
            payment.Amount = txtAmount.Value;
            payment.Remark = txtRemark.Text.Trim();
            repo.Insert(payment);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("saveAndCloseWindow"))
            ClientScript.RegisterStartupScript(this.GetType(), "saveAndCloseWindow", script);
    }        
}
