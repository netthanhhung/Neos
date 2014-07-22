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
using System.Threading;

public partial class CompanyDocumentPopup : System.Web.UI.Page
{
    #region page methods
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
            BindData();
        }
    }
    #endregion

    #region binding data
    private void InitControls()
    {
        txtDocumentLegend.Focus();
        
        //edit
        if (!string.IsNullOrEmpty(Request.QueryString["docID"]))
        {
            btnSave.CausesValidation = false;
            MViewUploadDoc.ActiveViewIndex = 0;
            btnAddMultiFile.Visible = false;
            btnAddSingleFile.Visible = false;

        }
        else    //add new
        {
            
            btnSave.Attributes.Add("onclick",string.Format("return checkUploadFiles(\"{0}\",'{1}')", ResourceManager.GetString("msgNotSelectFiles"),radUploadSingle.ClientID));
            btnMultiUploadSave.Attributes.Add("onclick", string.Format("return checkUploadFiles(\"{0}\",'{1}')", ResourceManager.GetString("msgNotSelectFiles"), radUploadMulti.ClientID));
            //btnSave.CausesValidation = true;
            //btnSave.ValidationGroup = "ValidSingleDocument";
            //rfvFileUpload.ErrorMessage = ResourceManager.GetString("lblSelectADocFile");
        }
        
    }

    private void BindData()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["docID"]))
        {
            int docID = Int32.Parse(Request.QueryString["docID"]);
            CompanyDocument doc = new CompanyDocumentRepository().FindOne(new CompanyDocument(docID));
            if (doc != null)
            {
                txtDocumentLegend.Text = doc.DocumentLegend;
            }
        }
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("companyDocumentPopupTitle");
        lblDocumentLegend.Text = ResourceManager.GetString("lblDocumentLegend");
        lblFileUpload.Text = ResourceManager.GetString("lblFileUpload");
        btnSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");
        btnAddMultiFile.Text = ResourceManager.GetString("addMultiFile");
        btnAddSingleFile.Text = ResourceManager.GetString("addSingleFile");
        btnMultiUploadCancel.Text = ResourceManager.GetString("cancelText");
        btnMultiUploadSave.Text = ResourceManager.GetString("saveText");
    }

    protected void OnAddMultiFileClick(object sender, EventArgs e)
    {
        MViewUploadDoc.ActiveViewIndex = 1;
    }

    protected void OnAddSingleFileClick(object sender, EventArgs e)
    {
        MViewUploadDoc.ActiveViewIndex = 0;
    }
    #endregion

    #region events

    protected void OnButtonSaveClick(object sender, EventArgs e)
    {
        radUploadSingle.AllowedFileExtensions = WebConfig.AllowFileExtension;
        radUploadSingle.MaxFileSize = WebConfig.MaxFileSize;

        CompanyDocumentRepository docRepo = new CompanyDocumentRepository();        
        //edit
        if (!string.IsNullOrEmpty(Request.QueryString["docID"]))
        {
            int docID = Int32.Parse(Request.QueryString["docID"]);
            CompanyDocument doc = docRepo.FindOne(new CompanyDocument(docID));
            if (doc != null)
            {
                if (!string.IsNullOrEmpty(txtDocumentLegend.Text.Trim()))
                    doc.DocumentLegend = txtDocumentLegend.Text.Trim();
                if (radUploadSingle.UploadedFiles.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + radUploadSingle.UploadedFiles[0].GetName();

                    doc.DocumentName = fileName;//radUploadSingle.UploadedFiles[0].GetName();
                    doc.ContentType = radUploadSingle.UploadedFiles[0].ContentType;

                    doc.AbsoluteURL = WebConfig.CompanyDocumentAbsolutePath + fileName;
                    radUploadSingle.UploadedFiles[0].SaveAs(WebConfig.CompanyDocumentPhysicalPath + fileName);
                }
                docRepo.Update(doc);
            }
        }
        else //add new
            if (!string.IsNullOrEmpty(Request.QueryString["compID"]))
            {
                if (radUploadSingle.UploadedFiles.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + radUploadSingle.UploadedFiles[0].GetName();
                    CompanyDocument doc = new CompanyDocument();
                    doc.DocumentLegend = txtDocumentLegend.Text.Trim();
                    doc.DocumentName = fileName;// radUploadSingle.UploadedFiles[0].GetName();
                    doc.ContentType = radUploadSingle.UploadedFiles[0].ContentType;
                    doc.CreatedDate = DateTime.Now;
                    doc.CompanyID = Int32.Parse(Request.QueryString["compID"]);

                    doc.AbsoluteURL = WebConfig.CompanyDocumentAbsolutePath + fileName;
                    radUploadSingle.UploadedFiles[0].SaveAs(WebConfig.CompanyDocumentPhysicalPath + fileName);

                    docRepo.Insert(doc);
                }
            }        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("saveAndCloseWindow"))
            ClientScript.RegisterStartupScript(this.GetType(), "saveAndCloseWindow", script);
    }

    protected void OnUploadMutiSaveClick(object sender, EventArgs e)
    {
        CompanyDocumentRepository docRepo = new CompanyDocumentRepository();

        radUploadMulti.MaxFileSize = WebConfig.MaxFileSize;
        radUploadMulti.AllowedFileExtensions = WebConfig.AllowFileExtension;
        radUploadMulti.MaxFileInputsCount = WebConfig.MaxDocumentFilePerMultiUpload;
        foreach (UploadedFile file in radUploadMulti.UploadedFiles)
        {
            string fileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + file.GetName();
            CompanyDocument doc = new CompanyDocument();
            doc.DocumentLegend = file.GetFieldValue("Legend");
            doc.DocumentName = fileName;// file.GetName();
            doc.AbsoluteURL = WebConfig.CompanyDocumentAbsolutePath + fileName;
            doc.CreatedDate = DateTime.Now;
            doc.CompanyID = Int16.Parse(Request.QueryString["compID"]);
            doc.ContentType = file.ContentType;
            docRepo.Insert(doc);

            file.SaveAs(WebConfig.CompanyDocumentPhysicalPath + fileName);
        }
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("saveAndCloseWindow"))
            ClientScript.RegisterStartupScript(this.GetType(), "saveAndCloseWindow", script);
    }
    #endregion

}
