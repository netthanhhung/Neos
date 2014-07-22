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

public partial class InvoiceDetailPopup : System.Web.UI.Page
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
            
            txtDescription.TabIndex = 1;
            txtQuantity.TabIndex = 2;
            txtUnitPrice.TabIndex = 3;
            txtAmount.TabIndex = 4;
            ddlVAT.TabIndex = 5;
            txtVATCode.TabIndex = 6;
            txtDescription.Focus();
        }
    }   
    
    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("lblInvoiceDetailHeader");

        lblDescription.Text = ResourceManager.GetString("columnInvoiceDetailDescriptionHeader");
        lblQuantity.Text = ResourceManager.GetString("columnInvoiceDetailQuantityHeader");
        lblUnitPrice.Text = ResourceManager.GetString("columnInvoiceDetailUnitPriceHeader");
        lblAmount.Text = ResourceManager.GetString("columnInvoiceDetailAmountHeader");
        
        lblVATHeader.Text = ResourceManager.GetString("lblInvoiceDetailVATHeader");
        lblVAT.Text = ResourceManager.GetString("lblInvoiceDetailVAT");
        lblVATCode.Text = ResourceManager.GetString("lblInvoiceDetailVATCode");

        btnSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");
    }

    private void BindData()
    {
        txtDescription.Focus();
        ddlVAT.DataValueField = "IdVatCode";
        ddlVAT.DataTextField = "TauxVat";
        IList<InvoiceVatCodes> vatCodeList = new InvoiceVatCodesRepository().GetAllVatCode();        
        ddlVAT.DataSource = vatCodeList;
        ddlVAT.DataBind();
        if (!string.IsNullOrEmpty(WebConfig.DefaultVatRate))
        {
            foreach (InvoiceVatCodes item in vatCodeList)            
            {
                double vatRate = Convert.ToDouble(WebConfig.DefaultVatRate, Common.GetDoubleFormatProvider());
                if (item.TauxVat.HasValue && item.TauxVat.Value == vatRate)
                {
                    ddlVAT.SelectedValue = item.IdVatCode.ToString();
                }
            }
        }


        if (!string.IsNullOrEmpty(Request.QueryString["InvoiceDetailId"]))
        {
            string[] key = Request.QueryString["InvoiceDetailId"].Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);
            int idLigneNumber = int.Parse(key[3]);
            IList<InvoiceDetails> invDetailList = new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(
                idFactNumber, type, idYear, idLigneNumber);
            if (invDetailList.Count == 1)
            {
                InvoiceDetails invDetail = invDetailList[0];
                txtDescription.Text = invDetail.Description;
                txtQuantity.Value = invDetail.Quantity;
                txtUnitPrice.Value = invDetail.UnitPriceEuro;
                txtAmount.Value = invDetail.AmountEuro;
                if (invDetail.VatCode.HasValue)
                {
                    ddlVAT.SelectedValue = invDetail.VatCode.Value.ToString();
                    txtVATCode.Text = invDetail.VatCode.Value.ToString();
                }

            }
        }
        else
        {
            OnVATSelectedIndexChanged(ddlVAT, null);
        }

    }

    protected void OnVATSelectedIndexChanged(object sender, EventArgs e)
    {
        txtVATCode.Text = ((RadComboBox)sender).SelectedValue;

    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        InvoiceDetailsRepository repo = new InvoiceDetailsRepository();
        InvoicesRepository repoInv = new InvoicesRepository();        
        if (!string.IsNullOrEmpty(Request.QueryString["InvoiceDetailId"]))
        {
            string[] key = Request.QueryString["InvoiceDetailId"].Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);
            int idLigneNumber = int.Parse(key[3]);
            IList<InvoiceDetails> invDetailList = repo.GetInvoiceDetailsOfInvoice(
                idFactNumber, type, idYear, idLigneNumber);
            if (invDetailList.Count == 1)
            {
                InvoiceDetails invDetail = invDetailList[0];
                invDetail.Description = txtDescription.Text;
                invDetail.Quantity = txtQuantity.Value;
                invDetail.UnitPriceEuro = txtUnitPrice.Value;
                invDetail.AmountEuro = (invDetail.Quantity.HasValue ? invDetail.Quantity.Value : 0) *
                    (invDetail.UnitPriceEuro.HasValue ? invDetail.UnitPriceEuro.Value : 0);
                invDetail.VatCode = int.Parse(ddlVAT.SelectedValue);
                repo.UpdateInvoiceDetails(invDetail);
                repoInv.ReComputeAmountOfInvoice(idFactNumber, type, idYear);
            }
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["InvoiceIdPK"]))
        {
            string[] key = Request.QueryString["InvoiceIdPK"].Split('-');
            int idFactNumber = int.Parse(key[0]);
            string type = key[1];
            int idYear = int.Parse(key[2]);
            int? idLigneNumber = repo.GetMaxInvoiceDetailOrderNumber(idFactNumber, type, idYear);
            if (!idLigneNumber.HasValue)
                idLigneNumber = 1;
            else
                idLigneNumber += 1;
            InvoiceDetails invDetail = new InvoiceDetails();
            invDetail.IdFactNumber = idFactNumber;
            invDetail.IdTypeInvoice = type;
            invDetail.IdYear = idYear;
            invDetail.IdLigneNumber = idLigneNumber.Value;

            invDetail.Description = txtDescription.Text;
            invDetail.Quantity = txtQuantity.Value;
            invDetail.UnitPriceEuro = txtUnitPrice.Value;
            invDetail.AmountEuro = (invDetail.Quantity.HasValue ? invDetail.Quantity.Value : 0) *
                    (invDetail.UnitPriceEuro.HasValue ? invDetail.UnitPriceEuro.Value : 0);
            invDetail.VatCode = int.Parse(ddlVAT.SelectedValue);
            repo.InserNewInvoiceDetails(invDetail);
            repoInv.ReComputeAmountOfInvoice(idFactNumber, type, idYear);
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("saveAndCloseWindow"))
            ClientScript.RegisterStartupScript(this.GetType(), "saveAndCloseWindow", script);
    }

    protected void OnInvoiceDetailAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("DataBindCompanyAddress") != -1)
        {
            string[] param = e.Argument.Split('-');
            if (param.Length == 2)
            {
                
            }
        }
    }
    
}
