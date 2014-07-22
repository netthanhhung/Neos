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
using System.Collections.Generic;
using Neos.Data;

public partial class SendEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }

        if (!IsPostBack)
        {
            FillLabelTexts();
            BindData();
        }
    }

    private void FillLabelTexts()
    {
        lblTo.Text = ResourceManager.GetString("lblSendMailTo");
        lblCC.Text = ResourceManager.GetString("lblSendMailCC");
        lblSubject.Text = ResourceManager.GetString("lblSendMailSubject");
        btnSendMail.Text = ResourceManager.GetString("btnSendMailSendText");
        lblAttach.Text = ResourceManager.GetString("lblSendMailAttachments");  
    }

    private void BindData()
    {
        if (Request.QueryString["type"] == "invoice")
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ids"]))
            {
                string companyEmail = "";
                List<string> exportFiles = new List<string>();
                string[] invoiceIds = Request.QueryString["ids"].Split(';');
                
                for (int i = 0; i < invoiceIds.Length; i++)
                {
                    string[] key = invoiceIds[i].Split('-');
                    int idFactNumber = int.Parse(key[0]);
                    string type = key[1];
                    int idYear = int.Parse(key[2]);
                    Invoices invoice = new InvoicesRepository().GetInvoiceByID(idFactNumber, type, idYear);
                    if(string.IsNullOrEmpty(companyEmail))
                    {
                        companyEmail = GetEmailOfCompany(invoice.CompanyId.Value);
                        txtTo.Text = companyEmail;
                    }
                    string fileName = Common.ExportInvoices(invoice, WebConfig.AddressFillInInvoice, 
                        WebConfig.AbsoluteExportDirectory);
                    
                    exportFiles.Add(fileName);
                }
                rptAttachInvoices.DataSource = exportFiles;
                rptAttachInvoices.DataBind();
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

    protected void onButtonSendMail_Click(object sender, EventArgs e)
    {
        List<string> toEmails = new List<string>();
        List<string> ccEmails = new List<string>();
        List<string> attachedFiles = new List<string>();

        if (!string.IsNullOrEmpty(txtTo.Text))
        {
            toEmails = new List<string>(txtTo.Text.Split(';'));
        }

        if (!string.IsNullOrEmpty(txtCC.Text))
        {
            ccEmails = new List<string>(txtCC.Text.Split(';'));
        }
        foreach (RepeaterItem item in rptAttachInvoices.Items)
        {
            CheckBox chkSelectInvoice = item.FindControl("chkSelectInvoice") as CheckBox;
            if (chkSelectInvoice.Checked)
            {
                attachedFiles.Add(WebConfig.AbsoluteExportDirectory + "\\" + chkSelectInvoice.Text);
            }
        }
        string message = "";
        bool isValid = true;

        if (toEmails.Count + ccEmails.Count <= 0)
        {   
            //msgMissingEmailAddress
            message = ResourceManager.GetString("msgMissingEmailAddress");
            isValid = false;
        }
        else
        {
            List<string> emailsToCheck = new List<string>();
            emailsToCheck.AddRange(toEmails);
            emailsToCheck.AddRange(ccEmails);

            foreach (string email in emailsToCheck)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    if (!Common.IsValidEmailAddress(email))
                    {
                        message = ResourceManager.GetString("msgInvalidEmailFormat");
                        isValid = false;
                        break;
                    }
                }
            }           
        }

        string script = "<script type='text/javascript'>";
        if (isValid)
        {
            Common.SendEmail(toEmails, ccEmails, attachedFiles, txtSubject.Text, txtEmailContent.Content);
            message = ResourceManager.GetString("msgEmailSent");
            script += "closeWindow();";
        }
        script += string.Format("alert(\"{0}\");", message);
        script += "</script>";
        if (!ClientScript.IsClientScriptBlockRegistered("ShowMessage"))
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script);
    }

    protected void OnRepeaterAttachInvoices_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            string fileName = (string)e.Item.DataItem;
            if (!string.IsNullOrEmpty(fileName))
            {
                string name = fileName.Substring(fileName.LastIndexOf("\\") + 1, fileName.Length - fileName.LastIndexOf('\\') - 1);
                CheckBox chkSelectInvoice = (CheckBox)e.Item.FindControl("chkSelectInvoice");
                if (chkSelectInvoice != null)
                {
                    chkSelectInvoice.Text = name;
                }
            }
        }
    }
}
