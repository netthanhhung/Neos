using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Store 
/// </summary>
public class WebConfig
{
    public WebConfig()
    {
        //
        // TODO: Add constructor logic here
        //
    }     

    #region Language
    public static string DefaultLanguage
    {
        get { return ConfigurationManager.AppSettings["DefaultLanguage"]; }
    }    
    public static string ENLanguageFile
    {
        get { return ConfigurationManager.AppSettings["EN_Language_File"]; }
    }
    public static string FRLanguageFile
    {
        get { return ConfigurationManager.AppSettings["FR_Language_File"]; }
    }
    public static string NLLanguageFile
    {
        get { return ConfigurationManager.AppSettings["NL_Language_File"]; }
    }

    #endregion

    #region documents
    public static string UserDocumentPath
    {
        get { return ConfigurationManager.AppSettings["UserDocumentPath"]; }
    }
    public static int MaxDocumentFilePerMultiUpload
    {
        get 
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxDocumentFilePerMultiUpload"]))
                return Int32.Parse(ConfigurationManager.AppSettings["MaxDocumentFilePerMultiUpload"]);
            return 1;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static string[] AllowFileExtension
    {
        get 
        {
            return ConfigurationManager.AppSettings["FileExtension"].Split(','); 
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static int MaxFileSize
    {
        get 
        {
            return Int32.Parse(ConfigurationManager.AppSettings["MaxFileSize"]); 
        }
    }

    public static string DocumentPhysicalPath
    {
        get 
        {
            return ConfigurationManager.AppSettings["DocumentPhysicalPath"]; 
        }
    }

    public static string DocumentAbsolutePath
    {
        get
        {
            return ConfigurationManager.AppSettings["DocumentAbsolutePath"];
        }
    }

    public static string CompanyDocumentPhysicalPath
    {
        get
        {
            return ConfigurationManager.AppSettings["CompanyDocumentPhysicalPath"];
        }
    }

    public static string CompanyDocumentAbsolutePath
    {
        get
        {
            return ConfigurationManager.AppSettings["CompanyDocumentAbsolutePath"];
        }
    }

    public static string UserImages
    {
        get
        {
            return ConfigurationManager.AppSettings["UserImages"];
        }
    }
    public static string UserImagePath
    {
        get
        {
            return ConfigurationManager.AppSettings["UserImagePath"];
        }
    }

    public static string CVDocumentPhysicalPath
    {
        get
        {
            return ConfigurationManager.AppSettings["CVDocPhysicalPath"];
        }
    }

    public static string CVDocumentAbsolutePath
    {
        get
        {
            return ConfigurationManager.AppSettings["CVDocAbsolutePath"];
        }
    }

    public static string DocumentIndexPhysicalPath
    {
        get
        {
            return ConfigurationManager.AppSettings["DocumentIndexPhysicalPath"];
        }
    }
    //
    #endregion

    #region Job
    public static string NeosJobDetailURL
    {
        get
        {
            return ConfigurationManager.AppSettings["NeosJobDetailURL"];
        }
    }    
    #endregion

    #region invoice
    public static string FiscalDate
    {
        get
        {
            return ConfigurationManager.AppSettings["FiscalDate"];
        }
    }
    public static string FirstNumberInvoice
    {
        get
        {
            return ConfigurationManager.AppSettings["FirstNumberInvoice"];
        }
    }
    public static string FirstNumberCreditNote
    {
        get
        {
            return ConfigurationManager.AppSettings["FirstNumberCreditNote"];
        }
    }

    public static string FirstNumberFutureInvoice
    {
        get
        {
            return ConfigurationManager.AppSettings["FirstNumberFutureInvoice"];
        }
    }
    public static string Currency
    {
        get
        {
            return ConfigurationManager.AppSettings["Currency"];
        }
    }

    public static string AddressFillInInvoice
    {
        get
        {
            return ConfigurationManager.AppSettings["AddressFillInInvoice"];
        }
    }
    public static string AbsoluteExportDirectory
    {
        get
        {
            return ConfigurationManager.AppSettings["AbsoluteExportDirectory"];
        }
    }

    public static string DefaultVatRate
    {
        get
        {
            return ConfigurationManager.AppSettings["DefaultVatRate"];
        }
    }

    public static string UsedPredefinedInvoicePaperToPrintInvoice
    {
        get
        {
            return ConfigurationManager.AppSettings["UsedPredefinedInvoicePaperToPrintInvoice"];
        }
    }
    public static string AbsolutePathPredefinedInvoicePaper
    {
        get
        {
            return ConfigurationManager.AppSettings["AbsolutePathPredefinedInvoicePaper"];
        }
    }
    public static string AbsolutePathImageFooterPath
    {
        get
        {
            return ConfigurationManager.AppSettings["AbsolutePathImageFooterPath"];
        }
    } 
    
    #endregion

    public static int NumberOfRecentCompany
    {
        get
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfRecentCompany"]);
        }
    }

    public static int NumberOfRecentCandidate
    {
        get
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfRecentCandidate"]);
        }
    }
}   