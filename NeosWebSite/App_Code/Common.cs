using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using Neos.Data;
using ASPPDFLib;
using System.Reflection;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;
using System.IO;
using EPocalipse.IFilter;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Globalization;

/// <summary>
/// Summary description for Common
/// </summary>
public static class Common
{
    public static void RedirectToLoginPage(Page page)
    {
        string currentURL = HttpContext.Current.Request.Url.AbsoluteUri;
        string applicationPath = HttpContext.Current.Request.ApplicationPath;
        string loginURL = currentURL.Substring(0, currentURL.LastIndexOf(applicationPath) + applicationPath.Length + 1) + "Login.aspx";

        StringBuilder script = new StringBuilder();
        script.Append("<script type=\"text/javascript\">");
        script.AppendFormat("redirectToLoginPage('{0}');", loginURL);
        script.Append("</script>");

        if (!page.ClientScript.IsClientScriptBlockRegistered("onLoginRedirectToLoginPage"))
            page.ClientScript.RegisterStartupScript(page.GetType(), "onLoginRedirectToLoginPage", script.ToString());
    }

    public static void KeepSessionAlive(Page page)
    {
        string currentURL = HttpContext.Current.Request.Url.AbsoluteUri;
        string applicationPath = HttpContext.Current.Request.ApplicationPath;
        string loginURL = currentURL.Substring(0, currentURL.LastIndexOf(applicationPath) + applicationPath.Length + 1) + "Login.aspx";

        StringBuilder script = new StringBuilder();
        script.Append("<script type=\"text/javascript\">");
        script.AppendFormat("sessionKeepAlive('{0}');", loginURL);
        script.Append("</script>");

        if (!page.ClientScript.IsClientScriptBlockRegistered("onSessionKeepAlive"))
            page.ClientScript.RegisterStartupScript(page.GetType(), "onSessionKeepAlive", script.ToString());
    }

    public static DateTime GetBeginDayOfWeek(DateTime date)
    {
        DateTime dateFrom = date;
        if (date.DayOfWeek == DayOfWeek.Tuesday)
            dateFrom = date.Subtract(new TimeSpan(1, 0, 0, 0));
        else if (date.DayOfWeek == DayOfWeek.Wednesday)
            dateFrom = date.Subtract(new TimeSpan(2, 0, 0, 0));
        else if (date.DayOfWeek == DayOfWeek.Thursday)
            dateFrom = date.Subtract(new TimeSpan(3, 0, 0, 0));
        else if (date.DayOfWeek == DayOfWeek.Friday)
            dateFrom = date.Subtract(new TimeSpan(4, 0, 0, 0));
        else if (date.DayOfWeek == DayOfWeek.Saturday)
            dateFrom = date.Subtract(new TimeSpan(5, 0, 0, 0));
        else if (date.DayOfWeek == DayOfWeek.Sunday)
            dateFrom = date.Subtract(new TimeSpan(6, 0, 0, 0));
        return dateFrom;
    }

    public static string ExportInvoicesTemplate(Invoices currentInvoice, string addressFillInInvoice,
        string exportDirectory)
    {
        IPdfManager manager = new PdfManager();
        IPdfDocument document = null;
        IPdfPage page = null;
        
        document = manager.CreateDocument(Missing.Value);
        page = document.Pages.Add(Missing.Value, Missing.Value, Missing.Value);
      
        ////////////////Draw logo and template///////////////////////////////////
        IPdfImage verticalLine = document.OpenImage(HttpContext.Current.Request.MapPath("~/images/neos-vertical-line.gif"), Missing.Value);
        IPdfParam verticalParam = manager.CreateParam(Missing.Value);
        verticalParam["x"].Value = 5;
        verticalParam["y"].Value = 0;
        verticalParam["ScaleX"].Value = 0.4f;
        //logoParam1["ScaleY"].Value = 0.8f;

        page.Canvas.DrawImage(verticalLine, verticalParam);

        IPdfImage logoImage = document.OpenImage(HttpContext.Current.Request.MapPath("~/images/logo_neos_new.gif"), Missing.Value);
        IPdfParam logoParam = manager.CreateParam(Missing.Value);
        logoParam["x"].Value = 20;
        logoParam["y"].Value = page.Height - 140;
        logoParam["ScaleX"].Value = 0.8f;
        logoParam["ScaleY"].Value = 0.8f;

        page.Canvas.DrawImage(logoImage, logoParam);

        string imageFooterPath = WebConfig.AbsolutePathImageFooterPath;
        if (!File.Exists(imageFooterPath))
        {
            imageFooterPath = HttpContext.Current.Request.MapPath("~/images/logo-neos-footer.gif");
        }
        IPdfImage footerImage = document.OpenImage(imageFooterPath, Missing.Value);
        IPdfParam footerParam = manager.CreateParam(Missing.Value);
        footerParam["x"].Value = 130;
        footerParam["y"].Value = 10;
        footerParam["ScaleX"].Value = 0.55f;
        footerParam["ScaleY"].Value = 0.55f;

        page.Canvas.DrawImage(footerImage, footerParam);  
        
        string fileName = "C:\\Temp\\Invoice-Template.pdf";
        string strFilename = document.Save(fileName, true);
        //document.SaveHttp(fileName, Missing.Value);
        return fileName;
    }

    public static string ExportInvoices(Invoices currentInvoice, string addressFillInInvoice,
        string exportDirectory)
    {        
        IPdfManager manager = new PdfManager();
        IPdfDocument document = null;
        IPdfPage page = null;
        if (WebConfig.UsedPredefinedInvoicePaperToPrintInvoice.Trim() == "true")
        {

            document = manager.OpenDocument(WebConfig.AbsolutePathPredefinedInvoicePaper, Missing.Value);            
            page = document.Pages[1];                        
        }
        else
        {
            document = manager.CreateDocument(Missing.Value);
            page = document.Pages.Add(Missing.Value, Missing.Value, Missing.Value);

            ////////////////Draw logo and template///////////////////////////////////
            IPdfImage verticalLine = document.OpenImage(HttpContext.Current.Request.MapPath("~/images/neos-vertical-line.gif"), Missing.Value);
            IPdfParam verticalParam = manager.CreateParam(Missing.Value);
            verticalParam["x"].Value = 5;
            verticalParam["y"].Value = 0;
            verticalParam["ScaleX"].Value = 0.4f;
            //logoParam1["ScaleY"].Value = 0.8f;

            page.Canvas.DrawImage(verticalLine, verticalParam);

            IPdfImage logoImage = document.OpenImage(HttpContext.Current.Request.MapPath("~/images/logo_neos_new.gif"), Missing.Value);
            IPdfParam logoParam = manager.CreateParam(Missing.Value);
            logoParam["x"].Value = 20;
            logoParam["y"].Value = page.Height - 140;
            logoParam["ScaleX"].Value = 0.8f;
            logoParam["ScaleY"].Value = 0.8f;

            page.Canvas.DrawImage(logoImage, logoParam);

            string imageFooterPath = WebConfig.AbsolutePathImageFooterPath;
            if (!File.Exists(imageFooterPath))
            {
                imageFooterPath = HttpContext.Current.Request.MapPath("~/images/logo-neos-footer.gif");
            }
            IPdfImage footerImage = document.OpenImage(imageFooterPath, Missing.Value);
            IPdfParam footerParam = manager.CreateParam(Missing.Value);
            footerParam["x"].Value = 130;
            footerParam["y"].Value = 10;
            footerParam["ScaleX"].Value = 0.55f;
            footerParam["ScaleY"].Value = 0.55f;

            page.Canvas.DrawImage(footerImage, footerParam);                   
        }
        
        IPdfParam param = manager.CreateParam(Missing.Value);

        //Get invoice details and payments.
        IList<InvoiceDetails> detailList = new InvoiceDetailsRepository().GetInvoiceDetailsOfInvoice(
                                        currentInvoice.IdFactNumber, currentInvoice.IdTypeInvoice, currentInvoice.IdYear, null);
        CompanyAddress comAddress = new CompanyAddressRepository().FindOne(
            new CompanyAddress(currentInvoice.RefCustomerNumber.Value));

        //-----Do not show payment----------------//
        //IList<InvoicePayments> paymentList =
        //    new InvoicePaymentsRepository().GetInvoicePaymentsOfInvoice(
        //        currentInvoice.IdFactNumber, currentInvoice.IdTypeInvoice, currentInvoice.IdYear);
        //double paymentTotal = new InvoicePaymentsRepository().GetSumPaymentOfInvoice(
        //        currentInvoice.IdFactNumber, currentInvoice.IdTypeInvoice, currentInvoice.IdYear);              
        
        //////////////// Draw Invoice title. ////////////////////////////////////
        //int height = 750;
        string title = currentInvoice.IdTypeInvoice == "I" ? ResourceManager.GetString("lblInvoiceInvoice") : ResourceManager.GetString("lblInvoiceCreditNote");
        title = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 12pt; font-weight: bold; color: black"">{0}</FONT>",
            title);
        page.Canvas.DrawText(title, "x=360, y=720, html=true", document.Fonts["Arial", Missing.Value]);                

        ///////////////////////Draw Customers .///////////////////////////////////
        string companyName = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 12pt; font-weight: bold; color: black"">{0}</FONT>", 
            (string.IsNullOrEmpty(comAddress.Name) ? "" : comAddress.Name));
        page.Canvas.DrawText(companyName, "x=320, y=650, html=true", document.Fonts["Arial", Missing.Value]);

        string coName = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-weight: normal; color: black"">{0}</FONT>",
            (string.IsNullOrEmpty(comAddress.Co) ? "" : comAddress.Co));
        page.Canvas.DrawText(coName, "x=320, y=635, html=true", document.Fonts["Arial", Missing.Value]);

        string companyAddr = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-weight: normal; color: black"">{0}</FONT>",
            (string.IsNullOrEmpty(comAddress.Address) ? "" : comAddress.Address));
        page.Canvas.DrawText(companyAddr, "x=320, y=620, html=true", document.Fonts["Arial", Missing.Value]);

        string companyCity = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-weight: normal; color: black"">{0}</FONT>",
            (string.IsNullOrEmpty(comAddress.ZipCode) ? "" : comAddress.ZipCode) + " " +
            (string.IsNullOrEmpty(comAddress.City) ? "" : comAddress.City));
        page.Canvas.DrawText(companyCity, "x=320, y=605, html=true", document.Fonts["Arial", Missing.Value]);

        string vatNumber = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-weight: normal; color: black"">{0}</FONT>",
            ResourceManager.GetString("lblInvoiceVATNumber") + " : " 
            + (string.IsNullOrEmpty(comAddress.VatNumber) ? "" : comAddress.VatNumber));
        page.Canvas.DrawText(vatNumber, "x=320, y=575, html=true", document.Fonts["Arial", Missing.Value]);

        string invoiceNumber = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-weight: normal; color: black"">{0}</FONT>",
            ResourceManager.GetString("lblInvoiceNumber") + " : " + currentInvoice.IdFactNumber.ToString());
        page.Canvas.DrawText(invoiceNumber, "x=70, y=560, html=true", document.Fonts["Arial", Missing.Value]);

        string invoiceDate = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-weight: normal; color: black"">{0}</FONT>",
            ResourceManager.GetString("lblInvoiceDate") + " : "
            + (currentInvoice.Date.HasValue ? currentInvoice.Date.Value.ToString("dd/MM/yyyy") : string.Empty));
        page.Canvas.DrawText(invoiceDate, "x=320, y=560, html=true", document.Fonts["Arial", Missing.Value]);

        ///////////////////Draw details//////////////////////////////////////  
        int heightDetail = 15;
        for (int i = 0; i < detailList.Count; i++)
        {
            heightDetail += CountLineOfString(detailList[i].Description, 33) * 13;
        }
        
        string rowsDetail = ((int)(detailList.Count + 1)).ToString();
        string detailParam = "width=500;height=" + heightDetail + "; Rows=" + rowsDetail
            + "; Cols=4; cellborder=0.1; cellbordercolor = lightgray ; cellspacing=0; cellpadding=0";
        IPdfTable tableDetail = document.CreateTable(detailParam);
        tableDetail.Font = document.Fonts["Arial", Missing.Value];
        param.Set("alignment=left; size=10;");

        tableDetail.Rows[1].Cells[1].Width = 330;
        tableDetail.Rows[1].Cells[2].Width = 50;
        tableDetail.Rows[1].Cells[3].Width = 50;
        tableDetail.Rows[1].Cells[4].Width = 70;
        //tableDetail.Rows[1].Cells[5].Width = 50;
        //tableDetail.Rows[1].Cells[5].Width = 40;
        //tableDetail.Rows[1].Cells[6].Width = 50;
        //tableDetail.Rows[1].Cells[7].Width = 50;

        //New requirement (20-11-2009) : not show %VAT, VAT amuount and total 
        tableDetail.Rows[1].Cells[1].AddText(ResourceManager.GetString("columnInvoiceDetailDescriptionHeader"), param, Missing.Value);
        tableDetail.Rows[1].Cells[2].AddText(ResourceManager.GetString("columnInvoiceDetailQuantityHeader"), param, Missing.Value);
        tableDetail.Rows[1].Cells[3].AddText(ResourceManager.GetString("columnInvoiceDetailUnitPriceHeader"), param, Missing.Value);
        tableDetail.Rows[1].Cells[4].AddText(ResourceManager.GetString("columnInvoiceDetailAmountHeader"), param, Missing.Value);
        //tableDetail.Rows[1].Cells[5].AddText(ResourceManager.GetString("columnInvoiceDetailCodeVATHeader"), param, Missing.Value);
        //tableDetail.Rows[1].Cells[5].AddText(ResourceManager.GetString("columnInvoiceDetailPercentVATHeader"), param, Missing.Value);
        //tableDetail.Rows[1].Cells[6].AddText(ResourceManager.GetString("columnInvoiceDetailAmountVATHeader"), param, Missing.Value);
        //tableDetail.Rows[1].Cells[7].AddText(ResourceManager.GetString("columnInvoiceDetailTotalAmountHeader"), param, Missing.Value);
        IPdfParam paramLeft = manager.CreateParam(Missing.Value);
        paramLeft.Set("alignment=right; size=10;");
        for (int i = 0; i < detailList.Count; i++)
        {
            InvoiceDetails detail = detailList[i];
            tableDetail.Rows[i + 2].Cells[1].Height = CountLineOfString(detail.Description, 33) * 13;
            tableDetail.Rows[i + 2].Cells[1].AddText(string.IsNullOrEmpty(detail.Description) ? "" : detail.Description, param, document.Fonts["Arial", Missing.Value]);
            tableDetail.Rows[i + 2].Cells[2].AddText(Get2DigitStringOfDouble(detail.Quantity), paramLeft, document.Fonts["Arial", Missing.Value]);
            tableDetail.Rows[i + 2].Cells[3].AddText(Get2DigitStringOfDouble(detail.UnitPriceEuro), paramLeft, document.Fonts["Arial", Missing.Value]);
            tableDetail.Rows[i + 2].Cells[4].AddText(Get2DigitStringOfDouble(detail.AmountEuro), paramLeft, document.Fonts["Arial", Missing.Value]);
            //tableDetail.Rows[i + 2].Cells[5].AddText(detail.VatCode.HasValue ? detail.VatCode.Value.ToString() : "", param, document.Fonts["Arial", Missing.Value]);
            //tableDetail.Rows[i + 2].Cells[5].AddText(detail.VatRate.HasValue ? detail.VatRate.Value.ToString() : "", param, document.Fonts["Arial", Missing.Value]);
            //tableDetail.Rows[i + 2].Cells[6].AddText(Get2DigitStringOfDouble(detail.AmountVAT), param, document.Fonts["Arial", Missing.Value]);
            //tableDetail.Rows[i + 2].Cells[7].AddText(Get2DigitStringOfDouble(detail.TotalAmountVAT), param, document.Fonts["Arial", Missing.Value]);
        }
        page.Canvas.DrawTable(tableDetail, "x=70, y=530");
        
        
        //////////////////Draw total of details/////////////////////////////////////////////////
        //string totalHVTA = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-weight: normal; color: black"">{0}</FONT>",
        //    ResourceManager.GetString("lblInvoiceTotalHTVA") + " : "
        //    + (currentInvoice.TotalHtvaEuro.HasValue ? currentInvoice.TotalHtvaEuro.Value.ToString() : "0"));
        //page.Canvas.DrawText(invoiceNumber, "x=70, y=575, html=true", document.Fonts["Arial", Missing.Value]);

        IPdfTable tableTotal = document.CreateTable("width=165;height=50; Rows=3; Cols=2; cellborder=0;  cellspacing=0; cellpadding=0");
        tableTotal.Font = document.Fonts["Arial", Missing.Value];
        param.Set("alignment=left; size=10;");
        tableTotal.Rows[1].Cells[1].Width = 85;
        tableTotal.Rows[1].Cells[2].Width = 80;

        tableTotal.Rows[1].Cells[1].AddText(ResourceManager.GetString("lblInvoiceTotalHTVA"), param, Missing.Value);
        tableTotal.Rows[1].Cells[2].AddText(": " + (Get2DigitStringOfDouble(currentInvoice.TotalHtvaEuro)), param, Missing.Value);

        tableTotal.Rows[2].Cells[1].AddText(ResourceManager.GetString("lblInvoiceTotalVAT"), param, Missing.Value);
        tableTotal.Rows[2].Cells[2].AddText(": " + (Get2DigitStringOfDouble(currentInvoice.AmountVatEuro)), param, Missing.Value);

        tableTotal.Rows[3].Cells[1].AddText(ResourceManager.GetString("lblInvoiceTotal"), param, Missing.Value);
        tableTotal.Rows[3].Cells[2].AddText(": " + (Get2DigitStringOfDouble(currentInvoice.TotalAmountIncludeVatEuro)), param, Missing.Value);
        
        int totalY = 530 - heightDetail - 20;
        if (!string.IsNullOrEmpty(comAddress.FactoringCode))
        {
            page.Canvas.DrawTable(tableTotal, "x=400, y=200");
        }
        else
        {
            page.Canvas.DrawTable(tableTotal, "x=400, y=145");
        }

        ///////////////Draw factoring code////////////////////////////////////
        if (!string.IsNullOrEmpty(currentInvoice.Remark))
        {
            int remarkHeight = CountLineOfString(currentInvoice.Remark, 33) * 13;
            string remarkParam = "width=300;height=" + remarkHeight + "; Rows=1; Cols=1; cellborder=0.0; cellbordercolor = lightgray ; cellspacing=0; cellpadding=0";
            IPdfTable tableRemark = document.CreateTable(remarkParam);
            tableRemark.Font = document.Fonts["Arial", Missing.Value];
            param.Set("alignment=left; size=10;");

            tableRemark.Rows[1].Cells[1].AddText(currentInvoice.Remark, param, Missing.Value);
            
            page.Canvas.DrawTable(tableRemark, "x=70, y=220");            
        }

        ///////////////Draw factoring code////////////////////////////////////
        if (!string.IsNullOrEmpty(comAddress.FactoringCode))
        {
            //string factParam = "width=500;height=40; Rows=2; Cols=1; cellborder=0.0; cellbordercolor = lightgray ; cellspacing=0; cellpadding=0";
            //IPdfTable tableFact = document.CreateTable(factParam);
            //tableFact.Font = document.Fonts["Arial", Missing.Value];
            //param.Set("alignment=left; size=10;");
            string factoringCode1 = ResourceManager.GetString("MessagePrintFactoringCode1");
            string factoringCode2 = ResourceManager.GetString("MessagePrintFactoringCode2");
            factoringCode1 = string.Format(factoringCode1, comAddress.FactoringCode,
                currentInvoice.IdFactNumber, currentInvoice.IdYear.ToString().Substring(2, 2));
            //string factoringCode = ResourceManager.GetString("lblFactoringCode") + " : " + comAddress.FactoringCode.HasValue.ToString()
            //    + currentInvoice.IdFactNumber.ToString();
            //if (currentInvoice.Date.HasValue)
            //{
            //    factoringCode += currentInvoice.Date.Value.Year.ToString().Substring(2);
            //}
            //tableFact.Rows[1].Cells[1].Width = 470;
            //tableFact.Rows[1].Cells[1].AddText(factoringCode1, param, Missing.Value);
            //tableFact.Rows[2].Cells[1].AddText(factoringCode2, param, Missing.Value);
            //totalY = totalY - 100;
            //page.Canvas.DrawTable(tableFact, "x=70, y=140");

            factoringCode1 = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-style: Italic; font-weight: Normal; color: black"">{0}</FONT>",
                                                factoringCode1);
            page.Canvas.DrawText(factoringCode1, "x=70, y=120, html=true", document.Fonts["Arial", Missing.Value]);
            factoringCode2 = string.Format(@"<FONT STYLE=""font-family: Arial; font-size: 10pt; font-style: Italic; font-weight: Normal; color: black"">{0}</FONT>",
                                                factoringCode2);
            page.Canvas.DrawText(factoringCode2, "x=70, y=105, html=true", document.Fonts["Arial", Missing.Value]); 
        }

        /*
        ////////////////////////////////Draw payments /////////////////////////////////       
        totalY = totalY - 25;
        page.Canvas.DrawText(ResourceManager.GetString("tabInvoicePayment"), "x=70, y=" + totalY.ToString() + ", size=15;", document.Fonts["Helvetica", Missing.Value]);

        string heightPayment = ((int)(paymentList.Count * 12 + 20)).ToString();
        string rowsPayment = ((int)(paymentList.Count + 1)).ToString();
        string paymentParam = "width=500;height=" + heightPayment + "; Rows=" + rowsPayment
            + "; Cols=3; cellborder=0.1; cellbordercolor = lightgray ; cellspacing=0; cellpadding=0";
        IPdfTable tablePayment = document.CreateTable(paymentParam);
        tablePayment.Font = document.Fonts["Helvetica", Missing.Value];
        param.Set("alignment=left; size=10;");

        tablePayment.Rows[1].Cells[1].Width = 300;
        tablePayment.Rows[1].Cells[2].Width = 110;
        tablePayment.Rows[1].Cells[3].Width = 60;

        tablePayment.Rows[1].Cells[1].AddText(ResourceManager.GetString("columnInvoicePaymentRemarkHeader"), param, Missing.Value);
        tablePayment.Rows[1].Cells[2].AddText(ResourceManager.GetString("columnInvoicePaymentDateHeader"), param, Missing.Value);
        tablePayment.Rows[1].Cells[3].AddText(ResourceManager.GetString("columnInvoicePaymentAmountHeader"), param, Missing.Value);

        for (int i = 0; i < paymentList.Count; i++)
        {
            InvoicePayments payment = paymentList[i];
            tablePayment.Rows[i + 2].Cells[1].AddText(string.IsNullOrEmpty(payment.Remark) ? "" : payment.Remark, param, document.Fonts["Arial", Missing.Value]);
            tablePayment.Rows[i + 2].Cells[2].AddText(
                payment.DatePayment.HasValue ? payment.DatePayment.Value.ToString("dd/MM/yyyy") : "",
                param, document.Fonts["Courier", Missing.Value]);
            tablePayment.Rows[i + 2].Cells[3].AddText(
                payment.Amount.HasValue ? payment.Amount.Value.ToString() : "0",
                param, document.Fonts["Courier", Missing.Value]);
        }
        totalY = totalY - 20;
        page.Canvas.DrawTable(tablePayment, "x=70, y=" + totalY.ToString());

        totalY = totalY - int.Parse(heightPayment) - 5;

        ////////////////////Draw total of payment///////////////////////////////////////////
        page.Canvas.DrawText(ResourceManager.GetString("labelTotalInvoicePaymentAmount"), "x=70, y=" + totalY.ToString() + ", size=10;", document.Fonts["Helvetica", Missing.Value]);
        page.Canvas.DrawText(paymentTotal.ToString(), "x=480, y=" + totalY.ToString() + ", size=10;", document.Fonts["Helvetica", Missing.Value]);

        totalY = totalY - 15;
        double remain = 0;
        if (currentInvoice.TotalAmountIncludeVatEuro.HasValue)
        {
            remain = currentInvoice.TotalAmountIncludeVatEuro.Value - paymentTotal;
        }
        else
        {
            remain = -paymentTotal;
        }
        page.Canvas.DrawText(ResourceManager.GetString("labelTotalInvoiceRemainAmount"), "x=70, y=" + totalY.ToString() + ", size=10;", document.Fonts["Helvetica", Missing.Value]);
        page.Canvas.DrawText(remain.ToString(), "x=480, y=" + totalY.ToString() + ", size=10;", document.Fonts["Helvetica", Missing.Value]);

        totalY = totalY - 15;
        if (currentInvoice.Payement.HasValue && currentInvoice.Payement.Value && currentInvoice.DateOfPayement.HasValue)
        {
            page.Canvas.DrawText(ResourceManager.GetString("labelTotalInvoicePaid"), "x=70, y=" + totalY.ToString() + ", size=10;", document.Fonts["Helvetica", Missing.Value]);
            page.Canvas.DrawText(currentInvoice.DateOfPayement.Value.ToString("dd/MM/yyyy"), "x=370, y=" + totalY.ToString() + ", size=10;", document.Fonts["Helvetica", Missing.Value]);
        }
        else
        {
            page.Canvas.DrawText(ResourceManager.GetString("labelTotalInvoiceUnpaid"), "x=70, y=" + totalY.ToString() + ", size=10;", document.Fonts["Helvetica", Missing.Value]);
        }
        */

        /////// Save document, the Save method returns generated file name///////////////
        string fileName = exportDirectory + "\\";
        //string fileName = string.Empty;
        if (currentInvoice.IdTypeInvoice == "I")
            fileName += "Invoice";
        else
            fileName += "Credit Note";
        fileName += "-" + currentInvoice.IdFactNumber.ToString() + currentInvoice.IdTypeInvoice + currentInvoice.IdYear;
        fileName += "-" + DateTime.Today.Year;
        if (DateTime.Today.Month < 10)
        {
            fileName += "0";
        }
        fileName += DateTime.Today.Month;
        if (DateTime.Today.Day < 10)
        {
            fileName += "0";
        }
        fileName += DateTime.Today.Day + ".pdf";
        //fileName = "attachment;filename=" + fileName;
        string strFilename = document.Save(fileName, true);
        //document.SaveHttp(fileName, Missing.Value);
        return fileName;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="to"></param>
    /// <param name="Cc"></param>
    /// <param name="attachedFiles"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static void SendEmail(List<string> to, List<string> Cc, List<string> attachedFiles, string subject, string content)
    {
        MailMessage mail = new MailMessage();
        foreach (string mailAdrr in to)
        {
            if(!string.IsNullOrEmpty(mailAdrr))
                mail.To.Add(new MailAddress(mailAdrr));
        }
        foreach (string ccMailAdrr in Cc)
        {
            if (!string.IsNullOrEmpty(ccMailAdrr))
                mail.CC.Add(new MailAddress(ccMailAdrr));
        }
        foreach (string attach in attachedFiles)
        {
            if (!string.IsNullOrEmpty(attach))
                mail.Attachments.Add(new Attachment(attach));
        }
        mail.IsBodyHtml = true;
        mail.Subject = subject;
        mail.Body = content;
        SendMail(mail);    
    }

    public static void SendMail(MailMessage mail)
    {
        try
        {
            Configuration webconfigFile = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
            MailSettingsSectionGroup mailSettings = webconfigFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            SmtpClient _smtpClient = new SmtpClient();
            if (!string.IsNullOrEmpty(mailSettings.Smtp.Network.UserName))
            {
                _smtpClient.Credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);
                _smtpClient.EnableSsl = true;
            }
            _smtpClient.Port = mailSettings.Smtp.Network.Port;
            _smtpClient.Host = mailSettings.Smtp.Network.Host;
            //client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
            _smtpClient.Send(mail);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName">file name only: such as: abc.doc</param>
    public static void IndexingDocumentFile(string fileName, int candidateID)
    {
        try
        {
            string indexFileLocation = WebConfig.DocumentIndexPhysicalPath;
            Lucene.Net.Store.Directory dir = Lucene.Net.Store.FSDirectory.GetDirectory(indexFileLocation, false);
            //create an analyzer to process the text
            Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer();
            //create the index writer with the directory and analyzer defined.
            Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(dir, analyzer, !IsIndexExists(indexFileLocation));

            Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();

            TextReader reader = new FilterReader(WebConfig.CVDocumentPhysicalPath + fileName);
            using (reader)
            {
                Lucene.Net.Documents.Field fldContent = new Lucene.Net.Documents.Field("content",
                                                                                    reader.ReadToEnd(),
                                                                                    Lucene.Net.Documents.Field.Store.YES,
                                                                                    Lucene.Net.Documents.Field.Index.TOKENIZED,
                                                                                    Lucene.Net.Documents.Field.TermVector.YES);
                Lucene.Net.Documents.Field fldPath = new Lucene.Net.Documents.Field("path",
                                                                                    WebConfig.CVDocumentAbsolutePath + fileName,
                                                                                    Lucene.Net.Documents.Field.Store.YES,
                                                                                    Lucene.Net.Documents.Field.Index.TOKENIZED,
                                                                                    Lucene.Net.Documents.Field.TermVector.YES);
                Lucene.Net.Documents.Field fldCandidateID = new Lucene.Net.Documents.Field("candidateID",
                                                                                    candidateID.ToString(),
                                                                                    Lucene.Net.Documents.Field.Store.YES,
                                                                                    Lucene.Net.Documents.Field.Index.TOKENIZED,
                                                                                    Lucene.Net.Documents.Field.TermVector.YES);

                doc.Add(fldContent);
                doc.Add(fldPath);
                doc.Add(fldCandidateID);
            }
            //write the document to the index
            indexWriter.AddDocument(doc);
            //optimize and close the writer
            indexWriter.Optimize();
            indexWriter.Close();
        }
        catch (Exception ex)
        {

        }
    }

    public static bool IsIndexExists(string sIndexDirectoryPath)
    {
        return IndexReader.IndexExists(sIndexDirectoryPath);        
    }

    public static string FirstWords(string input, int numberWords)
    {
        try
        {
            // Number of words we still want to display.
            int words = numberWords;
            // Loop through entire summary.
            for (int i = 0; i < input.Length; i++)
            {
                // Increment words on a space.
                if (input[i] == ' ')
                {
                    words--;
                }
                // If we have no more words to display, return the substring.
                if (words == 0)
                {
                    return input.Substring(0, i);
                }
            }
        }
        catch (Exception)
        {
            // Log the error.
        }
        return string.Empty;
    }

    public static bool ExportActionToOutlook(Page page, Neos.Data.Action action)
    {        
        string message = string.Empty;
        try
        {
            //First thing you need to do is add a reference to Microsoft Outlook 11.0 Object Library. Then, create new instance of Outlook.Application object:             
            Microsoft.Office.Interop.Outlook.Application outlookApp =
                new Microsoft.Office.Interop.Outlook.Application();

            //Next, create an instance of AppointmentItem object and set the properties: 
            Microsoft.Office.Interop.Outlook.AppointmentItem oAppointment =
                (Microsoft.Office.Interop.Outlook.AppointmentItem)outlookApp.CreateItem(
                    Microsoft.Office.Interop.Outlook.OlItemType.olAppointmentItem);

            oAppointment.Subject = action.TypeActionLabel + "-" + action.CompanyName + "-" + action.CandidateFullName;
            oAppointment.Body = action.DescrAction;
            oAppointment.Location = action.LieuRDV;

            // Set the start date
            //if(action.DateAction.HasValue) 
            //    oAppointment.Start = action.DateAction.Value;
            // End date 
            if (action.Hour.HasValue)
            {
                oAppointment.Start = action.Hour.Value;
            }
            // Set the reminder 15 minutes before start
            oAppointment.ReminderSet = true;
            oAppointment.ReminderMinutesBeforeStart = 15;

            //Setting the sound file for a reminder: 
            oAppointment.ReminderPlaySound = true;
            //set ReminderSoundFile to a filename. 

            //Setting the importance: 
            //use OlImportance enum to set the importance to low, medium or high
            oAppointment.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;

            /* OlBusyStatus is enum with following values:
            olBusy
            olFree
            olOutOfOffice
            olTentative
            */
            oAppointment.BusyStatus = Microsoft.Office.Interop.Outlook.OlBusyStatus.olBusy;

            //Finally, save the appointment: 
            // Save the appointment
            //oAppointment.Save();

            // When you call the Save () method, the appointment is saved in Outlook. 

            //string recipientsMail = string.Empty;
            //foreach (ListItem item in listEmail.Items)
            //{
            //    if (recipientsMail == string.Empty)
            //    {
            //        recipientsMail += item.Value;
            //    }
            //    else
            //    {
            //        recipientsMail += "; " + item.Value;
            //    }
            //}
            if (action.ContactID.HasValue)
            {
                string emailContact = string.Empty;
                List<CompanyContactTelephone> contactInfoList =
                    new CompanyContactTelephoneRepository().GetContactInfo(action.ContactID.Value);
                foreach (CompanyContactTelephone item in contactInfoList)
                {
                    if (item.Type == "E")
                    {
                        emailContact = item.Tel;
                        break;
                    }
                }
                //ParamUser currentUser = SessionManager.CurrentUser;
                if (!string.IsNullOrEmpty(emailContact))
                {
                    //oAppointment.RequiredAttendees = "nga@vn.netika.com";
                    //oAppointment.OptionalAttendees = "nga@vn.netika.com";
                    // Another useful method is ForwardAsVcal () which can be used to send the Vcs file via email. 
                    Microsoft.Office.Interop.Outlook.MailItem mailItem = oAppointment.ForwardAsVcal();
                    
                    mailItem.To = emailContact;
                    mailItem.Send();
                    //oAppointment.Send();
                }
                else
                {
                    message = ResourceManager.GetString("messageExportActionNotHaveEmail");
                }
            }
        }
        catch (System.Exception ex)
        {
            message = ex.Message;            
        }
        if (message != string.Empty)
        {
            string script1 = "<script type=\"text/javascript\">";

            script1 += " alert(\"" + message + "\")";
            script1 += " </script>";

            if (!page.ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                page.ClientScript.RegisterStartupScript(page.GetType(), "redirectUser", script1);
            return false;
        }
        else
        {
            return true;
        }        
    }

    public static string ExportActionToAppoinment(Neos.Data.Action action)
    {
        string message = string.Empty;
        try
        {
            if (action.ContactID.HasValue)
            {
                string emailContact = string.Empty;
                List<CompanyContactTelephone> contactInfoList =
                    new CompanyContactTelephoneRepository().GetContactInfo(action.ContactID.Value);
                foreach (CompanyContactTelephone item in contactInfoList)
                {
                    if (item.Type == "E")
                    {
                        emailContact = item.Tel;
                        break;
                    }
                }
                //ParamUser currentUser = SessionManager.CurrentUser;
                if (!string.IsNullOrEmpty(emailContact))
                {
                    DateTime today = DateTime.Now;
                    string fileName = WebConfig.AbsoluteExportDirectory + "\\ExportAppointment";
                    fileName += today.Hour.ToString() + "-" + today.Minute.ToString() + "-" + today.Second.ToString() + ".vcs";

                    StreamWriter writer = new StreamWriter(fileName);
                    writer.WriteLine("BEGIN:VCALENDAR");
                    writer.WriteLine(@"PRODID:-//Microsoft Corporation//Outlook MIMEDIR//EN");
                    writer.WriteLine("VERSION:1.0");
                    writer.WriteLine("BEGIN:VEVENT");

                    //Start and End Date in YYYYMMDDTHHMMSSZ format            
                    if (action.Hour.HasValue)
                        today = action.Hour.Value;
                    else
                        today = today.AddDays(1);

                    writer.WriteLine("DTSTART:" + today.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
                    writer.WriteLine("DTEND:" + today.AddHours(1).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
                    
                    string subject = action.TypeActionLabel + "-" + action.CompanyName + "-" + action.CandidateFullName;
                    writer.WriteLine("LOCATION:" + action.LieuRDV);
                    writer.WriteLine("CATEGORIES:" + subject);
                    writer.WriteLine("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + action.DescrAction);
                    writer.WriteLine("SUMMARY:" + subject);
                    writer.WriteLine("PRIORITY:3");
                    writer.WriteLine("END:VEVENT");
                    writer.WriteLine("END:VCALENDAR");
                    writer.Close();

                    //Send email 
                    MailMessage mail = new MailMessage();
                    //mail.To.Add(new MailAddress("netthanhhung@yahoo.com"));
                    mail.To.Add(new MailAddress(emailContact));
                    mail.Subject = action.TypeActionLabel + "-"
                        + action.CompanyName + "-" + action.CandidateFullName;
                    mail.Attachments.Add(new Attachment(fileName));
                    SendMail(mail);
                    mail.Dispose();
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                }
                else
                {
                    message = ResourceManager.GetString("messageExportActionNotHaveEmail");
                }
            }                       
        }
        catch (System.Exception ex)
        {
            message = ex.Message;
        }
        if(message == string.Empty) 
        {
            message = ResourceManager.GetString("messageExportActionSuccessfull");            
        }
        return message;
    }

    public static bool IsValidEmailAddress(string email)
    {
        // Return true if strIn is in valid e-mail format.
        return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

    }


    /// <summary>
    /// Round a number to money with precision to 2 decimal digit.
    /// </summary>
    /// <returns>Rounded money</returns>
    public static decimal RoundMoneyDoulbePercentagWith2Digit(decimal percentage)
    {
        //decimal result = percentage;
        //decimal dec = Convert.ToDecimal(percentage);
        return Decimal.Round(percentage, 2, MidpointRounding.AwayFromZero);
        //result = Decimal.ToDouble(dec);
        //return result;
    }

    /// <summary>
    /// Round a number to money with precision to 2 decimal digit.
    /// </summary>
    /// <returns>Rounded money</returns>
    public static decimal? RoundMoneyDoulbePercentagWith2Digit(double? percentage)
    {
        if (percentage.HasValue)
        {
            decimal dec = Convert.ToDecimal(percentage.Value);
            return Decimal.Round(dec, 2, MidpointRounding.AwayFromZero);           
        }
        else
        {
            return null;
        }
    }

    public static double? ConvertDecimalToDouble(decimal? value)
    {
        if (value.HasValue)
        {
            return Decimal.ToDouble(value.Value);
        }
        else
        {
            return null;
        }
    }

    public static double? RoundDoubleToDouble2Digits(double? amount)
    {
        if (amount.HasValue)
        {
            decimal dec = Convert.ToDecimal(amount.Value);
            dec = Decimal.Round(dec, 2, MidpointRounding.AwayFromZero);
            return Decimal.ToDouble(dec);
        }
        else
        {
            return null;
        }
    }

    public static int CountLineOfString(string originalString, int length)
    {
        int count = 1;
        if (originalString != null && originalString.Length > 0)
        {
            string tempString = originalString;
            while (tempString.Length > length)
            {
                tempString = tempString.Substring(length);
                count++;
            }
        }
        
        return count;
    }

    public static IFormatProvider GetDoubleFormatProvider()
    {
        NumberFormatInfo provider = new NumberFormatInfo();

        provider.NumberDecimalSeparator = ".";
        provider.NumberGroupSeparator = ",";
        return provider;
    }

    public static string Get2DigitStringOfDouble(double? amount)
    {
        if (amount.HasValue)
        {
            double? temp = RoundDoubleToDouble2Digits(amount);
            if (temp.Value == 0)
                return string.Empty; 
            else 
                return temp.Value.ToString("###,##0.00");            
        }
        else
        {
            return string.Empty;
        }
    }
}
